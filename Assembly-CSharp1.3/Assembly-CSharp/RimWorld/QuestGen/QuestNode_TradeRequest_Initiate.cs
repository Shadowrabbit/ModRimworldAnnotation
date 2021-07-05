using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200170D RID: 5901
	public class QuestNode_TradeRequest_Initiate : QuestNode
	{
		// Token: 0x0600884B RID: 34891 RVA: 0x0030FAF0 File Offset: 0x0030DCF0
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

		// Token: 0x0600884C RID: 34892 RVA: 0x0030FB78 File Offset: 0x0030DD78
		protected override bool TestRunInt(Slate slate)
		{
			return this.settlement.GetValue(slate) != null && this.requestedThingCount.GetValue(slate) > 0 && this.requestedThingDef.GetValue(slate) != null && this.duration.GetValue(slate) > 0;
		}

		// Token: 0x0400562F RID: 22063
		public SlateRef<Settlement> settlement;

		// Token: 0x04005630 RID: 22064
		public SlateRef<ThingDef> requestedThingDef;

		// Token: 0x04005631 RID: 22065
		public SlateRef<int> requestedThingCount;

		// Token: 0x04005632 RID: 22066
		public SlateRef<int> duration;
	}
}
