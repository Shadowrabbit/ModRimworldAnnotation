using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020006F5 RID: 1781
	public static class TendUtility
	{
		// Token: 0x06003196 RID: 12694 RVA: 0x00120604 File Offset: 0x0011E804
		public static void DoTend(Pawn doctor, Pawn patient, Medicine medicine)
		{
			if (!patient.health.HasHediffsNeedingTend(false))
			{
				return;
			}
			if (medicine != null && medicine.Destroyed)
			{
				Log.Warning("Tried to use destroyed medicine.");
				medicine = null;
			}
			float quality = TendUtility.CalculateBaseTendQuality(doctor, patient, (medicine != null) ? medicine.def : null);
			TendUtility.GetOptimalHediffsToTendWithSingleTreatment(patient, medicine != null, TendUtility.tmpHediffsToTend, null);
			float maxQuality = (medicine != null) ? medicine.def.GetStatValueAbstract(StatDefOf.MedicalQualityMax, null) : 0.7f;
			for (int i = 0; i < TendUtility.tmpHediffsToTend.Count; i++)
			{
				TendUtility.tmpHediffsToTend[i].Tended(quality, maxQuality, i);
			}
			if (doctor != null && doctor.Faction == Faction.OfPlayer && patient.Faction != doctor.Faction && !patient.IsPrisoner && patient.Faction != null)
			{
				patient.mindState.timesGuestTendedToByPlayer++;
			}
			if (doctor != null && doctor.RaceProps.Humanlike && patient.RaceProps.Animal && RelationsUtility.TryDevelopBondRelation(doctor, patient, 0.004f) && doctor.Faction != null && doctor.Faction != patient.Faction)
			{
				InteractionWorker_RecruitAttempt.DoRecruit(doctor, patient, false);
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
				}
				else if (!medicine.Destroyed)
				{
					medicine.Destroy(DestroyMode.Vanish);
				}
			}
			if (ModsConfig.IdeologyActive && doctor != null && doctor.Ideo != null)
			{
				Precept_Role role = doctor.Ideo.GetRole(doctor);
				if (role != null && role.def.roleEffects != null)
				{
					foreach (RoleEffect roleEffect in role.def.roleEffects)
					{
						roleEffect.Notify_Tended(doctor, patient);
					}
				}
			}
			if (doctor != null && doctor.Faction == Faction.OfPlayer && doctor != patient)
			{
				QuestUtility.SendQuestTargetSignals(patient.questTags, "PlayerTended", patient.Named("SUBJECT"));
			}
		}

		// Token: 0x06003197 RID: 12695 RVA: 0x00120890 File Offset: 0x0011EA90
		public static float CalculateBaseTendQuality(Pawn doctor, Pawn patient, ThingDef medicine)
		{
			float medicinePotency = (medicine != null) ? medicine.GetStatValueAbstract(StatDefOf.MedicalPotency, null) : 0.3f;
			float medicineQualityMax = (medicine != null) ? medicine.GetStatValueAbstract(StatDefOf.MedicalQualityMax, null) : 0.7f;
			return TendUtility.CalculateBaseTendQuality(doctor, patient, medicinePotency, medicineQualityMax);
		}

		// Token: 0x06003198 RID: 12696 RVA: 0x001208D4 File Offset: 0x0011EAD4
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

		// Token: 0x06003199 RID: 12697 RVA: 0x0012093C File Offset: 0x0011EB3C
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

		// Token: 0x0600319A RID: 12698 RVA: 0x00120AC4 File Offset: 0x0011ECC4
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

		// Token: 0x04001D8B RID: 7563
		public const float NoMedicinePotency = 0.3f;

		// Token: 0x04001D8C RID: 7564
		public const float NoMedicineQualityMax = 0.7f;

		// Token: 0x04001D8D RID: 7565
		public const float NoDoctorTendQuality = 0.75f;

		// Token: 0x04001D8E RID: 7566
		public const float SelfTendQualityFactor = 0.7f;

		// Token: 0x04001D8F RID: 7567
		private const float ChanceToDevelopBondRelationOnTended = 0.004f;

		// Token: 0x04001D90 RID: 7568
		private static List<Hediff> tmpHediffsToTend = new List<Hediff>();

		// Token: 0x04001D91 RID: 7569
		private static List<Hediff> tmpHediffs = new List<Hediff>();

		// Token: 0x04001D92 RID: 7570
		private static List<Pair<Hediff, float>> tmpHediffsWithTendPriority = new List<Pair<Hediff, float>>();
	}
}
