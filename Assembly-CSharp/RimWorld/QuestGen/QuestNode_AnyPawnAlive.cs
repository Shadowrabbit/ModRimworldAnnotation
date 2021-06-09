using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FE4 RID: 8164
	public class QuestNode_AnyPawnAlive : QuestNode
	{
		// Token: 0x0600AD2B RID: 44331 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AD2C RID: 44332 RVA: 0x00326C10 File Offset: 0x00324E10
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

		// Token: 0x040076AD RID: 30381
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040076AE RID: 30382
		public SlateRef<List<Pawn>> pawns;

		// Token: 0x040076AF RID: 30383
		public QuestNode node;

		// Token: 0x040076B0 RID: 30384
		private const string OuterNodeCompletedSignal = "OuterNodeCompleted";
	}
}
