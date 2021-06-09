using System;

namespace Verse
{
	// Token: 0x020003F0 RID: 1008
	public class HediffComp_SeverityFromEntropy : HediffComp
	{
		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06001892 RID: 6290 RVA: 0x0001750E File Offset: 0x0001570E
		private float EntropyAmount
		{
			get
			{
				if (base.Pawn.psychicEntropy != null)
				{
					return base.Pawn.psychicEntropy.EntropyRelativeValue;
				}
				return 0f;
			}
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06001893 RID: 6291 RVA: 0x00017533 File Offset: 0x00015733
		public override bool CompShouldRemove
		{
			get
			{
				return this.EntropyAmount < float.Epsilon;
			}
		}

		// Token: 0x06001894 RID: 6292 RVA: 0x00017542 File Offset: 0x00015742
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.parent.Severity = this.EntropyAmount;
		}
	}
}
