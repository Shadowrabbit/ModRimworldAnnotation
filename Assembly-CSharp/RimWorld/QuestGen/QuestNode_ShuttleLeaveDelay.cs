using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FC1 RID: 8129
	public class QuestNode_ShuttleLeaveDelay : QuestNode
	{
		// Token: 0x0600AC8C RID: 44172 RVA: 0x00070A2B File Offset: 0x0006EC2B
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0600AC8D RID: 44173 RVA: 0x003226F4 File Offset: 0x003208F4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ShuttleLeaveDelay questPart_ShuttleLeaveDelay = new QuestPart_ShuttleLeaveDelay();
			questPart_ShuttleLeaveDelay.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_ShuttleLeaveDelay.delayTicks = this.delayTicks.GetValue(slate);
			questPart_ShuttleLeaveDelay.shuttle = this.shuttle.GetValue(slate);
			questPart_ShuttleLeaveDelay.expiryInfoPart = "ShuttleDepartsIn".Translate();
			questPart_ShuttleLeaveDelay.expiryInfoPartTip = "ShuttleDepartsOn".Translate();
			if (this.inSignalsDisable.GetValue(slate) != null)
			{
				foreach (string signal in this.inSignalsDisable.GetValue(slate))
				{
					questPart_ShuttleLeaveDelay.inSignalsDisable.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
				}
			}
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_ShuttleLeaveDelay);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_ShuttleLeaveDelay.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_ShuttleLeaveDelay);
		}

		// Token: 0x04007600 RID: 30208
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04007601 RID: 30209
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x04007602 RID: 30210
		[NoTranslate]
		public SlateRef<IEnumerable<string>> inSignalsDisable;

		// Token: 0x04007603 RID: 30211
		public SlateRef<int> delayTicks;

		// Token: 0x04007604 RID: 30212
		public SlateRef<Thing> shuttle;

		// Token: 0x04007605 RID: 30213
		public QuestNode node;
	}
}
