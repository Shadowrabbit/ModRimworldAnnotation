using System;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200156A RID: 5482
	public static class HealthAIUtility
	{
		// Token: 0x060081BF RID: 33215 RVA: 0x002DDD12 File Offset: 0x002DBF12
		public static bool ShouldSeekMedicalRestUrgent(Pawn pawn)
		{
			return pawn.Downed || pawn.health.HasHediffsNeedingTend(false) || HealthAIUtility.ShouldHaveSurgeryDoneNow(pawn);
		}

		// Token: 0x060081C0 RID: 33216 RVA: 0x002DDD32 File Offset: 0x002DBF32
		public static bool ShouldSeekMedicalRest(Pawn pawn)
		{
			return HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn) || pawn.health.hediffSet.HasTendedAndHealingInjury() || pawn.health.hediffSet.HasImmunizableNotImmuneHediff();
		}

		// Token: 0x060081C1 RID: 33217 RVA: 0x002DDD60 File Offset: 0x002DBF60
		public static bool ShouldBeTendedNowByPlayerUrgent(Pawn pawn)
		{
			return HealthAIUtility.ShouldBeTendedNowByPlayer(pawn) && HealthUtility.TicksUntilDeathDueToBloodLoss(pawn) < 45000;
		}

		// Token: 0x060081C2 RID: 33218 RVA: 0x002DDD79 File Offset: 0x002DBF79
		public static bool ShouldBeTendedNowByPlayer(Pawn pawn)
		{
			return pawn.playerSettings != null && HealthAIUtility.ShouldEverReceiveMedicalCareFromPlayer(pawn) && pawn.health.HasHediffsNeedingTendByPlayer(false);
		}

		// Token: 0x060081C3 RID: 33219 RVA: 0x002DDD9B File Offset: 0x002DBF9B
		public static bool ShouldEverReceiveMedicalCareFromPlayer(Pawn pawn)
		{
			return (pawn.playerSettings == null || pawn.playerSettings.medCare != MedicalCareCategory.NoCare) && (pawn.guest == null || pawn.guest.interactionMode != PrisonerInteractionModeDefOf.Execution) && !pawn.ShouldBeSlaughtered();
		}

		// Token: 0x060081C4 RID: 33220 RVA: 0x002DDDDB File Offset: 0x002DBFDB
		public static bool ShouldHaveSurgeryDoneNow(Pawn pawn)
		{
			return pawn.health.surgeryBills.AnyShouldDoNow;
		}

		// Token: 0x060081C5 RID: 33221 RVA: 0x002DDDF0 File Offset: 0x002DBFF0
		public static Thing FindBestMedicine(Pawn healer, Pawn patient, bool onlyUseInventory = false)
		{
			if (patient.playerSettings == null || patient.playerSettings.medCare <= MedicalCareCategory.NoMeds)
			{
				return null;
			}
			if (Medicine.GetMedicineCountToFullyHeal(patient) <= 0)
			{
				return null;
			}
			Func<Thing, float> func = (Thing t) => t.def.GetStatValueAbstract(StatDefOf.MedicalPotency, null);
			Predicate<Thing> validator = (Thing m) => !m.IsForbidden(healer) && patient.playerSettings.medCare.AllowsMedicine(m.def) && healer.CanReserve(m, 10, 1, null, false);
			Thing thing = (from t in healer.inventory.innerContainer
			where t.def.IsMedicine && validator(t)
			select t).OrderBy(func).FirstOrDefault<Thing>();
			if (thing != null)
			{
				return thing;
			}
			if (onlyUseInventory)
			{
				return null;
			}
			return GenClosest.ClosestThing_Global_Reachable(patient.Position, patient.Map, patient.Map.listerThings.ThingsInGroup(ThingRequestGroup.Medicine), PathEndMode.ClosestTouch, TraverseParms.For(healer, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, validator, func);
		}
	}
}
