using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012AC RID: 4780
	public class GenStep_Terrain : GenStep
	{
		// Token: 0x17001004 RID: 4100
		// (get) Token: 0x060067D3 RID: 26579 RVA: 0x00046C04 File Offset: 0x00044E04
		public override int SeedPart
		{
			get
			{
				return 262606459;
			}
		}

		// Token: 0x060067D4 RID: 26580 RVA: 0x001FFFA0 File Offset: 0x001FE1A0
		public override void Generate(Map map, GenStepParams parms)
		{
			BeachMaker.Init(map);
			RiverMaker riverMaker = this.GenerateRiver(map);
			List<IntVec3> list = new List<IntVec3>();
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			MapGenFloatGrid fertility = MapGenerator.Fertility;
			MapGenFloatGrid caves = MapGenerator.Caves;
			TerrainGrid terrainGrid = map.terrainGrid;
			foreach (IntVec3 c in map.AllCells)
			{
				Building edifice = c.GetEdifice(map);
				TerrainDef terrainDef;
				if ((edifice != null && edifice.def.Fillage == FillCategory.Full) || caves[c] > 0f)
				{
					terrainDef = this.TerrainFrom(c, map, elevation[c], fertility[c], riverMaker, true);
				}
				else
				{
					terrainDef = this.TerrainFrom(c, map, elevation[c], fertility[c], riverMaker, false);
				}
				if (terrainDef.IsRiver && edifice != null)
				{
					list.Add(edifice.Position);
					edifice.Destroy(DestroyMode.Vanish);
				}
				terrainGrid.SetTerrain(c, terrainDef);
			}
			if (riverMaker != null)
			{
				riverMaker.ValidatePassage(map);
			}
			this.RemoveIslands(map);
			RoofCollapseCellsFinder.RemoveBulkCollapsingRoofs(list, map);
			BeachMaker.Cleanup();
			foreach (TerrainPatchMaker terrainPatchMaker in map.Biome.terrainPatchMakers)
			{
				terrainPatchMaker.Cleanup();
			}
		}

		// Token: 0x060067D5 RID: 26581 RVA: 0x0020011C File Offset: 0x001FE31C
		private TerrainDef TerrainFrom(IntVec3 c, Map map, float elevation, float fertility, RiverMaker river, bool preferSolid)
		{
			TerrainDef terrainDef = null;
			if (river != null)
			{
				terrainDef = river.TerrainAt(c, true);
			}
			if (terrainDef == null && preferSolid)
			{
				return GenStep_RocksFromGrid.RockDefAt(c).building.naturalTerrain;
			}
			TerrainDef terrainDef2 = BeachMaker.BeachTerrainAt(c, map.Biome);
			if (terrainDef2 == TerrainDefOf.WaterOceanDeep)
			{
				return terrainDef2;
			}
			if (terrainDef != null && terrainDef.IsRiver)
			{
				return terrainDef;
			}
			if (terrainDef2 != null)
			{
				return terrainDef2;
			}
			if (terrainDef != null)
			{
				return terrainDef;
			}
			for (int i = 0; i < map.Biome.terrainPatchMakers.Count; i++)
			{
				terrainDef2 = map.Biome.terrainPatchMakers[i].TerrainAt(c, map, fertility);
				if (terrainDef2 != null)
				{
					return terrainDef2;
				}
			}
			if (elevation > 0.55f && elevation < 0.61f)
			{
				return TerrainDefOf.Gravel;
			}
			if (elevation >= 0.61f)
			{
				return GenStep_RocksFromGrid.RockDefAt(c).building.naturalTerrain;
			}
			terrainDef2 = TerrainThreshold.TerrainAtValue(map.Biome.terrainsByFertility, fertility);
			if (terrainDef2 != null)
			{
				return terrainDef2;
			}
			if (!GenStep_Terrain.debug_WarnedMissingTerrain)
			{
				Log.Error(string.Concat(new object[]
				{
					"No terrain found in biome ",
					map.Biome.defName,
					" for elevation=",
					elevation,
					", fertility=",
					fertility
				}), false);
				GenStep_Terrain.debug_WarnedMissingTerrain = true;
			}
			return TerrainDefOf.Sand;
		}

		// Token: 0x060067D6 RID: 26582 RVA: 0x00200260 File Offset: 0x001FE460
		private void RemoveIslands(Map map)
		{
			GenStep_Terrain.<>c__DisplayClass7_0 CS$<>8__locals1 = new GenStep_Terrain.<>c__DisplayClass7_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.mapRect = CellRect.WholeMap(CS$<>8__locals1.map);
			int num = 0;
			GenStep_Terrain.tmpVisited.Clear();
			foreach (IntVec3 intVec in CS$<>8__locals1.map.AllCells)
			{
				if (!GenStep_Terrain.tmpVisited.Contains(intVec) && !CS$<>8__locals1.<RemoveIslands>g__Impassable|0(intVec))
				{
					int area = 0;
					bool touchesMapEdge = false;
					FloodFiller floodFiller = CS$<>8__locals1.map.floodFiller;
					IntVec3 root = intVec;
					Predicate<IntVec3> passCheck;
					if ((passCheck = CS$<>8__locals1.<>9__1) == null)
					{
						passCheck = (CS$<>8__locals1.<>9__1 = ((IntVec3 x) => !base.<RemoveIslands>g__Impassable|0(x)));
					}
					floodFiller.FloodFill(root, passCheck, delegate(IntVec3 x)
					{
						GenStep_Terrain.tmpVisited.Add(x);
						int area = area;
						area++;
						if (CS$<>8__locals1.mapRect.IsOnEdge(x))
						{
							touchesMapEdge = true;
						}
					}, int.MaxValue, false, null);
					if (touchesMapEdge)
					{
						num = Mathf.Max(num, area);
					}
				}
			}
			if (num < 30)
			{
				return;
			}
			GenStep_Terrain.tmpVisited.Clear();
			foreach (IntVec3 intVec2 in CS$<>8__locals1.map.AllCells)
			{
				if (!GenStep_Terrain.tmpVisited.Contains(intVec2) && !CS$<>8__locals1.<RemoveIslands>g__Impassable|0(intVec2))
				{
					GenStep_Terrain.tmpIsland.Clear();
					TerrainDef adjacentImpassableTerrain = null;
					bool touchesMapEdge = false;
					CS$<>8__locals1.map.floodFiller.FloodFill(intVec2, delegate(IntVec3 x)
					{
						if (CS$<>8__locals1.<RemoveIslands>g__Impassable|0(x))
						{
							adjacentImpassableTerrain = x.GetTerrain(CS$<>8__locals1.map);
							return false;
						}
						return true;
					}, delegate(IntVec3 x)
					{
						GenStep_Terrain.tmpVisited.Add(x);
						GenStep_Terrain.tmpIsland.Add(x);
						if (CS$<>8__locals1.mapRect.IsOnEdge(x))
						{
							touchesMapEdge = true;
						}
					}, int.MaxValue, false, null);
					if ((GenStep_Terrain.tmpIsland.Count <= num / 20 || (!touchesMapEdge && GenStep_Terrain.tmpIsland.Count < num / 2)) && adjacentImpassableTerrain != null)
					{
						for (int i = 0; i < GenStep_Terrain.tmpIsland.Count; i++)
						{
							CS$<>8__locals1.map.terrainGrid.SetTerrain(GenStep_Terrain.tmpIsland[i], adjacentImpassableTerrain);
						}
					}
				}
			}
		}

		// Token: 0x060067D7 RID: 26583 RVA: 0x002004F4 File Offset: 0x001FE6F4
		private RiverMaker GenerateRiver(Map map)
		{
			List<Tile.RiverLink> rivers = Find.WorldGrid[map.Tile].Rivers;
			if (rivers == null || rivers.Count == 0)
			{
				return null;
			}
			float angle = Find.WorldGrid.GetHeadingFromTo(map.Tile, (from rl in rivers
			orderby -rl.river.degradeThreshold
			select rl).First<Tile.RiverLink>().neighbor);
			Rot4 a = Find.World.CoastDirectionAt(map.Tile);
			if (a != Rot4.Invalid)
			{
				angle = a.AsAngle + (float)Rand.RangeInclusive(-30, 30);
			}
			RiverMaker riverMaker = new RiverMaker(new Vector3(Rand.Range(0.3f, 0.7f) * (float)map.Size.x, 0f, Rand.Range(0.3f, 0.7f) * (float)map.Size.z), angle, (from rl in rivers
			orderby -rl.river.degradeThreshold
			select rl).FirstOrDefault<Tile.RiverLink>().river);
			this.GenerateRiverLookupTexture(map, riverMaker);
			return riverMaker;
		}

		// Token: 0x060067D8 RID: 26584 RVA: 0x00200618 File Offset: 0x001FE818
		private void UpdateRiverAnchorEntry(Dictionary<int, GenStep_Terrain.GRLT_Entry> entries, IntVec3 center, int entryId, float zValue)
		{
			float num = zValue - (float)entryId;
			if (num > 2f)
			{
				return;
			}
			if (!entries.ContainsKey(entryId) || entries[entryId].bestDistance > num)
			{
				entries[entryId] = new GenStep_Terrain.GRLT_Entry
				{
					bestDistance = num,
					bestNode = center
				};
			}
		}

		// Token: 0x060067D9 RID: 26585 RVA: 0x0020066C File Offset: 0x001FE86C
		private void GenerateRiverLookupTexture(Map map, RiverMaker riverMaker)
		{
			int num = Mathf.CeilToInt((from rd in DefDatabase<RiverDef>.AllDefs
			select rd.widthOnMap / 2f + 8f).Max());
			int num2 = Mathf.Max(4, num) * 2;
			Dictionary<int, GenStep_Terrain.GRLT_Entry> dictionary = new Dictionary<int, GenStep_Terrain.GRLT_Entry>();
			Dictionary<int, GenStep_Terrain.GRLT_Entry> dictionary2 = new Dictionary<int, GenStep_Terrain.GRLT_Entry>();
			Dictionary<int, GenStep_Terrain.GRLT_Entry> dictionary3 = new Dictionary<int, GenStep_Terrain.GRLT_Entry>();
			for (int i = -num2; i < map.Size.z + num2; i++)
			{
				for (int j = -num2; j < map.Size.x + num2; j++)
				{
					IntVec3 intVec = new IntVec3(j, 0, i);
					Vector3 vector = riverMaker.WaterCoordinateAt(intVec);
					int entryId = Mathf.FloorToInt(vector.z / 4f);
					this.UpdateRiverAnchorEntry(dictionary, intVec, entryId, (vector.z + Mathf.Abs(vector.x)) / 4f);
					this.UpdateRiverAnchorEntry(dictionary2, intVec, entryId, (vector.z + Mathf.Abs(vector.x - (float)num)) / 4f);
					this.UpdateRiverAnchorEntry(dictionary3, intVec, entryId, (vector.z + Mathf.Abs(vector.x + (float)num)) / 4f);
				}
			}
			int num3 = Mathf.Max(new int[]
			{
				dictionary.Keys.Min(),
				dictionary2.Keys.Min(),
				dictionary3.Keys.Min()
			});
			int num4 = Mathf.Min(new int[]
			{
				dictionary.Keys.Max(),
				dictionary2.Keys.Max(),
				dictionary3.Keys.Max()
			});
			for (int k = num3; k < num4; k++)
			{
				WaterInfo waterInfo = map.waterInfo;
				if (dictionary2.ContainsKey(k) && dictionary2.ContainsKey(k + 1))
				{
					List<Vector3> riverDebugData = waterInfo.riverDebugData;
					GenStep_Terrain.GRLT_Entry grlt_Entry = dictionary2[k];
					riverDebugData.Add(grlt_Entry.bestNode.ToVector3Shifted());
					List<Vector3> riverDebugData2 = waterInfo.riverDebugData;
					grlt_Entry = dictionary2[k + 1];
					riverDebugData2.Add(grlt_Entry.bestNode.ToVector3Shifted());
				}
				if (dictionary.ContainsKey(k) && dictionary.ContainsKey(k + 1))
				{
					List<Vector3> riverDebugData3 = waterInfo.riverDebugData;
					GenStep_Terrain.GRLT_Entry grlt_Entry = dictionary[k];
					riverDebugData3.Add(grlt_Entry.bestNode.ToVector3Shifted());
					List<Vector3> riverDebugData4 = waterInfo.riverDebugData;
					grlt_Entry = dictionary[k + 1];
					riverDebugData4.Add(grlt_Entry.bestNode.ToVector3Shifted());
				}
				if (dictionary3.ContainsKey(k) && dictionary3.ContainsKey(k + 1))
				{
					List<Vector3> riverDebugData5 = waterInfo.riverDebugData;
					GenStep_Terrain.GRLT_Entry grlt_Entry = dictionary3[k];
					riverDebugData5.Add(grlt_Entry.bestNode.ToVector3Shifted());
					List<Vector3> riverDebugData6 = waterInfo.riverDebugData;
					grlt_Entry = dictionary3[k + 1];
					riverDebugData6.Add(grlt_Entry.bestNode.ToVector3Shifted());
				}
				if (dictionary2.ContainsKey(k) && dictionary.ContainsKey(k))
				{
					List<Vector3> riverDebugData7 = waterInfo.riverDebugData;
					GenStep_Terrain.GRLT_Entry grlt_Entry = dictionary2[k];
					riverDebugData7.Add(grlt_Entry.bestNode.ToVector3Shifted());
					List<Vector3> riverDebugData8 = waterInfo.riverDebugData;
					grlt_Entry = dictionary[k];
					riverDebugData8.Add(grlt_Entry.bestNode.ToVector3Shifted());
				}
				if (dictionary.ContainsKey(k) && dictionary3.ContainsKey(k))
				{
					List<Vector3> riverDebugData9 = waterInfo.riverDebugData;
					GenStep_Terrain.GRLT_Entry grlt_Entry = dictionary[k];
					riverDebugData9.Add(grlt_Entry.bestNode.ToVector3Shifted());
					List<Vector3> riverDebugData10 = waterInfo.riverDebugData;
					grlt_Entry = dictionary3[k];
					riverDebugData10.Add(grlt_Entry.bestNode.ToVector3Shifted());
				}
			}
			CellRect cellRect = new CellRect(-2, -2, map.Size.x + 4, map.Size.z + 4);
			float[] array = new float[cellRect.Area * 2];
			int num5 = 0;
			for (int l = cellRect.minZ; l <= cellRect.maxZ; l++)
			{
				for (int m = cellRect.minX; m <= cellRect.maxX; m++)
				{
					IntVec3 a = new IntVec3(m, 0, l);
					bool flag = true;
					for (int n = 0; n < GenAdj.AdjacentCellsAndInside.Length; n++)
					{
						if (riverMaker.TerrainAt(a + GenAdj.AdjacentCellsAndInside[n], false) != null)
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						Vector2 p = a.ToIntVec2.ToVector2();
						int num6 = int.MinValue;
						Vector2 zero = Vector2.zero;
						for (int num7 = num3; num7 < num4; num7++)
						{
							if (dictionary2.ContainsKey(num7) && dictionary2.ContainsKey(num7 + 1) && dictionary.ContainsKey(num7) && dictionary.ContainsKey(num7 + 1) && dictionary3.ContainsKey(num7) && dictionary3.ContainsKey(num7 + 1))
							{
								GenStep_Terrain.GRLT_Entry grlt_Entry = dictionary2[num7];
								Vector2 p2 = grlt_Entry.bestNode.ToIntVec2.ToVector2();
								grlt_Entry = dictionary2[num7 + 1];
								Vector2 p3 = grlt_Entry.bestNode.ToIntVec2.ToVector2();
								grlt_Entry = dictionary[num7];
								Vector2 p4 = grlt_Entry.bestNode.ToIntVec2.ToVector2();
								grlt_Entry = dictionary[num7 + 1];
								Vector2 p5 = grlt_Entry.bestNode.ToIntVec2.ToVector2();
								grlt_Entry = dictionary3[num7];
								Vector2 p6 = grlt_Entry.bestNode.ToIntVec2.ToVector2();
								grlt_Entry = dictionary3[num7 + 1];
								Vector2 p7 = grlt_Entry.bestNode.ToIntVec2.ToVector2();
								Vector2 vector2 = GenGeo.InverseQuadBilinear(p, p4, p2, p5, p3);
								if (vector2.x >= -0.0001f && vector2.x <= 1.0001f && vector2.y >= -0.0001f && vector2.y <= 1.0001f)
								{
									zero = new Vector2(-vector2.x * (float)num, (vector2.y + (float)num7) * 4f);
									num6 = num7;
									break;
								}
								Vector2 vector3 = GenGeo.InverseQuadBilinear(p, p4, p6, p5, p7);
								if (vector3.x >= -0.0001f && vector3.x <= 1.0001f && vector3.y >= -0.0001f && vector3.y <= 1.0001f)
								{
									zero = new Vector2(vector3.x * (float)num, (vector3.y + (float)num7) * 4f);
									num6 = num7;
									break;
								}
							}
						}
						if (num6 == -2147483648)
						{
							Log.ErrorOnce("Failed to find all necessary river flow data", 5273133, false);
						}
						array[num5] = zero.x;
						array[num5 + 1] = zero.y;
					}
					num5 += 2;
				}
			}
			float[] array2 = new float[cellRect.Area * 2];
			float[] array3 = new float[]
			{
				0.123317f,
				0.123317f,
				0.123317f,
				0.123317f,
				0.077847f,
				0.077847f,
				0.077847f,
				0.077847f,
				0.195346f
			};
			int num8 = 0;
			for (int num9 = cellRect.minZ; num9 <= cellRect.maxZ; num9++)
			{
				for (int num10 = cellRect.minX; num10 <= cellRect.maxX; num10++)
				{
					IntVec3 a2 = new IntVec3(num10, 0, num9);
					float num11 = 0f;
					float num12 = 0f;
					float num13 = 0f;
					for (int num14 = 0; num14 < GenAdj.AdjacentCellsAndInside.Length; num14++)
					{
						IntVec3 c = a2 + GenAdj.AdjacentCellsAndInside[num14];
						if (cellRect.Contains(c))
						{
							int num15 = num8 + (GenAdj.AdjacentCellsAndInside[num14].x + GenAdj.AdjacentCellsAndInside[num14].z * cellRect.Width) * 2;
							if (array[num15] != 0f || array[num15 + 1] != 0f)
							{
								num11 += array[num15] * array3[num14];
								num12 += array[num15 + 1] * array3[num14];
								num13 += array3[num14];
							}
						}
					}
					if (num13 > 0f)
					{
						array2[num8] = num11 / num13;
						array2[num8 + 1] = num12 / num13;
					}
					num8 += 2;
				}
			}
			array = array2;
			for (int num16 = 0; num16 < array.Length; num16 += 2)
			{
				if (array[num16] != 0f || array[num16 + 1] != 0f)
				{
					Vector2 vector4 = Rand.InsideUnitCircle * 0.4f;
					array[num16] += vector4.x;
					array[num16 + 1] += vector4.y;
				}
			}
			byte[] array4 = new byte[array.Length * 4];
			Buffer.BlockCopy(array, 0, array4, 0, array.Length * 4);
			map.waterInfo.riverOffsetMap = array4;
			map.waterInfo.GenerateRiverFlowMap();
		}

		// Token: 0x04004518 RID: 17688
		private static bool debug_WarnedMissingTerrain = false;

		// Token: 0x04004519 RID: 17689
		private static HashSet<IntVec3> tmpVisited = new HashSet<IntVec3>();

		// Token: 0x0400451A RID: 17690
		private static List<IntVec3> tmpIsland = new List<IntVec3>();

		// Token: 0x020012AD RID: 4781
		private struct GRLT_Entry
		{
			// Token: 0x0400451B RID: 17691
			public float bestDistance;

			// Token: 0x0400451C RID: 17692
			public IntVec3 bestNode;
		}
	}
}
