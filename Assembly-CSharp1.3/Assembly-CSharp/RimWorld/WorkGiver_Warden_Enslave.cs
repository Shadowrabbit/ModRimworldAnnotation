using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200082D RID: 2093
	public class WorkGiver_Warden_Enslave : WorkGiver_Warden
	{
		// Token: 0x0600378A RID: 14218 RVA: 0x001392F0 File Offset: 0x001374F0
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModLister.CheckIdeology("WorkGiver_Warden_Enslave"))
			{
				return null;
			}
			if (!base.ShouldTakeCareOfPrisoner(pawn, t, false))
			{
				return null;
			}
			Pawn pawn2 = (Pawn)t;
			PrisonerInteractionModeDef interactionMode = pawn2.guest.interactionMode;
			if ((interactionMode == PrisonerInteractionModeDefOf.Enslave || interactionMode == PrisonerInteractionModeDefOf.ReduceWill) && pawn2.guest.ScheduledForInteraction && pawn2.guest.IsPrisoner && (interactionMode != PrisonerInteractionModeDefOf.ReduceWill || pawn2.guest.will > 0f) && (!pawn2.Downed || pawn2.InBed()) && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && pawn.CanReserve(t, 1, -1, null, false) && pawn2.Awake() && new HistoryEvent(HistoryEventDefOf.EnslavedPrisoner, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
			{
				return JobMaker.MakeJob(JobDefOf.PrisonerEnslave, t);
			}
			return null;
		}
	}
}
