using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D24 RID: 3364
	public class JobGiver_Berserk : ThinkNode_JobGiver
	{
		// Token: 0x06004D1C RID: 19740 RVA: 0x001AD2A8 File Offset: 0x001AB4A8
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
				job2.canBash = true;
				return job2;
			}
			return null;
		}

		// Token: 0x06004D1D RID: 19741 RVA: 0x001AD328 File Offset: 0x001AB528
		private Pawn FindPawnTarget(Pawn pawn)
		{
			return (Pawn)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedReachable, delegate(Thing x)
			{
				Pawn pawn2;
				return (pawn2 = (x as Pawn)) != null && pawn2.Spawned && !pawn2.Downed && !pawn2.IsInvisible();
			}, 0f, 40f, default(IntVec3), float.MaxValue, true, true);
		}

		// Token: 0x040032BB RID: 12987
		private const float MaxAttackDistance = 40f;

		// Token: 0x040032BC RID: 12988
		private const float WaitChance = 0.5f;

		// Token: 0x040032BD RID: 12989
		private const int WaitTicks = 90;

		// Token: 0x040032BE RID: 12990
		private const int MinMeleeChaseTicks = 420;

		// Token: 0x040032BF RID: 12991
		private const int MaxMeleeChaseTicks = 900;
	}
}
