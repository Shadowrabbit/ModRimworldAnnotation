using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014CA RID: 5322
	public abstract class StatPart_Curve : StatPart
	{
		// Token: 0x06007EFE RID: 32510
		protected abstract bool AppliesTo(StatRequest req);

		// Token: 0x06007EFF RID: 32511
		protected abstract float CurveXGetter(StatRequest req);

		// Token: 0x06007F00 RID: 32512
		protected abstract string ExplanationLabel(StatRequest req);

		// Token: 0x06007F01 RID: 32513 RVA: 0x002CF308 File Offset: 0x002CD508
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && this.AppliesTo(req))
			{
				val *= this.curve.Evaluate(this.CurveXGetter(req));
			}
		}

		// Token: 0x06007F02 RID: 32514 RVA: 0x002CF333 File Offset: 0x002CD533
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && this.AppliesTo(req))
			{
				return this.ExplanationLabel(req) + ": x" + this.curve.Evaluate(this.CurveXGetter(req)).ToStringPercent();
			}
			return null;
		}

		// Token: 0x04004F67 RID: 20327
		protected SimpleCurve curve;
	}
}
