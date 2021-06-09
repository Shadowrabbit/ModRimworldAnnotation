using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FC0 RID: 8128
	public class QuestNode_ShuttleDelay : QuestNode
	{
		// Token: 0x0600AC89 RID: 44169 RVA: 0x00070A13 File Offset: 0x0006EC13
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0600AC8A RID: 44170 RVA: 0x00322600 File Offset: 0x00320800
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ShuttleDelay questPart_ShuttleDelay = new QuestPart_ShuttleDelay();
			questPart_ShuttleDelay.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_ShuttleDelay.delayTicks = this.delayTicks.GetValue(slate);
			if (this.lodgers.GetValue(slate) != null)
			{
				questPart_ShuttleDelay.lodgers.AddRange(this.lodgers.GetValue(slate));
			}
			questPart_ShuttleDelay.expiryInfoPart = "ShuttleArrivesIn".Translate();
			questPart_ShuttleDelay.expiryInfoPartTip = "ShuttleArrivesOn".Translate();
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_ShuttleDelay);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_ShuttleDelay.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_ShuttleDelay);
		}

		// Token: 0x040075FB RID: 30203
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x040075FC RID: 30204
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x040075FD RID: 30205
		public SlateRef<int> delayTicks;

		// Token: 0x040075FE RID: 30206
		public SlateRef<IEnumerable<Pawn>> lodgers;

		// Token: 0x040075FF RID: 30207
		public QuestNode node;
	}
}
