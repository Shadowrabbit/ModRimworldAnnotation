using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002D3 RID: 723
	public class HediffGiver_Birthday : HediffGiver
	{
		// Token: 0x0600138D RID: 5005 RVA: 0x0006EEA8 File Offset: 0x0006D0A8
		public void TryApplyAndSimulateSeverityChange(Pawn pawn, float gotAtAge, bool tryNotToKillPawn)
		{
			HediffGiver_Birthday.addedHediffs.Clear();
			if (!base.TryApply(pawn, HediffGiver_Birthday.addedHediffs))
			{
				return;
			}
			if (this.averageSeverityPerDayBeforeGeneration != 0f)
			{
				float num = (pawn.ageTracker.AgeBiologicalYearsFloat - gotAtAge) * 60f;
				if (num < 0f)
				{
					Log.Error(string.Concat(new object[]
					{
						"daysPassed < 0, pawn=",
						pawn,
						", gotAtAge=",
						gotAtAge
					}));
					return;
				}
				for (int i = 0; i < HediffGiver_Birthday.addedHediffs.Count; i++)
				{
					this.SimulateSeverityChange(pawn, HediffGiver_Birthday.addedHediffs[i], num, tryNotToKillPawn);
				}
			}
			HediffGiver_Birthday.addedHediffs.Clear();
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x0006EF58 File Offset: 0x0006D158
		private void SimulateSeverityChange(Pawn pawn, Hediff hediff, float daysPassed, bool tryNotToKillPawn)
		{
			float num = this.averageSeverityPerDayBeforeGeneration * daysPassed;
			num *= Rand.Range(0.5f, 1.4f);
			num += hediff.def.initialSeverity;
			if (tryNotToKillPawn)
			{
				this.AvoidLifeThreateningStages(ref num, hediff.def.stages);
			}
			hediff.Severity = num;
			pawn.health.Notify_HediffChanged(hediff);
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x0006EFB8 File Offset: 0x0006D1B8
		private void AvoidLifeThreateningStages(ref float severity, List<HediffStage> stages)
		{
			if (stages.NullOrEmpty<HediffStage>())
			{
				return;
			}
			int num = -1;
			for (int i = 0; i < stages.Count; i++)
			{
				if (stages[i].lifeThreatening)
				{
					num = i;
					break;
				}
			}
			if (num >= 0)
			{
				if (num == 0)
				{
					severity = Mathf.Min(severity, stages[num].minSeverity);
					return;
				}
				severity = Mathf.Min(severity, (stages[num].minSeverity + stages[num - 1].minSeverity) / 2f);
			}
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x0006F03C File Offset: 0x0006D23C
		public float DebugChanceToHaveAtAge(Pawn pawn, int age)
		{
			float num = 1f;
			for (int i = 1; i <= age; i++)
			{
				float x = (float)i / pawn.RaceProps.lifeExpectancy;
				num *= 1f - this.ageFractionChanceCurve.Evaluate(x);
			}
			return 1f - num;
		}

		// Token: 0x04000E66 RID: 3686
		public SimpleCurve ageFractionChanceCurve;

		// Token: 0x04000E67 RID: 3687
		public float averageSeverityPerDayBeforeGeneration;

		// Token: 0x04000E68 RID: 3688
		private static List<Hediff> addedHediffs = new List<Hediff>();
	}
}
