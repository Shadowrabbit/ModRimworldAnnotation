using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CC4 RID: 3268
	public class JobGiver_ManTurretsNearPoint : JobGiver_ManTurrets
	{
		// Token: 0x06004BA2 RID: 19362 RVA: 0x00035E45 File Offset: 0x00034045
		protected override IntVec3 GetRoot(Pawn pawn)
		{
			return pawn.GetLord().CurLordToil.FlagLoc;
		}
	}
}
