using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000722 RID: 1826
	public static class GUIEventFilterForOSX
	{
		// Token: 0x06002E0C RID: 11788 RVA: 0x00136764 File Offset: 0x00134964
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

		// Token: 0x06002E0D RID: 11789 RVA: 0x00024383 File Offset: 0x00022583
		private static bool EventsAreEquivalent(Event A, Event B)
		{
			return A.button == B.button && A.keyCode == B.keyCode && A.type == B.type;
		}

		// Token: 0x06002E0E RID: 11790 RVA: 0x001367F4 File Offset: 0x001349F4
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
				}), false);
			}
			Event.current.Use();
		}

		// Token: 0x04001F76 RID: 8054
		private static List<Event> eventsThisFrame = new List<Event>();

		// Token: 0x04001F77 RID: 8055
		private static int lastRecordedFrame = -1;
	}
}
