using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007D5 RID: 2005
	public class JobGiver_UnloadYourInventory : ThinkNode_JobGiver
	{
		// Token: 0x060035E5 RID: 13797 RVA: 0x00131481 File Offset: 0x0012F681
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
