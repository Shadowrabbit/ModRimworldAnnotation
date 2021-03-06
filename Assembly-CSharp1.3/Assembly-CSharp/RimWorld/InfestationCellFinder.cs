using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013EC RID: 5100
	public static class InfestationCellFinder
	{
		// Token: 0x06007C0E RID: 31758 RVA: 0x002BD85C File Offset: 0x002BBA5C
		public static bool TryFindCell(out IntVec3 cell, Map map)
		{
			InfestationCellFinder.CalculateLocationCandidates(map);
			InfestationCellFinder.LocationCandidate locationCandidate;
			if (!InfestationCellFinder.locationCandidates.TryRandomElementByWeight((InfestationCellFinder.LocationCandidate x) => x.score, out locationCandidate))
			{
				cell = IntVec3.Invalid;
				return false;
			}
			cell = CellFinder.FindNoWipeSpawnLocNear(locationCandidate.cell, map, ThingDefOf.Hive, Rot4.North, 2, (IntVec3 x) => InfestationCellFinder.GetScoreAt(x, map) > 0f && x.GetFirstThing(map, ThingDefOf.Hive) == null && x.GetFirstThing(map, ThingDefOf.TunnelHiveSpawner) == null);
			return true;
		}

		// Token: 0x06007C0F RID: 31759 RVA: 0x002BD8EC File Offset: 0x002BBAEC
		private static float GetScoreAt(IntVec3 cell, Map map)
		{
			if ((float)InfestationCellFinder.distToColonyBuilding[cell] > 30f)
			{
				return 0f;
			}
			if (!cell.Walkable(map))
			{
				return 0f;
			}
			if (cell.Fogged(map))
			{
				return 0f;
			}
			if (InfestationCellFinder.CellHasBlockingThings(cell, map))
			{
				return 0f;
			}
			if (!cell.Roofed(map) || !cell.GetRoof(map).isThickRoof)
			{
				return 0f;
			}
			Region region = cell.GetRegion(map, RegionType.Set_Passable);
			if (region == null)
			{
				return 0f;
			}
			if (InfestationCellFinder.closedAreaSize[cell] < 2)
			{
				return 0f;
			}
			float temperature = cell.GetTemperature(map);
			if (temperature < -17f)
			{
				return 0f;
			}
			float mountainousnessScoreAt = InfestationCellFinder.GetMountainousnessScoreAt(cell, map);
			if (mountainousnessScoreAt < 0.17f)
			{
				return 0f;
			}
			int num = InfestationCellFinder.StraightLineDistToUnroofed(cell, map);
			float num2;
			if (!InfestationCellFinder.regionsDistanceToUnroofed.TryGetValue(region, out num2))
			{
				num2 = (float)num * 1.15f;
			}
			else
			{
				num2 = Mathf.Min(num2, (float)num * 4f);
			}
			num2 = Mathf.Pow(num2, 1.55f);
			float num3 = Mathf.InverseLerp(0f, 12f, (float)num);
			float num4 = Mathf.Lerp(1f, 0.18f, map.glowGrid.GameGlowAt(cell, false));
			float num5 = 1f - Mathf.Clamp(InfestationCellFinder.DistToBlocker(cell, map) / 11f, 0f, 0.6f);
			float num6 = Mathf.InverseLerp(-17f, -7f, temperature);
			float num7 = num2 * num3 * num5 * mountainousnessScoreAt * num4 * num6;
			num7 = Mathf.Pow(num7, 1.2f);
			if (num7 < 7.5f)
			{
				return 0f;
			}
			return num7;
		}

		// Token: 0x06007C10 RID: 31760 RVA: 0x002BDA8C File Offset: 0x002BBC8C
		public static void DebugDraw()
		{
			if (DebugViewSettings.drawInfestationChance)
			{
				if (InfestationCellFinder.tmpCachedInfestationChanceCellColors == null)
				{
					InfestationCellFinder.tmpCachedInfestationChanceCellColors = new List<Pair<IntVec3, float>>();
				}
				if (Time.frameCount % 8 == 0)
				{
					InfestationCellFinder.tmpCachedInfestationChanceCellColors.Clear();
					Map currentMap = Find.CurrentMap;
					CellRect cellRect = Find.CameraDriver.CurrentViewRect;
					cellRect.ClipInsideMap(currentMap);
					cellRect = cellRect.ExpandedBy(1);
					InfestationCellFinder.CalculateTraversalDistancesToUnroofed(currentMap);
					InfestationCellFinder.CalculateClosedAreaSizeGrid(currentMap);
					InfestationCellFinder.CalculateDistanceToColonyBuildingGrid(currentMap);
					float num = 0.001f;
					for (int i = 0; i < currentMap.Size.z; i++)
					{
						for (int j = 0; j < currentMap.Size.x; j++)
						{
							float scoreAt = InfestationCellFinder.GetScoreAt(new IntVec3(j, 0, i), currentMap);
							if (scoreAt > num)
							{
								num = scoreAt;
							}
						}
					}
					for (int k = 0; k < currentMap.Size.z; k++)
					{
						for (int l = 0; l < currentMap.Size.x; l++)
						{
							IntVec3 intVec = new IntVec3(l, 0, k);
							if (cellRect.Contains(intVec))
							{
								float scoreAt2 = InfestationCellFinder.GetScoreAt(intVec, currentMap);
								if (scoreAt2 > 7.5f)
								{
									float second = GenMath.LerpDouble(7.5f, num, 0f, 1f, scoreAt2);
									InfestationCellFinder.tmpCachedInfestationChanceCellColors.Add(new Pair<IntVec3, float>(intVec, second));
								}
							}
						}
					}
				}
				for (int m = 0; m < InfestationCellFinder.tmpCachedInfestationChanceCellColors.Count; m++)
				{
					IntVec3 first = InfestationCellFinder.tmpCachedInfestationChanceCellColors[m].First;
					float second2 = InfestationCellFinder.tmpCachedInfestationChanceCellColors[m].Second;
					CellRenderer.RenderCell(first, SolidColorMaterials.SimpleSolidColorMaterial(new Color(0f, 0f, 1f, second2), false));
				}
				return;
			}
			InfestationCellFinder.tmpCachedInfestationChanceCellColors = null;
		}

		// Token: 0x06007C11 RID: 31761 RVA: 0x002BDC48 File Offset: 0x002BBE48
		private static void CalculateLocationCandidates(Map map)
		{
			InfestationCellFinder.locationCandidates.Clear();
			InfestationCellFinder.CalculateTraversalDistancesToUnroofed(map);
			InfestationCellFinder.CalculateClosedAreaSizeGrid(map);
			InfestationCellFinder.CalculateDistanceToColonyBuildingGrid(map);
			for (int i = 0; i < map.Size.z; i++)
			{
				for (int j = 0; j < map.Size.x; j++)
				{
					IntVec3 cell = new IntVec3(j, 0, i);
					float scoreAt = InfestationCellFinder.GetScoreAt(cell, map);
					if (scoreAt > 0f)
					{
						InfestationCellFinder.locationCandidates.Add(new InfestationCellFinder.LocationCandidate(cell, scoreAt));
					}
				}
			}
		}

		// Token: 0x06007C12 RID: 31762 RVA: 0x002BDCC8 File Offset: 0x002BBEC8
		private static bool CellHasBlockingThings(IntVec3 cell, Map map)
		{
			List<Thing> thingList = cell.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i] is Pawn || thingList[i] is Hive || thingList[i] is TunnelHiveSpawner)
				{
					return true;
				}
				if (thingList[i].def.category == ThingCategory.Building && thingList[i].def.passability == Traversability.Impassable && GenSpawn.SpawningWipes(ThingDefOf.Hive, thingList[i].def))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007C13 RID: 31763 RVA: 0x002BDD64 File Offset: 0x002BBF64
		private static int StraightLineDistToUnroofed(IntVec3 cell, Map map)
		{
			int num = int.MaxValue;
			int i = 0;
			while (i < 4)
			{
				IntVec3 facingCell = new Rot4(i).FacingCell;
				int num2 = 0;
				int num3;
				for (;;)
				{
					IntVec3 intVec = cell + facingCell * num2;
					if (!intVec.InBounds(map))
					{
						goto Block_1;
					}
					num3 = num2;
					if (InfestationCellFinder.NoRoofAroundAndWalkable(intVec, map))
					{
						break;
					}
					num2++;
				}
				IL_56:
				if (num3 < num)
				{
					num = num3;
				}
				i++;
				continue;
				Block_1:
				num3 = int.MaxValue;
				goto IL_56;
			}
			if (num == 2147483647)
			{
				return map.Size.x;
			}
			return num;
		}

		// Token: 0x06007C14 RID: 31764 RVA: 0x002BDDEC File Offset: 0x002BBFEC
		private static float DistToBlocker(IntVec3 cell, Map map)
		{
			int num = int.MinValue;
			int num2 = int.MinValue;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 facingCell = new Rot4(i).FacingCell;
				int num3 = 0;
				int num4;
				for (;;)
				{
					IntVec3 c = cell + facingCell * num3;
					num4 = num3;
					if (!c.InBounds(map) || !c.Walkable(map))
					{
						break;
					}
					num3++;
				}
				if (num4 > num)
				{
					num2 = num;
					num = num4;
				}
				else if (num4 > num2)
				{
					num2 = num4;
				}
			}
			return (float)Mathf.Min(num, num2);
		}

		// Token: 0x06007C15 RID: 31765 RVA: 0x002BDE70 File Offset: 0x002BC070
		private static bool NoRoofAroundAndWalkable(IntVec3 cell, Map map)
		{
			if (!cell.Walkable(map))
			{
				return false;
			}
			if (cell.Roofed(map))
			{
				return false;
			}
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = new Rot4(i).FacingCell + cell;
				if (c.InBounds(map) && c.Roofed(map))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007C16 RID: 31766 RVA: 0x002BDECC File Offset: 0x002BC0CC
		private static float GetMountainousnessScoreAt(IntVec3 cell, Map map)
		{
			float num = 0f;
			int num2 = 0;
			for (int i = 0; i < 700; i += 10)
			{
				IntVec3 c = cell + GenRadial.RadialPattern[i];
				if (c.InBounds(map))
				{
					Building edifice = c.GetEdifice(map);
					if (edifice != null && edifice.def.category == ThingCategory.Building && edifice.def.building.isNaturalRock)
					{
						num += 1f;
					}
					else if (c.Roofed(map) && c.GetRoof(map).isThickRoof)
					{
						num += 0.5f;
					}
					num2++;
				}
			}
			return num / (float)num2;
		}

		// Token: 0x06007C17 RID: 31767 RVA: 0x002BDF70 File Offset: 0x002BC170
		private static void CalculateTraversalDistancesToUnroofed(Map map)
		{
			InfestationCellFinder.tempUnroofedRegions.Clear();
			for (int i = 0; i < map.Size.z; i++)
			{
				for (int j = 0; j < map.Size.x; j++)
				{
					IntVec3 intVec = new IntVec3(j, 0, i);
					Region region = intVec.GetRegion(map, RegionType.Set_Passable);
					if (region != null && InfestationCellFinder.NoRoofAroundAndWalkable(intVec, map))
					{
						InfestationCellFinder.tempUnroofedRegions.Add(region);
					}
				}
			}
			Dijkstra<Region>.Run(InfestationCellFinder.tempUnroofedRegions, (Region x) => x.Neighbors, (Region a, Region b) => Mathf.Sqrt((float)a.extentsClose.CenterCell.DistanceToSquared(b.extentsClose.CenterCell)), InfestationCellFinder.regionsDistanceToUnroofed, null);
			InfestationCellFinder.tempUnroofedRegions.Clear();
		}

		// Token: 0x06007C18 RID: 31768 RVA: 0x002BE038 File Offset: 0x002BC238
		private static void CalculateClosedAreaSizeGrid(Map map)
		{
			if (InfestationCellFinder.closedAreaSize == null)
			{
				InfestationCellFinder.closedAreaSize = new ByteGrid(map);
			}
			else
			{
				InfestationCellFinder.closedAreaSize.ClearAndResizeTo(map);
			}
			Predicate<IntVec3> <>9__0;
			Predicate<IntVec3> <>9__2;
			for (int i = 0; i < map.Size.z; i++)
			{
				for (int j = 0; j < map.Size.x; j++)
				{
					IntVec3 intVec = new IntVec3(j, 0, i);
					if (InfestationCellFinder.closedAreaSize[j, i] == 0 && !intVec.Impassable(map))
					{
						int area = 0;
						FloodFiller floodFiller = map.floodFiller;
						IntVec3 root = intVec;
						Predicate<IntVec3> passCheck;
						if ((passCheck = <>9__0) == null)
						{
							passCheck = (<>9__0 = ((IntVec3 c) => !c.Impassable(map)));
						}
						floodFiller.FloodFill(root, passCheck, delegate(IntVec3 c)
						{
							int area = area;
							area++;
						}, int.MaxValue, false, null);
						area = Mathf.Min(area, 255);
						FloodFiller floodFiller2 = map.floodFiller;
						IntVec3 root2 = intVec;
						Predicate<IntVec3> passCheck2;
						if ((passCheck2 = <>9__2) == null)
						{
							passCheck2 = (<>9__2 = ((IntVec3 c) => !c.Impassable(map)));
						}
						floodFiller2.FloodFill(root2, passCheck2, delegate(IntVec3 c)
						{
							InfestationCellFinder.closedAreaSize[c] = (byte)area;
						}, int.MaxValue, false, null);
					}
				}
			}
		}

		// Token: 0x06007C19 RID: 31769 RVA: 0x002BE19C File Offset: 0x002BC39C
		private static void CalculateDistanceToColonyBuildingGrid(Map map)
		{
			if (InfestationCellFinder.distToColonyBuilding == null)
			{
				InfestationCellFinder.distToColonyBuilding = new ByteGrid(map);
			}
			else if (!InfestationCellFinder.distToColonyBuilding.MapSizeMatches(map))
			{
				InfestationCellFinder.distToColonyBuilding.ClearAndResizeTo(map);
			}
			InfestationCellFinder.distToColonyBuilding.Clear(byte.MaxValue);
			InfestationCellFinder.tmpColonyBuildingsLocs.Clear();
			List<Building> allBuildingsColonist = map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				InfestationCellFinder.tmpColonyBuildingsLocs.Add(allBuildingsColonist[i].Position);
			}
			Dijkstra<IntVec3>.Run(InfestationCellFinder.tmpColonyBuildingsLocs, (IntVec3 x) => DijkstraUtility.AdjacentCellsNeighborsGetter(x, map), delegate(IntVec3 a, IntVec3 b)
			{
				if (a.x == b.x || a.z == b.z)
				{
					return 1f;
				}
				return 1.4142135f;
			}, InfestationCellFinder.tmpDistanceResult, null);
			for (int j = 0; j < InfestationCellFinder.tmpDistanceResult.Count; j++)
			{
				InfestationCellFinder.distToColonyBuilding[InfestationCellFinder.tmpDistanceResult[j].Key] = (byte)Mathf.Min(InfestationCellFinder.tmpDistanceResult[j].Value, 254.999f);
			}
		}

		// Token: 0x040044B3 RID: 17587
		private static List<InfestationCellFinder.LocationCandidate> locationCandidates = new List<InfestationCellFinder.LocationCandidate>();

		// Token: 0x040044B4 RID: 17588
		private static Dictionary<Region, float> regionsDistanceToUnroofed = new Dictionary<Region, float>();

		// Token: 0x040044B5 RID: 17589
		private static ByteGrid closedAreaSize;

		// Token: 0x040044B6 RID: 17590
		private static ByteGrid distToColonyBuilding;

		// Token: 0x040044B7 RID: 17591
		private const float MinRequiredScore = 7.5f;

		// Token: 0x040044B8 RID: 17592
		private const float MinMountainousnessScore = 0.17f;

		// Token: 0x040044B9 RID: 17593
		private const int MountainousnessScoreRadialPatternIdx = 700;

		// Token: 0x040044BA RID: 17594
		private const int MountainousnessScoreRadialPatternSkip = 10;

		// Token: 0x040044BB RID: 17595
		private const float MountainousnessScorePerRock = 1f;

		// Token: 0x040044BC RID: 17596
		private const float MountainousnessScorePerThickRoof = 0.5f;

		// Token: 0x040044BD RID: 17597
		private const float MinCellTempToSpawnHive = -17f;

		// Token: 0x040044BE RID: 17598
		private const float MaxDistanceToColonyBuilding = 30f;

		// Token: 0x040044BF RID: 17599
		private static List<Pair<IntVec3, float>> tmpCachedInfestationChanceCellColors;

		// Token: 0x040044C0 RID: 17600
		private static HashSet<Region> tempUnroofedRegions = new HashSet<Region>();

		// Token: 0x040044C1 RID: 17601
		private static List<IntVec3> tmpColonyBuildingsLocs = new List<IntVec3>();

		// Token: 0x040044C2 RID: 17602
		private static List<KeyValuePair<IntVec3, float>> tmpDistanceResult = new List<KeyValuePair<IntVec3, float>>();

		// Token: 0x020027D9 RID: 10201
		private struct LocationCandidate
		{
			// Token: 0x0600DB2E RID: 56110 RVA: 0x00416652 File Offset: 0x00414852
			public LocationCandidate(IntVec3 cell, float score)
			{
				this.cell = cell;
				this.score = score;
			}

			// Token: 0x040096AD RID: 38573
			public IntVec3 cell;

			// Token: 0x040096AE RID: 38574
			public float score;
		}
	}
}
