using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DE3 RID: 3555
	public class RecordWorker_TimeUnderRoof : RecordWorker
	{
		// Token: 0x06005259 RID: 21081 RVA: 0x001BC474 File Offset: 0x001BA674
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Spawned && pawn.Position.Roofed(pawn.Map);
		}
	}
}
