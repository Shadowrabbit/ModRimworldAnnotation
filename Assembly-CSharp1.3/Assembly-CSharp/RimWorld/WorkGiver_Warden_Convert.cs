using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000829 RID: 2089
	public class WorkGiver_Warden_Convert : WorkGiver_Warden
	{
		// Token: 0x0600377E RID: 14206 RVA: 0x00138E04 File Offset: 0x00137004
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModLister.CheckIdeology("WorkGiver_Warden_Convert"))
			{
				return null;
			}
			if (!base.ShouldTakeCareOfPrisoner(pawn, t, false))
			{
				return null;
			}
			Pawn pawn2 = (Pawn)t;
			if (pawn2.guest.interactionMode == PrisonerInteractionModeDefOf.Convert && pawn2.guest.ScheduledForInteraction && pawn2.guest.IsPrisoner && (!pawn2.Downed || pawn2.InBed()) && pawn2.Ideo != pawn.Ideo && pawn.Ideo == pawn2.guest.ideoForConversion && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && pawn.CanReserve(t, 1, -1, null, false) && pawn2.Awake())
			{
				return JobMaker.MakeJob(JobDefOf.PrisonerConvert, t);
			}
			return null;
		}
	}
}
