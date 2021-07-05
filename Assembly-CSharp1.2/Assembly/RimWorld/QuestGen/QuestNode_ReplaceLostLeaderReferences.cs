using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FB2 RID: 8114
	public class QuestNode_ReplaceLostLeaderReferences : QuestNode
	{
		// Token: 0x0600AC57 RID: 44119 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AC58 RID: 44120 RVA: 0x00321F7C File Offset: 0x0032017C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ReplaceLostLeaderReferences questPart_ReplaceLostLeaderReferences = new QuestPart_ReplaceLostLeaderReferences();
			questPart_ReplaceLostLeaderReferences.inSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate));
			questPart_ReplaceLostLeaderReferences.signalListenMode = (this.signalListenMode.GetValue(slate) ?? QuestPart.SignalListenMode.OngoingOnly);
			QuestGen.quest.AddPart(questPart_ReplaceLostLeaderReferences);
		}

		// Token: 0x040075D4 RID: 30164
		public SlateRef<string> inSignal;

		// Token: 0x040075D5 RID: 30165
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;
	}
}
