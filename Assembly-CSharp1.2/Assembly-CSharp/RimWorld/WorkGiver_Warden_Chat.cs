using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D5F RID: 3423
	public class WorkGiver_Warden_Chat : WorkGiver_Warden
	{
		// Token: 0x06004E3A RID: 20026 RVA: 0x001B082C File Offset: 0x001AEA2C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfPrisoner_NewTemp(pawn, t, forced))
			{
				return null;
			}
			Pawn pawn2 = (Pawn)t;
			PrisonerInteractionModeDef interactionMode = pawn2.guest.interactionMode;
			if ((interactionMode != PrisonerInteractionModeDefOf.AttemptRecruit && interactionMode != PrisonerInteractionModeDefOf.ReduceResistance) || !pawn2.guest.ScheduledForInteraction || !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) || (pawn2.Downed && !pawn2.InBed()) || !pawn2.Awake())
			{
				return null;
			}
			if (interactionMode == PrisonerInteractionModeDefOf.ReduceResistance && pawn2.guest.Resistance <= 0f)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.PrisonerAttemptRecruit, t);
		}
	}
}
