using System;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000781 RID: 1921
	public class JobGiver_AISapper : ThinkNode_JobGiver
	{
		// Token: 0x060034DA RID: 13530 RVA: 0x0012B2D0 File Offset: 0x001294D0
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AISapper jobGiver_AISapper = (JobGiver_AISapper)base.DeepCopy(resolve);
			jobGiver_AISapper.canMineMineables = this.canMineMineables;
			jobGiver_AISapper.canMineNonMineables = this.canMineNonMineables;
			return jobGiver_AISapper;
		}

		// Token: 0x060034DB RID: 13531 RVA: 0x0012B2F8 File Offset: 0x001294F8
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 intVec = pawn.mindState.duty.focus.Cell;
			if (intVec.IsValid && (float)intVec.DistanceToSquared(pawn.Position) < 100f && intVec.GetRoom(pawn.Map) == pawn.GetRoom(RegionType.Set_All) && intVec.WithinRegions(pawn.Position, pawn.Map, 9, TraverseMode.NoPassClosedDoors, RegionType.Set_Passable))
			{
				pawn.GetLord().Notify_ReachedDutyLocation(pawn);
				return null;
			}
			if (!intVec.IsValid)
			{
				IAttackTarget attackTarget;
				if (!(from x in pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn)
				where !x.ThreatDisabled(pawn) && x.Thing.Faction == Faction.OfPlayer && pawn.CanReach(x.Thing, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.PassAllDestroyableThings)
				select x).TryRandomElement(out attackTarget))
				{
					return null;
				}
				intVec = attackTarget.Thing.Position;
			}
			if (!pawn.CanReach(intVec, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.PassAllDestroyableThings))
			{
				return null;
			}
			using (PawnPath pawnPath = pawn.Map.pathFinder.FindPath(pawn.Position, intVec, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.PassAllDestroyableThings, false, false, false), PathEndMode.OnCell, null))
			{
				IntVec3 cellBeforeBlocker;
				Thing thing = pawnPath.FirstBlockingBuilding(out cellBeforeBlocker, pawn);
				if (thing != null)
				{
					Job job = DigUtility.PassBlockerJob(pawn, thing, cellBeforeBlocker, this.canMineMineables, this.canMineNonMineables);
					if (job != null)
					{
						return job;
					}
				}
			}
			return JobMaker.MakeJob(JobDefOf.Goto, intVec, 500, true);
		}

		// Token: 0x04001E6B RID: 7787
		private bool canMineMineables = true;

		// Token: 0x04001E6C RID: 7788
		private bool canMineNonMineables = true;

		// Token: 0x04001E6D RID: 7789
		private const float ReachDestDist = 10f;

		// Token: 0x04001E6E RID: 7790
		private const int CheckOverrideInterval = 500;
	}
}
