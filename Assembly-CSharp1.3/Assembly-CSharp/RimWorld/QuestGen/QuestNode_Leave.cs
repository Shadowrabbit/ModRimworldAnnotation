using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016CA RID: 5834
	public class QuestNode_Leave : QuestNode
	{
		// Token: 0x0600871A RID: 34586 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600871B RID: 34587 RVA: 0x00305D70 File Offset: 0x00303F70
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

		// Token: 0x0400550F RID: 21775
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04005510 RID: 21776
		[NoTranslate]
		public SlateRef<string> inSignalRemovePawn;

		// Token: 0x04005511 RID: 21777
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x04005512 RID: 21778
		public SlateRef<bool?> sendStandardLetter;

		// Token: 0x04005513 RID: 21779
		public SlateRef<bool?> leaveOnCleanup;
	}
}
