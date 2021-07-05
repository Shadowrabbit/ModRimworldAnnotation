using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000581 RID: 1409
	public class JobDriver_FollowRoper : JobDriver
	{
		// Token: 0x06002955 RID: 10581 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06002956 RID: 10582 RVA: 0x000FA29A File Offset: 0x000F849A
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return new Toil
			{
				tickAction = delegate()
				{
					Pawn pawn = this.job.GetTarget(TargetIndex.A).Thing as Pawn;
					if (!this.pawn.CanReach(pawn, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
					{
						base.EndJobWith(JobCondition.Incompletable);
						return;
					}
					IntVec3 intVec = IntVec3.Invalid;
					if (pawn.jobs.curDriver is JobDriver_RopeToDestination)
					{
						intVec = pawn.CurJob.GetTarget(TargetIndex.B).Cell;
					}
					if (intVec.IsValid && pawn.Position == intVec)
					{
						this.GotoStandCell(intVec);
						return;
					}
					this.FollowRoper(pawn);
				},
				defaultCompleteMode = ToilCompleteMode.Never
			};
			yield break;
		}

		// Token: 0x06002957 RID: 10583 RVA: 0x000FA2AC File Offset: 0x000F84AC
		private void FollowRoper(Pawn roper)
		{
			LocalTargetInfo localTargetInfo = new LocalTargetInfo(roper);
			PathEndMode peMode = PathEndMode.Touch;
			if (roper.Position.InHorDistOf(this.pawn.Position, 7.2f) && roper.pather.Moving && roper.pather.curPath != null)
			{
				int marchingPos = Mathf.Abs(roper.roping.Ropees.IndexOf(this.pawn));
				IntVec3 c = this.MarchingOrder(roper, marchingPos);
				if (c.IsValid)
				{
					localTargetInfo = c;
					peMode = PathEndMode.OnCell;
				}
			}
			if (!this.pawn.pather.Moving || this.pawn.pather.Destination != localTargetInfo)
			{
				this.pawn.pather.StartPath(localTargetInfo, peMode);
			}
		}

		// Token: 0x06002958 RID: 10584 RVA: 0x000FA378 File Offset: 0x000F8578
		private IntVec3 MarchingOrder(Pawn roper, int marchingPos)
		{
			PawnPath curPath = roper.pather.curPath;
			if (curPath.NodesLeftCount <= 0)
			{
				return IntVec3.Invalid;
			}
			Map map = this.pawn.Map;
			int num = -2 - marchingPos % 4;
			num = Mathf.Clamp(num, -curPath.NodesConsumedCount, curPath.NodesLeftCount);
			IntVec3 intVec = curPath.Peek(num);
			int num2 = Mathf.Abs(this.pawn.HashOffset()) % 3;
			if (num + 1 < curPath.NodesLeftCount && num2 != 0)
			{
				IntVec3 orig = curPath.Peek(num + 1) - intVec;
				IntVec3 intVec2 = (num2 == 1) ? (intVec + orig.RotatedBy(Rot4.East)) : (intVec + orig.RotatedBy(Rot4.West));
				if (intVec2.InBounds(map) && intVec2.Standable(map) && intVec2.GetDistrict(map, RegionType.Set_Passable) == intVec.GetDistrict(map, RegionType.Set_Passable))
				{
					intVec = intVec2;
				}
			}
			return intVec;
		}

		// Token: 0x06002959 RID: 10585 RVA: 0x000FA460 File Offset: 0x000F8660
		private void GotoStandCell(IntVec3 standCell)
		{
			if (!this.pawn.pather.Moving || this.pawn.pather.Destination != standCell)
			{
				this.pawn.pather.StartPath(standCell, PathEndMode.OnCell);
			}
		}

		// Token: 0x0600295A RID: 10586 RVA: 0x000FA4B3 File Offset: 0x000F86B3
		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}

		// Token: 0x040019A2 RID: 6562
		private const TargetIndex RoperInd = TargetIndex.A;
	}
}
