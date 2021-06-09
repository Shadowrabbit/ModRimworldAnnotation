using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D3F RID: 7487
	public class StatPart_Mood : StatPart
	{
		// Token: 0x0600A2AD RID: 41645 RVA: 0x0006C13C File Offset: 0x0006A33C
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.factorFromMoodCurve == null)
			{
				yield return "curve is null.";
			}
			yield break;
		}

		// Token: 0x0600A2AE RID: 41646 RVA: 0x002F5A78 File Offset: 0x002F3C78
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && this.ActiveFor(pawn))
				{
					val *= this.FactorFromMood(pawn);
				}
			}
		}

		// Token: 0x0600A2AF RID: 41647 RVA: 0x002F5AB4 File Offset: 0x002F3CB4
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && this.ActiveFor(pawn))
				{
					return "StatsReport_MoodMultiplier".Translate(pawn.needs.mood.CurLevel.ToStringPercent()) + ": x" + this.FactorFromMood(pawn).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x0600A2B0 RID: 41648 RVA: 0x0003E9D0 File Offset: 0x0003CBD0
		private bool ActiveFor(Pawn pawn)
		{
			return pawn.needs.mood != null;
		}

		// Token: 0x0600A2B1 RID: 41649 RVA: 0x0006C14C File Offset: 0x0006A34C
		private float FactorFromMood(Pawn pawn)
		{
			return this.factorFromMoodCurve.Evaluate(pawn.needs.mood.CurLevel);
		}

		// Token: 0x04006E7D RID: 28285
		private SimpleCurve factorFromMoodCurve;
	}
}
