using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016EC RID: 5868
	public class QuestNode_ThingsProduced : QuestNode
	{
		// Token: 0x06008785 RID: 34693 RVA: 0x003079FC File Offset: 0x00305BFC
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x06008786 RID: 34694 RVA: 0x00307A14 File Offset: 0x00305C14
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ThingsProduced questPart_ThingsProduced = new QuestPart_ThingsProduced();
			questPart_ThingsProduced.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_ThingsProduced.def = this.def.GetValue(slate);
			questPart_ThingsProduced.stuff = this.stuff.GetValue(slate);
			questPart_ThingsProduced.count = this.count.GetValue(slate);
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_ThingsProduced);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_ThingsProduced.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_ThingsProduced);
		}

		// Token: 0x040055A1 RID: 21921
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x040055A2 RID: 21922
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x040055A3 RID: 21923
		public SlateRef<ThingDef> def;

		// Token: 0x040055A4 RID: 21924
		public SlateRef<ThingDef> stuff;

		// Token: 0x040055A5 RID: 21925
		public SlateRef<int> count;

		// Token: 0x040055A6 RID: 21926
		public QuestNode node;
	}
}
