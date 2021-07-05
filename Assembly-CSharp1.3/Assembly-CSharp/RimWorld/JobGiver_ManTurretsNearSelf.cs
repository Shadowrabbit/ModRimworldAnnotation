using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007B4 RID: 1972
	public class JobGiver_ManTurretsNearSelf : JobGiver_ManTurrets
	{
		// Token: 0x0600357A RID: 13690 RVA: 0x0011056E File Offset: 0x0010E76E
		protected override IntVec3 GetRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
