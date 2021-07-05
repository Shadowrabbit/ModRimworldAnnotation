using System;

namespace RimWorld
{
	// Token: 0x020013B8 RID: 5048
	public class TransferableComparer_Name : TransferableComparer
	{
		// Token: 0x06007ACD RID: 31437 RVA: 0x002B67B3 File Offset: 0x002B49B3
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return lhs.Label.CompareTo(rhs.Label);
		}
	}
}
