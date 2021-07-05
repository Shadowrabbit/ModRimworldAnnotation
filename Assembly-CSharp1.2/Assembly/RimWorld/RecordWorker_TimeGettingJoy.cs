using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001454 RID: 5204
	public class RecordWorker_TimeGettingJoy : RecordWorker
	{
		// Token: 0x0600706D RID: 28781 RVA: 0x002269C8 File Offset: 0x00224BC8
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			Job curJob = pawn.CurJob;
			return curJob != null && curJob.def.joyKind != null;
		}
	}
}
