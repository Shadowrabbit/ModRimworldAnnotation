using System;
using System.Linq;
using System.Text;

namespace Verse.Sound
{
	// Token: 0x02000569 RID: 1385
	public static class DebugSoundEventsLog
	{
		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x060028D0 RID: 10448 RVA: 0x000F799C File Offset: 0x000F5B9C
		public static string EventsListingDebugString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (LogMessage logMessage in DebugSoundEventsLog.queue.Messages.Reverse<LogMessage>())
				{
					stringBuilder.AppendLine(logMessage.ToString());
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x060028D1 RID: 10449 RVA: 0x000F7A04 File Offset: 0x000F5C04
		public static void Notify_SoundEvent(SoundDef def, SoundInfo info)
		{
			if (!DebugViewSettings.writeSoundEventsRecord)
			{
				return;
			}
			string str;
			if (def == null)
			{
				str = "null: ";
			}
			else if (def.isUndefined)
			{
				str = "Undefined: ";
			}
			else
			{
				str = (def.sustain ? "SustainerSpawn: " : "OneShot: ");
			}
			string str2 = (def != null) ? def.defName : "null";
			DebugSoundEventsLog.CreateRecord(str + str2 + " - " + info.ToString());
		}

		// Token: 0x060028D2 RID: 10450 RVA: 0x000F7A78 File Offset: 0x000F5C78
		public static void Notify_SustainerEnded(Sustainer sustainer, SoundInfo info)
		{
			DebugSoundEventsLog.CreateRecord("SustainerEnd: " + sustainer.def.defName + " - " + info.ToString());
		}

		// Token: 0x060028D3 RID: 10451 RVA: 0x000F7AA6 File Offset: 0x000F5CA6
		private static void CreateRecord(string str)
		{
			DebugSoundEventsLog.queue.Enqueue(new LogMessage(str));
		}

		// Token: 0x04001959 RID: 6489
		private static LogMessageQueue queue = new LogMessageQueue();
	}
}
