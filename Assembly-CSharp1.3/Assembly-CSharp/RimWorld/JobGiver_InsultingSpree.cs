using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007F8 RID: 2040
	public class JobGiver_InsultingSpree : ThinkNode_JobGiver
	{
		// Token: 0x0600368C RID: 13964 RVA: 0x001353A8 File Offset: 0x001335A8
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_InsultingSpree mentalState_InsultingSpree = pawn.MentalState as MentalState_InsultingSpree;
			if (mentalState_InsultingSpree == null || mentalState_InsultingSpree.target == null || !pawn.CanReach(mentalState_InsultingSpree.target, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Insult, mentalState_InsultingSpree.target);
		}
	}
}
