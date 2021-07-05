using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000850 RID: 2128
	public class WorkGiver_PatientGoToBedEmergencyTreatment : WorkGiver_PatientGoToBedRecuperate
	{
		// Token: 0x0600384B RID: 14411 RVA: 0x0013CB9E File Offset: 0x0013AD9E
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
