using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016E6 RID: 5862
	public class QuestNode_ShuttleDelay : QuestNode
	{
		// Token: 0x06008772 RID: 34674 RVA: 0x00307281 File Offset: 0x00305481
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x06008773 RID: 34675 RVA: 0x0030729C File Offset: 0x0030549C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ShuttleDelay questPart_ShuttleDelay = new QuestPart_ShuttleDelay();
			questPart_ShuttleDelay.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_ShuttleDelay.delayTicks = this.delayTicks.GetValue(slate);
			if (this.lodgers.GetValue(slate) != null)
			{
				questPart_ShuttleDelay.lodgers.AddRange(this.lodgers.GetValue(slate));
			}
			questPart_ShuttleDelay.expiryInfoPart = "ShuttleArrivesIn".Translate();
			questPart_ShuttleDelay.expiryInfoPartTip = "ShuttleArrivesOn".Translate();
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_ShuttleDelay);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_ShuttleDelay.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_ShuttleDelay);
		}

		// Token: 0x04005582 RID: 21890
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04005583 RID: 21891
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x04005584 RID: 21892
		public SlateRef<int> delayTicks;

		// Token: 0x04005585 RID: 21893
		public SlateRef<IEnumerable<Pawn>> lodgers;

		// Token: 0x04005586 RID: 21894
		public QuestNode node;
	}
}
