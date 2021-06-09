using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001457 RID: 5207
	public class RecordWorker_TimeInBedForMedicalReasons : RecordWorker
	{
		// Token: 0x06007073 RID: 28787 RVA: 0x002269F0 File Offset: 0x00224BF0
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InBed() && (HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn) || (HealthAIUtility.ShouldSeekMedicalRest(pawn) && (pawn.needs.rest == null || pawn.needs.rest.CurLevel >= 1f || pawn.CurJob.restUntilHealed)));
		}
	}
}
