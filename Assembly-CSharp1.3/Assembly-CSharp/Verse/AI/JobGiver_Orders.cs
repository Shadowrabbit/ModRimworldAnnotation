using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000638 RID: 1592
	public class JobGiver_Orders : ThinkNode_JobGiver
	{
		// Token: 0x06002D6A RID: 11626 RVA: 0x0010FED4 File Offset: 0x0010E0D4
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
