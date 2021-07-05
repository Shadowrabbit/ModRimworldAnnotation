﻿using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000828 RID: 2088
	public class WorkGiver_Warden_Chat : WorkGiver_Warden
	{
		// Token: 0x0600377C RID: 14204 RVA: 0x00138D40 File Offset: 0x00136F40
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfPrisoner(pawn, t, false))
			{
				return null;
			}
			Pawn pawn2 = (Pawn)t;
			PrisonerInteractionModeDef interactionMode = pawn2.guest.interactionMode;
			if ((interactionMode != PrisonerInteractionModeDefOf.AttemptRecruit && interactionMode != PrisonerInteractionModeDefOf.ReduceResistance) || !pawn2.guest.ScheduledForInteraction || !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) || (pawn2.Downed && !pawn2.InBed()) || !pawn.CanReserve(t, 1, -1, null, false) || !pawn2.Awake())
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
