using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000253 RID: 595
	public sealed class DynamicDrawManager
	{
		// Token: 0x06000F25 RID: 3877 RVA: 0x0001157D File Offset: 0x0000F77D
		public DynamicDrawManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x00011597 File Offset: 0x0000F797
		public void RegisterDrawable(Thing t)
		{
			if (t.def.drawerType != DrawerType.None)
			{
				if (this.drawingNow)
				{
					Log.Warning("Cannot register drawable " + t + " while drawing is in progress. Things shouldn't be spawned in Draw methods.", false);
				}
				this.drawThings.Add(t);
			}
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x000115D1 File Offset: 0x0000F7D1
		public void DeRegisterDrawable(Thing t)
		{
			if (t.def.drawerType != DrawerType.None)
			{
				if (this.drawingNow)
				{
					Log.Warning("Cannot deregister drawable " + t + " while drawing is in progress. Things shouldn't be despawned in Draw methods.", false);
				}
				this.drawThings.Remove(t);
			}
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x000B53EC File Offset: 0x000B35EC
		public void DrawDynamicThings()
		{
			if (!DebugViewSettings.drawThingsDynamic)
			{
				return;
			}
			this.drawingNow = true;
			try
			{
				bool[] fogGrid = this.map.fogGrid.fogGrid;
				CellRect cellRect = Find.CameraDriver.CurrentViewRect;
				cellRect.ClipInsideMap(this.map);
				cellRect = cellRect.ExpandedBy(1);
				CellIndices cellIndices = this.map.cellIndices;
				foreach (Thing thing in this.drawThings)
				{
					IntVec3 position = thing.Position;
					if ((cellRect.Contains(position) || thing.def.drawOffscreen) && (!fogGrid[cellIndices.CellToIndex(position)] || thing.def.seeThroughFog) && (thing.def.hideAtSnowDepth >= 1f || this.map.snowGrid.GetDepth(position) <= thing.def.hideAtSnowDepth))
					{
						try
						{
							thing.Draw();
						}
						catch (Exception ex)
						{
							Log.Error(string.Concat(new object[]
							{
								"Exception drawing ",
								thing,
								": ",
								ex.ToString()
							}), false);
						}
					}
				}
			}
			catch (Exception arg)
			{
				Log.Error("Exception drawing dynamic things: " + arg, false);
			}
			this.drawingNow = false;
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x0001160B File Offset: 0x0000F80B
		public void LogDynamicDrawThings()
		{
			Log.Message(DebugLogsUtility.ThingListToUniqueCountString(this.drawThings), false);
		}

		// Token: 0x04000C6A RID: 3178
		private Map map;

		// Token: 0x04000C6B RID: 3179
		private HashSet<Thing> drawThings = new HashSet<Thing>();

		// Token: 0x04000C6C RID: 3180
		private bool drawingNow;
	}
}
