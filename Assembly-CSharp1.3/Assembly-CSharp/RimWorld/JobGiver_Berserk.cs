using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007F2 RID: 2034
	public class JobGiver_Berserk : ThinkNode_JobGiver
	{
		// Token: 0x0600367C RID: 13948 RVA: 0x00134F3C File Offset: 0x0013313C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (Rand.Value < 0.5f)
			{
				Job job = JobMaker.MakeJob(JobDefOf.Wait_Combat);
				job.expiryInterval = 90;
				job.canUseRangedWeapon = false;
				return job;
			}
			if (pawn.TryGetAttackVerb(null, false) == null)
			{
				return null;
			}
			Pawn pawn2 = this.FindPawnTarget(pawn);
			if (pawn2 != null)
			{
				Job job2 = JobMaker.MakeJob(JobDefOf.AttackMelee, pawn2);
				job2.maxNumMeleeAttacks = 1;
				job2.expiryInterval = Rand.Range(420, 900);
				job2.canBashDoors = true;
				return job2;
			}
			return null;
		}

		// Token: 0x0600367D RID: 13949 RVA: 0x00134FBC File Offset: 0x001331BC
		private Pawn FindPawnTarget(Pawn pawn)
		{
			return (Pawn)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedReachable, delegate(Thing x)
			{
				Pawn pawn2;
				return (pawn2 = (x as Pawn)) != null && pawn2.Spawned && !pawn2.Downed && !pawn2.IsInvisible();
			}, 0f, 40f, default(IntVec3), float.MaxValue, true, true, false);
		}

		// Token: 0x04001EEE RID: 7918
		private const float MaxAttackDistance = 40f;

		// Token: 0x04001EEF RID: 7919
		private const float WaitChance = 0.5f;

		// Token: 0x04001EF0 RID: 7920
		private const int WaitTicks = 90;

		// Token: 0x04001EF1 RID: 7921
		private const int MinMeleeChaseTicks = 420;

		// Token: 0x04001EF2 RID: 7922
		private const int MaxMeleeChaseTicks = 900;
	}
}
