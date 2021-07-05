using System;

namespace RimWorld
{
	// Token: 0x020013B6 RID: 5046
	public class TransferableComparer_MarketValue : TransferableComparer
	{
		// Token: 0x06007AC9 RID: 31433 RVA: 0x002B6750 File Offset: 0x002B4950
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return lhs.AnyThing.MarketValue.CompareTo(rhs.AnyThing.MarketValue);
		}
	}
}
