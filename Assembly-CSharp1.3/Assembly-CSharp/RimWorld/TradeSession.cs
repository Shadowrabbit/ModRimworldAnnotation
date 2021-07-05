using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200122F RID: 4655
	public static class TradeSession
	{
		// Token: 0x1700135C RID: 4956
		// (get) Token: 0x06006F8F RID: 28559 RVA: 0x002535AA File Offset: 0x002517AA
		public static bool Active
		{
			get
			{
				return TradeSession.trader != null;
			}
		}

		// Token: 0x1700135D RID: 4957
		// (get) Token: 0x06006F90 RID: 28560 RVA: 0x002535B4 File Offset: 0x002517B4
		public static TradeCurrency TradeCurrency
		{
			get
			{
				return TradeSession.trader.TradeCurrency;
			}
		}

		// Token: 0x06006F91 RID: 28561 RVA: 0x002535C0 File Offset: 0x002517C0
		public static void SetupWith(ITrader newTrader, Pawn newPlayerNegotiator, bool giftMode)
		{
			if (!newTrader.CanTradeNow)
			{
				Log.Warning("Called SetupWith with a trader not willing to trade now.");
			}
			TradeSession.trader = newTrader;
			TradeSession.playerNegotiator = newPlayerNegotiator;
			TradeSession.giftMode = giftMode;
			TradeSession.deal = new TradeDeal();
			if (!giftMode && TradeSession.deal.cannotSellReasons.Count > 0)
			{
				Messages.Message("MessageCannotSellItemsReason".Translate() + TradeSession.deal.cannotSellReasons.ToCommaList(true, false).CapitalizeFirst(), MessageTypeDefOf.NegativeEvent, false);
			}
		}

		// Token: 0x06006F92 RID: 28562 RVA: 0x00253645 File Offset: 0x00251845
		public static void Close()
		{
			TradeSession.trader = null;
		}

		// Token: 0x04003D9F RID: 15775
		public static ITrader trader;

		// Token: 0x04003DA0 RID: 15776
		public static Pawn playerNegotiator;

		// Token: 0x04003DA1 RID: 15777
		public static TradeDeal deal;

		// Token: 0x04003DA2 RID: 15778
		public static bool giftMode;
	}
}
