using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DDE RID: 3550
	public class RecordWorker_TimeHauling : RecordWorker
	{
		// Token: 0x0600524F RID: 21071 RVA: 0x001BC3EB File Offset: 0x001BA5EB
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return !pawn.Dead && pawn.carryTracker.CarriedThing != null;
		}
	}
}
