using System;

namespace RimWorld
{
	// Token: 0x02001BBA RID: 7098
	public class TransferableComparer_Quality : TransferableComparer
	{
		// Token: 0x06009C45 RID: 40005 RVA: 0x002DDB8C File Offset: 0x002DBD8C
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return this.GetValueFor(lhs).CompareTo(this.GetValueFor(rhs));
		}

		// Token: 0x06009C46 RID: 40006 RVA: 0x002DDBB0 File Offset: 0x002DBDB0
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
