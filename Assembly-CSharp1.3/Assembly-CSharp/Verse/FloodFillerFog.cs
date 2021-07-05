using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001A2 RID: 418
	public static class FloodFillerFog
	{
		// Token: 0x06000BB9 RID: 3001 RVA: 0x0003F790 File Offset: 0x0003D990
		public static FloodUnfogResult FloodUnfog(IntVec3 root, Map map)
		{
			FloodFillerFog.cellsToUnfog.Clear();
			FloodUnfogResult result = default(FloodUnfogResult);
			bool[] fogGridDirect = map.fogGrid.fogGrid;
			FogGrid fogGrid = map.fogGrid;
			List<IntVec3> newlyUnfoggedCells = new List<IntVec3>();
			int numUnfogged = 0;
			bool expanding = false;
			CellRect viewRect = CellRect.ViewRect(map);
			result.allOnScreen = true;
			Predicate<IntVec3> predicate = delegate(IntVec3 c)
			{
				if (!fogGridDirect[map.cellIndices.CellToIndex(c)])
				{
					return false;
				}
				Thing edifice = c.GetEdifice(map);
				return (edifice == null || !edifice.def.MakeFog) && (!FloodFillerFog.testMode || expanding || numUnfogged <= 500);
			};
			Action<IntVec3> processor = delegate(IntVec3 c)
			{
				fogGrid.Unfog(c);
				newlyUnfoggedCells.Add(c);
				List<Thing> thingList = c.GetThingList(map);
				for (int l = 0; l < thingList.Count; l++)
				{
					Pawn pawn = thingList[l] as Pawn;
					if (pawn != null)
					{
						pawn.mindState.Active = true;
						if (pawn.def.race.IsMechanoid)
						{
							result.mechanoidFound = true;
						}
					}
				}
				if (!viewRect.Contains(c))
				{
					result.allOnScreen = false;
				}
				result.cellsUnfogged++;
				if (FloodFillerFog.testMode)
				{
					int numUnfogged = numUnfogged;
					numUnfogged++;
					map.debugDrawer.FlashCell(c, (float)numUnfogged / 200f, numUnfogged.ToStringCached(), 50);
				}
			};
			map.floodFiller.FloodFill(root, predicate, processor, int.MaxValue, false, null);
			expanding = true;
			for (int i = 0; i < newlyUnfoggedCells.Count; i++)
			{
				IntVec3 a = newlyUnfoggedCells[i];
				for (int j = 0; j < 8; j++)
				{
					IntVec3 intVec = a + GenAdj.AdjacentCells[j];
					if (intVec.InBounds(map) && fogGrid.IsFogged(intVec) && !predicate(intVec))
					{
						FloodFillerFog.cellsToUnfog.Add(intVec);
					}
				}
			}
			for (int k = 0; k < FloodFillerFog.cellsToUnfog.Count; k++)
			{
				fogGrid.Unfog(FloodFillerFog.cellsToUnfog[k]);
				if (FloodFillerFog.testMode)
				{
					map.debugDrawer.FlashCell(FloodFillerFog.cellsToUnfog[k], 0.3f, "x", 50);
				}
			}
			FloodFillerFog.cellsToUnfog.Clear();
			return result;
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x0003F944 File Offset: 0x0003DB44
		public static void DebugFloodUnfog(IntVec3 root, Map map)
		{
			map.fogGrid.SetAllFogged();
			foreach (IntVec3 loc in map.AllCells)
			{
				map.mapDrawer.MapMeshDirty(loc, MapMeshFlag.FogOfWar);
			}
			FloodFillerFog.testMode = true;
			FloodFillerFog.FloodUnfog(root, map);
			FloodFillerFog.testMode = false;
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x0003F9B8 File Offset: 0x0003DBB8
		public static void DebugRefogMap(Map map)
		{
			map.fogGrid.SetAllFogged();
			foreach (IntVec3 loc in map.AllCells)
			{
				map.mapDrawer.MapMeshDirty(loc, MapMeshFlag.FogOfWar);
			}
			FloodFillerFog.FloodUnfog(map.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>().Position, map);
		}

		// Token: 0x040009BF RID: 2495
		private static bool testMode = false;

		// Token: 0x040009C0 RID: 2496
		private static List<IntVec3> cellsToUnfog = new List<IntVec3>(1024);

		// Token: 0x040009C1 RID: 2497
		private const int MaxNumTestUnfog = 500;
	}
}
