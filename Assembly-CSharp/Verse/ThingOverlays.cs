using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000759 RID: 1881
	public class ThingOverlays
	{
		// Token: 0x06002F79 RID: 12153 RVA: 0x0013B430 File Offset: 0x00139630
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
						}), false);
					}
				}
			}
		}
	}
}
