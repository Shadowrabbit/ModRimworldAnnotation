using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DDF RID: 3551
	public class RecordWorker_TimeInBed : RecordWorker
	{
		// Token: 0x06005251 RID: 21073 RVA: 0x001BC405 File Offset: 0x001BA605
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InBed();
		}
	}
}
