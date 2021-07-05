using System;

namespace RimWorld
{
	// Token: 0x020013BA RID: 5050
	public class TransferableComparer_Quality : TransferableComparer
	{
		// Token: 0x06007AD1 RID: 31441 RVA: 0x002B67C8 File Offset: 0x002B49C8
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return this.GetValueFor(lhs).CompareTo(this.GetValueFor(rhs));
		}

		// Token: 0x06007AD2 RID: 31442 RVA: 0x002B67EC File Offset: 0x002B49EC
		private int GetValueFor(Transferable t)
		{
			QualityCategory result;
			if (!t.AnyThing.TryGetQuality(out result))
			{
				return -1;
			}
			return (int)result;
		}
	}
}
