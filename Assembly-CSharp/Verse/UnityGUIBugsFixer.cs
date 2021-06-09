using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000784 RID: 1924
	public static class UnityGUIBugsFixer
	{
		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06003036 RID: 12342 RVA: 0x0013EDCC File Offset: 0x0013CFCC
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

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06003037 RID: 12343 RVA: 0x00025F81 File Offset: 0x00024181
		public static Vector2 CurrentEventDelta
		{
			get
			{
				return UnityGUIBugsFixer.currentEventDelta;
			}
		}

		// Token: 0x06003038 RID: 12344 RVA: 0x00025F88 File Offset: 0x00024188
		public static void OnGUI()
		{
			UnityGUIBugsFixer.FixScrolling();
			UnityGUIBugsFixer.FixShift();
			UnityGUIBugsFixer.FixDelta();
		}

		// Token: 0x06003039 RID: 12345 RVA: 0x0013EE70 File Offset: 0x0013D070
		private static void FixScrolling()
		{
			if (Event.current.type == EventType.ScrollWheel && (Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.LinuxPlayer))
			{
				Vector2 delta = Event.current.delta;
				Event.current.delta = new Vector2(delta.x, delta.y * 6f);
			}
		}

		// Token: 0x0600303A RID: 12346 RVA: 0x0013EEC8 File Offset: 0x0013D0C8
		private static void FixShift()
		{
			if ((Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.LinuxPlayer) && !Event.current.shift)
			{
				Event.current.shift = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
			}
		}

		// Token: 0x0600303B RID: 12347 RVA: 0x00025F99 File Offset: 0x00024199
		public static bool ResolutionsEqual(IntVec2 a, IntVec2 b)
		{
			return a == b;
		}

		// Token: 0x0600303C RID: 12348 RVA: 0x0013EF18 File Offset: 0x0013D118
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

		// Token: 0x04002125 RID: 8485
		private static List<Resolution> resolutions = new List<Resolution>();

		// Token: 0x04002126 RID: 8486
		private static Vector2 currentEventDelta;

		// Token: 0x04002127 RID: 8487
		private static int lastMousePositionFrame;

		// Token: 0x04002128 RID: 8488
		private const float ScrollFactor = 6f;

		// Token: 0x04002129 RID: 8489
		private static Vector2? lastMousePosition;
	}
}
