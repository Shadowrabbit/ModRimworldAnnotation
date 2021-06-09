using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001452 RID: 5202
	public class RecordWorker_TimeDowned : RecordWorker
	{
		// Token: 0x06007069 RID: 28777 RVA: 0x00039CC0 File Offset: 0x00037EC0
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Downed;
		}
	}
}
