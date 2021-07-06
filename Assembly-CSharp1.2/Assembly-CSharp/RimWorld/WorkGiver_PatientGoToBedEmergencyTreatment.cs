using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D90 RID: 3472
	public class WorkGiver_PatientGoToBedEmergencyTreatment : WorkGiver_PatientGoToBedRecuperate
	{
		// Token: 0x06004F34 RID: 20276 RVA: 0x00037BCF File Offset: 0x00035DCF
		public override Job NonScanJob(Pawn pawn)
		{
			if (!HealthAIUtility.ShouldBeTendedNowByPlayerUrgent(pawn))
			{
				return null;
			}
			return base.NonScanJob(pawn);
		}
	}
}
