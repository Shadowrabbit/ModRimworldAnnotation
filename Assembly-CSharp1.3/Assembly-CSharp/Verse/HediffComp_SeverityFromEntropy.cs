using System;

namespace Verse
{
	// Token: 0x020002B5 RID: 693
	public class HediffComp_SeverityFromEntropy : HediffComp
	{
		// Token: 0x170003AC RID: 940
		// (get) Token: 0x060012CD RID: 4813 RVA: 0x0006BA46 File Offset: 0x00069C46
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

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x060012CE RID: 4814 RVA: 0x0006BA6B File Offset: 0x00069C6B
		public override bool CompShouldRemove
		{
			get
			{
				return this.EntropyAmount < float.Epsilon;
			}
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x0006BA7A File Offset: 0x00069C7A
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.parent.Severity = this.EntropyAmount;
		}
	}
}
