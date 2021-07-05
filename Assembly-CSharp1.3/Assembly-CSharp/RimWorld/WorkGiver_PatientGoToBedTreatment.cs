using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000852 RID: 2130
	public class WorkGiver_PatientGoToBedTreatment : WorkGiver_PatientGoToBedRecuperate
	{
		// Token: 0x06003850 RID: 14416 RVA: 0x0013CC03 File Offset: 0x0013AE03
		public override Job NonScanJob(Pawn pawn)
		{
			if (!HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn))
			{
				return null;
			}
			if (!this.AnyAvailableDoctorFor(pawn))
			{
				return null;
			}
			return base.NonScanJob(pawn);
		}

		// Token: 0x06003851 RID: 14417 RVA: 0x0013CC24 File Offset: 0x0013AE24
		private bool AnyAvailableDoctorFor(Pawn pawn)
		{
			Map mapHeld = pawn.MapHeld;
			if (mapHeld == null || pawn.Faction == null)
			{
				return false;
			}
			List<Pawn> list = mapHeld.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn2 = list[i];
				if (pawn2 != pawn && pawn2.RaceProps.Humanlike && !pawn2.Downed && pawn2.Awake() && !pawn2.InBed() && !pawn2.InMentalState && !pawn2.IsPrisoner && pawn2.workSettings != null && pawn2.workSettings.WorkIsActive(WorkTypeDefOf.Doctor) && pawn2.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) && pawn2.CanReach(pawn, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return true;
				}
			}
			return false;
		}
	}
}
