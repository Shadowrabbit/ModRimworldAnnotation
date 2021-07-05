using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007B7 RID: 1975
	public static class StealAIDebugDrawer
	{
		// Token: 0x06003583 RID: 13699 RVA: 0x0012E710 File Offset: 0x0012C910
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

		// Token: 0x06003584 RID: 13700 RVA: 0x0012E89C File Offset: 0x0012CA9C
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

		// Token: 0x06003585 RID: 13701 RVA: 0x0012E968 File Offset: 0x0012CB68
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

		// Token: 0x06003586 RID: 13702 RVA: 0x0012EA0C File Offset: 0x0012CC0C
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

		// Token: 0x06003587 RID: 13703 RVA: 0x0012EA8C File Offset: 0x0012CC8C
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

		// Token: 0x04001EA0 RID: 7840
		private static List<Thing> tmpToSteal = new List<Thing>();

		// Token: 0x04001EA1 RID: 7841
		private static BoolGrid debugDrawGrid;

		// Token: 0x04001EA2 RID: 7842
		private static Lord debugDrawLord = null;
	}
}
