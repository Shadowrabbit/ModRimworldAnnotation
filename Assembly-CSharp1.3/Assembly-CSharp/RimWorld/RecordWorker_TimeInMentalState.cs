using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DE1 RID: 3553
	public class RecordWorker_TimeInMentalState : RecordWorker
	{
		// Token: 0x06005255 RID: 21077 RVA: 0x0012949F File Offset: 0x0012769F
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InMentalState;
		}
	}
}
