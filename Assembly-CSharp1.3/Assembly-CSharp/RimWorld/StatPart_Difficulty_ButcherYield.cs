using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014CC RID: 5324
	public class StatPart_Difficulty_ButcherYield : StatPart
	{
		// Token: 0x06007F08 RID: 32520 RVA: 0x002CF3D0 File Offset: 0x002CD5D0
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num = (Find.Storyteller != null) ? Find.Storyteller.difficulty.butcherYieldFactor : 1f;
			val *= num;
		}

		// Token: 0x06007F09 RID: 32521 RVA: 0x002CF404 File Offset: 0x002CD604
		public override string ExplanationPart(StatRequest req)
		{
			return "StatsReport_DifficultyMultiplier".Translate(Find.Storyteller.difficultyDef.label) + ": " + Find.Storyteller.difficulty.butcherYieldFactor.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor);
		}
	}
}
