using System;

namespace RimWorld
{
	// Token: 0x02001BB8 RID: 7096
	public class TransferableComparer_Name : TransferableComparer
	{
		// Token: 0x06009C41 RID: 40001 RVA: 0x00067D74 File Offset: 0x00065F74
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return lhs.Label.CompareTo(rhs.Label);
		}
	}
}
