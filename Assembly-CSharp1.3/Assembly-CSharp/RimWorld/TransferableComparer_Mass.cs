using System;

namespace RimWorld
{
	// Token: 0x020013B7 RID: 5047
	public class TransferableComparer_Mass : TransferableComparer
	{
		// Token: 0x06007ACB RID: 31435 RVA: 0x002B677C File Offset: 0x002B497C
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return lhs.AnyThing.GetStatValue(StatDefOf.Mass, true).CompareTo(rhs.AnyThing.GetStatValue(StatDefOf.Mass, true));
		}
	}
}
