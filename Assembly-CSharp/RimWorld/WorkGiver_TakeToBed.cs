using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DA2 RID: 3490
	public abstract class WorkGiver_TakeToBed : WorkGiver_Scanner
	{
		// Token: 0x06004F94 RID: 20372 RVA: 0x00037F1C File Offset: 0x0003611C
		protected Building_Bed FindBed(Pawn pawn, Pawn patient)
		{
			return RestUtility.FindBedFor(patient, pawn, patient.HostFaction == pawn.Faction, false, false);
		}
	}
}
