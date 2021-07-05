using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000423 RID: 1059
	public class ThingOverlays
	{
		// Token: 0x06001FE2 RID: 8162 RVA: 0x000C58A0 File Offset: 0x000C3AA0
		public void ThingOverlaysOnGUI()
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
			List<Thing> list = Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.HasGUIOverlay);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (currentViewRect.Contains(thing.Position) && !Find.CurrentMap.fogGrid.IsFogged(thing.Position))
				{
					try
					{
						thing.DrawGUIOverlay();
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Exception drawing ThingOverlay for ",
							thing,
							": ",
							ex
						}));
					}
				}
			}
		}
	}
}
