using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200041B RID: 1051
	[StaticConstructorOnStartup]
	public static class Messages
	{
		// Token: 0x06001F9A RID: 8090 RVA: 0x000C4684 File Offset: 0x000C2884
		public static void Update()
		{
			if (Current.ProgramState == ProgramState.Playing && Messages.mouseoverMessageIndex >= 0 && Messages.mouseoverMessageIndex < Messages.liveMessages.Count)
			{
				Messages.liveMessages[Messages.mouseoverMessageIndex].lookTargets.TryHighlight(true, true, false);
			}
			Messages.mouseoverMessageIndex = -1;
			Messages.liveMessages.RemoveAll((Message m) => m.Expired);
		}

		// Token: 0x06001F9B RID: 8091 RVA: 0x000C46FE File Offset: 0x000C28FE
		public static void Message(string text, LookTargets lookTargets, MessageTypeDef def, Quest quest, bool historical = true)
		{
			if (!Messages.AcceptsMessage(text, lookTargets))
			{
				return;
			}
			Messages.Message(new Message(text.CapitalizeFirst(), def, lookTargets, quest), historical);
		}

		// Token: 0x06001F9C RID: 8092 RVA: 0x000C471F File Offset: 0x000C291F
		public static void Message(string text, LookTargets lookTargets, MessageTypeDef def, bool historical = true)
		{
			if (!Messages.AcceptsMessage(text, lookTargets))
			{
				return;
			}
			Messages.Message(new Message(text.CapitalizeFirst(), def, lookTargets), historical);
		}

		// Token: 0x06001F9D RID: 8093 RVA: 0x000C473E File Offset: 0x000C293E
		public static void Message(string text, MessageTypeDef def, bool historical = true)
		{
			if (!Messages.AcceptsMessage(text, TargetInfo.Invalid))
			{
				return;
			}
			Messages.Message(new Message(text.CapitalizeFirst(), def), historical);
		}

		// Token: 0x06001F9E RID: 8094 RVA: 0x000C4768 File Offset: 0x000C2968
		public static void Message(Message msg, bool historical = true)
		{
			if (!Messages.AcceptsMessage(msg.text, msg.lookTargets))
			{
				return;
			}
			if (historical && Find.Archive != null)
			{
				Find.Archive.Add(msg);
			}
			Messages.liveMessages.Add(msg);
			while (Messages.liveMessages.Count > 12)
			{
				Messages.liveMessages.RemoveAt(0);
			}
			if (msg.def.sound != null)
			{
				msg.def.sound.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06001F9F RID: 8095 RVA: 0x000C47E3 File Offset: 0x000C29E3
		public static bool IsLive(Message msg)
		{
			return Messages.liveMessages.Contains(msg);
		}

		// Token: 0x06001FA0 RID: 8096 RVA: 0x000C47F0 File Offset: 0x000C29F0
		public static void MessagesDoGUI()
		{
			Text.Font = GameFont.Small;
			int xOffset = (int)Messages.MessagesTopLeftStandard.x;
			int num = (int)Messages.MessagesTopLeftStandard.y;
			if (Current.Game != null && Find.ActiveLesson.ActiveLessonVisible)
			{
				num += (int)Find.ActiveLesson.Current.MessagesYOffset;
			}
			for (int i = Messages.liveMessages.Count - 1; i >= 0; i--)
			{
				Messages.liveMessages[i].Draw(xOffset, num);
				num += 26;
			}
		}

		// Token: 0x06001FA1 RID: 8097 RVA: 0x000C4870 File Offset: 0x000C2A70
		public static bool CollidesWithAnyMessage(Rect rect, out float messageAlpha)
		{
			bool result = false;
			float num = 0f;
			for (int i = 0; i < Messages.liveMessages.Count; i++)
			{
				Message message = Messages.liveMessages[i];
				if (rect.Overlaps(message.lastDrawRect))
				{
					result = true;
					num = Mathf.Max(num, message.Alpha);
				}
			}
			messageAlpha = num;
			return result;
		}

		// Token: 0x06001FA2 RID: 8098 RVA: 0x000C48C8 File Offset: 0x000C2AC8
		public static void Clear()
		{
			Messages.liveMessages.Clear();
		}

		// Token: 0x06001FA3 RID: 8099 RVA: 0x000C48D4 File Offset: 0x000C2AD4
		public static void Notify_LoadedLevelChanged()
		{
			for (int i = 0; i < Messages.liveMessages.Count; i++)
			{
				Messages.liveMessages[i].lookTargets = null;
			}
		}

		// Token: 0x06001FA4 RID: 8100 RVA: 0x000C4908 File Offset: 0x000C2B08
		private static bool AcceptsMessage(string text, LookTargets lookTargets)
		{
			if (text.NullOrEmpty())
			{
				return false;
			}
			for (int i = 0; i < Messages.liveMessages.Count; i++)
			{
				if (Messages.liveMessages[i].text == text && Messages.liveMessages[i].startingFrame == RealTime.frameCount && LookTargets.SameTargets(Messages.liveMessages[i].lookTargets, lookTargets))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001FA5 RID: 8101 RVA: 0x000C497E File Offset: 0x000C2B7E
		public static void Notify_Mouseover(Message msg)
		{
			Messages.mouseoverMessageIndex = Messages.liveMessages.IndexOf(msg);
		}

		// Token: 0x04001332 RID: 4914
		private static List<Message> liveMessages = new List<Message>();

		// Token: 0x04001333 RID: 4915
		private static int mouseoverMessageIndex = -1;

		// Token: 0x04001334 RID: 4916
		public static readonly Vector2 MessagesTopLeftStandard = new Vector2(140f, 16f);

		// Token: 0x04001335 RID: 4917
		private const int MessageYInterval = 26;

		// Token: 0x04001336 RID: 4918
		private const int MaxLiveMessages = 12;
	}
}
