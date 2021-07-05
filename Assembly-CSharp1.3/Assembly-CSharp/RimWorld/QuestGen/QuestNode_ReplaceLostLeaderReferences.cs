using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020016DB RID: 5851
	public class QuestNode_ReplaceLostLeaderReferences : QuestNode
	{
		// Token: 0x0600874F RID: 34639 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06008750 RID: 34640 RVA: 0x00306E74 File Offset: 0x00305074
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ReplaceLostLeaderReferences questPart_ReplaceLostLeaderReferences = new QuestPart_ReplaceLostLeaderReferences();
			questPart_ReplaceLostLeaderReferences.inSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate));
			questPart_ReplaceLostLeaderReferences.signalListenMode = (this.signalListenMode.GetValue(slate) ?? QuestPart.SignalListenMode.OngoingOnly);
			QuestGen.quest.AddPart(questPart_ReplaceLostLeaderReferences);
		}

		// Token: 0x0400556B RID: 21867
		public SlateRef<string> inSignal;

		// Token: 0x0400556C RID: 21868
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;
	}
}
