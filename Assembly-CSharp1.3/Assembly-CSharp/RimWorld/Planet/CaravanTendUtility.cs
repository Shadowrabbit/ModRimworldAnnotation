using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017AD RID: 6061
	public static class CaravanTendUtility
	{
		// Token: 0x06008C82 RID: 35970 RVA: 0x00327254 File Offset: 0x00325454
		public static void CheckTend(Caravan caravan)
		{
			for (int i = 0; i < caravan.pawns.Count; i++)
			{
				Pawn pawn = caravan.pawns[i];
				if (CaravanTendUtility.IsValidDoctorFor(pawn, null, caravan) && pawn.IsHashIntervalTick(1250))
				{
					CaravanTendUtility.TryTendToAnyPawn(caravan);
				}
			}
		}

		// Token: 0x06008C83 RID: 35971 RVA: 0x003272A4 File Offset: 0x003254A4
		public static void TryTendToAnyPawn(Caravan caravan)
		{
			CaravanTendUtility.FindPawnsNeedingTend(caravan, CaravanTendUtility.tmpPawnsNeedingTreatment);
			if (!CaravanTendUtility.tmpPawnsNeedingTreatment.Any<Pawn>())
			{
				return;
			}
			CaravanTendUtility.tmpPawnsNeedingTreatment.SortByDescending((Pawn x) => CaravanTendUtility.GetTendPriority(x));
			Pawn patient = null;
			Pawn pawn = null;
			for (int i = 0; i < CaravanTendUtility.tmpPawnsNeedingTreatment.Count; i++)
			{
				patient = CaravanTendUtility.tmpPawnsNeedingTreatment[i];
				pawn = CaravanTendUtility.FindBestDoctorFor(caravan, patient);
				if (pawn != null)
				{
					break;
				}
			}
			if (pawn == null)
			{
				return;
			}
			Medicine medicine = null;
			Pawn pawn2 = null;
			CaravanInventoryUtility.TryGetBestMedicine(caravan, patient, out medicine, out pawn2);
			TendUtility.DoTend(pawn, patient, medicine);
			if (medicine != null && medicine.Destroyed && pawn2 != null)
			{
				pawn2.inventory.innerContainer.Remove(medicine);
			}
			CaravanTendUtility.tmpPawnsNeedingTreatment.Clear();
		}

		// Token: 0x06008C84 RID: 35972 RVA: 0x00327370 File Offset: 0x00325570
		private static void FindPawnsNeedingTend(Caravan caravan, List<Pawn> outPawnsNeedingTend)
		{
			outPawnsNeedingTend.Clear();
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Pawn pawn = pawnsListForReading[i];
				if ((pawn.playerSettings == null || pawn.playerSettings.medCare > MedicalCareCategory.NoCare) && pawn.health.HasHediffsNeedingTend(false))
				{
					outPawnsNeedingTend.Add(pawn);
				}
			}
		}

		// Token: 0x06008C85 RID: 35973 RVA: 0x003273D0 File Offset: 0x003255D0
		private static Pawn FindBestDoctorFor(Caravan caravan, Pawn patient)
		{
			float num = 0f;
			Pawn pawn = null;
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Pawn pawn2 = pawnsListForReading[i];
				if (CaravanTendUtility.IsValidDoctorFor(pawn2, patient, caravan))
				{
					float statValue = pawn2.GetStatValue(StatDefOf.MedicalTendQuality, true);
					if (statValue > num || pawn == null)
					{
						num = statValue;
						pawn = pawn2;
					}
				}
			}
			return pawn;
		}

		// Token: 0x06008C86 RID: 35974 RVA: 0x00327430 File Offset: 0x00325630
		private static bool IsValidDoctorFor(Pawn doctor, Pawn patient, Caravan caravan)
		{
			return doctor.RaceProps.Humanlike && caravan.IsOwner(doctor) && (doctor != patient || (doctor.IsColonist && doctor.playerSettings.selfTend)) && !doctor.Downed && !doctor.InMentalState && (doctor.story == null || !doctor.WorkTypeIsDisabled(WorkTypeDefOf.Doctor));
		}

		// Token: 0x06008C87 RID: 35975 RVA: 0x0032749C File Offset: 0x0032569C
		private static float GetTendPriority(Pawn patient)
		{
			int num = HealthUtility.TicksUntilDeathDueToBloodLoss(patient);
			if (num < 15000)
			{
				if (patient.RaceProps.Humanlike)
				{
					return GenMath.LerpDouble(0f, 15000f, 5f, 4f, (float)num);
				}
				return GenMath.LerpDouble(0f, 15000f, 4f, 3f, (float)num);
			}
			else
			{
				int i = 0;
				while (i < patient.health.hediffSet.hediffs.Count)
				{
					Hediff hediff = patient.health.hediffSet.hediffs[i];
					HediffStage curStage = hediff.CurStage;
					if (((curStage != null && curStage.lifeThreatening) || hediff.def.lethalSeverity >= 0f) && hediff.TendableNow(false))
					{
						if (patient.RaceProps.Humanlike)
						{
							return 2.5f;
						}
						return 2f;
					}
					else
					{
						i++;
					}
				}
				if (patient.health.hediffSet.BleedRateTotal >= 0.0001f)
				{
					if (patient.RaceProps.Humanlike)
					{
						return 1.5f;
					}
					return 1f;
				}
				else
				{
					if (patient.RaceProps.Humanlike)
					{
						return 0.5f;
					}
					return 0f;
				}
			}
		}

		// Token: 0x04005924 RID: 22820
		private static List<Pawn> tmpPawnsNeedingTreatment = new List<Pawn>();

		// Token: 0x04005925 RID: 22821
		private const int TendIntervalTicks = 1250;
	}
}
