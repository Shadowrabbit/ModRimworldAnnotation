using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FA1 RID: 8097
	public class QuestNode_LeaveOnCleanup : QuestNode
	{
		// Token: 0x0600AC23 RID: 44067 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AC24 RID: 44068 RVA: 0x00321180 File Offset: 0x0031F380
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

		// Token: 0x04007583 RID: 30083
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x04007584 RID: 30084
		public SlateRef<bool?> sendStandardLetter;

		// Token: 0x04007585 RID: 30085
		[NoTranslate]
		public SlateRef<string> inSignalRemovePawn;
	}
}
