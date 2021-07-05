using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200043F RID: 1087
	public static class UnityGUIBugsFixer
	{
		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x06002088 RID: 8328 RVA: 0x000CA56C File Offset: 0x000C876C
		public static List<Resolution> ScreenResolutionsWithoutDuplicates
		{
			get
			{
				UnityGUIBugsFixer.resolutions.Clear();
				Resolution[] array = Screen.resolutions;
				for (int i = 0; i < array.Length; i++)
				{
					bool flag = false;
					for (int j = 0; j < UnityGUIBugsFixer.resolutions.Count; j++)
					{
						if (UnityGUIBugsFixer.resolutions[j].width == array[i].width && UnityGUIBugsFixer.resolutions[j].height == array[i].height)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						UnityGUIBugsFixer.resolutions.Add(array[i]);
					}
				}
				return UnityGUIBugsFixer.resolutions;
			}
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06002089 RID: 8329 RVA: 0x000CA60F File Offset: 0x000C880F
		public static Vector2 CurrentEventDelta
		{
			get
			{
				return UnityGUIBugsFixer.currentEventDelta;
			}
		}

		// Token: 0x0600208A RID: 8330 RVA: 0x000CA616 File Offset: 0x000C8816
		public static void OnGUI()
		{
			UnityGUIBugsFixer.FixScrolling();
			UnityGUIBugsFixer.FixShift();
			UnityGUIBugsFixer.FixDelta();
		}

		// Token: 0x0600208B RID: 8331 RVA: 0x000CA628 File Offset: 0x000C8828
		private static void FixScrolling()
		{
			if (Event.current.type == EventType.ScrollWheel && (Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.LinuxPlayer))
			{
				Vector2 delta = Event.current.delta;
				Event.current.delta = new Vector2(delta.x, delta.y * 6f);
			}
		}

		// Token: 0x0600208C RID: 8332 RVA: 0x000CA680 File Offset: 0x000C8880
		private static void FixShift()
		{
			if ((Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.LinuxPlayer) && !Event.current.shift)
			{
				Event.current.shift = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
			}
		}

		// Token: 0x0600208D RID: 8333 RVA: 0x000CA6CE File Offset: 0x000C88CE
		public static bool ResolutionsEqual(IntVec2 a, IntVec2 b)
		{
			return a == b;
		}

		// Token: 0x0600208E RID: 8334 RVA: 0x000CA6D8 File Offset: 0x000C88D8
		private static void FixDelta()
		{
			Vector2 vector = UI.GUIToScreenPoint(Event.current.mousePosition);
			if (Event.current.rawType == EventType.MouseDrag)
			{
				if (vector != UnityGUIBugsFixer.lastMousePosition || Time.frameCount != UnityGUIBugsFixer.lastMousePositionFrame)
				{
					if (UnityGUIBugsFixer.lastMousePosition != null)
					{
						UnityGUIBugsFixer.currentEventDelta = vector - UnityGUIBugsFixer.lastMousePosition.Value;
					}
					else
					{
						UnityGUIBugsFixer.currentEventDelta = default(Vector2);
					}
					UnityGUIBugsFixer.lastMousePosition = new Vector2?(vector);
					UnityGUIBugsFixer.lastMousePositionFrame = Time.frameCount;
					return;
				}
			}
			else
			{
				UnityGUIBugsFixer.currentEventDelta = Event.current.delta;
				if (Event.current.rawType == EventType.MouseDown)
				{
					UnityGUIBugsFixer.lastMousePosition = new Vector2?(vector);
					UnityGUIBugsFixer.lastMousePositionFrame = Time.frameCount;
					return;
				}
				if (Event.current.rawType == EventType.MouseUp)
				{
					UnityGUIBugsFixer.lastMousePosition = null;
				}
			}
		}

		// Token: 0x04001433 RID: 5171
		private static List<Resolution> resolutions = new List<Resolution>();

		// Token: 0x04001434 RID: 5172
		private static Vector2 currentEventDelta;

		// Token: 0x04001435 RID: 5173
		private static int lastMousePositionFrame;

		// Token: 0x04001436 RID: 5174
		private const float ScrollFactor = 6f;

		// Token: 0x04001437 RID: 5175
		private static Vector2? lastMousePosition;
	}
}
