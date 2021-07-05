using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016D1 RID: 5841
	public class QuestNode_MoodBelow : QuestNode
	{
		// Token: 0x0600872F RID: 34607 RVA: 0x00306414 File Offset: 0x00304614
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x06008730 RID: 34608 RVA: 0x0030642C File Offset: 0x0030462C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_MoodBelow questPart_MoodBelow = new QuestPart_MoodBelow();
			questPart_MoodBelow.pawns.AddRange(this.pawns.GetValue(slate));
			questPart_MoodBelow.threshold = this.threshold.GetValue(slate);
			questPart_MoodBelow.minTicksBelowThreshold = 40000;
			questPart_MoodBelow.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_MoodBelow);
			}
			if (!this.outSignal.GetValue(slate).NullOrEmpty())
			{
				questPart_MoodBelow.outSignalsCompleted.Add(this.outSignal.GetValue(slate));
			}
			QuestGen.quest.AddPart(questPart_MoodBelow);
		}

		// Token: 0x04005538 RID: 21816
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04005539 RID: 21817
		[NoTranslate]
		public SlateRef<string> outSignal;

		// Token: 0x0400553A RID: 21818
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x0400553B RID: 21819
		public SlateRef<float> threshold;

		// Token: 0x0400553C RID: 21820
		public QuestNode node;

		// Token: 0x0400553D RID: 21821
		private const int MinTicksBelowMinMood = 40000;
	}
}
