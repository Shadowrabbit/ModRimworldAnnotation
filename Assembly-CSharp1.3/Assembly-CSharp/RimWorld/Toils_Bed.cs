using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000757 RID: 1879
	public static class Toils_Bed
	{
		// Token: 0x0600340F RID: 13327 RVA: 0x001275C8 File Offset: 0x001257C8
		public static Toil GotoBed(TargetIndex bedIndex)
		{
			Toil gotoBed = new Toil();
			gotoBed.initAction = delegate()
			{
				Pawn actor = gotoBed.actor;
				Building_Bed bed = (Building_Bed)actor.CurJob.GetTarget(bedIndex).Thing;
				IntVec3 bedSleepingSlotPosFor = RestUtility.GetBedSleepingSlotPosFor(actor, bed);
				if (actor.Position == bedSleepingSlotPosFor)
				{
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				actor.pather.StartPath(bedSleepingSlotPosFor, PathEndMode.OnCell);
			};
			gotoBed.tickAction = delegate()
			{
				Pawn actor = gotoBed.actor;
				Building_Bed building_Bed = (Building_Bed)actor.CurJob.GetTarget(bedIndex).Thing;
				Pawn curOccupantAt = building_Bed.GetCurOccupantAt(actor.pather.Destination.Cell);
				if (curOccupantAt != null && curOccupantAt != actor)
				{
					actor.pather.StartPath(RestUtility.GetBedSleepingSlotPosFor(actor, building_Bed), PathEndMode.OnCell);
				}
			};
			gotoBed.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			gotoBed.FailOnBedNoLongerUsable(bedIndex);
			return gotoBed;
		}

		// Token: 0x06003410 RID: 13328 RVA: 0x00127640 File Offset: 0x00125840
		public static Toil ClaimBedIfNonMedical(TargetIndex ind, TargetIndex claimantIndex = TargetIndex.None)
		{
			Toil claim = new Toil();
			claim.initAction = delegate()
			{
				Pawn actor = claim.GetActor();
				Pawn pawn = (claimantIndex == TargetIndex.None) ? actor : ((Pawn)actor.CurJob.GetTarget(claimantIndex).Thing);
				if (pawn.ownership != null)
				{
					pawn.ownership.ClaimBedIfNonMedical((Building_Bed)actor.CurJob.GetTarget(ind).Thing);
				}
			};
			claim.FailOnDespawnedOrNull(ind);
			return claim;
		}

		// Token: 0x06003411 RID: 13329 RVA: 0x0012769C File Offset: 0x0012589C
		public static T FailOnNonMedicalBedNotOwned<T>(this T f, TargetIndex bedIndex, TargetIndex claimantIndex = TargetIndex.None) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn actor = f.GetActor();
				Pawn pawn = (claimantIndex == TargetIndex.None) ? actor : ((Pawn)actor.CurJob.GetTarget(claimantIndex).Thing);
				if (pawn.ownership != null)
				{
					Building_Bed building_Bed = (Building_Bed)actor.CurJob.GetTarget(bedIndex).Thing;
					if (building_Bed.Medical)
					{
						if ((!pawn.InBed() || pawn.CurrentBed() != building_Bed) && !building_Bed.AnyUnoccupiedSleepingSlot)
						{
							return JobCondition.Incompletable;
						}
					}
					else
					{
						if (!building_Bed.OwnersForReading.Contains(pawn))
						{
							return JobCondition.Incompletable;
						}
						if (pawn.InBed() && pawn.CurrentBed() == building_Bed)
						{
							int curOccupantSlotIndex = building_Bed.GetCurOccupantSlotIndex(pawn);
							if (curOccupantSlotIndex >= building_Bed.OwnersForReading.Count || building_Bed.OwnersForReading[curOccupantSlotIndex] != pawn)
							{
								return JobCondition.Incompletable;
							}
						}
					}
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06003412 RID: 13330 RVA: 0x001276E8 File Offset: 0x001258E8
		public static void FailOnBedNoLongerUsable(this Toil toil, TargetIndex bedIndex)
		{
			toil.FailOnDespawnedOrNull(bedIndex);
			toil.FailOn(() => ((Building_Bed)toil.actor.CurJob.GetTarget(bedIndex).Thing).IsBurning());
			toil.FailOn(() => ((Building_Bed)toil.actor.CurJob.GetTarget(bedIndex).Thing).ForPrisoners != toil.actor.IsPrisoner);
			toil.FailOnNonMedicalBedNotOwned(bedIndex, TargetIndex.None);
			toil.FailOn(() => !HealthAIUtility.ShouldSeekMedicalRest(toil.actor) && !HealthAIUtility.ShouldSeekMedicalRestUrgent(toil.actor) && ((Building_Bed)toil.actor.CurJob.GetTarget(bedIndex).Thing).Medical);
			toil.FailOn(() => toil.actor.IsColonist && !toil.actor.CurJob.ignoreForbidden && !toil.actor.Downed && toil.actor.CurJob.GetTarget(bedIndex).Thing.IsForbidden(toil.actor));
		}

		// Token: 0x06003413 RID: 13331 RVA: 0x00127790 File Offset: 0x00125990
		public static Toil TuckIntoBed(Building_Bed bed, Pawn taker, Pawn takee, bool rescued = false)
		{
			return new Toil
			{
				initAction = delegate()
				{
					IntVec3 position = bed.Position;
					Thing thing;
					taker.carryTracker.TryDropCarriedThing(position, ThingPlaceMode.Direct, out thing, null);
					if (!bed.Destroyed && (bed.OwnersForReading.Contains(takee) || (bed.Medical && bed.AnyUnoccupiedSleepingSlot) || takee.ownership == null))
					{
						takee.jobs.Notify_TuckedIntoBed(bed);
						if (rescued)
						{
							takee.relations.Notify_RescuedBy(taker);
						}
						takee.mindState.Notify_TuckedIntoBed();
					}
					if (takee.IsPrisonerOfColony)
					{
						LessonAutoActivator.TeachOpportunity(ConceptDefOf.PrisonerTab, takee, OpportunityType.GoodToKnow);
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}
	}
}
