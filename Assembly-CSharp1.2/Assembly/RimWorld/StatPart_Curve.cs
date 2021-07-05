using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D31 RID: 7473
	public abstract class StatPart_Curve : StatPart
	{
		// Token: 0x0600A26E RID: 41582
		protected abstract bool AppliesTo(StatRequest req);

		// Token: 0x0600A26F RID: 41583
		protected abstract float CurveXGetter(StatRequest req);

		// Token: 0x0600A270 RID: 41584
		protected abstract string ExplanationLabel(StatRequest req);

		// Token: 0x0600A271 RID: 41585 RVA: 0x0006BE4F File Offset: 0x0006A04F
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && this.AppliesTo(req))
			{
				val *= this.curve.Evaluate(this.CurveXGetter(req));
			}
		}

		// Token: 0x0600A272 RID: 41586 RVA: 0x0006BE7A File Offset: 0x0006A07A
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && this.AppliesTo(req))
			{
				return this.ExplanationLabel(req) + ": x" + this.curve.Evaluate(this.CurveXGetter(req)).ToStringPercent();
			}
			return null;
		}

		// Token: 0x04006E6B RID: 28267
		protected SimpleCurve curve;
	}
}
