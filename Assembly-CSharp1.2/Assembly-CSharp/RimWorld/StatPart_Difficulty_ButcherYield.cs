using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D33 RID: 7475
	public class StatPart_Difficulty_ButcherYield : StatPart
	{
		// Token: 0x0600A278 RID: 41592 RVA: 0x002F5524 File Offset: 0x002F3724
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num = (Find.Storyteller != null) ? Find.Storyteller.difficultyValues.butcherYieldFactor : 1f;
			val *= num;
		}

		// Token: 0x0600A279 RID: 41593 RVA: 0x002F5558 File Offset: 0x002F3758
		public override string ExplanationPart(StatRequest req)
		{
			return "StatsReport_DifficultyMultiplier".Translate(Find.Storyteller.difficulty.label) + ": " + Find.Storyteller.difficultyValues.butcherYieldFactor.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor);
		}
	}
}
