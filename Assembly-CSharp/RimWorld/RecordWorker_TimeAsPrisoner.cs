using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001451 RID: 5201
	public class RecordWorker_TimeAsPrisoner : RecordWorker
	{
		// Token: 0x06007067 RID: 28775 RVA: 0x00039CDB File Offset: 0x00037EDB
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.IsPrisoner;
		}
	}
}
