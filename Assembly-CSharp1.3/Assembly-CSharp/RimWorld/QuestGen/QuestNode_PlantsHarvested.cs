using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016D6 RID: 5846
	public class QuestNode_PlantsHarvested : QuestNode
	{
		// Token: 0x0600873E RID: 34622 RVA: 0x00306ADC File Offset: 0x00304CDC
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0600873F RID: 34623 RVA: 0x00306AF4 File Offset: 0x00304CF4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_PlantsHarvested questPart_PlantsHarvested = new QuestPart_PlantsHarvested();
			questPart_PlantsHarvested.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_PlantsHarvested.plant = this.plant.GetValue(slate);
			questPart_PlantsHarvested.count = this.count.GetValue(slate);
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_PlantsHarvested);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_PlantsHarvested.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_PlantsHarvested);
		}

		// Token: 0x04005559 RID: 21849
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400555A RID: 21850
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x0400555B RID: 21851
		public SlateRef<ThingDef> plant;

		// Token: 0x0400555C RID: 21852
		public SlateRef<int> count;

		// Token: 0x0400555D RID: 21853
		public QuestNode node;
	}
}
