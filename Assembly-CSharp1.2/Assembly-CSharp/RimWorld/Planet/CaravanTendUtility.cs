using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020020FE RID: 8446
	public static class CaravanTendUtility
	{
		// Token: 0x0600B364 RID: 45924 RVA: 0x0033FE20 File Offset: 0x0033E020
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

		// Token: 0x0600B365 RID: 45925 RVA: 0x0033FE70 File Offset: 0x0033E070
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

		// Token: 0x0600B366 RID: 45926 RVA: 0x0033FF3C File Offset: 0x0033E13C
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

		// Token: 0x0600B367 RID: 45927 RVA: 0x0033FF9C File Offset: 0x0033E19C
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

		// Token: 0x0600B368 RID: 45928 RVA: 0x0033FFFC File Offset: 0x0033E1FC
		private static bool IsValidDoctorFor(Pawn doctor, Pawn patient, Caravan caravan)
		{
			return doctor.RaceProps.Humanlike && caravan.IsOwner(doctor) && (doctor != patient || (doctor.IsColonist && doctor.playerSettings.selfTend)) && !doctor.Downed && !doctor.InMentalState && (doctor.story == null || !doctor.WorkTypeIsDisabled(WorkTypeDefOf.Doctor));
		}

		// Token: 0x0600B369 RID: 45929 RVA: 0x00340068 File Offset: 0x0033E268
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

		// Token: 0x04007B43 RID: 31555
		private static List<Pawn> tmpPawnsNeedingTreatment = new List<Pawn>();

		// Token: 0x04007B44 RID: 31556
		private const int TendIntervalTicks = 1250;
	}
}
