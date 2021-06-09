using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002045 RID: 8261
	public class WorldDynamicDrawManager
	{
		// Token: 0x0600AF18 RID: 44824 RVA: 0x00072011 File Offset: 0x00070211
		public void RegisterDrawable(WorldObject o)
		{
			if (o.def.useDynamicDrawer)
			{
				if (this.drawingNow)
				{
					Log.Warning("Cannot register drawable " + o + " while drawing is in progress. WorldObjects shouldn't be spawned in Draw methods.", false);
				}
				this.drawObjects.Add(o);
			}
		}

		// Token: 0x0600AF19 RID: 44825 RVA: 0x0007204B File Offset: 0x0007024B
		public void DeRegisterDrawable(WorldObject o)
		{
			if (o.def.useDynamicDrawer)
			{
				if (this.drawingNow)
				{
					Log.Warning("Cannot deregister drawable " + o + " while drawing is in progress. WorldObjects shouldn't be despawned in Draw methods.", false);
				}
				this.drawObjects.Remove(o);
			}
		}

		// Token: 0x0600AF1A RID: 44826 RVA: 0x0032E828 File Offset: 0x0032CA28
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
						}), false);
					}
				}
			}
			catch (Exception arg)
			{
				Log.Error("Exception drawing dynamic world objects: " + arg, false);
			}
			this.drawingNow = false;
		}

		// Token: 0x04007861 RID: 30817
		private HashSet<WorldObject> drawObjects = new HashSet<WorldObject>();

		// Token: 0x04007862 RID: 30818
		private bool drawingNow;
	}
}
