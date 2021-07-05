using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003F3 RID: 1011
	public static class GUIEventFilterForOSX
	{
		// Token: 0x06001E67 RID: 7783 RVA: 0x000BE2F4 File Offset: 0x000BC4F4
		public static void CheckRejectGUIEvent()
		{
			if (UnityData.platform != RuntimePlatform.OSXPlayer)
			{
				return;
			}
			if (Event.current.type != EventType.MouseDown && Event.current.type != EventType.MouseUp)
			{
				return;
			}
			if (Time.frameCount != GUIEventFilterForOSX.lastRecordedFrame)
			{
				GUIEventFilterForOSX.eventsThisFrame.Clear();
				GUIEventFilterForOSX.lastRecordedFrame = Time.frameCount;
			}
			for (int i = 0; i < GUIEventFilterForOSX.eventsThisFrame.Count; i++)
			{
				if (GUIEventFilterForOSX.EventsAreEquivalent(GUIEventFilterForOSX.eventsThisFrame[i], Event.current))
				{
					GUIEventFilterForOSX.RejectEvent();
				}
			}
			GUIEventFilterForOSX.eventsThisFrame.Add(Event.current);
		}

		// Token: 0x06001E68 RID: 7784 RVA: 0x000BE384 File Offset: 0x000BC584
		private static bool EventsAreEquivalent(Event A, Event B)
		{
			return A.button == B.button && A.keyCode == B.keyCode && A.type == B.type;
		}

		// Token: 0x06001E69 RID: 7785 RVA: 0x000BE3B4 File Offset: 0x000BC5B4
		private static void RejectEvent()
		{
			if (DebugViewSettings.logInput)
			{
				Log.Message(string.Concat(new object[]
				{
					"Frame ",
					Time.frameCount,
					": REJECTED ",
					Event.current.ToStringFull()
				}));
			}
			Event.current.Use();
		}

		// Token: 0x0400127A RID: 4730
		private static List<Event> eventsThisFrame = new List<Event>();

		// Token: 0x0400127B RID: 4731
		private static int lastRecordedFrame = -1;
	}
}
