using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000191 RID: 401
	public sealed class DynamicDrawManager
	{
		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000B59 RID: 2905 RVA: 0x0003D7F9 File Offset: 0x0003B9F9
		public HashSet<Thing> DrawThingsForReading
		{
			get
			{
				return this.drawThings;
			}
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x0003D801 File Offset: 0x0003BA01
		public DynamicDrawManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x0003D81B File Offset: 0x0003BA1B
		public void RegisterDrawable(Thing t)
		{
			if (t.def.drawerType != DrawerType.None)
			{
				if (this.drawingNow)
				{
					Log.Warning("Cannot register drawable " + t + " while drawing is in progress. Things shouldn't be spawned in Draw methods.");
				}
				this.drawThings.Add(t);
			}
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x0003D854 File Offset: 0x0003BA54
		public void DeRegisterDrawable(Thing t)
		{
			if (t.def.drawerType != DrawerType.None)
			{
				if (this.drawingNow)
				{
					Log.Warning("Cannot deregister drawable " + t + " while drawing is in progress. Things shouldn't be despawned in Draw methods.");
				}
				this.drawThings.Remove(t);
			}
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x0003D890 File Offset: 0x0003BA90
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
							}));
						}
					}
				}
			}
			catch (Exception arg)
			{
				Log.Error("Exception drawing dynamic things: " + arg);
			}
			this.drawingNow = false;
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x0003DA38 File Offset: 0x0003BC38
		public void LogDynamicDrawThings()
		{
			Log.Message(DebugLogsUtility.ThingListToUniqueCountString(this.drawThings));
		}

		// Token: 0x0400095C RID: 2396
		private Map map;

		// Token: 0x0400095D RID: 2397
		private HashSet<Thing> drawThings = new HashSet<Thing>();

		// Token: 0x0400095E RID: 2398
		private bool drawingNow;
	}
}
