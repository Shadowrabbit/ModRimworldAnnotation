using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000778 RID: 1912
	public class JobGiver_AIDefendEscortee : JobGiver_AIDefendPawn
	{
		// Token: 0x060034B3 RID: 13491 RVA: 0x0012AB26 File Offset: 0x00128D26
		protected override Pawn GetDefendee(Pawn pawn)
		{
			return ((Thing)pawn.mindState.duty.focus) as Pawn;
		}

		// Token: 0x060034B4 RID: 13492 RVA: 0x0012867C File Offset: 0x0012687C
		protected override float GetFlagRadius(Pawn pawn)
		{
			return pawn.mindState.duty.radius;
		}
	}
}
