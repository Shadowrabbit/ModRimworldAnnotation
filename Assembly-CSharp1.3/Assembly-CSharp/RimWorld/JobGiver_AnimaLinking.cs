using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007BA RID: 1978
	public class JobGiver_AnimaLinking : ThinkNode_JobGiver
	{
		// Token: 0x06003590 RID: 13712 RVA: 0x0012EC40 File Offset: 0x0012CE40
		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			if (duty == null)
			{
				return null;
			}
			if (!pawn.CanReserveAndReach(duty.focus, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, false))
			{
				return null;
			}
			Thing thing = duty.focusSecond.Thing;
			CompPsylinkable compPsylinkable = (thing != null) ? thing.TryGetComp<CompPsylinkable>() : null;
			if (compPsylinkable == null || !compPsylinkable.CanPsylink(pawn, null).Accepted)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.LinkPsylinkable, duty.focusSecond, duty.focus);
		}
	}
}
