using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C93 RID: 3219
	public class JobGiver_AIDefendSelf : JobGiver_AIDefendPawn
	{
		// Token: 0x06004B0C RID: 19212 RVA: 0x0001037D File Offset: 0x0000E57D
		protected override Pawn GetDefendee(Pawn pawn)
		{
			return pawn;
		}

		// Token: 0x06004B0D RID: 19213 RVA: 0x000356F5 File Offset: 0x000338F5
		protected override float GetFlagRadius(Pawn pawn)
		{
			return pawn.mindState.duty.radius;
		}
	}
}
