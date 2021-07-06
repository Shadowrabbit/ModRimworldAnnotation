using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018E0 RID: 6368
	public interface ITrader
	{
		// Token: 0x17001629 RID: 5673
		// (get) Token: 0x06008D06 RID: 36102
		TraderKindDef TraderKind { get; }

		// Token: 0x1700162A RID: 5674
		// (get) Token: 0x06008D07 RID: 36103
		IEnumerable<Thing> Goods { get; }

		// Token: 0x1700162B RID: 5675
		// (get) Token: 0x06008D08 RID: 36104
		int RandomPriceFactorSeed { get; }

		// Token: 0x1700162C RID: 5676
		// (get) Token: 0x06008D09 RID: 36105
		string TraderName { get; }

		// Token: 0x1700162D RID: 5677
		// (get) Token: 0x06008D0A RID: 36106
		bool CanTradeNow { get; }

		// Token: 0x1700162E RID: 5678
		// (get) Token: 0x06008D0B RID: 36107
		float TradePriceImprovementOffsetForPlayer { get; }

		// Token: 0x1700162F RID: 5679
		// (get) Token: 0x06008D0C RID: 36108
		Faction Faction { get; }

		// Token: 0x17001630 RID: 5680
		// (get) Token: 0x06008D0D RID: 36109
		TradeCurrency TradeCurrency { get; }

		// Token: 0x06008D0E RID: 36110
		IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator);

		// Token: 0x06008D0F RID: 36111
		void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator);

		// Token: 0x06008D10 RID: 36112
		void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator);
	}
}
