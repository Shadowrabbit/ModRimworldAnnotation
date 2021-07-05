using System;

namespace Verse.AI
{
	// Token: 0x0200065C RID: 1628
	public static class BreachingNoiseDebugDrawer
	{
		// Token: 0x06002E0B RID: 11787 RVA: 0x00113EE0 File Offset: 0x001120E0
		public static void DebugDrawNoise(BreachingGrid grid)
		{
			Map currentMap = Find.CurrentMap;
			BreachingNoiseDebugDrawer.CheckInitDebugDrawGrid(grid);
			foreach (IntVec3 c in currentMap.AllCells)
			{
				if (BreachingNoiseDebugDrawer.debugDrawGrid[c] > 0)
				{
					CellRenderer.RenderCell(c, (float)BreachingNoiseDebugDrawer.debugDrawGrid[c] / 100f);
				}
			}
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x00113F58 File Offset: 0x00112158
		private static void CheckInitDebugDrawGrid(BreachingGrid grid)
		{
			if (grid != BreachingNoiseDebugDrawer.debugGrid)
			{
				BreachingNoiseDebugDrawer.debugDrawGrid = null;
				BreachingNoiseDebugDrawer.debugGrid = grid;
			}
			if (BreachingNoiseDebugDrawer.debugDrawGrid == null)
			{
				BreachingNoiseDebugDrawer.debugDrawGrid = new IntGrid(grid.Map);
				BreachingNoiseDebugDrawer.debugDrawGrid.Clear(0);
				foreach (IntVec3 intVec in grid.Map.AllCells)
				{
					if (BreachingNoiseDebugDrawer.debugGrid.WithinNoise(intVec))
					{
						BreachingNoiseDebugDrawer.debugDrawGrid[intVec] = 1;
					}
				}
			}
		}

		// Token: 0x04001C53 RID: 7251
		private static BreachingGrid debugGrid;

		// Token: 0x04001C54 RID: 7252
		private static IntGrid debugDrawGrid;
	}
}
