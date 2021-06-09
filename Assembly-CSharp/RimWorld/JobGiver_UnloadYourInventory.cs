using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CEC RID: 3308
	public class JobGiver_UnloadYourInventory : ThinkNode_JobGiver
	{
		// Token: 0x06004C1B RID: 19483 RVA: 0x00036212 File Offset: 0x00034412
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.inventory.UnloadEverything)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.UnloadYourInventory);
		}
	}
}
