﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017A4 RID: 6052
	public static class CaravanExitMapUtility
	{
		// Token: 0x06008C32 RID: 35890 RVA: 0x003248B0 File Offset: 0x00322AB0
		public static Caravan ExitMapAndCreateCaravan(IEnumerable<Pawn> pawns, Faction faction, int exitFromTile, Direction8Way dir, int destinationTile, bool sendMessage = true)
		{
			int directionTile = CaravanExitMapUtility.FindRandomStartingTileBasedOnExitDir(exitFromTile, dir);
			return CaravanExitMapUtility.ExitMapAndCreateCaravan(pawns, faction, exitFromTile, directionTile, destinationTile, sendMessage);
		}

		// Token: 0x06008C33 RID: 35891 RVA: 0x003248D4 File Offset: 0x00322AD4
		public static Caravan ExitMapAndCreateCaravan(IEnumerable<Pawn> pawns, Faction faction, int exitFromTile, int directionTile, int destinationTile, bool sendMessage = true)
		{
			if (!GenWorldClosest.TryFindClosestPassableTile(exitFromTile, out exitFromTile))
			{
				Log.Error("Could not find any passable tile for a new caravan.");
				return null;
			}
			if (Find.World.Impassable(directionTile))
			{
				directionTile = exitFromTile;
			}
			CaravanExitMapUtility.tmpPawns.Clear();
			CaravanExitMapUtility.tmpPawns.AddRange(pawns);
			Map map = null;
			for (int i = 0; i < CaravanExitMapUtility.tmpPawns.Count; i++)
			{
				CaravanExitMapUtility.AddCaravanExitTaleIfShould(CaravanExitMapUtility.tmpPawns[i]);
				map = CaravanExitMapUtility.tmpPawns[i].MapHeld;
				if (map != null)
				{
					break;
				}
			}
			Caravan caravan = CaravanMaker.MakeCaravan(CaravanExitMapUtility.tmpPawns, faction, exitFromTile, false);
			Rot4 exitDir = (map != null) ? Find.WorldGrid.GetRotFromTo(exitFromTile, directionTile) : Rot4.Invalid;
			for (int j = 0; j < CaravanExitMapUtility.tmpPawns.Count; j++)
			{
				CaravanExitMapUtility.tmpPawns[j].ExitMap(false, exitDir);
			}
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int k = 0; k < pawnsListForReading.Count; k++)
			{
				if (!pawnsListForReading[k].IsWorldPawn())
				{
					Find.WorldPawns.PassToWorld(pawnsListForReading[k], PawnDiscardDecideMode.Decide);
				}
			}
			if (map != null)
			{
				map.Parent.Notify_CaravanFormed(caravan);
				map.retainedCaravanData.Notify_CaravanFormed(caravan);
			}
			if (!caravan.pather.Moving && caravan.Tile != directionTile)
			{
				caravan.pather.StartPath(directionTile, null, true, true);
				caravan.pather.nextTileCostLeft /= 2f;
				caravan.tweener.ResetTweenedPosToRoot();
			}
			if (destinationTile != -1)
			{
				List<FloatMenuOption> list = FloatMenuMakerWorld.ChoicesAtFor(destinationTile, caravan);
				if (list.Any((FloatMenuOption x) => !x.Disabled))
				{
					list.First((FloatMenuOption x) => !x.Disabled).action();
				}
				else
				{
					caravan.pather.StartPath(destinationTile, null, true, true);
				}
			}
			if (sendMessage)
			{
				TaggedString taggedString = "MessageFormedCaravan".Translate(caravan.Name).CapitalizeFirst();
				if (caravan.pather.Moving && caravan.pather.ArrivalAction != null)
				{
					taggedString += " " + "MessageFormedCaravan_Orders".Translate() + ": " + caravan.pather.ArrivalAction.Label + ".";
				}
				Messages.Message(taggedString, caravan, MessageTypeDefOf.TaskCompletion, true);
			}
			return caravan;
		}

		// Token: 0x06008C34 RID: 35892 RVA: 0x00324B6C File Offset: 0x00322D6C
		public static void ExitMapAndJoinOrCreateCaravan(Pawn pawn, Rot4 exitDir)
		{
			Caravan caravan = CaravanExitMapUtility.FindCaravanToJoinFor(pawn);
			if (caravan != null)
			{
				CaravanExitMapUtility.AddCaravanExitTaleIfShould(pawn);
				caravan.AddPawn(pawn, true);
				pawn.ExitMap(false, exitDir);
				return;
			}
			if (pawn.IsColonist)
			{
				Map map = pawn.Map;
				int directionTile = CaravanExitMapUtility.FindRandomStartingTileBasedOnExitDir(map.Tile, exitDir);
				Caravan caravan2 = CaravanExitMapUtility.ExitMapAndCreateCaravan(Gen.YieldSingle<Pawn>(pawn), pawn.Faction, map.Tile, directionTile, -1, false);
				caravan2.autoJoinable = true;
				bool flag = false;
				List<Pawn> allPawnsSpawned = map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (CaravanExitMapUtility.FindCaravanToJoinFor(allPawnsSpawned[i]) != null && !allPawnsSpawned[i].Downed && !allPawnsSpawned[i].Drafted)
					{
						if (allPawnsSpawned[i].RaceProps.Animal)
						{
							flag = true;
						}
						RestUtility.WakeUp(allPawnsSpawned[i]);
						allPawnsSpawned[i].jobs.CheckForJobOverride();
					}
				}
				TaggedString taggedString = "MessagePawnLeftMapAndCreatedCaravan".Translate(pawn.LabelShort, pawn).CapitalizeFirst();
				if (flag)
				{
					taggedString += " " + "MessagePawnLeftMapAndCreatedCaravan_AnimalsWantToJoin".Translate();
				}
				Messages.Message(taggedString, caravan2, MessageTypeDefOf.TaskCompletion, true);
				return;
			}
			Log.Error("Pawn " + pawn + " didn't find any caravan to join, and he can't create one.");
		}

		// Token: 0x06008C35 RID: 35893 RVA: 0x00324CDF File Offset: 0x00322EDF
		public static bool CanExitMapAndJoinOrCreateCaravanNow(Pawn pawn)
		{
			return pawn.Spawned && pawn.Map.exitMapGrid.MapUsesExitGrid && (pawn.IsColonist || CaravanExitMapUtility.FindCaravanToJoinFor(pawn) != null);
		}

		// Token: 0x06008C36 RID: 35894 RVA: 0x00324D14 File Offset: 0x00322F14
		public static List<int> AvailableExitTilesAt(Map map)
		{
			CaravanExitMapUtility.retTiles.Clear();
			int currentTileID = map.Tile;
			World world = Find.World;
			WorldGrid grid = world.grid;
			grid.GetTileNeighbors(currentTileID, CaravanExitMapUtility.tmpNeighbors);
			Predicate<IntVec3> <>9__1;
			Predicate<IntVec3> <>9__2;
			for (int i = 0; i < CaravanExitMapUtility.tmpNeighbors.Count; i++)
			{
				int num = CaravanExitMapUtility.tmpNeighbors[i];
				if (CaravanExitMapUtility.IsGoodCaravanStartingTile(num))
				{
					Rot4 rot;
					Rot4 rot2;
					CaravanExitMapUtility.GetExitMapEdges(grid, currentTileID, num, out rot, out rot2);
					IntVec3 intVec;
					if (rot != Rot4.Invalid)
					{
						Predicate<IntVec3> validator;
						if ((validator = <>9__1) == null)
						{
							validator = (<>9__1 = ((IntVec3 x) => x.Walkable(map) && !x.Fogged(map)));
						}
						if (CellFinder.TryFindRandomEdgeCellWith(validator, map, rot, CellFinder.EdgeRoadChance_Ignore, out intVec))
						{
							goto IL_10E;
						}
					}
					if (!(rot2 != Rot4.Invalid))
					{
						goto IL_126;
					}
					Predicate<IntVec3> validator2;
					if ((validator2 = <>9__2) == null)
					{
						validator2 = (<>9__2 = ((IntVec3 x) => x.Walkable(map) && !x.Fogged(map)));
					}
					if (!CellFinder.TryFindRandomEdgeCellWith(validator2, map, rot2, CellFinder.EdgeRoadChance_Ignore, out intVec))
					{
						goto IL_126;
					}
					IL_10E:
					if (!CaravanExitMapUtility.retTiles.Contains(num))
					{
						CaravanExitMapUtility.retTiles.Add(num);
					}
				}
				IL_126:;
			}
			CaravanExitMapUtility.retTiles.SortBy((int x) => grid.GetHeadingFromTo(currentTileID, x));
			return CaravanExitMapUtility.retTiles;
		}

		// Token: 0x06008C37 RID: 35895 RVA: 0x00324E78 File Offset: 0x00323078
		public static void GetExitMapEdges(WorldGrid grid, int fromTileID, int toTileID, out Rot4 primary, out Rot4 secondary)
		{
			primary = (secondary = Rot4.Invalid);
			int num = 0;
			float heading = grid.GetHeadingFromTo(fromTileID, toTileID);
			if (heading >= 292.5f || heading <= 67.5f)
			{
				CaravanExitMapUtility.rotTmp[num++] = Rot4.North;
			}
			else if (heading >= 112.5f && heading <= 247.5f)
			{
				CaravanExitMapUtility.rotTmp[num++] = Rot4.South;
			}
			if (heading >= 22.5f && heading <= 157.5f)
			{
				CaravanExitMapUtility.rotTmp[num++] = Rot4.East;
			}
			else if (heading >= 202.5f && heading <= 337.5f)
			{
				CaravanExitMapUtility.rotTmp[num++] = Rot4.West;
			}
			Array.Sort<Rot4>(CaravanExitMapUtility.rotTmp, (Rot4 r1, Rot4 r2) => Mathf.Abs(r1.AsAngle - heading).CompareTo(Mathf.Abs(r2.AsAngle - heading)));
			if (num >= 1)
			{
				primary = CaravanExitMapUtility.rotTmp[0];
			}
			if (num >= 2)
			{
				secondary = CaravanExitMapUtility.rotTmp[1];
			}
		}

		// Token: 0x06008C38 RID: 35896 RVA: 0x00324FB0 File Offset: 0x003231B0
		public static int RandomBestExitTileFrom(Map map)
		{
			CaravanExitMapUtility.<>c__DisplayClass10_0 CS$<>8__locals1 = new CaravanExitMapUtility.<>c__DisplayClass10_0();
			Tile tileInfo = map.TileInfo;
			CS$<>8__locals1.options = CaravanExitMapUtility.AvailableExitTilesAt(map);
			if (!CS$<>8__locals1.options.Any<int>())
			{
				return -1;
			}
			CS$<>8__locals1.roads = tileInfo.Roads;
			if (CS$<>8__locals1.roads == null)
			{
				return CS$<>8__locals1.options.RandomElement<int>();
			}
			int bestRoadIndex = -1;
			for (int i = 0; i < CS$<>8__locals1.roads.Count; i++)
			{
				if (CS$<>8__locals1.options.Contains(CS$<>8__locals1.roads[i].neighbor) && (bestRoadIndex == -1 || CS$<>8__locals1.roads[i].road.priority > CS$<>8__locals1.roads[bestRoadIndex].road.priority))
				{
					bestRoadIndex = i;
				}
			}
			if (bestRoadIndex == -1)
			{
				return CS$<>8__locals1.options.RandomElement<int>();
			}
			return (from rl in CS$<>8__locals1.roads
			where CS$<>8__locals1.options.Contains(rl.neighbor) && rl.road == CS$<>8__locals1.roads[bestRoadIndex].road
			select rl).RandomElement<Tile.RoadLink>().neighbor;
		}

		// Token: 0x06008C39 RID: 35897 RVA: 0x003250EC File Offset: 0x003232EC
		public static int BestExitTileToGoTo(int destinationTile, Map from)
		{
			int num = -1;
			using (WorldPath worldPath = Find.WorldPathFinder.FindPath(from.Tile, destinationTile, null, null))
			{
				if (worldPath.Found && worldPath.NodesLeftCount >= 2)
				{
					num = worldPath.NodesReversed[worldPath.NodesReversed.Count - 2];
				}
			}
			if (num == -1)
			{
				return CaravanExitMapUtility.RandomBestExitTileFrom(from);
			}
			float num2 = 0f;
			int num3 = -1;
			List<int> list = CaravanExitMapUtility.AvailableExitTilesAt(from);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] == num)
				{
					return list[i];
				}
				float num4 = (Find.WorldGrid.GetTileCenter(list[i]) - Find.WorldGrid.GetTileCenter(num)).MagnitudeHorizontalSquared();
				if (num3 == -1 || num4 < num2)
				{
					num3 = list[i];
					num2 = num4;
				}
			}
			return num3;
		}

		// Token: 0x06008C3A RID: 35898 RVA: 0x003251E0 File Offset: 0x003233E0
		private static int FindRandomStartingTileBasedOnExitDir(int tileID, Rot4 exitDir)
		{
			CaravanExitMapUtility.tileCandidates.Clear();
			World world = Find.World;
			WorldGrid grid = world.grid;
			grid.GetTileNeighbors(tileID, CaravanExitMapUtility.tmpNeighbors);
			for (int i = 0; i < CaravanExitMapUtility.tmpNeighbors.Count; i++)
			{
				int num = CaravanExitMapUtility.tmpNeighbors[i];
				if (CaravanExitMapUtility.IsGoodCaravanStartingTile(num) && (!exitDir.IsValid || !(grid.GetRotFromTo(tileID, num) != exitDir)))
				{
					CaravanExitMapUtility.tileCandidates.Add(num);
				}
			}
			int result;
			if (CaravanExitMapUtility.tileCandidates.TryRandomElement(out result))
			{
				return result;
			}
			if (CaravanExitMapUtility.tmpNeighbors.Where(delegate(int x)
			{
				if (!CaravanExitMapUtility.IsGoodCaravanStartingTile(x))
				{
					return false;
				}
				Rot4 rotFromTo = grid.GetRotFromTo(tileID, x);
				return ((exitDir == Rot4.North || exitDir == Rot4.South) && (rotFromTo == Rot4.East || rotFromTo == Rot4.West)) || ((exitDir == Rot4.East || exitDir == Rot4.West) && (rotFromTo == Rot4.North || rotFromTo == Rot4.South));
			}).TryRandomElement(out result))
			{
				return result;
			}
			if ((from x in CaravanExitMapUtility.tmpNeighbors
			where CaravanExitMapUtility.IsGoodCaravanStartingTile(x)
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return tileID;
		}

		// Token: 0x06008C3B RID: 35899 RVA: 0x00325304 File Offset: 0x00323504
		private static int FindRandomStartingTileBasedOnExitDir(int tileID, Direction8Way exitDir)
		{
			CaravanExitMapUtility.tileCandidates.Clear();
			WorldGrid grid = Find.World.grid;
			grid.GetTileNeighbors(tileID, CaravanExitMapUtility.tmpNeighbors);
			for (int i = 0; i < CaravanExitMapUtility.tmpNeighbors.Count; i++)
			{
				int num = CaravanExitMapUtility.tmpNeighbors[i];
				if (CaravanExitMapUtility.IsGoodCaravanStartingTile(num) && grid.GetDirection8WayFromTo(tileID, num) == exitDir)
				{
					CaravanExitMapUtility.tileCandidates.Add(num);
				}
			}
			int result;
			if (CaravanExitMapUtility.tileCandidates.TryRandomElement(out result))
			{
				return result;
			}
			if ((from x in CaravanExitMapUtility.tmpNeighbors
			where CaravanExitMapUtility.IsGoodCaravanStartingTile(x)
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return tileID;
		}

		// Token: 0x06008C3C RID: 35900 RVA: 0x003253B6 File Offset: 0x003235B6
		private static bool IsGoodCaravanStartingTile(int tile)
		{
			return !Find.World.Impassable(tile);
		}

		// Token: 0x06008C3D RID: 35901 RVA: 0x003253C8 File Offset: 0x003235C8
		public static Caravan FindCaravanToJoinFor(Pawn pawn)
		{
			if (pawn.Faction != Faction.OfPlayer && pawn.HostFaction != Faction.OfPlayer)
			{
				return null;
			}
			if (!pawn.Spawned || !pawn.CanReachMapEdge())
			{
				return null;
			}
			int tile = pawn.Map.Tile;
			Find.WorldGrid.GetTileNeighbors(tile, CaravanExitMapUtility.tmpNeighbors);
			CaravanExitMapUtility.tmpNeighbors.Add(tile);
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = 0; i < caravans.Count; i++)
			{
				Caravan caravan = caravans[i];
				if (CaravanExitMapUtility.tmpNeighbors.Contains(caravan.Tile) && caravan.autoJoinable)
				{
					if (pawn.HostFaction == null)
					{
						if (caravan.Faction == pawn.Faction)
						{
							return caravan;
						}
					}
					else if (caravan.Faction == pawn.HostFaction)
					{
						return caravan;
					}
				}
			}
			return null;
		}

		// Token: 0x06008C3E RID: 35902 RVA: 0x00325494 File Offset: 0x00323694
		public static bool AnyoneTryingToJoinCaravan(Caravan c)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (!map.IsPlayerHome && Find.WorldGrid.IsNeighborOrSame(c.Tile, map.Tile))
				{
					List<Pawn> allPawnsSpawned = map.mapPawns.AllPawnsSpawned;
					for (int j = 0; j < allPawnsSpawned.Count; j++)
					{
						if (!allPawnsSpawned[j].IsColonistPlayerControlled && !allPawnsSpawned[j].Downed && CaravanExitMapUtility.FindCaravanToJoinFor(allPawnsSpawned[j]) == c)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06008C3F RID: 35903 RVA: 0x00325535 File Offset: 0x00323735
		public static void OpenSomeoneTryingToJoinCaravanDialog(Caravan c, Action confirmAction)
		{
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmMoveAutoJoinableCaravan".Translate(), confirmAction, false, null));
		}

		// Token: 0x06008C40 RID: 35904 RVA: 0x00325554 File Offset: 0x00323754
		private static void AddCaravanExitTaleIfShould(Pawn pawn)
		{
			if (pawn.Spawned && pawn.IsFreeColonist)
			{
				if (pawn.Map.IsPlayerHome)
				{
					TaleRecorder.RecordTale(TaleDefOf.CaravanFormed, new object[]
					{
						pawn
					});
					return;
				}
				if (GenHostility.AnyHostileActiveThreatToPlayer(pawn.Map, false, true))
				{
					TaleRecorder.RecordTale(TaleDefOf.CaravanFled, new object[]
					{
						pawn
					});
				}
			}
		}

		// Token: 0x0400590C RID: 22796
		private static List<int> tmpNeighbors = new List<int>();

		// Token: 0x0400590D RID: 22797
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x0400590E RID: 22798
		private static List<int> retTiles = new List<int>();

		// Token: 0x0400590F RID: 22799
		private static readonly Rot4[] rotTmp = new Rot4[2];

		// Token: 0x04005910 RID: 22800
		private static List<int> tileCandidates = new List<int>();
	}
}
