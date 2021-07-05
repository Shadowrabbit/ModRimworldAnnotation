using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007C5 RID: 1989
	public class JobGiver_KeepLyingDown : ThinkNode_JobGiver
	{
		// Token: 0x060035AE RID: 13742 RVA: 0x0012F77D File Offset: 0x0012D97D
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.GetPosture().Laying())
			{
				return pawn.CurJob;
			}
			return JobMaker.MakeJob(JobDefOf.LayDown, pawn.Position);
		}
	}
}
