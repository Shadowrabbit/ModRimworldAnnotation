using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007B3 RID: 1971
	public class JobGiver_ManTurretsNearPoint : JobGiver_ManTurrets
	{
		// Token: 0x06003578 RID: 13688 RVA: 0x0012E424 File Offset: 0x0012C624
		protected override IntVec3 GetRoot(Pawn pawn)
		{
			return pawn.GetLord().CurLordToil.FlagLoc;
		}
	}
}
