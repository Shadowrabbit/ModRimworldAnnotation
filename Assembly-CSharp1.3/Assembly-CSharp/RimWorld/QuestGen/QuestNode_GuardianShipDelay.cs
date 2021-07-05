using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016C7 RID: 5831
	public class QuestNode_GuardianShipDelay : QuestNode
	{
		// Token: 0x06008711 RID: 34577 RVA: 0x00305B3C File Offset: 0x00303D3C
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x06008712 RID: 34578 RVA: 0x00305B54 File Offset: 0x00303D54
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_GuardianShipDelay questPart_GuardianShipDelay = new QuestPart_GuardianShipDelay();
			questPart_GuardianShipDelay.pawn = this.pawn.GetValue(slate);
			questPart_GuardianShipDelay.delayTicks = this.delayTicks.GetValue(slate);
			questPart_GuardianShipDelay.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_GuardianShipDelay.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			questPart_GuardianShipDelay.reactivatable = true;
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_GuardianShipDelay);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_GuardianShipDelay.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_GuardianShipDelay);
		}

		// Token: 0x04005502 RID: 21762
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04005503 RID: 21763
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x04005504 RID: 21764
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x04005505 RID: 21765
		public SlateRef<Pawn> pawn;

		// Token: 0x04005506 RID: 21766
		public SlateRef<int> delayTicks;

		// Token: 0x04005507 RID: 21767
		public QuestNode node;
	}
}
