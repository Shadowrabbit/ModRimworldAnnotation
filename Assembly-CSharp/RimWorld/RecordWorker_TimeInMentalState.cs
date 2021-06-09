using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001458 RID: 5208
	public class RecordWorker_TimeInMentalState : RecordWorker
	{
		// Token: 0x06007075 RID: 28789 RVA: 0x000357AF File Offset: 0x000339AF
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InMentalState;
		}
	}
}
