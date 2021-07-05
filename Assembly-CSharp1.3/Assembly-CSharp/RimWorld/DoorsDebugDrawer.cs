using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001078 RID: 4216
	public static class DoorsDebugDrawer
	{
		// Token: 0x06006438 RID: 25656 RVA: 0x0021CE08 File Offset: 0x0021B008
		public static void DrawDebug()
		{
			if (!DebugViewSettings.drawDoorsDebug)
			{
				return;
			}
			CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
			List<Thing> list = Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
			for (int i = 0; i < list.Count; i++)
			{
				if (currentViewRect.Contains(list[i].Position))
				{
					Building_Door building_Door = list[i] as Building_Door;
					if (building_Door != null)
					{
						Color col;
						if (building_Door.FreePassage)
						{
							col = new Color(0f, 1f, 0f, 0.5f);
						}
						else
						{
							col = new Color(1f, 0f, 0f, 0.5f);
						}
						CellRenderer.RenderCell(building_Door.Position, SolidColorMaterials.SimpleSolidColorMaterial(col, false));
					}
				}
			}
		}
	}
}
