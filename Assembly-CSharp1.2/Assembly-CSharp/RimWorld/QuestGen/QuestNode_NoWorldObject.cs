using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FAA RID: 8106
	public class QuestNode_NoWorldObject : QuestNode
	{
		// Token: 0x0600AC3D RID: 44093 RVA: 0x00070853 File Offset: 0x0006EA53
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0600AC3E RID: 44094 RVA: 0x003217A4 File Offset: 0x0031F9A4
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

		// Token: 0x040075AC RID: 30124
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x040075AD RID: 30125
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x040075AE RID: 30126
		public SlateRef<WorldObject> worldObject;

		// Token: 0x040075AF RID: 30127
		public QuestNode node;
	}
}
