using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C92 RID: 3218
	public class JobGiver_AIDefendEscortee : JobGiver_AIDefendPawn
	{
		// Token: 0x06004B09 RID: 19209 RVA: 0x000358DC File Offset: 0x00033ADC
		protected override Pawn GetDefendee(Pawn pawn)
		{
			return ((Thing)pawn.mindState.duty.focus) as Pawn;
		}

		// Token: 0x06004B0A RID: 19210 RVA: 0x000356F5 File Offset: 0x000338F5
		protected override float GetFlagRadius(Pawn pawn)
		{
			return pawn.mindState.duty.radius;
		}
	}
}
