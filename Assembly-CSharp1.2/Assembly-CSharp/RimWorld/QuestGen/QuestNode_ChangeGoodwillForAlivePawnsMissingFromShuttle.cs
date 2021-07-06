using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F85 RID: 8069
	public class QuestNode_ChangeGoodwillForAlivePawnsMissingFromShuttle : QuestNode
	{
		// Token: 0x0600ABC7 RID: 43975 RVA: 0x000706FA File Offset: 0x0006E8FA
		protected override bool TestRunInt(Slate slate)
		{
			return this.faction.GetValue(slate) != null;
		}

		// Token: 0x0600ABC8 RID: 43976 RVA: 0x00320024 File Offset: 0x0031E224
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ChangeGoodwillForAlivePawnsMissingFromShuttle part = new QuestPart_ChangeGoodwillForAlivePawnsMissingFromShuttle
			{
				inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				pawns = this.pawns.GetValue(slate),
				faction = this.faction.GetValue(slate),
				goodwillChange = this.goodwillChange.GetValue(slate),
				reason = this.reason.GetValue(slate)
			};
			QuestGen.quest.AddPart(part);
		}

		// Token: 0x04007510 RID: 29968
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04007511 RID: 29969
		public SlateRef<List<Pawn>> pawns;

		// Token: 0x04007512 RID: 29970
		public SlateRef<Faction> faction;

		// Token: 0x04007513 RID: 29971
		public SlateRef<int> goodwillChange;

		// Token: 0x04007514 RID: 29972
		public SlateRef<string> reason;
	}
}
