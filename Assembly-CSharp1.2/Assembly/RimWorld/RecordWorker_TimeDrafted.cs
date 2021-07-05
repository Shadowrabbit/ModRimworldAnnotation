using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001453 RID: 5203
	public class RecordWorker_TimeDrafted : RecordWorker
	{
		// Token: 0x0600706B RID: 28779 RVA: 0x00039E1B File Offset: 0x0003801B
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Drafted;
		}
	}
}
