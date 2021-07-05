using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014D0 RID: 5328
	public class StatPart_Glow : StatPart
	{
		// Token: 0x06007F16 RID: 32534 RVA: 0x002CF63E File Offset: 0x002CD83E
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.factorFromGlowCurve == null)
			{
				yield return "factorFromLightCurve is null.";
			}
			yield break;
		}

		// Token: 0x06007F17 RID: 32535 RVA: 0x002CF64E File Offset: 0x002CD84E
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				val *= this.FactorFromGlow(req.Thing);
			}
		}

		// Token: 0x06007F18 RID: 32536 RVA: 0x002CF67C File Offset: 0x002CD87C
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				return "StatsReport_LightMultiplier".Translate(this.GlowLevel(req.Thing).ToStringPercent()) + ": x" + this.FactorFromGlow(req.Thing).ToStringPercent();
			}
			return null;
		}

		// Token: 0x06007F19 RID: 32537 RVA: 0x002CF6EC File Offset: 0x002CD8EC
		private bool ActiveFor(Thing t)
		{
			if (this.humanlikeOnly)
			{
				Pawn pawn = t as Pawn;
				if (pawn != null && !pawn.RaceProps.Humanlike)
				{
					return false;
				}
			}
			Pawn pawn2;
			return (!this.ignoreIfPrefersDarkness || (pawn2 = (t as Pawn)) == null || pawn2.Ideo == null || !pawn2.Ideo.IdeoPrefersDarkness()) && t.Spawned;
		}

		// Token: 0x06007F1A RID: 32538 RVA: 0x002CF749 File Offset: 0x002CD949
		private float GlowLevel(Thing t)
		{
			return t.Map.glowGrid.GameGlowAt(t.Position, false);
		}

		// Token: 0x06007F1B RID: 32539 RVA: 0x002CF762 File Offset: 0x002CD962
		private float FactorFromGlow(Thing t)
		{
			return this.factorFromGlowCurve.Evaluate(this.GlowLevel(t));
		}

		// Token: 0x04004F6D RID: 20333
		private bool humanlikeOnly;

		// Token: 0x04004F6E RID: 20334
		private SimpleCurve factorFromGlowCurve;

		// Token: 0x04004F6F RID: 20335
		private bool ignoreIfPrefersDarkness;
	}
}
