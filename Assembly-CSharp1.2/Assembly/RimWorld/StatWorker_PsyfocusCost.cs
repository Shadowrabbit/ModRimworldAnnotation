using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D6A RID: 7530
	public class StatWorker_PsyfocusCost : StatWorker
	{
		// Token: 0x0600A3B8 RID: 41912 RVA: 0x002FADC0 File Offset: 0x002F8FC0
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

		// Token: 0x0600A3B9 RID: 41913 RVA: 0x002FAE5C File Offset: 0x002F905C
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
