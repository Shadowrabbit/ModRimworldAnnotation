using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CCA RID: 3274
	public static class StealAIDebugDrawer
	{
		// Token: 0x06004BB2 RID: 19378 RVA: 0x001A673C File Offset: 0x001A493C
		public static void DebugDraw()
		{
			if (!DebugViewSettings.drawStealDebug)
			{
				StealAIDebugDrawer.debugDrawLord = null;
				return;
			}
			Lord lord = StealAIDebugDrawer.debugDrawLord;
			StealAIDebugDrawer.debugDrawLord = StealAIDebugDrawer.FindHostileLord();
			if (StealAIDebugDrawer.debugDrawLord == null)
			{
				return;
			}
			StealAIDebugDrawer.CheckInitDebugDrawGrid();
			float num = StealAIUtility.StartStealingMarketValueThreshold(StealAIDebugDrawer.debugDrawLord);
			if (lord != StealAIDebugDrawer.debugDrawLord)
			{
				foreach (IntVec3 intVec in Find.CurrentMap.AllCells)
				{
					StealAIDebugDrawer.debugDrawGrid[intVec] = (StealAIDebugDrawer.TotalMarketValueAround(intVec, Find.CurrentMap, StealAIDebugDrawer.debugDrawLord.ownedPawns.Count) > num);
				}
			}
			foreach (IntVec3 c in Find.CurrentMap.AllCells)
			{
				if (StealAIDebugDrawer.debugDrawGrid[c])
				{
					CellRenderer.RenderCell(c, 0.5f);
				}
			}
			StealAIDebugDrawer.tmpToSteal.Clear();
			for (int i = 0; i < StealAIDebugDrawer.debugDrawLord.ownedPawns.Count; i++)
			{
				Pawn pawn = StealAIDebugDrawer.debugDrawLord.ownedPawns[i];
				Thing thing;
				if (StealAIUtility.TryFindBestItemToSteal(pawn.Position, pawn.Map, 7f, out thing, pawn, StealAIDebugDrawer.tmpToSteal))
				{
					GenDraw.DrawLineBetween(pawn.TrueCenter(), thing.TrueCenter());
					StealAIDebugDrawer.tmpToSteal.Add(thing);
				}
			}
			StealAIDebugDrawer.tmpToSteal.Clear();
		}

		// Token: 0x06004BB3 RID: 19379 RVA: 0x001A68C8 File Offset: 0x001A4AC8
		public static void Notify_ThingChanged(Thing thing)
		{
			if (StealAIDebugDrawer.debugDrawLord == null)
			{
				return;
			}
			StealAIDebugDrawer.CheckInitDebugDrawGrid();
			if (thing.def.category != ThingCategory.Building && thing.def.category != ThingCategory.Item && thing.def.passability != Traversability.Impassable)
			{
				return;
			}
			if (thing.def.passability == Traversability.Impassable)
			{
				StealAIDebugDrawer.debugDrawLord = null;
				return;
			}
			int num = GenRadial.NumCellsInRadius(8f);
			float num2 = StealAIUtility.StartStealingMarketValueThreshold(StealAIDebugDrawer.debugDrawLord);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = thing.Position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(thing.Map))
				{
					StealAIDebugDrawer.debugDrawGrid[intVec] = (StealAIDebugDrawer.TotalMarketValueAround(intVec, Find.CurrentMap, StealAIDebugDrawer.debugDrawLord.ownedPawns.Count) > num2);
				}
			}
		}

		// Token: 0x06004BB4 RID: 19380 RVA: 0x001A6994 File Offset: 0x001A4B94
		private static float TotalMarketValueAround(IntVec3 center, Map map, int pawnsCount)
		{
			if (center.Impassable(map))
			{
				return 0f;
			}
			float num = 0f;
			StealAIDebugDrawer.tmpToSteal.Clear();
			for (int i = 0; i < pawnsCount; i++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[i];
				if (!intVec.InBounds(map) || intVec.Impassable(map) || !GenSight.LineOfSight(center, intVec, map, false, null, 0, 0))
				{
					intVec = center;
				}
				Thing thing;
				if (StealAIUtility.TryFindBestItemToSteal(intVec, map, 7f, out thing, null, StealAIDebugDrawer.tmpToSteal))
				{
					num += StealAIUtility.GetValue(thing);
					StealAIDebugDrawer.tmpToSteal.Add(thing);
				}
			}
			StealAIDebugDrawer.tmpToSteal.Clear();
			return num;
		}

		// Token: 0x06004BB5 RID: 19381 RVA: 0x001A6A38 File Offset: 0x001A4C38
		private static Lord FindHostileLord()
		{
			Lord lord = null;
			List<Lord> lords = Find.CurrentMap.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				if (lords[i].faction != null && lords[i].faction.HostileTo(Faction.OfPlayer) && (lord == null || lords[i].ownedPawns.Count > lord.ownedPawns.Count))
				{
					lord = lords[i];
				}
			}
			return lord;
		}

		// Token: 0x06004BB6 RID: 19382 RVA: 0x00035EA3 File Offset: 0x000340A3
		private static void CheckInitDebugDrawGrid()
		{
			if (StealAIDebugDrawer.debugDrawGrid == null)
			{
				StealAIDebugDrawer.debugDrawGrid = new BoolGrid(Find.CurrentMap);
				return;
			}
			if (!StealAIDebugDrawer.debugDrawGrid.MapSizeMatches(Find.CurrentMap))
			{
				StealAIDebugDrawer.debugDrawGrid.ClearAndResizeTo(Find.CurrentMap);
			}
		}

		// Token: 0x040031F3 RID: 12787
		private static List<Thing> tmpToSteal = new List<Thing>();

		// Token: 0x040031F4 RID: 12788
		private static BoolGrid debugDrawGrid;

		// Token: 0x040031F5 RID: 12789
		private static Lord debugDrawLord = null;
	}
}
