using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FE0 RID: 8160
	public class QuestNode_TradeRequest_Initiate : QuestNode
	{
		// Token: 0x0600AD1A RID: 44314 RVA: 0x003266A0 File Offset: 0x003248A0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_InitiateTradeRequest questPart_InitiateTradeRequest = new QuestPart_InitiateTradeRequest();
			questPart_InitiateTradeRequest.settlement = this.settlement.GetValue(slate);
			questPart_InitiateTradeRequest.requestedThingDef = this.requestedThingDef.GetValue(slate);
			questPart_InitiateTradeRequest.requestedCount = this.requestedThingCount.GetValue(slate);
			questPart_InitiateTradeRequest.requestDuration = this.duration.GetValue(slate);
			questPart_InitiateTradeRequest.keepAfterQuestEnds = false;
			questPart_InitiateTradeRequest.inSignal = slate.Get<string>("inSignal", null, false);
			QuestGen.quest.AddPart(questPart_InitiateTradeRequest);
		}

		// Token: 0x0600AD1B RID: 44315 RVA: 0x00326728 File Offset: 0x00324928
		protected override bool TestRunInt(Slate slate)
		{
			return this.settlement.GetValue(slate) != null && this.requestedThingCount.GetValue(slate) > 0 && this.requestedThingDef.GetValue(slate) != null && this.duration.GetValue(slate) > 0;
		}

		// Token: 0x04007699 RID: 30361
		public SlateRef<Settlement> settlement;

		// Token: 0x0400769A RID: 30362
		public SlateRef<ThingDef> requestedThingDef;

		// Token: 0x0400769B RID: 30363
		public SlateRef<int> requestedThingCount;

		// Token: 0x0400769C RID: 30364
		public SlateRef<int> duration;
	}
}
