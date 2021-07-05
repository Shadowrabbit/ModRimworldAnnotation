using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000788 RID: 1928
	public class JobGiver_PrepareCaravan_GatherDownedPawns : ThinkNode_JobGiver
	{
		// Token: 0x060034F3 RID: 13555 RVA: 0x0012BF14 File Offset: 0x0012A114
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return null;
			}
			Pawn pawn2 = this.FindDownedPawn(pawn);
			if (pawn2 == null)
			{
				return null;
			}
			IntVec3 c = this.FindRandomDropCell(pawn, pawn2);
			Job job = JobMaker.MakeJob(JobDefOf.PrepareCaravan_GatherDownedPawns, pawn2, c);
			job.lord = pawn.GetLord();
			job.count = 1;
			return job;
		}

		// Token: 0x060034F4 RID: 13556 RVA: 0x0012BF7C File Offset: 0x0012A17C
		private Pawn FindDownedPawn(Pawn pawn)
		{
			float num = 0f;
			Pawn pawn2 = null;
			List<Pawn> downedPawns = ((LordJob_FormAndSendCaravan)pawn.GetLord().LordJob).downedPawns;
			IntVec3 cell = pawn.mindState.duty.focusSecond.Cell;
			for (int i = 0; i < downedPawns.Count; i++)
			{
				Pawn pawn3 = downedPawns[i];
				if (pawn3.Downed && pawn3 != pawn && !JobGiver_PrepareCaravan_GatherDownedPawns.IsDownedPawnNearExitPoint(pawn3, cell))
				{
					float num2 = (float)pawn.Position.DistanceToSquared(pawn3.Position);
					if ((pawn2 == null || num2 < num) && pawn.CanReserveAndReach(pawn3, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
					{
						pawn2 = pawn3;
						num = num2;
					}
				}
			}
			return pawn2;
		}

		// Token: 0x060034F5 RID: 13557 RVA: 0x0012C030 File Offset: 0x0012A230
		private IntVec3 FindRandomDropCell(Pawn pawn, Pawn downedPawn)
		{
			return CellFinder.RandomClosewalkCellNear(pawn.mindState.duty.focusSecond.Cell, pawn.Map, 6, (IntVec3 x) => x.Standable(pawn.Map) && StoreUtility.IsGoodStoreCell(x, pawn.Map, downedPawn, pawn, pawn.Faction));
		}

		// Token: 0x060034F6 RID: 13558 RVA: 0x0012C088 File Offset: 0x0012A288
		public static bool IsDownedPawnNearExitPoint(Pawn downedPawn, IntVec3 exitPoint)
		{
			return downedPawn.Spawned && downedPawn.Position.InHorDistOf(exitPoint, 7f);
		}

		// Token: 0x04001E75 RID: 7797
		private const float MaxDownedPawnToExitPointDistance = 7f;
	}
}
