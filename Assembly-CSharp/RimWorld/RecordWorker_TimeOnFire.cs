using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001459 RID: 5209
	public class RecordWorker_TimeOnFire : RecordWorker
	{
		// Token: 0x06007077 RID: 28791 RVA: 0x0004BCFB File Offset: 0x00049EFB
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.IsBurning();
		}
	}
}
