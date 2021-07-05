using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000DDD RID: 3549
	public class RecordWorker_TimeGettingJoy : RecordWorker
	{
		// Token: 0x0600524D RID: 21069 RVA: 0x001BC3C4 File Offset: 0x001BA5C4
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			Job curJob = pawn.CurJob;
			return curJob != null && curJob.def.joyKind != null;
		}
	}
}
