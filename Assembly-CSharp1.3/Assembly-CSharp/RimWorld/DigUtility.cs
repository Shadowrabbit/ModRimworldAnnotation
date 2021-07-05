using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000782 RID: 1922
	public static class DigUtility
	{
		// Token: 0x060034DD RID: 13533 RVA: 0x0012B4D8 File Offset: 0x001296D8
		public static Job PassBlockerJob(Pawn pawn, Thing blocker, IntVec3 cellBeforeBlocker, bool canMineMineables, bool canMineNonMineables)
		{
			if (StatDefOf.MiningSpeed.Worker.IsDisabledFor(pawn))
			{
				canMineMineables = false;
				canMineNonMineables = false;
			}
			if (blocker.def.mineable)
			{
				if (canMineMineables)
				{
					return DigUtility.MineOrWaitJob(pawn, blocker, cellBeforeBlocker);
				}
				return DigUtility.MeleeOrWaitJob(pawn, blocker, cellBeforeBlocker);
			}
			else
			{
				if (pawn.equipment != null && pawn.equipment.Primary != null)
				{
					Verb primaryVerb = pawn.equipment.PrimaryEq.PrimaryVerb;
					if (primaryVerb.verbProps.ai_IsBuildingDestroyer && (!primaryVerb.IsIncendiary() || blocker.FlammableNow))
					{
						Job job = JobMaker.MakeJob(JobDefOf.UseVerbOnThing);
						job.targetA = blocker;
						job.verbToUse = primaryVerb;
						job.expiryInterval = JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange;
						return job;
					}
				}
				if (canMineNonMineables)
				{
					return DigUtility.MineOrWaitJob(pawn, blocker, cellBeforeBlocker);
				}
				return DigUtility.MeleeOrWaitJob(pawn, blocker, cellBeforeBlocker);
			}
		}

		// Token: 0x060034DE RID: 13534 RVA: 0x0012B5A8 File Offset: 0x001297A8
		private static Job MeleeOrWaitJob(Pawn pawn, Thing blocker, IntVec3 cellBeforeBlocker)
		{
			if (!pawn.CanReserve(blocker, 1, -1, null, false))
			{
				return DigUtility.WaitNearJob(pawn, cellBeforeBlocker);
			}
			Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, blocker);
			job.ignoreDesignations = true;
			job.expiryInterval = JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange;
			job.checkOverrideOnExpire = true;
			return job;
		}

		// Token: 0x060034DF RID: 13535 RVA: 0x0012B600 File Offset: 0x00129800
		private static Job MineOrWaitJob(Pawn pawn, Thing blocker, IntVec3 cellBeforeBlocker)
		{
			if (!pawn.CanReserve(blocker, 1, -1, null, false))
			{
				return DigUtility.WaitNearJob(pawn, cellBeforeBlocker);
			}
			Job job = JobMaker.MakeJob(JobDefOf.Mine, blocker);
			job.ignoreDesignations = true;
			job.expiryInterval = JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange;
			job.checkOverrideOnExpire = true;
			return job;
		}

		// Token: 0x060034E0 RID: 13536 RVA: 0x0012B658 File Offset: 0x00129858
		private static Job WaitNearJob(Pawn pawn, IntVec3 cellBeforeBlocker)
		{
			IntVec3 intVec = CellFinder.RandomClosewalkCellNear(cellBeforeBlocker, pawn.Map, 10, null);
			if (intVec == pawn.Position)
			{
				return JobMaker.MakeJob(JobDefOf.Wait, 20, true);
			}
			return JobMaker.MakeJob(JobDefOf.Goto, intVec, 500, true);
		}

		// Token: 0x04001E6F RID: 7791
		private const int CheckOverrideInterval = 500;
	}
}
