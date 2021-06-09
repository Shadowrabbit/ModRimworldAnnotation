using System;

namespace RimWorld
{
	// Token: 0x02001BB7 RID: 7095
	public class TransferableComparer_Mass : TransferableComparer
	{
		// Token: 0x06009C3F RID: 39999 RVA: 0x002DDB54 File Offset: 0x002DBD54
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return lhs.AnyThing.GetStatValue(StatDefOf.Mass, true).CompareTo(rhs.AnyThing.GetStatValue(StatDefOf.Mass, true));
		}
	}
}
