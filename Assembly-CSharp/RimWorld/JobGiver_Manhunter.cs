using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D2C RID: 3372
	public class JobGiver_Manhunter : ThinkNode_JobGiver
	{
		// Token: 0x06004D31 RID: 19761 RVA: 0x001AD714 File Offset: 0x001AB914
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.TryGetAttackVerb(null, false) == null)
			{
				return null;
			}
			Pawn pawn2 = this.FindPawnTarget(pawn);
			if (pawn2 != null && pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				return this.MeleeAttackJob(pawn, pawn2);
			}
			Building building = this.FindTurretTarget(pawn);
			if (building != null)
			{
				return this.MeleeAttackJob(pawn, building);
			}
			if (pawn2 != null)
			{
				using (PawnPath pawnPath = pawn.Map.pathFinder.FindPath(pawn.Position, pawn2.Position, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.PassDoors, false), PathEndMode.OnCell))
				{
					if (!pawnPath.Found)
					{
						return null;
					}
					IntVec3 loc;
					if (!pawnPath.TryFindLastCellBeforeBlockingDoor(pawn, out loc))
					{
						Log.Error(pawn + " did TryFindLastCellBeforeDoor but found none when it should have been one. Target: " + pawn2.LabelCap, false);
						return null;
					}
					IntVec3 randomCell = CellFinder.RandomRegionNear(loc.GetRegion(pawn.Map, RegionType.Set_Passable), 9, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), null, null, RegionType.Set_Passable).RandomCell;
					if (randomCell == pawn.Position)
					{
						return JobMaker.MakeJob(JobDefOf.Wait, 30, false);
					}
					return JobMaker.MakeJob(JobDefOf.Goto, randomCell);
				}
			}
			return null;
		}

		// Token: 0x06004D32 RID: 19762 RVA: 0x00036ACE File Offset: 0x00034CCE
		private Job MeleeAttackJob(Pawn pawn, Thing target)
		{
			Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, target);
			job.maxNumMeleeAttacks = 1;
			job.expiryInterval = Rand.Range(420, 900);
			job.attackDoorIfTargetLost = true;
			return job;
		}

		// Token: 0x06004D33 RID: 19763 RVA: 0x001AD84C File Offset: 0x001ABA4C
		private Pawn FindPawnTarget(Pawn pawn)
		{
			return (Pawn)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable, (Thing x) => x is Pawn && x.def.race.intelligence >= Intelligence.ToolUser, 0f, 9999f, default(IntVec3), float.MaxValue, true, true);
		}

		// Token: 0x06004D34 RID: 19764 RVA: 0x001AD8A4 File Offset: 0x001ABAA4
		private Building FindTurretTarget(Pawn pawn)
		{
			return (Building)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedLOSToNonPawns | TargetScanFlags.NeedReachable | TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable, (Thing t) => t is Building, 0f, 70f, default(IntVec3), float.MaxValue, false, true);
		}

		// Token: 0x040032C6 RID: 12998
		private const float WaitChance = 0.75f;

		// Token: 0x040032C7 RID: 12999
		private const int WaitTicks = 90;

		// Token: 0x040032C8 RID: 13000
		private const int MinMeleeChaseTicks = 420;

		// Token: 0x040032C9 RID: 13001
		private const int MaxMeleeChaseTicks = 900;

		// Token: 0x040032CA RID: 13002
		private const int WanderOutsideDoorRegions = 9;
	}
}
