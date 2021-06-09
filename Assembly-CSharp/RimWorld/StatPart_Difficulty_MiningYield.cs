using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D34 RID: 7476
	public class StatPart_Difficulty_MiningYield : StatPart
	{
		// Token: 0x0600A27B RID: 41595 RVA: 0x0006BF17 File Offset: 0x0006A117
		public override void TransformValue(StatRequest req, ref float val)
		{
			val *= Find.Storyteller.difficultyValues.mineYieldFactor;
		}

		// Token: 0x0600A27C RID: 41596 RVA: 0x002F55B0 File Offset: 0x002F37B0
		public override string ExplanationPart(StatRequest req)
		{
			return "StatsReport_DifficultyMultiplier".Translate(Find.Storyteller.difficulty.label) + ": " + Find.Storyteller.difficultyValues.mineYieldFactor.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor);
		}
	}
}
