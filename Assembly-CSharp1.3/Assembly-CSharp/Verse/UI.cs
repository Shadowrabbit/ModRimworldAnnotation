using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000034 RID: 52
	public static class UI
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000316 RID: 790 RVA: 0x000110F0 File Offset: 0x0000F2F0
		public static Vector2 MousePositionOnUI
		{
			get
			{
				return Input.mousePosition / Prefs.UIScale;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000317 RID: 791 RVA: 0x00011108 File Offset: 0x0000F308
		public static Vector2 MousePositionOnUIInverted
		{
			get
			{
				Vector2 mousePositionOnUI = UI.MousePositionOnUI;
				mousePositionOnUI.y = (float)UI.screenHeight - mousePositionOnUI.y;
				return mousePositionOnUI;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000318 RID: 792 RVA: 0x00011130 File Offset: 0x0000F330
		public static Vector2 MousePosUIInvertedUseEventIfCan
		{
			get
			{
				if (Event.current != null)
				{
					return UI.GUIToScreenPoint(Event.current.mousePosition);
				}
				return UI.MousePositionOnUIInverted;
			}
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00011150 File Offset: 0x0000F350
		public static void ApplyUIScale()
		{
			if (Prefs.UIScale == 1f)
			{
				UI.screenWidth = Screen.width;
				UI.screenHeight = Screen.height;
				return;
			}
			UI.screenWidth = Mathf.RoundToInt((float)Screen.width / Prefs.UIScale);
			UI.screenHeight = Mathf.RoundToInt((float)Screen.height / Prefs.UIScale);
			float uiscale = Prefs.UIScale;
			float uiscale2 = Prefs.UIScale;
			GUI.matrix = Matrix4x4.TRS(new Vector3(0f, 0f, 0f), Quaternion.identity, new Vector3(uiscale, uiscale2, 1f));
		}

		// Token: 0x0600031A RID: 794 RVA: 0x000111E5 File Offset: 0x0000F3E5
		public static void FocusControl(string controlName, Window window)
		{
			GUI.FocusControl(controlName);
			Find.WindowStack.Notify_ManuallySetFocus(window);
		}

		// Token: 0x0600031B RID: 795 RVA: 0x000111F8 File Offset: 0x0000F3F8
		public static void UnfocusCurrentControl()
		{
			GUI.FocusControl(null);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00011200 File Offset: 0x0000F400
		public static Vector2 GUIToScreenPoint(Vector2 guiPoint)
		{
			return GUIUtility.GUIToScreenPoint(guiPoint / Prefs.UIScale);
		}

		// Token: 0x0600031D RID: 797 RVA: 0x00011214 File Offset: 0x0000F414
		public static Rect GUIToScreenRect(Rect guiRect)
		{
			return new Rect
			{
				min = UI.GUIToScreenPoint(guiRect.min),
				max = UI.GUIToScreenPoint(guiRect.max)
			};
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00011250 File Offset: 0x0000F450
		public static void RotateAroundPivot(float angle, Vector2 center)
		{
			GUIUtility.RotateAroundPivot(angle, center * Prefs.UIScale);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00011264 File Offset: 0x0000F464
		public static Vector2 MapToUIPosition(this Vector3 v)
		{
			Vector3 vector = Find.Camera.WorldToScreenPoint(v) / Prefs.UIScale;
			return new Vector2(vector.x, (float)UI.screenHeight - vector.y);
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0001129F File Offset: 0x0000F49F
		public static Vector3 UIToMapPosition(float x, float y)
		{
			return UI.UIToMapPosition(new Vector2(x, y));
		}

		// Token: 0x06000321 RID: 801 RVA: 0x000112B0 File Offset: 0x0000F4B0
		public static Vector3 UIToMapPosition(Vector2 screenLoc)
		{
			Ray ray = Find.Camera.ScreenPointToRay(screenLoc * Prefs.UIScale);
			return new Vector3(ray.origin.x, 0f, ray.origin.z);
		}

		// Token: 0x06000322 RID: 802 RVA: 0x000112FA File Offset: 0x0000F4FA
		public static float CurUICellSize()
		{
			return (new Vector3(1f, 0f, 0f).MapToUIPosition() - new Vector3(0f, 0f, 0f).MapToUIPosition()).x;
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00011338 File Offset: 0x0000F538
		public static Vector3 MouseMapPosition()
		{
			return UI.UIToMapPosition(UI.MousePositionOnUI);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x00011344 File Offset: 0x0000F544
		public static IntVec3 MouseCell()
		{
			return UI.UIToMapPosition(UI.MousePositionOnUI).ToIntVec3();
		}

		// Token: 0x04000095 RID: 149
		public static int screenWidth;

		// Token: 0x04000096 RID: 150
		public static int screenHeight;
	}
}
