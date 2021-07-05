using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CD5 RID: 3285
	public class JobGiver_JumpInWater : ThinkNode_JobGiver
	{
		// Token: 0x06004BD5 RID: 19413 RVA: 0x001A7410 File Offset: 0x001A5610
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			if (Rand.Value < 1f && RCellFinder.TryFindRandomCellNearWith(pawn.Position, (IntVec3 pos) => pos.GetTerrain(pawn.Map).extinguishesFire, pawn.Map, out c, 5, this.MaxDistance.RandomInRange))
			{
				return JobMaker.MakeJob(JobDefOf.Goto, c);
			}
			return null;
		}

		// Token: 0x04003203 RID: 12803
		private const float ActivateChance = 1f;

		// Token: 0x04003204 RID: 12804
		private readonly IntRange MaxDistance = new IntRange(10, 16);
	}
}
