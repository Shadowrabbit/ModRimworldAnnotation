using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A96 RID: 2710
	public class JobGiver_Orders : ThinkNode_JobGiver
	{
		// Token: 0x0600404A RID: 16458 RVA: 0x000301ED File Offset: 0x0002E3ED
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.Drafted)
			{
				return JobMaker.MakeJob(JobDefOf.Wait_Combat, pawn.Position);
			}
			return null;
		}
	}
}
