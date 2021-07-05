using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007F9 RID: 2041
	public class JobGiver_Manhunter : ThinkNode_JobGiver
	{
		// Token: 0x0600368E RID: 13966 RVA: 0x001353FC File Offset: 0x001335FC
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.TryGetAttackVerb(null, false) == null)
			{
				return null;
			}
			bool fenceBlocked = pawn.def.race.FenceBlocked;
			Pawn pawn2 = this.FindPawnTarget(pawn, fenceBlocked);
			if (pawn2 != null && pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, fenceBlocked, TraverseMode.ByPawn))
			{
				return this.MeleeAttackJob(pawn2, fenceBlocked);
			}
			Building building = this.FindTurretTarget(pawn, fenceBlocked);
			if (building != null)
			{
				return this.MeleeAttackJob(building, fenceBlocked);
			}
			if (pawn2 != null)
			{
				using (PawnPath pawnPath = pawn.Map.pathFinder.FindPath(pawn.Position, pawn2.Position, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.PassDoors, false, false, false), PathEndMode.OnCell, null))
				{
					if (!pawnPath.Found)
					{
						return null;
					}
					IntVec3 loc;
					if (!pawnPath.TryFindLastCellBeforeBlockingDoor(pawn, out loc))
					{
						Log.Error(pawn + " did TryFindLastCellBeforeDoor but found none when it should have been one. Target: " + pawn2.LabelCap);
						return null;
					}
					IntVec3 randomCell = CellFinder.RandomRegionNear(loc.GetRegion(pawn.Map, RegionType.Set_Passable), 9, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), null, null, RegionType.Set_Passable).RandomCell;
					if (randomCell == pawn.Position)
					{
						return JobMaker.MakeJob(JobDefOf.Wait, 30, false);
					}
					return JobMaker.MakeJob(JobDefOf.Goto, randomCell);
				}
			}
			return null;
		}

		// Token: 0x0600368F RID: 13967 RVA: 0x00135550 File Offset: 0x00133750
		private Job MeleeAttackJob(Thing target, bool canBashFences)
		{
			Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, target);
			job.maxNumMeleeAttacks = 1;
			job.expiryInterval = Rand.Range(420, 900);
			job.attackDoorIfTargetLost = true;
			job.canBashFences = canBashFences;
			return job;
		}

		// Token: 0x06003690 RID: 13968 RVA: 0x0013558C File Offset: 0x0013378C
		private Pawn FindPawnTarget(Pawn pawn, bool canBashFences)
		{
			return (Pawn)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable, (Thing x) => x is Pawn && x.def.race.intelligence >= Intelligence.ToolUser, 0f, 9999f, default(IntVec3), float.MaxValue, true, true, canBashFences);
		}

		// Token: 0x06003691 RID: 13969 RVA: 0x001355E4 File Offset: 0x001337E4
		private Building FindTurretTarget(Pawn pawn, bool canBashFences)
		{
			return (Building)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedLOSToNonPawns | TargetScanFlags.NeedReachable | TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable, (Thing t) => t is Building, 0f, 70f, default(IntVec3), float.MaxValue, false, true, canBashFences);
		}

		// Token: 0x04001EF6 RID: 7926
		private const float WaitChance = 0.75f;

		// Token: 0x04001EF7 RID: 7927
		private const int WaitTicks = 90;

		// Token: 0x04001EF8 RID: 7928
		private const int MinMeleeChaseTicks = 420;

		// Token: 0x04001EF9 RID: 7929
		private const int MaxMeleeChaseTicks = 900;

		// Token: 0x04001EFA RID: 7930
		private const int WanderOutsideDoorRegions = 9;
	}
}
