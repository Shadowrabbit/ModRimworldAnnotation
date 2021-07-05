using System;

namespace RimWorld
{
	// Token: 0x02001232 RID: 4658
	public static class TradeabilityUtility
	{
		// Token: 0x06006FB0 RID: 28592 RVA: 0x00253B91 File Offset: 0x00251D91
		public static bool PlayerCanSell(this Tradeability tradeability)
		{
			return tradeability == Tradeability.All || tradeability == Tradeability.Sellable;
		}

		// Token: 0x06006FB1 RID: 28593 RVA: 0x00253B9D File Offset: 0x00251D9D
		public static bool TraderCanSell(this Tradeability tradeability)
		{
			return tradeability == Tradeability.All || tradeability == Tradeability.Buyable;
		}
	}
}
