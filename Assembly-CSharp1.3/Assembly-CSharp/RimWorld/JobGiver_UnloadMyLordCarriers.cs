using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200078F RID: 1935
	public class JobGiver_UnloadMyLordCarriers : ThinkNode_JobGiver
	{
		// Token: 0x06003510 RID: 13584 RVA: 0x0012C4C4 File Offset: 0x0012A6C4
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return null;
			}
			Lord lord = pawn.GetLord();
			if (lord == null)
			{
				return null;
			}
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (UnloadCarriersJobGiverUtility.HasJobOnThing(pawn, lord.ownedPawns[i], false))
				{
					return JobMaker.MakeJob(JobDefOf.UnloadInventory, lord.ownedPawns[i]);
				}
			}
			return null;
		}
	}
}
