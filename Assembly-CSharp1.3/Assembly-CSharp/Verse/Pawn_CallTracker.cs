using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002ED RID: 749
	public class Pawn_CallTracker
	{
		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06001569 RID: 5481 RVA: 0x0007C520 File Offset: 0x0007A720
		private bool PawnAggressive
		{
			get
			{
				return this.pawn.InAggroMentalState || (this.pawn.mindState.enemyTarget != null && this.pawn.mindState.enemyTarget.Spawned && Find.TickManager.TicksGame - this.pawn.mindState.lastEngageTargetTick <= 360) || (this.pawn.CurJob != null && this.pawn.CurJob.def == JobDefOf.AttackMelee);
			}
		}

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x0600156A RID: 5482 RVA: 0x0007C5B0 File Offset: 0x0007A7B0
		private float IdleCallVolumeFactor
		{
			get
			{
				switch (Find.TickManager.CurTimeSpeed)
				{
				case TimeSpeed.Paused:
					return 1f;
				case TimeSpeed.Normal:
					return 1f;
				case TimeSpeed.Fast:
					return 1f;
				case TimeSpeed.Superfast:
					return 0.25f;
				case TimeSpeed.Ultrafast:
					return 0.25f;
				default:
					throw new NotImplementedException();
				}
			}
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x0007C607 File Offset: 0x0007A807
		public Pawn_CallTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x0007C61D File Offset: 0x0007A81D
		public void CallTrackerTick()
		{
			if (this.ticksToNextCall < 0)
			{
				this.ResetTicksToNextCall();
			}
			this.ticksToNextCall--;
			if (this.ticksToNextCall <= 0)
			{
				this.TryDoCall();
				this.ResetTicksToNextCall();
			}
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x0007C651 File Offset: 0x0007A851
		private void ResetTicksToNextCall()
		{
			this.ticksToNextCall = this.pawn.def.race.soundCallIntervalRange.RandomInRange;
			if (this.PawnAggressive)
			{
				this.ticksToNextCall /= 4;
			}
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x0007C68C File Offset: 0x0007A88C
		private void TryDoCall()
		{
			if (!Find.CameraDriver.CurrentViewRect.ExpandedBy(10).Contains(this.pawn.Position))
			{
				return;
			}
			if (this.pawn.Downed || !this.pawn.Awake())
			{
				return;
			}
			if (this.pawn.Position.Fogged(this.pawn.Map))
			{
				return;
			}
			this.DoCall();
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x0007C704 File Offset: 0x0007A904
		public void DoCall()
		{
			if (!this.pawn.Spawned)
			{
				return;
			}
			if (this.PawnAggressive)
			{
				LifeStageUtility.PlayNearestLifestageSound(this.pawn, (LifeStageAge ls) => ls.soundAngry, 1f);
				return;
			}
			LifeStageUtility.PlayNearestLifestageSound(this.pawn, (LifeStageAge ls) => ls.soundCall, this.IdleCallVolumeFactor);
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x0007C788 File Offset: 0x0007A988
		public void Notify_InAggroMentalState()
		{
			this.ticksToNextCall = Pawn_CallTracker.CallOnAggroDelayRange.RandomInRange;
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x0007C7A8 File Offset: 0x0007A9A8
		public void Notify_DidMeleeAttack()
		{
			if (Rand.Value < 0.5f)
			{
				this.ticksToNextCall = Pawn_CallTracker.CallOnMeleeDelayRange.RandomInRange;
			}
		}

		// Token: 0x06001572 RID: 5490 RVA: 0x0007C7D4 File Offset: 0x0007A9D4
		public void Notify_Released()
		{
			if (Rand.Value < 0.75f)
			{
				this.ticksToNextCall = Pawn_CallTracker.CallOnAggroDelayRange.RandomInRange;
			}
		}

		// Token: 0x04000F1B RID: 3867
		public Pawn pawn;

		// Token: 0x04000F1C RID: 3868
		private int ticksToNextCall = -1;

		// Token: 0x04000F1D RID: 3869
		private static readonly IntRange CallOnAggroDelayRange = new IntRange(0, 120);

		// Token: 0x04000F1E RID: 3870
		private static readonly IntRange CallOnMeleeDelayRange = new IntRange(0, 20);

		// Token: 0x04000F1F RID: 3871
		private const float AngryCallOnMeleeChance = 0.5f;

		// Token: 0x04000F20 RID: 3872
		private const int AggressiveDurationAfterEngagingTarget = 360;
	}
}
