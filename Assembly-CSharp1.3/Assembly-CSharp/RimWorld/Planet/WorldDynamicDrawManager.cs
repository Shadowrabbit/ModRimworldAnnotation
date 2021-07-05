using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001758 RID: 5976
	public class WorldDynamicDrawManager
	{
		// Token: 0x060089EF RID: 35311 RVA: 0x00318A83 File Offset: 0x00316C83
		public void RegisterDrawable(WorldObject o)
		{
			if (o.def.useDynamicDrawer)
			{
				if (this.drawingNow)
				{
					Log.Warning("Cannot register drawable " + o + " while drawing is in progress. WorldObjects shouldn't be spawned in Draw methods.");
				}
				this.drawObjects.Add(o);
			}
		}

		// Token: 0x060089F0 RID: 35312 RVA: 0x00318ABC File Offset: 0x00316CBC
		public void DeRegisterDrawable(WorldObject o)
		{
			if (o.def.useDynamicDrawer)
			{
				if (this.drawingNow)
				{
					Log.Warning("Cannot deregister drawable " + o + " while drawing is in progress. WorldObjects shouldn't be despawned in Draw methods.");
				}
				this.drawObjects.Remove(o);
			}
		}

		// Token: 0x060089F1 RID: 35313 RVA: 0x00318AF8 File Offset: 0x00316CF8
		public void DrawDynamicWorldObjects()
		{
			this.drawingNow = true;
			try
			{
				foreach (WorldObject worldObject in this.drawObjects)
				{
					try
					{
						if (!worldObject.def.expandingIcon || ExpandableWorldObjectsUtility.TransitionPct < 1f)
						{
							worldObject.Draw();
						}
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Exception drawing ",
							worldObject,
							": ",
							ex
						}));
					}
				}
			}
			catch (Exception arg)
			{
				Log.Error("Exception drawing dynamic world objects: " + arg);
			}
			this.drawingNow = false;
		}

		// Token: 0x040057B4 RID: 22452
		private HashSet<WorldObject> drawObjects = new HashSet<WorldObject>();

		// Token: 0x040057B5 RID: 22453
		private bool drawingNow;
	}
}
