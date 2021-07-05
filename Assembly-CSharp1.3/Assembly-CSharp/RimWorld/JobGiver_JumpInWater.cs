using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007C4 RID: 1988
	public class JobGiver_JumpInWater : ThinkNode_JobGiver
	{
		// Token: 0x060035AC RID: 13740 RVA: 0x0012F6F4 File Offset: 0x0012D8F4
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			if (Rand.Value < 1f && RCellFinder.TryFindRandomCellNearWith(pawn.Position, (IntVec3 pos) => pos.GetTerrain(pawn.Map).extinguishesFire, pawn.Map, out c, 5, this.MaxDistance.RandomInRange))
			{
				return JobMaker.MakeJob(JobDefOf.Goto, c);
			}
			return null;
		}

		// Token: 0x04001EAD RID: 7853
		private const float ActivateChance = 1f;

		// Token: 0x04001EAE RID: 7854
		private readonly IntRange MaxDistance = new IntRange(10, 16);
	}
}
