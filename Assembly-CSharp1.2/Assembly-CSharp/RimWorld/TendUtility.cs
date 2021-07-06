using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000B89 RID: 2953
	public static class TendUtility
	{
		// Token: 0x06004565 RID: 17765 RVA: 0x001928C0 File Offset: 0x00190AC0
		public static void DoTend(Pawn doctor, Pawn patient, Medicine medicine)
		{
			if (!patient.health.HasHediffsNeedingTend(false))
			{
				return;
			}
			if (medicine != null && medicine.Destroyed)
			{
				Log.Warning("Tried to use destroyed medicine.", false);
				medicine = null;
			}
			float quality = TendUtility.CalculateBaseTendQuality(doctor, patient, (medicine != null) ? medicine.def : null);
			TendUtility.GetOptimalHediffsToTendWithSingleTreatment(patient, medicine != null, TendUtility.tmpHediffsToTend, null);
			float maxQuality = (medicine != null) ? medicine.def.GetStatValueAbstract(StatDefOf.MedicalQualityMax, null) : 0.7f;
			for (int i = 0; i < TendUtility.tmpHediffsToTend.Count; i++)
			{
				TendUtility.tmpHediffsToTend[i].Tended_NewTemp(quality, maxQuality, i);
			}
			if (doctor != null && doctor.Faction == Faction.OfPlayer && patient.Faction != doctor.Faction && !patient.IsPrisoner && patient.Faction != null)
			{
				patient.mindState.timesGuestTendedToByPlayer++;
			}
			if (doctor != null && doctor.IsColonistPlayerControlled)
			{
				patient.records.AccumulateStoryEvent(StoryEventDefOf.TendedByPlayer);
			}
			if (doctor != null && doctor.RaceProps.Humanlike && patient.RaceProps.Animal && RelationsUtility.TryDevelopBondRelation(doctor, patient, 0.004f) && doctor.Faction != null && doctor.Faction != patient.Faction)
			{
				InteractionWorker_RecruitAttempt.DoRecruit(doctor, patient, 1f, false);
			}
			patient.records.Increment(RecordDefOf.TimesTendedTo);
			if (doctor != null)
			{
				doctor.records.Increment(RecordDefOf.TimesTendedOther);
			}
			if (doctor == patient && !doctor.Dead)
			{
				doctor.mindState.Notify_SelfTended();
			}
			if (medicine != null)
			{
				if ((patient.Spawned || (doctor != null && doctor.Spawned)) && medicine != null && medicine.GetStatValue(StatDefOf.MedicalPotency, true) > ThingDefOf.MedicineIndustrial.GetStatValueAbstract(StatDefOf.MedicalPotency, null))
				{
					SoundDefOf.TechMedicineUsed.PlayOneShot(new TargetInfo(patient.Position, patient.Map, false));
				}
				if (medicine.stackCount > 1)
				{
					medicine.stackCount--;
					return;
				}
				if (!medicine.Destroyed)
				{
					medicine.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06004566 RID: 17766 RVA: 0x00192AC0 File Offset: 0x00190CC0
		public static float CalculateBaseTendQuality(Pawn doctor, Pawn patient, ThingDef medicine)
		{
			float medicinePotency = (medicine != null) ? medicine.GetStatValueAbstract(StatDefOf.MedicalPotency, null) : 0.3f;
			float medicineQualityMax = (medicine != null) ? medicine.GetStatValueAbstract(StatDefOf.MedicalQualityMax, null) : 0.7f;
			return TendUtility.CalculateBaseTendQuality(doctor, patient, medicinePotency, medicineQualityMax);
		}

		// Token: 0x06004567 RID: 17767 RVA: 0x00192B04 File Offset: 0x00190D04
		public static float CalculateBaseTendQuality(Pawn doctor, Pawn patient, float medicinePotency, float medicineQualityMax)
		{
			float num;
			if (doctor != null)
			{
				num = doctor.GetStatValue(StatDefOf.MedicalTendQuality, true);
			}
			else
			{
				num = 0.75f;
			}
			num *= medicinePotency;
			Building_Bed building_Bed = (patient != null) ? patient.CurrentBed() : null;
			if (building_Bed != null)
			{
				num += building_Bed.GetStatValue(StatDefOf.MedicalTendQualityOffset, true);
			}
			if (doctor == patient && doctor != null)
			{
				num *= 0.7f;
			}
			return Mathf.Clamp(num, 0f, medicineQualityMax);
		}

		// Token: 0x06004568 RID: 17768 RVA: 0x00192B6C File Offset: 0x00190D6C
		public static void GetOptimalHediffsToTendWithSingleTreatment(Pawn patient, bool usingMedicine, List<Hediff> outHediffsToTend, List<Hediff> tendableHediffsInTendPriorityOrder = null)
		{
			outHediffsToTend.Clear();
			TendUtility.tmpHediffs.Clear();
			if (tendableHediffsInTendPriorityOrder != null)
			{
				TendUtility.tmpHediffs.AddRange(tendableHediffsInTendPriorityOrder);
			}
			else
			{
				List<Hediff> hediffs = patient.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					if (hediffs[i].TendableNow(false))
					{
						TendUtility.tmpHediffs.Add(hediffs[i]);
					}
				}
				TendUtility.SortByTendPriority(TendUtility.tmpHediffs);
			}
			if (!TendUtility.tmpHediffs.Any<Hediff>())
			{
				return;
			}
			Hediff hediff = TendUtility.tmpHediffs[0];
			outHediffsToTend.Add(hediff);
			HediffCompProperties_TendDuration hediffCompProperties_TendDuration = hediff.def.CompProps<HediffCompProperties_TendDuration>();
			if (hediffCompProperties_TendDuration != null && hediffCompProperties_TendDuration.tendAllAtOnce)
			{
				for (int j = 0; j < TendUtility.tmpHediffs.Count; j++)
				{
					if (TendUtility.tmpHediffs[j] != hediff && TendUtility.tmpHediffs[j].def == hediff.def)
					{
						outHediffsToTend.Add(TendUtility.tmpHediffs[j]);
					}
				}
			}
			else if (hediff is Hediff_Injury && usingMedicine)
			{
				float num = hediff.Severity;
				for (int k = 0; k < TendUtility.tmpHediffs.Count; k++)
				{
					if (TendUtility.tmpHediffs[k] != hediff)
					{
						Hediff_Injury hediff_Injury = TendUtility.tmpHediffs[k] as Hediff_Injury;
						if (hediff_Injury != null)
						{
							float severity = hediff_Injury.Severity;
							if (num + severity <= 20f)
							{
								num += severity;
								outHediffsToTend.Add(hediff_Injury);
							}
						}
					}
				}
			}
			TendUtility.tmpHediffs.Clear();
		}

		// Token: 0x06004569 RID: 17769 RVA: 0x00192CF4 File Offset: 0x00190EF4
		public static void SortByTendPriority(List<Hediff> hediffs)
		{
			if (hediffs.Count <= 1)
			{
				return;
			}
			TendUtility.tmpHediffsWithTendPriority.Clear();
			for (int i = 0; i < hediffs.Count; i++)
			{
				TendUtility.tmpHediffsWithTendPriority.Add(new Pair<Hediff, float>(hediffs[i], hediffs[i].TendPriority));
			}
			TendUtility.tmpHediffsWithTendPriority.SortByDescending((Pair<Hediff, float> x) => x.Second, (Pair<Hediff, float> x) => x.First.Severity);
			hediffs.Clear();
			for (int j = 0; j < TendUtility.tmpHediffsWithTendPriority.Count; j++)
			{
				hediffs.Add(TendUtility.tmpHediffsWithTendPriority[j].First);
			}
			TendUtility.tmpHediffsWithTendPriority.Clear();
		}

		// Token: 0x04002EE8 RID: 12008
		public const float NoMedicinePotency = 0.3f;

		// Token: 0x04002EE9 RID: 12009
		public const float NoMedicineQualityMax = 0.7f;

		// Token: 0x04002EEA RID: 12010
		public const float NoDoctorTendQuality = 0.75f;

		// Token: 0x04002EEB RID: 12011
		public const float SelfTendQualityFactor = 0.7f;

		// Token: 0x04002EEC RID: 12012
		private const float ChanceToDevelopBondRelationOnTended = 0.004f;

		// Token: 0x04002EED RID: 12013
		private static List<Hediff> tmpHediffsToTend = new List<Hediff>();

		// Token: 0x04002EEE RID: 12014
		private static List<Hediff> tmpHediffs = new List<Hediff>();

		// Token: 0x04002EEF RID: 12015
		private static List<Pair<Hediff, float>> tmpHediffsWithTendPriority = new List<Pair<Hediff, float>>();
	}
}
