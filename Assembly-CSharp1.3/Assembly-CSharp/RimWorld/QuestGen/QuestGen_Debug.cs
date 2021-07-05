using System;

namespace RimWorld.QuestGen
{
	// Token: 0x0200160E RID: 5646
	public static class QuestGen_Debug
	{
		// Token: 0x06008451 RID: 33873 RVA: 0x002F6D24 File Offset: 0x002F4F24
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
