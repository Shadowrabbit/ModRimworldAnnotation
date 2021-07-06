using System;

namespace RimWorld
{
	// Token: 0x02001BB6 RID: 7094
	public class TransferableComparer_MarketValue : TransferableComparer
	{
		// Token: 0x06009C3D RID: 39997 RVA: 0x002DDB28 File Offset: 0x002DBD28
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return lhs.AnyThing.MarketValue.CompareTo(rhs.AnyThing.MarketValue);
		}
	}
}
