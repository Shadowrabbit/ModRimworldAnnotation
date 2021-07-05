using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014C2 RID: 5314
	public class StatPart_Age : StatPart
	{
		// Token: 0x06007EDD RID: 32477 RVA: 0x002CEA64 File Offset: 0x002CCC64
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

		// Token: 0x06007EDE RID: 32478 RVA: 0x002CEAA0 File Offset: 0x002CCCA0
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

		// Token: 0x06007EDF RID: 32479 RVA: 0x002CEB0A File Offset: 0x002CCD0A
		private float AgeMultiplier(Pawn pawn)
		{
			return this.curve.Evaluate((float)pawn.ageTracker.AgeBiologicalYears / pawn.RaceProps.lifeExpectancy);
		}

		// Token: 0x06007EE0 RID: 32480 RVA: 0x002CEB2F File Offset: 0x002CCD2F
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.curve == null)
			{
				yield return "curve is null.";
			}
			yield break;
		}

		// Token: 0x04004F5F RID: 20319
		private SimpleCurve curve;
	}
}
