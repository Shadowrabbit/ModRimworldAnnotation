using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016B2 RID: 5810
	public class QuestNode_ChangeFactionGoodwill : QuestNode
	{
		// Token: 0x060086C8 RID: 34504 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060086C9 RID: 34505 RVA: 0x00304758 File Offset: 0x00302958
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_FactionGoodwillChange questPart_FactionGoodwillChange = new QuestPart_FactionGoodwillChange();
			questPart_FactionGoodwillChange.change = this.change.GetValue(slate);
			questPart_FactionGoodwillChange.faction = (this.faction.GetValue(slate) ?? this.factionOf.GetValue(slate).Faction);
			questPart_FactionGoodwillChange.canSendHostilityLetter = (this.canSendLetter.GetValue(slate) ?? true);
			questPart_FactionGoodwillChange.canSendMessage = (this.canSendMessage.GetValue(slate) ?? true);
			questPart_FactionGoodwillChange.ensureMakesHostile = this.ensureHostile.GetValue(slate);
			questPart_FactionGoodwillChange.historyEvent = this.reason.GetValue(slate);
			questPart_FactionGoodwillChange.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_FactionGoodwillChange);
		}

		// Token: 0x0400548C RID: 21644
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400548D RID: 21645
		public SlateRef<Faction> faction;

		// Token: 0x0400548E RID: 21646
		public SlateRef<Thing> factionOf;

		// Token: 0x0400548F RID: 21647
		public SlateRef<int> change;

		// Token: 0x04005490 RID: 21648
		public SlateRef<bool?> canSendLetter;

		// Token: 0x04005491 RID: 21649
		public SlateRef<bool?> canSendMessage;

		// Token: 0x04005492 RID: 21650
		public SlateRef<bool> ensureHostile;

		// Token: 0x04005493 RID: 21651
		public SlateRef<HistoryEventDef> reason;
	}
}
