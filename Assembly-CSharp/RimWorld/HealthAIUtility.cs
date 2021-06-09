using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001DF0 RID: 7664
	public static class HealthAIUtility
	{
		// Token: 0x0600A61D RID: 42525 RVA: 0x0006DDA1 File Offset: 0x0006BFA1
		public static bool ShouldSeekMedicalRestUrgent(Pawn pawn)
		{
			return pawn.Downed || pawn.health.HasHediffsNeedingTend(false) || HealthAIUtility.ShouldHaveSurgeryDoneNow(pawn);
		}

		// Token: 0x0600A61E RID: 42526 RVA: 0x0006DDC1 File Offset: 0x0006BFC1
		public static bool ShouldSeekMedicalRest(Pawn pawn)
		{
			return HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn) || pawn.health.hediffSet.HasTendedAndHealingInjury() || pawn.health.hediffSet.HasImmunizableNotImmuneHediff();
		}

		// Token: 0x0600A61F RID: 42527 RVA: 0x0006DDEF File Offset: 0x0006BFEF
		public static bool ShouldBeTendedNowByPlayerUrgent(Pawn pawn)
		{
			return HealthAIUtility.ShouldBeTendedNowByPlayer(pawn) && HealthUtility.TicksUntilDeathDueToBloodLoss(pawn) < 45000;
		}

		// Token: 0x0600A620 RID: 42528 RVA: 0x0006DE08 File Offset: 0x0006C008
		public static bool ShouldBeTendedNowByPlayer(Pawn pawn)
		{
			return pawn.playerSettings != null && HealthAIUtility.ShouldEverReceiveMedicalCareFromPlayer(pawn) && pawn.health.HasHediffsNeedingTendByPlayer(false);
		}

		// Token: 0x0600A621 RID: 42529 RVA: 0x00302D28 File Offset: 0x00300F28
		public static bool ShouldEverReceiveMedicalCareFromPlayer(Pawn pawn)
		{
			if (pawn.playerSettings != null && pawn.playerSettings.medCare == MedicalCareCategory.NoCare)
			{
				return false;
			}
			if (pawn.guest != null && pawn.guest.interactionMode == PrisonerInteractionModeDefOf.Execution)
			{
				return false;
			}
			Map map = pawn.Map;
			return ((map != null) ? map.designationManager.DesignationOn(pawn, DesignationDefOf.Slaughter) : null) == null;
		}

		// Token: 0x0600A622 RID: 42530 RVA: 0x0006DE2A File Offset: 0x0006C02A
		public static bool ShouldHaveSurgeryDoneNow(Pawn pawn)
		{
			return pawn.health.surgeryBills.AnyShouldDoNow;
		}

		// Token: 0x0600A623 RID: 42531 RVA: 0x00302D8C File Offset: 0x00300F8C
		public static Thing FindBestMedicine(Pawn healer, Pawn patient)
		{
			if (patient.playerSettings == null || patient.playerSettings.medCare <= MedicalCareCategory.NoMeds)
			{
				return null;
			}
			if (Medicine.GetMedicineCountToFullyHeal(patient) <= 0)
			{
				return null;
			}
			Predicate<Thing> validator = (Thing m) => !m.IsForbidden(healer) && patient.playerSettings.medCare.AllowsMedicine(m.def) && healer.CanReserve(m, 10, 1, null, false);
			Func<Thing, float> priorityGetter = (Thing t) => t.def.GetStatValueAbstract(StatDefOf.MedicalPotency, null);
			return GenClosest.ClosestThing_Global_Reachable(patient.Position, patient.Map, patient.Map.listerThings.ThingsInGroup(ThingRequestGroup.Medicine), PathEndMode.ClosestTouch, TraverseParms.For(healer, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, priorityGetter);
		}
	}
}
