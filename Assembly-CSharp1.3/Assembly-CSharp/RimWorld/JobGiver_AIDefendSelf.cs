using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000779 RID: 1913
	public class JobGiver_AIDefendSelf : JobGiver_AIDefendPawn
	{
		// Token: 0x060034B6 RID: 13494 RVA: 0x000210E7 File Offset: 0x0001F2E7
		protected override Pawn GetDefendee(Pawn pawn)
		{
			return pawn;
		}

		// Token: 0x060034B7 RID: 13495 RVA: 0x0012867C File Offset: 0x0012687C
		protected override float GetFlagRadius(Pawn pawn)
		{
			return pawn.mindState.duty.radius;
		}
	}
}
