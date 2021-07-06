using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020021E9 RID: 8681
	public static class WorldObjectSelectionUtility
	{
		// Token: 0x0600B9E9 RID: 47593 RVA: 0x0007871F File Offset: 0x0007691F
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

		// Token: 0x0600B9EA RID: 47594 RVA: 0x0007872F File Offset: 0x0007692F
		public static bool HiddenBehindTerrainNow(this WorldObject o)
		{
			return WorldRendererUtility.HiddenBehindTerrainNow(o.DrawPos);
		}

		// Token: 0x0600B9EB RID: 47595 RVA: 0x0007873C File Offset: 0x0007693C
		public static Vector2 ScreenPos(this WorldObject o)
		{
			return GenWorldUI.WorldToUIPosition(o.DrawPos);
		}

		// Token: 0x0600B9EC RID: 47596 RVA: 0x00357924 File Offset: 0x00355B24
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

		// Token: 0x0600B9ED RID: 47597 RVA: 0x00357970 File Offset: 0x00355B70
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
