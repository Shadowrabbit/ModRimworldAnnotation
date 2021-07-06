using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002067 RID: 8295
	public static class ExpandableWorldObjectsUtility
	{
		// Token: 0x17001A0A RID: 6666
		// (get) Token: 0x0600AFE9 RID: 45033 RVA: 0x000725DB File Offset: 0x000707DB
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

		// Token: 0x17001A0B RID: 6667
		// (get) Token: 0x0600AFEA RID: 45034 RVA: 0x000725F4 File Offset: 0x000707F4
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

		// Token: 0x0600AFEB RID: 45035 RVA: 0x0033155C File Offset: 0x0032F75C
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

		// Token: 0x0600AFEC RID: 45036 RVA: 0x003315F0 File Offset: 0x0032F7F0
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
					}), false);
				}
			}
			ExpandableWorldObjectsUtility.tmpWorldObjects.Clear();
			GUI.color = Color.white;
		}

		// Token: 0x0600AFED RID: 45037 RVA: 0x003317B4 File Offset: 0x0032F9B4
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

		// Token: 0x0600AFEE RID: 45038 RVA: 0x0007260D File Offset: 0x0007080D
		public static bool IsExpanded(WorldObject o)
		{
			return ExpandableWorldObjectsUtility.TransitionPct > 0.5f && o.def.expandingIcon;
		}

		// Token: 0x0600AFEF RID: 45039 RVA: 0x0033183C File Offset: 0x0032FA3C
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

		// Token: 0x0600AFF0 RID: 45040 RVA: 0x003318C0 File Offset: 0x0032FAC0
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

		// Token: 0x040078E9 RID: 30953
		private static float transitionPct;

		// Token: 0x040078EA RID: 30954
		private static float expandMoreTransitionPct;

		// Token: 0x040078EB RID: 30955
		private static List<WorldObject> tmpWorldObjects = new List<WorldObject>();

		// Token: 0x040078EC RID: 30956
		private const float WorldObjectIconSize = 30f;

		// Token: 0x040078ED RID: 30957
		private const float ExpandMoreWorldObjectIconSizeFactor = 1.35f;

		// Token: 0x040078EE RID: 30958
		private const float TransitionSpeed = 3f;

		// Token: 0x040078EF RID: 30959
		private const float ExpandMoreTransitionSpeed = 4f;
	}
}
