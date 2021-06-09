using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C76 RID: 3190
	public class JobGiver_LayEgg : ThinkNode_JobGiver
	{
		// Token: 0x06004AAE RID: 19118 RVA: 0x001A2798 File Offset: 0x001A0998
		protected override Job TryGiveJob(Pawn pawn)
		{
			CompEggLayer compEggLayer = pawn.TryGetComp<CompEggLayer>();
			if (compEggLayer == null || !compEggLayer.CanLayNow)
			{
				return null;
			}
			IntVec3 c = RCellFinder.RandomWanderDestFor(pawn, pawn.Position, 5f, null, Danger.Some);
			return JobMaker.MakeJob(JobDefOf.LayEgg, c);
		}

		// Token: 0x04003183 RID: 12675
		private const float LayRadius = 5f;
	}
}
