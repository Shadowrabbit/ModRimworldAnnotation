using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016CB RID: 5835
	public class QuestNode_LeaveOnCleanup : QuestNode
	{
		// Token: 0x0600871D RID: 34589 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600871E RID: 34590 RVA: 0x00305E4C File Offset: 0x0030404C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			IEnumerable<Pawn> value = this.pawns.GetValue(slate);
			if (value.EnumerableNullOrEmpty<Pawn>())
			{
				return;
			}
			QuestPart_Leave questPart_Leave = new QuestPart_Leave();
			questPart_Leave.pawns.AddRange(value);
			questPart_Leave.sendStandardLetter = (this.sendStandardLetter.GetValue(slate) ?? questPart_Leave.sendStandardLetter);
			questPart_Leave.leaveOnCleanup = true;
			questPart_Leave.inSignalRemovePawn = this.inSignalRemovePawn.GetValue(slate);
			QuestGen.quest.AddPart(questPart_Leave);
		}

		// Token: 0x04005514 RID: 21780
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x04005515 RID: 21781
		public SlateRef<bool?> sendStandardLetter;

		// Token: 0x04005516 RID: 21782
		[NoTranslate]
		public SlateRef<string> inSignalRemovePawn;
	}
}
