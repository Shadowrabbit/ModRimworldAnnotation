using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200181A RID: 6170
	public static class WorldObjectSelectionUtility
	{
		// Token: 0x06009098 RID: 37016 RVA: 0x0033DCF1 File Offset: 0x0033BEF1
		public static IEnumerable<WorldObject> MultiSelectableWorldObjectsInScreenRectDistinct(Rect rect)
		{
			List<WorldObject> allObjects = Find.WorldObjects.AllWorldObjects;
			int num;
			for (int i = 0; i < allObjects.Count; i = num + 1)
			{
				if (!allObjects[i].NeverMultiSelect && !allObjects[i].HiddenBehindTerrainNow())
				{
					if (ExpandableWorldObjectsUtility.IsExpanded(allObjects[i]))
					{
						if (rect.Overlaps(ExpandableWorldObjectsUtility.ExpandedIconScreenRect(allObjects[i])))
						{
							yield return allObjects[i];
						}
					}
					else if (rect.Contains(allObjects[i].ScreenPos()))
					{
						yield return allObjects[i];
					}
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06009099 RID: 37017 RVA: 0x0033DD01 File Offset: 0x0033BF01
		public static bool HiddenBehindTerrainNow(this WorldObject o)
		{
			return WorldRendererUtility.HiddenBehindTerrainNow(o.DrawPos);
		}

		// Token: 0x0600909A RID: 37018 RVA: 0x0033DD0E File Offset: 0x0033BF0E
		public static Vector2 ScreenPos(this WorldObject o)
		{
			return GenWorldUI.WorldToUIPosition(o.DrawPos);
		}

		// Token: 0x0600909B RID: 37019 RVA: 0x0033DD1C File Offset: 0x0033BF1C
		public static bool VisibleToCameraNow(this WorldObject o)
		{
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				return false;
			}
			if (o.HiddenBehindTerrainNow())
			{
				return false;
			}
			Vector2 point = o.ScreenPos();
			return new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight).Contains(point);
		}

		// Token: 0x0600909C RID: 37020 RVA: 0x0033DD68 File Offset: 0x0033BF68
		public static float DistanceToMouse(this WorldObject o, Vector2 mousePos)
		{
			Ray ray = Find.WorldCamera.ScreenPointToRay(mousePos * Prefs.UIScale);
			int worldLayerMask = WorldCameraManager.WorldLayerMask;
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit, 1500f, worldLayerMask))
			{
				return Vector3.Distance(raycastHit.point, o.DrawPos);
			}
			return Vector3.Cross(ray.direction, o.DrawPos - ray.origin).magnitude;
		}
	}
}
