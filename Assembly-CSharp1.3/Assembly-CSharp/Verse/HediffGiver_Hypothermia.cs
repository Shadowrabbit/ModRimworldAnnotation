using System;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002D8 RID: 728
	public class HediffGiver_Hypothermia : HediffGiver
	{
		// Token: 0x0600139C RID: 5020 RVA: 0x0006F474 File Offset: 0x0006D674
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			float ambientTemperature = pawn.AmbientTemperature;
			FloatRange floatRange = pawn.ComfortableTemperatureRange();
			FloatRange floatRange2 = pawn.SafeTemperatureRange();
			HediffSet hediffSet = pawn.health.hediffSet;
			HediffDef hediffDef = (pawn.RaceProps.FleshType == FleshTypeDefOf.Insectoid) ? this.hediffInsectoid : this.hediff;
			Hediff firstHediffOfDef = hediffSet.GetFirstHediffOfDef(hediffDef, false);
			if (ambientTemperature < floatRange2.min)
			{
				float num = Mathf.Abs(ambientTemperature - floatRange2.min) * 6.45E-05f;
				num = Mathf.Max(num, 0.00075f);
				HealthUtility.AdjustSeverity(pawn, hediffDef, num);
				if (pawn.Dead)
				{
					return;
				}
			}
			if (firstHediffOfDef != null)
			{
				if (ambientTemperature > floatRange.min)
				{
					float num2 = firstHediffOfDef.Severity * 0.027f;
					num2 = Mathf.Clamp(num2, 0.0015f, 0.015f);
					firstHediffOfDef.Severity -= num2;
					return;
				}
				if (pawn.RaceProps.FleshType != FleshTypeDefOf.Insectoid && ambientTemperature < 0f && firstHediffOfDef.Severity > 0.37f)
				{
					float num3 = 0.025f * firstHediffOfDef.Severity;
					if (Rand.Value < num3)
					{
						BodyPartRecord bodyPartRecord;
						if ((from x in pawn.RaceProps.body.AllPartsVulnerableToFrostbite
						where !hediffSet.PartIsMissing(x)
						select x).TryRandomElementByWeight((BodyPartRecord x) => x.def.frostbiteVulnerability, out bodyPartRecord))
						{
							int num4 = Mathf.CeilToInt((float)bodyPartRecord.def.hitPoints * 0.5f);
							DamageInfo dinfo = new DamageInfo(DamageDefOf.Frostbite, (float)num4, 0f, -1f, null, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true);
							pawn.TakeDamage(dinfo);
						}
					}
				}
			}
		}

		// Token: 0x04000E70 RID: 3696
		public HediffDef hediffInsectoid;
	}
}
