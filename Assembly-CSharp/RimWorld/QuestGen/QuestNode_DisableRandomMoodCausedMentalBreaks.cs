using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F8E RID: 8078
	public class QuestNode_DisableRandomMoodCausedMentalBreaks : QuestNode
	{
		// Token: 0x0600ABE3 RID: 44003 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600ABE4 RID: 44004 RVA: 0x00320554 File Offset: 0x0031E754
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			IEnumerable<Pawn> value = this.pawns.GetValue(slate);
			if (value.EnumerableNullOrEmpty<Pawn>())
			{
				return;
			}
			QuestPart_DisableRandomMoodCausedMentalBreaks questPart_DisableRandomMoodCausedMentalBreaks = new QuestPart_DisableRandomMoodCausedMentalBreaks();
			questPart_DisableRandomMoodCausedMentalBreaks.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_DisableRandomMoodCausedMentalBreaks.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			questPart_DisableRandomMoodCausedMentalBreaks.pawns.AddRange(value);
			QuestGen.quest.AddPart(questPart_DisableRandomMoodCausedMentalBreaks);
		}

		// Token: 0x0400752D RID: 29997
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400752E RID: 29998
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x0400752F RID: 29999
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
