using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A93 RID: 2707
	public class JobGiver_IdleForever : ThinkNode_JobGiver
	{
		// Token: 0x06004043 RID: 16451 RVA: 0x000301B7 File Offset: 0x0002E3B7
		protected override Job TryGiveJob(Pawn pawn)
		{
			return JobMaker.MakeJob(JobDefOf.Wait_Downed);
		}
	}
}
