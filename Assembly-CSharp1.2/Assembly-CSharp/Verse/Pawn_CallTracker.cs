using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000450 RID: 1104
	public class Pawn_CallTracker
	{
		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06001BC6 RID: 7110 RVA: 0x000ED978 File Offset: 0x000EBB78
		private bool PawnAggressive
		{
			get
			{
				return this.pawn.InAggroMentalState || (this.pawn.mindState.enemyTarget != null && this.pawn.mindState.enemyTarget.Spawned && Find.TickManager.TicksGame - this.pawn.mindState.lastEngageTargetTick <= 360) || (this.pawn.CurJob != null && this.pawn.CurJob.def == JobDefOf.AttackMelee);
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06001BC7 RID: 7111 RVA: 0x000EDA08 File Offset: 0x000EBC08
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

		// Token: 0x06001BC8 RID: 7112 RVA: 0x000194A9 File Offset: 0x000176A9
		public Pawn_CallTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06001BC9 RID: 7113 RVA: 0x000194BF File Offset: 0x000176BF
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

		// Token: 0x06001BCA RID: 7114 RVA: 0x000194F3 File Offset: 0x000176F3
		private void ResetTicksToNextCall()
		{
			this.ticksToNextCall = this.pawn.def.race.soundCallIntervalRange.RandomInRange;
			if (this.PawnAggressive)
			{
				this.ticksToNextCall /= 4;
			}
		}

		// Token: 0x06001BCB RID: 7115 RVA: 0x000EDA60 File Offset: 0x000EBC60
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

		// Token: 0x06001BCC RID: 7116 RVA: 0x000EDAD8 File Offset: 0x000EBCD8
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

		// Token: 0x06001BCD RID: 7117 RVA: 0x000EDB5C File Offset: 0x000EBD5C
		public void Notify_InAggroMentalState()
		{
			this.ticksToNextCall = Pawn_CallTracker.CallOnAggroDelayRange.RandomInRange;
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x000EDB7C File Offset: 0x000EBD7C
		public void Notify_DidMeleeAttack()
		{
			if (Rand.Value < 0.5f)
			{
				this.ticksToNextCall = Pawn_CallTracker.CallOnMeleeDelayRange.RandomInRange;
			}
		}

		// Token: 0x06001BCF RID: 7119 RVA: 0x000EDBA8 File Offset: 0x000EBDA8
		public void Notify_Released()
		{
			if (Rand.Value < 0.75f)
			{
				this.ticksToNextCall = Pawn_CallTracker.CallOnAggroDelayRange.RandomInRange;
			}
		}

		// Token: 0x04001417 RID: 5143
		public Pawn pawn;

		// Token: 0x04001418 RID: 5144
		private int ticksToNextCall = -1;

		// Token: 0x04001419 RID: 5145
		private static readonly IntRange CallOnAggroDelayRange = new IntRange(0, 120);

		// Token: 0x0400141A RID: 5146
		private static readonly IntRange CallOnMeleeDelayRange = new IntRange(0, 20);

		// Token: 0x0400141B RID: 5147
		private const float AngryCallOnMeleeChance = 0.5f;

		// Token: 0x0400141C RID: 5148
		private const int AggressiveDurationAfterEngagingTarget = 360;
	}
}
