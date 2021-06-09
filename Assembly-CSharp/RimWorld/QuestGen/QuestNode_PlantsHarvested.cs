using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FAE RID: 8110
	public class QuestNode_PlantsHarvested : QuestNode
	{
		// Token: 0x0600AC49 RID: 44105 RVA: 0x000708C5 File Offset: 0x0006EAC5
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0600AC4A RID: 44106 RVA: 0x00321C9C File Offset: 0x0031FE9C
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

		// Token: 0x040075C4 RID: 30148
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x040075C5 RID: 30149
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x040075C6 RID: 30150
		public SlateRef<ThingDef> plant;

		// Token: 0x040075C7 RID: 30151
		public SlateRef<int> count;

		// Token: 0x040075C8 RID: 30152
		public QuestNode node;
	}
}
