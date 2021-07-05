using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000823 RID: 2083
	public class JobGiver_MergeIntoGaumakerPod : ThinkNode_JobGiver
	{
		// Token: 0x06003760 RID: 14176 RVA: 0x001388EC File Offset: 0x00136AEC
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return null;
			}
			if (pawn.connections == null || pawn.connections.ConnectedThings.NullOrEmpty<Thing>())
			{
				return null;
			}
			foreach (Thing thing in pawn.connections.ConnectedThings)
			{
				CompTreeConnection compTreeConnection = thing.TryGetComp<CompTreeConnection>();
				if (compTreeConnection != null && compTreeConnection.ShouldEnterGaumakerPod(pawn) && pawn.CanReach(compTreeConnection.gaumakerPod, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return JobMaker.MakeJob(JobDefOf.MergeIntoGaumakerPod, thing, compTreeConnection.gaumakerPod);
				}
			}
			return null;
		}
	}
}
