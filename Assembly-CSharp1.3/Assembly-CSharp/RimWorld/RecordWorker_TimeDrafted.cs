using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DDC RID: 3548
	public class RecordWorker_TimeDrafted : RecordWorker
	{
		// Token: 0x0600524B RID: 21067 RVA: 0x0014F42B File Offset: 0x0014D62B
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Drafted;
		}
	}
}
