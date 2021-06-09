using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200006B RID: 107
	public static class UI
	{
		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x00009CA2 File Offset: 0x00007EA2
		public static Vector2 MousePositionOnUI
		{
			get
			{
				return Input.mousePosition / Prefs.UIScale;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000449 RID: 1097 RVA: 0x000876E4 File Offset: 0x000858E4
		public static Vector2 MousePositionOnUIInverted
		{
			get
			{
				Vector2 mousePositionOnUI = UI.MousePositionOnUI;
				mousePositionOnUI.y = (float)UI.screenHeight - mousePositionOnUI.y;
				return mousePositionOnUI;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x00009CB8 File Offset: 0x00007EB8
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

		// Token: 0x0600044B RID: 1099 RVA: 0x0008770C File Offset: 0x0008590C
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

		// Token: 0x0600044C RID: 1100 RVA: 0x00009CD6 File Offset: 0x00007ED6
		public static void FocusControl(string controlName, Window window)
		{
			GUI.FocusControl(controlName);
			Find.WindowStack.Notify_ManuallySetFocus(window);
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00009CE9 File Offset: 0x00007EE9
		public static void UnfocusCurrentControl()
		{
			GUI.FocusControl(null);
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00009CF1 File Offset: 0x00007EF1
		public static Vector2 GUIToScreenPoint(Vector2 guiPoint)
		{
			return GUIUtility.GUIToScreenPoint(guiPoint / Prefs.UIScale);
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00009D03 File Offset: 0x00007F03
		public static void RotateAroundPivot(float angle, Vector2 center)
		{
			GUIUtility.RotateAroundPivot(angle, center * Prefs.UIScale);
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x000877A4 File Offset: 0x000859A4
		public static Vector2 MapToUIPosition(this Vector3 v)
		{
			Vector3 vector = Find.Camera.WorldToScreenPoint(v) / Prefs.UIScale;
			return new Vector2(vector.x, (float)UI.screenHeight - vector.y);
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00009D16 File Offset: 0x00007F16
		public static Vector3 UIToMapPosition(float x, float y)
		{
			return UI.UIToMapPosition(new Vector2(x, y));
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x000877E0 File Offset: 0x000859E0
		public static Vector3 UIToMapPosition(Vector2 screenLoc)
		{
			Ray ray = Find.Camera.ScreenPointToRay(screenLoc * Prefs.UIScale);
			return new Vector3(ray.origin.x, 0f, ray.origin.z);
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00009D24 File Offset: 0x00007F24
		public static float CurUICellSize()
		{
			return (new Vector3(1f, 0f, 0f).MapToUIPosition() - new Vector3(0f, 0f, 0f).MapToUIPosition()).x;
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00009D62 File Offset: 0x00007F62
		public static Vector3 MouseMapPosition()
		{
			return UI.UIToMapPosition(UI.MousePositionOnUI);
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00009D6E File Offset: 0x00007F6E
		public static IntVec3 MouseCell()
		{
			return UI.UIToMapPosition(UI.MousePositionOnUI).ToIntVec3();
		}

		// Token: 0x040001D8 RID: 472
		public static int screenWidth;

		// Token: 0x040001D9 RID: 473
		public static int screenHeight;
	}
}
