using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CB8 RID: 3256
	public class JobGiver_WanderInGatheringArea : JobGiver_Wander
	{
		// Token: 0x06004B82 RID: 19330 RVA: 0x001A5D20 File Offset: 0x001A3F20
		protected override IntVec3 GetExactWanderDest(Pawn pawn)
		{
			IntVec3 result;
			if (!GatheringsUtility.TryFindRandomCellInGatheringArea(pawn, out result))
			{
				return IntVec3.Invalid;
			}
			return result;
		}

		// Token: 0x06004B83 RID: 19331 RVA: 0x0000FC33 File Offset: 0x0000DE33
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			throw new NotImplementedException();
		}
	}
}
