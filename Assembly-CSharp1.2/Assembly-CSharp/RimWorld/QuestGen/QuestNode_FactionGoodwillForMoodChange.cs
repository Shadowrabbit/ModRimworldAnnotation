using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F96 RID: 8086
	public class QuestNode_FactionGoodwillForMoodChange : QuestNode
	{
		// Token: 0x0600ABFB RID: 44027 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600ABFC RID: 44028 RVA: 0x00320A44 File Offset: 0x0031EC44
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

		// Token: 0x04007550 RID: 30032
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04007551 RID: 30033
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04007552 RID: 30034
		[NoTranslate]
		public SlateRef<string> outSignalSuccess;

		// Token: 0x04007553 RID: 30035
		[NoTranslate]
		public SlateRef<string> outSignalFailed;

		// Token: 0x04007554 RID: 30036
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
