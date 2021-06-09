using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D28 RID: 7464
	public class StatPart_Age : StatPart
	{
		// Token: 0x0600A243 RID: 41539 RVA: 0x002F4DC4 File Offset: 0x002F2FC4
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.ageTracker != null)
				{
					val *= this.AgeMultiplier(pawn);
				}
			}
		}

		// Token: 0x0600A244 RID: 41540 RVA: 0x002F4E00 File Offset: 0x002F3000
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.ageTracker != null)
				{
					return "StatsReport_AgeMultiplier".Translate(pawn.ageTracker.AgeBiologicalYears) + ": x" + this.AgeMultiplier(pawn).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x0600A245 RID: 41541 RVA: 0x0006BD36 File Offset: 0x00069F36
		private float AgeMultiplier(Pawn pawn)
		{
			return this.curve.Evaluate((float)pawn.ageTracker.AgeBiologicalYears / pawn.RaceProps.lifeExpectancy);
		}

		// Token: 0x0600A246 RID: 41542 RVA: 0x0006BD5B File Offset: 0x00069F5B
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.curve == null)
			{
				yield return "curve is null.";
			}
			yield break;
		}

		// Token: 0x04006E57 RID: 28247
		private SimpleCurve curve;
	}
}
