using System;
using System.Linq;
using System.Text;

namespace Verse.Sound
{
	// Token: 0x0200093F RID: 2367
	public static class DebugSoundEventsLog
	{
		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x06003A13 RID: 14867 RVA: 0x00168CCC File Offset: 0x00166ECC
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

		// Token: 0x06003A14 RID: 14868 RVA: 0x00168D34 File Offset: 0x00166F34
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

		// Token: 0x06003A15 RID: 14869 RVA: 0x0002CC1D File Offset: 0x0002AE1D
		public static void Notify_SustainerEnded(Sustainer sustainer, SoundInfo info)
		{
			DebugSoundEventsLog.CreateRecord("SustainerEnd: " + sustainer.def.defName + " - " + info.ToString());
		}

		// Token: 0x06003A16 RID: 14870 RVA: 0x0002CC4B File Offset: 0x0002AE4B
		private static void CreateRecord(string str)
		{
			DebugSoundEventsLog.queue.Enqueue(new LogMessage(str));
		}

		// Token: 0x04002855 RID: 10325
		private static LogMessageQueue queue = new LogMessageQueue();
	}
}
