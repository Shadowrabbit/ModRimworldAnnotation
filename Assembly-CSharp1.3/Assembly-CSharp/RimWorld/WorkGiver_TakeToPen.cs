using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200080B RID: 2059
	public class WorkGiver_TakeToPen : WorkGiver_InteractAnimal
	{
		// Token: 0x060036EF RID: 14063 RVA: 0x00137207 File Offset: 0x00135407
		public WorkGiver_TakeToPen()
		{
			this.canInteractWhileSleeping = true;
		}

		// Token: 0x060036F0 RID: 14064 RVA: 0x00136C3C File Offset: 0x00134E3C
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
		}

		// Token: 0x060036F1 RID: 14065 RVA: 0x00137218 File Offset: 0x00135418
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null)
			{
				return null;
			}
			if (this.targetRoamingAnimals && pawn2.MentalStateDef != MentalStateDefOf.Roaming)
			{
				return null;
			}
			if (t.Map.designationManager.DesignationOn(t, DesignationDefOf.ReleaseAnimalToWild) != null)
			{
				return null;
			}
			string text;
			CompAnimalPenMarker penAnimalShouldBeTakenTo = AnimalPenUtility.GetPenAnimalShouldBeTakenTo(pawn, pawn2, out text, forced, this.canInteractWhileSleeping, this.allowUnenclosedPens, false, this.ropingPriority);
			if (penAnimalShouldBeTakenTo != null)
			{
				Job job = WorkGiver_TakeToPen.MakeJob(pawn, pawn2, penAnimalShouldBeTakenTo, this.allowUnenclosedPens, this.ropingPriority, out text);
				if (job != null)
				{
					return job;
				}
			}
			else if (AnimalPenUtility.IsUnnecessarilyRoped(pawn2))
			{
				Job job2 = this.MakeUnropeJob(pawn, pawn2, forced, out text);
				if (job2 != null)
				{
					return job2;
				}
			}
			if (text != null)
			{
				JobFailReason.Is(text, null);
			}
			return null;
		}

		// Token: 0x060036F2 RID: 14066 RVA: 0x001372C8 File Offset: 0x001354C8
		private Job MakeUnropeJob(Pawn roper, Pawn animal, bool forced, out string jobFailReason)
		{
			jobFailReason = null;
			if (AnimalPenUtility.RopeAttachmentInteractionCell(roper, animal) == IntVec3.Invalid)
			{
				jobFailReason = "CantRopeAnimalCantTouch".Translate();
				return null;
			}
			if (!forced && !roper.CanReserve(animal, 1, -1, null, false))
			{
				return null;
			}
			if (!roper.CanReach(animal, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Unrope, animal);
		}

		// Token: 0x060036F3 RID: 14067 RVA: 0x0013733C File Offset: 0x0013553C
		public static Job MakeJob(Pawn pawn, Pawn animal, CompAnimalPenMarker targetPenMarker, bool allowUnenclosedPens, RopingPriority ropingPriority, out string jobFailReason)
		{
			jobFailReason = null;
			IntVec3 c = AnimalPenUtility.FindPlaceInPenToStand(targetPenMarker, pawn);
			if (!c.IsValid)
			{
				jobFailReason = "CantRopeAnimalNoSpace".Translate();
				return null;
			}
			Job job = JobMaker.MakeJob(targetPenMarker.PenState.Enclosed ? JobDefOf.RopeToPen : JobDefOf.RopeRoamerToUnenclosedPen, animal, c, targetPenMarker.parent);
			job.ropingPriority = ropingPriority;
			job.ropeToUnenclosedPens = allowUnenclosedPens;
			return job;
		}

		// Token: 0x04001F03 RID: 7939
		protected bool targetRoamingAnimals;

		// Token: 0x04001F04 RID: 7940
		protected bool allowUnenclosedPens;

		// Token: 0x04001F05 RID: 7941
		protected RopingPriority ropingPriority;
	}
}
