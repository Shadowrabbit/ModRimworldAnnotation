using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CD7 RID: 3287
	public class JobGiver_KeepLyingDown : ThinkNode_JobGiver
	{
		// Token: 0x06004BD9 RID: 19417 RVA: 0x00036007 File Offset: 0x00034207
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
