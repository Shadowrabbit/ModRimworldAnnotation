using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EB7 RID: 7863
	public static class QuestGen_Debug
	{
		// Token: 0x0600A8DD RID: 43229 RVA: 0x00313A6C File Offset: 0x00311C6C
		public static void Log(this Quest quest, string message, string inSignal = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			quest.AddPart(new QuestPart_Log
			{
				inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				signalListenMode = signalListenMode,
				message = message
			});
		}
	}
}
