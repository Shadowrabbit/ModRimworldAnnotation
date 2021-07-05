using System;

namespace RimWorld
{
	// Token: 0x0200192B RID: 6443
	public static class TradeabilityUtility
	{
		// Token: 0x06008EC3 RID: 36547 RVA: 0x0005F9D5 File Offset: 0x0005DBD5
		public static bool PlayerCanSell(this Tradeability tradeability)
		{
			return tradeability == Tradeability.All || tradeability == Tradeability.Sellable;
		}

		// Token: 0x06008EC4 RID: 36548 RVA: 0x0005F9E1 File Offset: 0x0005DBE1
		public static bool TraderCanSell(this Tradeability tradeability)
		{
			return tradeability == Tradeability.All || tradeability == Tradeability.Buyable;
		}
	}
}
