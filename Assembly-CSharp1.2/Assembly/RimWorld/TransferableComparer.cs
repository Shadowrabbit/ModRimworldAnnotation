using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02001008 RID: 4104
	public abstract class TransferableComparer : IComparer<Transferable>
	{
		// Token: 0x06005989 RID: 22921
		public abstract int Compare(Transferable lhs, Transferable rhs);
	}
}
