using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D38 RID: 7480
	public class StatPart_Glow : StatPart
	{
		// Token: 0x0600A28A RID: 41610 RVA: 0x0006BFB0 File Offset: 0x0006A1B0
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.factorFromGlowCurve == null)
			{
				yield return "factorFromLightCurve is null.";
			}
			yield break;
		}

		// Token: 0x0600A28B RID: 41611 RVA: 0x0006BFC0 File Offset: 0x0006A1C0
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				val *= this.FactorFromGlow(req.Thing);
			}
		}

		// Token: 0x0600A28C RID: 41612 RVA: 0x002F577C File Offset: 0x002F397C
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				return "StatsReport_LightMultiplier".Translate(this.GlowLevel(req.Thing).ToStringPercent()) + ": x" + this.FactorFromGlow(req.Thing).ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600A28D RID: 41613 RVA: 0x002F57EC File Offset: 0x002F39EC
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
			return t.Spawned;
		}

		// Token: 0x0600A28E RID: 41614 RVA: 0x0006BFEC File Offset: 0x0006A1EC
		private float GlowLevel(Thing t)
		{
			return t.Map.glowGrid.GameGlowAt(t.Position, false);
		}

		// Token: 0x0600A28F RID: 41615 RVA: 0x0006C005 File Offset: 0x0006A205
		private float FactorFromGlow(Thing t)
		{
			return this.factorFromGlowCurve.Evaluate(this.GlowLevel(t));
		}

		// Token: 0x04006E74 RID: 28276
		private bool humanlikeOnly;

		// Token: 0x04006E75 RID: 28277
		private SimpleCurve factorFromGlowCurve;
	}
}
