using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001925 RID: 6437
	public static class TradeSession
	{
		// Token: 0x1700167B RID: 5755
		// (get) Token: 0x06008E8B RID: 36491 RVA: 0x0005F7F2 File Offset: 0x0005D9F2
		public static bool Active
		{
			get
			{
				return TradeSession.trader != null;
			}
		}

		// Token: 0x1700167C RID: 5756
		// (get) Token: 0x06008E8C RID: 36492 RVA: 0x0005F7FC File Offset: 0x0005D9FC
		public static TradeCurrency TradeCurrency
		{
			get
			{
				return TradeSession.trader.TradeCurrency;
			}
		}

		// Token: 0x06008E8D RID: 36493 RVA: 0x00291C50 File Offset: 0x0028FE50
		public static void SetupWith(ITrader newTrader, Pawn newPlayerNegotiator, bool giftMode)
		{
			if (!newTrader.CanTradeNow)
			{
				Log.Warning("Called SetupWith with a trader not willing to trade now.", false);
			}
			TradeSession.trader = newTrader;
			TradeSession.playerNegotiator = newPlayerNegotiator;
			TradeSession.giftMode = giftMode;
			TradeSession.deal = new TradeDeal();
			if (!giftMode && TradeSession.deal.cannotSellReasons.Count > 0)
			{
				Messages.Message("MessageCannotSellItemsReason".Translate() + TradeSession.deal.cannotSellReasons.ToCommaList(true), MessageTypeDefOf.NegativeEvent, false);
			}
		}

		// Token: 0x06008E8E RID: 36494 RVA: 0x0005F808 File Offset: 0x0005DA08
		public static void Close()
		{
			TradeSession.trader = null;
		}

		// Token: 0x04005AE8 RID: 23272
		public static ITrader trader;

		// Token: 0x04005AE9 RID: 23273
		public static Pawn playerNegotiator;

		// Token: 0x04005AEA RID: 23274
		public static TradeDeal deal;

		// Token: 0x04005AEB RID: 23275
		public static bool giftMode;
	}
}
