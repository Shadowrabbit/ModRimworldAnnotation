using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FA9 RID: 8105
	public class QuestNode_MoodBelow : QuestNode
	{
		// Token: 0x0600AC3A RID: 44090 RVA: 0x0007083B File Offset: 0x0006EA3B
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0600AC3B RID: 44091 RVA: 0x003216D0 File Offset: 0x0031F8D0
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

		// Token: 0x040075A6 RID: 30118
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x040075A7 RID: 30119
		[NoTranslate]
		public SlateRef<string> outSignal;

		// Token: 0x040075A8 RID: 30120
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x040075A9 RID: 30121
		public SlateRef<float> threshold;

		// Token: 0x040075AA RID: 30122
		public QuestNode node;

		// Token: 0x040075AB RID: 30123
		private const int MinTicksBelowMinMood = 40000;
	}
}
