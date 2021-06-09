using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FC7 RID: 8135
	public class QuestNode_ThingsProduced : QuestNode
	{
		// Token: 0x0600ACA2 RID: 44194 RVA: 0x00070A96 File Offset: 0x0006EC96
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0600ACA3 RID: 44195 RVA: 0x00322D34 File Offset: 0x00320F34
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

		// Token: 0x0400761E RID: 30238
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400761F RID: 30239
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x04007620 RID: 30240
		public SlateRef<ThingDef> def;

		// Token: 0x04007621 RID: 30241
		public SlateRef<ThingDef> stuff;

		// Token: 0x04007622 RID: 30242
		public SlateRef<int> count;

		// Token: 0x04007623 RID: 30243
		public QuestNode node;
	}
}
