using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001211 RID: 4625
	public interface ITrader
	{
		// Token: 0x1700134A RID: 4938
		// (get) Token: 0x06006F05 RID: 28421
		TraderKindDef TraderKind { get; }

		// Token: 0x1700134B RID: 4939
		// (get) Token: 0x06006F06 RID: 28422
		IEnumerable<Thing> Goods { get; }

		// Token: 0x1700134C RID: 4940
		// (get) Token: 0x06006F07 RID: 28423
		int RandomPriceFactorSeed { get; }

		// Token: 0x1700134D RID: 4941
		// (get) Token: 0x06006F08 RID: 28424
		string TraderName { get; }

		// Token: 0x1700134E RID: 4942
		// (get) Token: 0x06006F09 RID: 28425
		bool CanTradeNow { get; }

		// Token: 0x1700134F RID: 4943
		// (get) Token: 0x06006F0A RID: 28426
		float TradePriceImprovementOffsetForPlayer { get; }

		// Token: 0x17001350 RID: 4944
		// (get) Token: 0x06006F0B RID: 28427
		Faction Faction { get; }

		// Token: 0x17001351 RID: 4945
		// (get) Token: 0x06006F0C RID: 28428
		TradeCurrency TradeCurrency { get; }

		// Token: 0x06006F0D RID: 28429
		IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator);

		// Token: 0x06006F0E RID: 28430
		void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator);

		// Token: 0x06006F0F RID: 28431
		void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator);
	}
}
