using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016E7 RID: 5863
	public class QuestNode_ShuttleLeaveDelay : QuestNode
	{
		// Token: 0x06008775 RID: 34677 RVA: 0x00307390 File Offset: 0x00305590
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x06008776 RID: 34678 RVA: 0x003073A8 File Offset: 0x003055A8
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

		// Token: 0x04005587 RID: 21895
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04005588 RID: 21896
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x04005589 RID: 21897
		[NoTranslate]
		public SlateRef<IEnumerable<string>> inSignalsDisable;

		// Token: 0x0400558A RID: 21898
		public SlateRef<int> delayTicks;

		// Token: 0x0400558B RID: 21899
		public SlateRef<Thing> shuttle;

		// Token: 0x0400558C RID: 21900
		public QuestNode node;
	}
}
