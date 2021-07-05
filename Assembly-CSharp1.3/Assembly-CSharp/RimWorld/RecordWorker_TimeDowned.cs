using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DDB RID: 3547
	public class RecordWorker_TimeDowned : RecordWorker
	{
		// Token: 0x06005249 RID: 21065 RVA: 0x0014F226 File Offset: 0x0014D426
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Downed;
		}
	}
}
