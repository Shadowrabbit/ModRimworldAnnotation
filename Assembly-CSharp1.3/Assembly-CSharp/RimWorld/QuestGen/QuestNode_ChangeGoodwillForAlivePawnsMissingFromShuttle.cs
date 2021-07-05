using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016B3 RID: 5811
	public class QuestNode_ChangeGoodwillForAlivePawnsMissingFromShuttle : QuestNode
	{
		// Token: 0x060086CB RID: 34507 RVA: 0x00304851 File Offset: 0x00302A51
		protected override bool TestRunInt(Slate slate)
		{
			return this.faction.GetValue(slate) != null;
		}

		// Token: 0x060086CC RID: 34508 RVA: 0x00304864 File Offset: 0x00302A64
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ChangeGoodwillForAlivePawnsMissingFromShuttle part = new QuestPart_ChangeGoodwillForAlivePawnsMissingFromShuttle
			{
				inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				pawns = this.pawns.GetValue(slate),
				faction = this.faction.GetValue(slate),
				goodwillChange = this.goodwillChange.GetValue(slate),
				historyEvent = this.reason.GetValue(slate)
			};
			QuestGen.quest.AddPart(part);
		}

		// Token: 0x04005494 RID: 21652
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04005495 RID: 21653
		public SlateRef<List<Pawn>> pawns;

		// Token: 0x04005496 RID: 21654
		public SlateRef<Faction> faction;

		// Token: 0x04005497 RID: 21655
		public SlateRef<int> goodwillChange;

		// Token: 0x04005498 RID: 21656
		public SlateRef<HistoryEventDef> reason;
	}
}
