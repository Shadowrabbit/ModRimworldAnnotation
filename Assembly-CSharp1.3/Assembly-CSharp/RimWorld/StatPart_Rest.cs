using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014E1 RID: 5345
	public class StatPart_Rest : StatPart
	{
		// Token: 0x06007F62 RID: 32610 RVA: 0x002D068C File Offset: 0x002CE88C
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.needs.rest != null)
				{
					val *= this.RestMultiplier(pawn.needs.rest.CurCategory);
				}
			}
		}

		// Token: 0x06007F63 RID: 32611 RVA: 0x002D06DC File Offset: 0x002CE8DC
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.needs.rest != null)
				{
					return pawn.needs.rest.CurCategory.GetLabel() + ": x" + this.RestMultiplier(pawn.needs.rest.CurCategory).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x06007F64 RID: 32612 RVA: 0x002D074B File Offset: 0x002CE94B
		private float RestMultiplier(RestCategory fatigue)
		{
			switch (fatigue)
			{
			case RestCategory.Rested:
				return this.factorRested;
			case RestCategory.Tired:
				return this.factorTired;
			case RestCategory.VeryTired:
				return this.factorVeryTired;
			case RestCategory.Exhausted:
				return this.factorExhausted;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x04004F8F RID: 20367
		private float factorExhausted = 1f;

		// Token: 0x04004F90 RID: 20368
		private float factorVeryTired = 1f;

		// Token: 0x04004F91 RID: 20369
		private float factorTired = 1f;

		// Token: 0x04004F92 RID: 20370
		private float factorRested = 1f;
	}
}
