using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000820 RID: 2080
	public class JobGiver_ReturnToGauranlenTree : ThinkNode_JobGiver
	{
		// Token: 0x06003754 RID: 14164 RVA: 0x0013879C File Offset: 0x0013699C
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
				if (compTreeConnection != null && compTreeConnection.ShouldReturnToTree(pawn) && pawn.CanReach(thing, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return JobMaker.MakeJob(JobDefOf.ReturnToGauranlenTree, thing);
				}
			}
			return null;
		}
	}
}
