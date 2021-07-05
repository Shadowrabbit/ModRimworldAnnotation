using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000643 RID: 1603
	public class JobGiver_WanderInPen : JobGiver_Wander
	{
		// Token: 0x06002D97 RID: 11671 RVA: 0x001106AE File Offset: 0x0010E8AE
		public JobGiver_WanderInPen()
		{
			this.wanderRadius = 10f;
		}

		// Token: 0x06002D98 RID: 11672 RVA: 0x001106C4 File Offset: 0x0010E8C4
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			if (pawn.GetDistrict(RegionType.Set_Passable).TouchesMapEdge)
			{
				CompAnimalPenMarker compAnimalPenMarker = AnimalPenUtility.ClosestSuitablePen(pawn, true);
				if (compAnimalPenMarker != null)
				{
					return compAnimalPenMarker.parent.Position;
				}
			}
			return pawn.Position;
		}
	}
}
