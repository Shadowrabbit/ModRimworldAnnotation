using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D47 RID: 7495
	public class StatPart_Rest : StatPart
	{
		// Token: 0x0600A2D5 RID: 41685 RVA: 0x002F6288 File Offset: 0x002F4488
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

		// Token: 0x0600A2D6 RID: 41686 RVA: 0x002F62D8 File Offset: 0x002F44D8
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

		// Token: 0x0600A2D7 RID: 41687 RVA: 0x0006C20F File Offset: 0x0006A40F
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

		// Token: 0x04006E96 RID: 28310
		private float factorExhausted = 1f;

		// Token: 0x04006E97 RID: 28311
		private float factorVeryTired = 1f;

		// Token: 0x04006E98 RID: 28312
		private float factorTired = 1f;

		// Token: 0x04006E99 RID: 28313
		private float factorRested = 1f;
	}
}
