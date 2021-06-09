using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CAA RID: 3242
	public class JobGiver_UnloadMyLordCarriers : ThinkNode_JobGiver
	{
		// Token: 0x06004B5F RID: 19295 RVA: 0x001A551C File Offset: 0x001A371C
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
