using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001456 RID: 5206
	public class RecordWorker_TimeInBed : RecordWorker
	{
		// Token: 0x06007071 RID: 28785 RVA: 0x0004BCF3 File Offset: 0x00049EF3
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InBed();
		}
	}
}
