using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016AF RID: 5807
	public class QuestNode_BetrayMTB : QuestNode
	{
		// Token: 0x060086BF RID: 34495 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060086C0 RID: 34496 RVA: 0x00304638 File Offset: 0x00302838
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate).EnumerableNullOrEmpty<Pawn>())
			{
				return;
			}
			QuestPart_BetrayMTB questPart_BetrayMTB = new QuestPart_BetrayMTB();
			questPart_BetrayMTB.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_BetrayMTB.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			questPart_BetrayMTB.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignal.GetValue(slate)));
			questPart_BetrayMTB.pawns.AddRange(this.pawns.GetValue(slate));
			QuestGen.quest.AddPart(questPart_BetrayMTB);
		}

		// Token: 0x04005486 RID: 21638
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04005487 RID: 21639
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x04005488 RID: 21640
		[NoTranslate]
		public SlateRef<string> outSignal;

		// Token: 0x04005489 RID: 21641
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
