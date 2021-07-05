using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001711 RID: 5905
	public abstract class QuestNode_Filter : QuestNode
	{
		// Token: 0x0600885B RID: 34907 RVA: 0x00310004 File Offset: 0x0030E204
		protected override bool TestRunInt(Slate slate)
		{
			return (this.node == null || this.node.TestRun(slate)) && (this.elseNode == null || this.elseNode.TestRun(slate));
		}

		// Token: 0x0600885C RID: 34908
		protected abstract QuestPart_Filter MakeFilterQuestPart();

		// Token: 0x0600885D RID: 34909 RVA: 0x00310038 File Offset: 0x0030E238
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Filter questPart_Filter = this.MakeFilterQuestPart();
			questPart_Filter.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			if (this.node != null)
			{
				questPart_Filter.outSignal = QuestGen.GenerateNewSignal("FilterPass", true);
				QuestGenUtility.RunInnerNode(this.node, questPart_Filter.outSignal);
			}
			if (this.elseNode != null)
			{
				questPart_Filter.outSignalElse = QuestGen.GenerateNewSignal("FilterNoPass", true);
				QuestGenUtility.RunInnerNode(this.elseNode, questPart_Filter.outSignalElse);
			}
			QuestGen.quest.AddPart(questPart_Filter);
		}

		// Token: 0x04005644 RID: 22084
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04005645 RID: 22085
		public QuestNode node;

		// Token: 0x04005646 RID: 22086
		public QuestNode elseNode;

		// Token: 0x04005647 RID: 22087
		private const string FilterPassSignal = "FilterPass";

		// Token: 0x04005648 RID: 22088
		private const string FilterNoPassSignal = "FilterNoPass";
	}
}
