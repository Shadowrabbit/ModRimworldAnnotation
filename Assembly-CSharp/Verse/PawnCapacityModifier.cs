using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200013E RID: 318
	public class PawnCapacityModifier
	{
		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000864 RID: 2148 RVA: 0x0000CAA6 File Offset: 0x0000ACA6
		public bool SetMaxDefined
		{
			get
			{
				return this.setMax != 999f || (this.setMaxCurveOverride != null && this.setMaxCurveEvaluateStat != null);
			}
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0000CACA File Offset: 0x0000ACCA
		public float EvaluateSetMax(Pawn pawn)
		{
			if (this.setMaxCurveOverride == null || this.setMaxCurveEvaluateStat == null)
			{
				return this.setMax;
			}
			return this.setMaxCurveOverride.Evaluate(pawn.GetStatValue(this.setMaxCurveEvaluateStat, true));
		}

		// Token: 0x0400065A RID: 1626
		public PawnCapacityDef capacity;

		// Token: 0x0400065B RID: 1627
		public float offset;

		// Token: 0x0400065C RID: 1628
		public float setMax = 999f;

		// Token: 0x0400065D RID: 1629
		public float postFactor = 1f;

		// Token: 0x0400065E RID: 1630
		public SimpleCurve setMaxCurveOverride;

		// Token: 0x0400065F RID: 1631
		public StatDef setMaxCurveEvaluateStat;
	}
}
