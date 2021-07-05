using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DE0 RID: 3552
	public class RecordWorker_TimeInBedForMedicalReasons : RecordWorker
	{
		// Token: 0x06005253 RID: 21075 RVA: 0x001BC410 File Offset: 0x001BA610
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InBed() && (HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn) || (HealthAIUtility.ShouldSeekMedicalRest(pawn) && (pawn.needs.rest == null || pawn.needs.rest.CurLevel >= 1f || pawn.CurJob.restUntilHealed)));
		}
	}
}
