using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CE7 RID: 3303
	public class JobGiver_SeekAllowedArea : ThinkNode_JobGiver
	{
		// Token: 0x06004C0C RID: 19468 RVA: 0x001A88A0 File Offset: 0x001A6AA0
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.Position.IsForbidden(pawn))
			{
				return null;
			}
			if (this.HasJobWithSpawnedAllowedTarget(pawn))
			{
				return null;
			}
			Region region = pawn.GetRegion(RegionType.Set_Passable);
			if (region == null)
			{
				return null;
			}
			TraverseParms traverseParms = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.Allows(traverseParms, false);
			Region reg = null;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				if (r.IsDoorway)
				{
					return false;
				}
				if (!r.IsForbiddenEntirely(pawn))
				{
					reg = r;
					return true;
				}
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, 9999, RegionType.Set_Passable);
			if (reg == null)
			{
				return null;
			}
			IntVec3 c;
			if (!reg.TryFindRandomCellInRegionUnforbidden(pawn, null, out c))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Goto, c);
		}

		// Token: 0x06004C0D RID: 19469 RVA: 0x001A896C File Offset: 0x001A6B6C
		private bool HasJobWithSpawnedAllowedTarget(Pawn pawn)
		{
			Job curJob = pawn.CurJob;
			return curJob != null && (this.IsSpawnedAllowedTarget(curJob.targetA, pawn) || this.IsSpawnedAllowedTarget(curJob.targetB, pawn) || this.IsSpawnedAllowedTarget(curJob.targetC, pawn));
		}

		// Token: 0x06004C0E RID: 19470 RVA: 0x001A89B4 File Offset: 0x001A6BB4
		private bool IsSpawnedAllowedTarget(LocalTargetInfo target, Pawn pawn)
		{
			if (!target.IsValid)
			{
				return false;
			}
			if (target.HasThing)
			{
				return target.Thing.Spawned && !target.Thing.Position.IsForbidden(pawn);
			}
			return target.Cell.InBounds(pawn.Map) && !target.Cell.IsForbidden(pawn);
		}
	}
}
