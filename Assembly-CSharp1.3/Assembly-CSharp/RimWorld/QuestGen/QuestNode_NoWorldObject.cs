using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016D2 RID: 5842
	public class QuestNode_NoWorldObject : QuestNode
	{
		// Token: 0x06008732 RID: 34610 RVA: 0x003064FD File Offset: 0x003046FD
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x06008733 RID: 34611 RVA: 0x00306518 File Offset: 0x00304718
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_NoWorldObject questPart_NoWorldObject = new QuestPart_NoWorldObject();
			questPart_NoWorldObject.worldObject = this.worldObject.GetValue(slate);
			questPart_NoWorldObject.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_NoWorldObject);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_NoWorldObject.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_NoWorldObject);
		}

		// Token: 0x0400553E RID: 21822
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400553F RID: 21823
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x04005540 RID: 21824
		public SlateRef<WorldObject> worldObject;

		// Token: 0x04005541 RID: 21825
		public QuestNode node;
	}
}
