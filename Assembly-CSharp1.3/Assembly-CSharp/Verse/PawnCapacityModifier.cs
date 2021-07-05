using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000CE RID: 206
	public class PawnCapacityModifier
	{
		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x0001E7DB File Offset: 0x0001C9DB
		public bool SetMaxDefined
		{
			get
			{
				return this.setMax != 999f || (this.setMaxCurveOverride != null && this.setMaxCurveEvaluateStat != null);
			}
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x0001E7FF File Offset: 0x0001C9FF
		public float EvaluateSetMax(Pawn pawn)
		{
			if (this.setMaxCurveOverride == null || this.setMaxCurveEvaluateStat == null)
			{
				return this.setMax;
			}
			return this.setMaxCurveOverride.Evaluate(pawn.GetStatValue(this.setMaxCurveEvaluateStat, true));
		}

		// Token: 0x04000462 RID: 1122
		public PawnCapacityDef capacity;

		// Token: 0x04000463 RID: 1123
		public float offset;

		// Token: 0x04000464 RID: 1124
		public float setMax = 999f;

		// Token: 0x04000465 RID: 1125
		public float postFactor = 1f;

		// Token: 0x04000466 RID: 1126
		public SimpleCurve setMaxCurveOverride;

		// Token: 0x04000467 RID: 1127
		public StatDef setMaxCurveEvaluateStat;
	}
}
