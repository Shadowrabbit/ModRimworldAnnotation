using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016BB RID: 5819
	public class QuestNode_DisableRandomMoodCausedMentalBreaks : QuestNode
	{
		// Token: 0x060086E4 RID: 34532 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060086E5 RID: 34533 RVA: 0x00304D98 File Offset: 0x00302F98
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

		// Token: 0x040054AF RID: 21679
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x040054B0 RID: 21680
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x040054B1 RID: 21681
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
