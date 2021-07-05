using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D92 RID: 3474
	public class WorkGiver_PatientGoToBedTreatment : WorkGiver_PatientGoToBedRecuperate
	{
		// Token: 0x06004F39 RID: 20281 RVA: 0x00037BFD File Offset: 0x00035DFD
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

		// Token: 0x06004F3A RID: 20282 RVA: 0x001B4448 File Offset: 0x001B2648
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
				if (pawn2 != pawn && pawn2.RaceProps.Humanlike && !pawn2.Downed && pawn2.Awake() && !pawn2.InBed() && !pawn2.InMentalState && !pawn2.IsPrisoner && pawn2.workSettings != null && pawn2.workSettings.WorkIsActive(WorkTypeDefOf.Doctor) && pawn2.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) && pawn2.CanReach(pawn, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					return true;
				}
			}
			return false;
		}
	}
}
