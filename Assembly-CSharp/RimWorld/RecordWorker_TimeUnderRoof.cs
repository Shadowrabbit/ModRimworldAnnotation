using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200145A RID: 5210
	public class RecordWorker_TimeUnderRoof : RecordWorker
	{
		// Token: 0x06007079 RID: 28793 RVA: 0x0004BD03 File Offset: 0x00049F03
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Spawned && pawn.Position.Roofed(pawn.Map);
		}
	}
}
