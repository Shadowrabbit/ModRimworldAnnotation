using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02000AE5 RID: 2789
	public abstract class TransferableComparer : IComparer<Transferable>
	{
		// Token: 0x060041AB RID: 16811
		public abstract int Compare(Transferable lhs, Transferable rhs);
	}
}
