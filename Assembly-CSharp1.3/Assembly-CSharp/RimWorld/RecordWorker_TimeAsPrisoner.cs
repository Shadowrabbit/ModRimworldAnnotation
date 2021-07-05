using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DDA RID: 3546
	public class RecordWorker_TimeAsPrisoner : RecordWorker
	{
		// Token: 0x06005247 RID: 21063 RVA: 0x0014F26E File Offset: 0x0014D46E
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.IsPrisoner;
		}
	}
}
