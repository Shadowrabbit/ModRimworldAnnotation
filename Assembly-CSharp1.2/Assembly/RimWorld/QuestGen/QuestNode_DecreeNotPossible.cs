using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FE5 RID: 8165
	public class QuestNode_DecreeNotPossible : QuestNode
	{
		// Token: 0x0600AD2E RID: 44334 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AD2F RID: 44335 RVA: 0x00326C9C File Offset: 0x00324E9C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Filter_DecreeNotPossible questPart_Filter_DecreeNotPossible = new QuestPart_Filter_DecreeNotPossible();
			questPart_Filter_DecreeNotPossible.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			if (this.node != null)
			{
				questPart_Filter_DecreeNotPossible.outSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				QuestGenUtility.RunInnerNode(this.node, questPart_Filter_DecreeNotPossible.outSignal);
			}
			QuestGen.quest.AddPart(questPart_Filter_DecreeNotPossible);
		}

		// Token: 0x040076B1 RID: 30385
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040076B2 RID: 30386
		public QuestNode node;

		// Token: 0x040076B3 RID: 30387
		private const string OuterNodeCompletedSignal = "OuterNodeCompleted";
	}
}
