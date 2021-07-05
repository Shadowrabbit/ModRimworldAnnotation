using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200176A RID: 5994
	public static class ExpandableWorldObjectsUtility
	{
		// Token: 0x17001690 RID: 5776
		// (get) Token: 0x06008A46 RID: 35398 RVA: 0x00319BCF File Offset: 0x00317DCF
		public static float TransitionPct
		{
			get
			{
				if (!Find.PlaySettings.showExpandingIcons)
				{
					return 0f;
				}
				return ExpandableWorldObjectsUtility.transitionPct;
			}
		}

		// Token: 0x17001691 RID: 5777
		// (get) Token: 0x06008A47 RID: 35399 RVA: 0x00319BE8 File Offset: 0x00317DE8
		public static float ExpandMoreTransitionPct
		{
			get
			{
				if (!Find.PlaySettings.showExpandingIcons)
				{
					return 0f;
				}
				return ExpandableWorldObjectsUtility.expandMoreTransitionPct;
			}
		}

		// Token: 0x06008A48 RID: 35400 RVA: 0x00319C04 File Offset: 0x00317E04
		public static void ExpandableWorldObjectsUpdate()
		{
			float num = Time.deltaTime * 3f;
			if (Find.WorldCameraDriver.CurrentZoom <= WorldCameraZoomRange.VeryClose)
			{
				ExpandableWorldObjectsUtility.transitionPct -= num;
			}
			else
			{
				ExpandableWorldObjectsUtility.transitionPct += num;
			}
			ExpandableWorldObjectsUtility.transitionPct = Mathf.Clamp01(ExpandableWorldObjectsUtility.transitionPct);
			float num2 = Time.deltaTime * 4f;
			if (Find.WorldCameraDriver.CurrentZoom <= WorldCameraZoomRange.Far)
			{
				ExpandableWorldObjectsUtility.expandMoreTransitionPct -= num2;
			}
			else
			{
				ExpandableWorldObjectsUtility.expandMoreTransitionPct += num2;
			}
			ExpandableWorldObjectsUtility.expandMoreTransitionPct = Mathf.Clamp01(ExpandableWorldObjectsUtility.expandMoreTransitionPct);
		}

		// Token: 0x06008A49 RID: 35401 RVA: 0x00319C98 File Offset: 0x00317E98
		public static void ExpandableWorldObjectsOnGUI()
		{
			if (ExpandableWorldObjectsUtility.TransitionPct == 0f)
			{
				return;
			}
			ExpandableWorldObjectsUtility.tmpWorldObjects.Clear();
			ExpandableWorldObjectsUtility.tmpWorldObjects.AddRange(Find.WorldObjects.AllWorldObjects);
			ExpandableWorldObjectsUtility.SortByExpandingIconPriority(ExpandableWorldObjectsUtility.tmpWorldObjects);
			WorldTargeter worldTargeter = Find.WorldTargeter;
			List<WorldObject> worldObjectsUnderMouse = null;
			if (worldTargeter.IsTargeting)
			{
				worldObjectsUnderMouse = GenWorldUI.WorldObjectsUnderMouse(UI.MousePositionOnUI);
			}
			for (int i = 0; i < ExpandableWorldObjectsUtility.tmpWorldObjects.Count; i++)
			{
				try
				{
					WorldObject worldObject = ExpandableWorldObjectsUtility.tmpWorldObjects[i];
					if (worldObject.def.expandingIcon)
					{
						if (!worldObject.HiddenBehindTerrainNow())
						{
							Color expandingIconColor = worldObject.ExpandingIconColor;
							expandingIconColor.a = ExpandableWorldObjectsUtility.TransitionPct;
							if (worldTargeter.IsTargetedNow(worldObject, worldObjectsUnderMouse))
							{
								float num = GenMath.LerpDouble(-1f, 1f, 0.7f, 1f, Mathf.Sin(Time.time * 8f));
								expandingIconColor.r *= num;
								expandingIconColor.g *= num;
								expandingIconColor.b *= num;
							}
							GUI.color = expandingIconColor;
							Rect rect = ExpandableWorldObjectsUtility.ExpandedIconScreenRect(worldObject);
							if (worldObject.ExpandingIconFlipHorizontal)
							{
								rect.x = rect.xMax;
								rect.width *= -1f;
							}
							Widgets.DrawTextureRotated(rect, worldObject.ExpandingIcon, worldObject.ExpandingIconRotation);
						}
					}
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Error while drawing ",
						ExpandableWorldObjectsUtility.tmpWorldObjects[i].ToStringSafe<WorldObject>(),
						": ",
						ex
					}));
				}
			}
			ExpandableWorldObjectsUtility.tmpWorldObjects.Clear();
			GUI.color = Color.white;
		}

		// Token: 0x06008A4A RID: 35402 RVA: 0x00319E5C File Offset: 0x0031805C
		public static Rect ExpandedIconScreenRect(WorldObject o)
		{
			Vector2 vector = o.ScreenPos();
			float num;
			if (o.ExpandMore)
			{
				num = Mathf.Lerp(30f * o.def.expandingIconDrawSize, 30f * o.def.expandingIconDrawSize * 1.35f, ExpandableWorldObjectsUtility.ExpandMoreTransitionPct);
			}
			else
			{
				num = 30f * o.def.expandingIconDrawSize;
			}
			return new Rect(vector.x - num / 2f, vector.y - num / 2f, num, num);
		}

		// Token: 0x06008A4B RID: 35403 RVA: 0x00319EE2 File Offset: 0x003180E2
		public static bool IsExpanded(WorldObject o)
		{
			return ExpandableWorldObjectsUtility.TransitionPct > 0.5f && o.def.expandingIcon;
		}

		// Token: 0x06008A4C RID: 35404 RVA: 0x00319F00 File Offset: 0x00318100
		public static void GetExpandedWorldObjectUnderMouse(Vector2 mousePos, List<WorldObject> outList)
		{
			outList.Clear();
			Vector2 vector = mousePos;
			vector.y = (float)UI.screenHeight - vector.y;
			List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				WorldObject worldObject = allWorldObjects[i];
				if (ExpandableWorldObjectsUtility.IsExpanded(worldObject) && ExpandableWorldObjectsUtility.ExpandedIconScreenRect(worldObject).Contains(vector) && !worldObject.HiddenBehindTerrainNow())
				{
					outList.Add(worldObject);
				}
			}
			ExpandableWorldObjectsUtility.SortByExpandingIconPriority(outList);
			outList.Reverse();
		}

		// Token: 0x06008A4D RID: 35405 RVA: 0x00319F84 File Offset: 0x00318184
		private static void SortByExpandingIconPriority(List<WorldObject> worldObjects)
		{
			worldObjects.SortBy(delegate(WorldObject x)
			{
				float num = x.ExpandingIconPriority;
				if (x.Faction != null && x.Faction.IsPlayer)
				{
					num += 0.001f;
				}
				return num;
			}, (WorldObject x) => x.ID);
		}

		// Token: 0x040057DF RID: 22495
		private static float transitionPct;

		// Token: 0x040057E0 RID: 22496
		private static float expandMoreTransitionPct;

		// Token: 0x040057E1 RID: 22497
		private static List<WorldObject> tmpWorldObjects = new List<WorldObject>();

		// Token: 0x040057E2 RID: 22498
		private const float WorldObjectIconSize = 30f;

		// Token: 0x040057E3 RID: 22499
		private const float ExpandMoreWorldObjectIconSizeFactor = 1.35f;

		// Token: 0x040057E4 RID: 22500
		private const float TransitionSpeed = 3f;

		// Token: 0x040057E5 RID: 22501
		private const float ExpandMoreTransitionSpeed = 4f;
	}
}
