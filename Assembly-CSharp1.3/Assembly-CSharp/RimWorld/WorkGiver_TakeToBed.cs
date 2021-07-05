using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000861 RID: 2145
	public abstract class WorkGiver_TakeToBed : WorkGiver_Scanner
	{
		// Token: 0x060038A5 RID: 14501 RVA: 0x0013DA47 File Offset: 0x0013BC47
		protected Building_Bed FindBed(Pawn pawn, Pawn patient)
		{
			return RestUtility.FindBedFor(patient, pawn, false, false, patient.GuestStatus);
		}
	}
}
