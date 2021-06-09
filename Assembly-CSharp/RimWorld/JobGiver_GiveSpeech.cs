using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CB3 RID: 3251
	public class JobGiver_GiveSpeech : ThinkNode_JobGiver
	{
		// Token: 0x06004B76 RID: 19318 RVA: 0x001A5B1C File Offset: 0x001A3D1C
		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			if (duty == null)
			{
				return null;
			}
			Building_Throne building_Throne = duty.focusSecond.Thing as Building_Throne;
			if (building_Throne == null || building_Throne.AssignedPawn != pawn)
			{
				return null;
			}
			if (!pawn.CanReach(building_Throne, PathEndMode.InteractionCell, Danger.None, false, TraverseMode.ByPawn))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.GiveSpeech, duty.focusSecond);
		}
	}
}
