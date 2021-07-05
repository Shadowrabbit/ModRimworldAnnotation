using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000635 RID: 1589
	public class JobGiver_IdleForever : ThinkNode_JobGiver
	{
		// Token: 0x06002D63 RID: 11619 RVA: 0x0010FDB4 File Offset: 0x0010DFB4
		protected override Job TryGiveJob(Pawn pawn)
		{
			return JobMaker.MakeJob(JobDefOf.Wait_Downed);
		}
	}
}
