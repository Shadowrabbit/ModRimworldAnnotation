using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014D7 RID: 5335
	public class StatPart_Mood : StatPart
	{
		// Token: 0x06007F35 RID: 32565 RVA: 0x002CFB69 File Offset: 0x002CDD69
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.factorFromMoodCurve == null)
			{
				yield return "curve is null.";
			}
			yield break;
		}

		// Token: 0x06007F36 RID: 32566 RVA: 0x002CFB7C File Offset: 0x002CDD7C
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

		// Token: 0x06007F37 RID: 32567 RVA: 0x002CFBB8 File Offset: 0x002CDDB8
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

		// Token: 0x06007F38 RID: 32568 RVA: 0x002CFC2D File Offset: 0x002CDE2D
		private bool ActiveFor(Pawn pawn)
		{
			return pawn.needs.mood != null;
		}

		// Token: 0x06007F39 RID: 32569 RVA: 0x002CFC3D File Offset: 0x002CDE3D
		private float FactorFromMood(Pawn pawn)
		{
			return this.factorFromMoodCurve.Evaluate(pawn.needs.mood.CurLevel);
		}

		// Token: 0x04004F72 RID: 20338
		private SimpleCurve factorFromMoodCurve;
	}
}
