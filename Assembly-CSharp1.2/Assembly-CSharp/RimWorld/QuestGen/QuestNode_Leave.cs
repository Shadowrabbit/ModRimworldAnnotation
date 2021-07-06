using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FA0 RID: 8096
	public class QuestNode_Leave : QuestNode
	{
		// Token: 0x0600AC20 RID: 44064 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AC21 RID: 44065 RVA: 0x003210A4 File Offset: 0x0031F2A4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			IEnumerable<Pawn> value = this.pawns.GetValue(slate);
			if (value.EnumerableNullOrEmpty<Pawn>())
			{
				return;
			}
			QuestPart_Leave questPart_Leave = new QuestPart_Leave();
			questPart_Leave.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Leave.pawns.AddRange(value);
			questPart_Leave.sendStandardLetter = (this.sendStandardLetter.GetValue(slate) ?? questPart_Leave.sendStandardLetter);
			questPart_Leave.leaveOnCleanup = (this.leaveOnCleanup.GetValue(slate) ?? questPart_Leave.leaveOnCleanup);
			questPart_Leave.inSignalRemovePawn = this.inSignalRemovePawn.GetValue(slate);
			QuestGen.quest.AddPart(questPart_Leave);
		}

		// Token: 0x0400757E RID: 30078
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400757F RID: 30079
		[NoTranslate]
		public SlateRef<string> inSignalRemovePawn;

		// Token: 0x04007580 RID: 30080
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x04007581 RID: 30081
		public SlateRef<bool?> sendStandardLetter;

		// Token: 0x04007582 RID: 30082
		public SlateRef<bool?> leaveOnCleanup;
	}
}
