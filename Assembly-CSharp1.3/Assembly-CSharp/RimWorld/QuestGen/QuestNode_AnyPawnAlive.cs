using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001710 RID: 5904
	public class QuestNode_AnyPawnAlive : QuestNode
	{
		// Token: 0x06008858 RID: 34904 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06008859 RID: 34905 RVA: 0x0030FF78 File Offset: 0x0030E178
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Filter_AnyPawnAlive questPart_Filter_AnyPawnAlive = new QuestPart_Filter_AnyPawnAlive
			{
				inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				pawns = this.pawns.GetValue(slate)
			};
			if (this.node != null)
			{
				questPart_Filter_AnyPawnAlive.outSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				QuestGenUtility.RunInnerNode(this.node, questPart_Filter_AnyPawnAlive.outSignal);
			}
			QuestGen.quest.AddPart(questPart_Filter_AnyPawnAlive);
		}

		// Token: 0x04005640 RID: 22080
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04005641 RID: 22081
		public SlateRef<List<Pawn>> pawns;

		// Token: 0x04005642 RID: 22082
		public QuestNode node;

		// Token: 0x04005643 RID: 22083
		private const string OuterNodeCompletedSignal = "OuterNodeCompleted";
	}
}
