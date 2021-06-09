using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001455 RID: 5205
	public class RecordWorker_TimeHauling : RecordWorker
	{
		// Token: 0x0600706F RID: 28783 RVA: 0x0004BCD9 File Offset: 0x00049ED9
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return !pawn.Dead && pawn.carryTracker.CarriedThing != null;
		}
	}
}
