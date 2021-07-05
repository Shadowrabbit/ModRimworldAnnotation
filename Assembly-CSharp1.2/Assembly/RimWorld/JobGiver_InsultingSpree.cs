using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D2B RID: 3371
	public class JobGiver_InsultingSpree : ThinkNode_JobGiver
	{
		// Token: 0x06004D2F RID: 19759 RVA: 0x001AD6C0 File Offset: 0x001AB8C0
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_InsultingSpree mentalState_InsultingSpree = pawn.MentalState as MentalState_InsultingSpree;
			if (mentalState_InsultingSpree == null || mentalState_InsultingSpree.target == null || !pawn.CanReach(mentalState_InsultingSpree.target, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Insult, mentalState_InsultingSpree.target);
		}
	}
}
