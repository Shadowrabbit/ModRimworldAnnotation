using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016C0 RID: 5824
	public class QuestNode_FactionGoodwillForMoodChange : QuestNode
	{
		// Token: 0x060086F3 RID: 34547 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060086F4 RID: 34548 RVA: 0x00305304 File Offset: 0x00303504
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) != null)
			{
				QuestPart_FactionGoodwillForMoodChange questPart_FactionGoodwillForMoodChange = new QuestPart_FactionGoodwillForMoodChange();
				questPart_FactionGoodwillForMoodChange.pawns.AddRange(this.pawns.GetValue(slate));
				questPart_FactionGoodwillForMoodChange.inSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate));
				questPart_FactionGoodwillForMoodChange.outSignalSuccess = QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalSuccess.GetValue(slate));
				questPart_FactionGoodwillForMoodChange.outSignalFailed = QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalFailed.GetValue(slate));
				questPart_FactionGoodwillForMoodChange.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
				QuestGen.quest.AddPart(questPart_FactionGoodwillForMoodChange);
			}
		}

		// Token: 0x040054D0 RID: 21712
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040054D1 RID: 21713
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x040054D2 RID: 21714
		[NoTranslate]
		public SlateRef<string> outSignalSuccess;

		// Token: 0x040054D3 RID: 21715
		[NoTranslate]
		public SlateRef<string> outSignalFailed;

		// Token: 0x040054D4 RID: 21716
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
