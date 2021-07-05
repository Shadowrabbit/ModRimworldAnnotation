using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014FD RID: 5373
	public class StatWorker_PsyfocusCost : StatWorker
	{
		// Token: 0x0600800E RID: 32782 RVA: 0x002D5984 File Offset: 0x002D3B84
		public override string GetStatDrawEntryLabel(StatDef stat, float value, ToStringNumberSense numberSense, StatRequest optionalReq, bool finalized = true)
		{
			if (optionalReq.ForAbility)
			{
				AbilityDef abilityDef = optionalReq.AbilityDef;
				if (abilityDef.AnyCompOverridesPsyfocusCost)
				{
					if (abilityDef.PsyfocusCostRange.Span > 1E-45f)
					{
						return (abilityDef.PsyfocusCostRange.min * 100f).ToString("0.##") + "-" + stat.ValueToString(abilityDef.PsyfocusCostRange.max, numberSense, finalized);
					}
					return stat.ValueToString(abilityDef.PsyfocusCostRange.max, numberSense, finalized);
				}
			}
			return base.GetStatDrawEntryLabel(stat, value, numberSense, optionalReq, finalized);
		}

		// Token: 0x0600800F RID: 32783 RVA: 0x002D5A20 File Offset: 0x002D3C20
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			if (req.ForAbility)
			{
				foreach (AbilityCompProperties abilityCompProperties in req.AbilityDef.comps)
				{
					if (abilityCompProperties.OverridesPsyfocusCost)
					{
						return abilityCompProperties.PsyfocusCostExplanation;
					}
				}
			}
			return base.GetExplanationUnfinalized(req, numberSense);
		}
	}
}
