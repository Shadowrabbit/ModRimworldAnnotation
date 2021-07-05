using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DE2 RID: 3554
	public class RecordWorker_TimeOnFire : RecordWorker
	{
		// Token: 0x06005257 RID: 21079 RVA: 0x001BC46C File Offset: 0x001BA66C
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.IsBurning();
		}
	}
}
