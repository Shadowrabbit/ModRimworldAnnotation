using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000464 RID: 1124
	public class Pawn_StanceTracker : IExposable
	{
		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06001C89 RID: 7305 RVA: 0x00019C8B File Offset: 0x00017E8B
		public bool FullBodyBusy
		{
			get
			{
				return this.stunner.Stunned || this.curStance.StanceBusy;
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06001C8A RID: 7306 RVA: 0x00019CA7 File Offset: 0x00017EA7
		public bool Staggered
		{
			get
			{
				return Find.TickManager.TicksGame < this.staggerUntilTick;
			}
		}

		// Token: 0x06001C8B RID: 7307 RVA: 0x00019CBB File Offset: 0x00017EBB
		public Pawn_StanceTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.stunner = new StunHandler(this.pawn);
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x00019CED File Offset: 0x00017EED
		public void StanceTrackerTick()
		{
			this.stunner.StunHandlerTick();
			if (!this.stunner.Stunned)
			{
				this.curStance.StanceTick();
			}
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x00019D12 File Offset: 0x00017F12
		public void StanceTrackerDraw()
		{
			this.curStance.StanceDraw();
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x000F1418 File Offset: 0x000EF618
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.staggerUntilTick, "staggerUntilTick", 0, false);
			Scribe_Deep.Look<StunHandler>(ref this.stunner, "stunner", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<Stance>(ref this.curStance, "curStance", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.curStance != null)
			{
				this.curStance.stanceTracker = this;
			}
		}

		// Token: 0x06001C8F RID: 7311 RVA: 0x00019D1F File Offset: 0x00017F1F
		public void StaggerFor(int ticks)
		{
			this.staggerUntilTick = Find.TickManager.TicksGame + ticks;
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x00019D33 File Offset: 0x00017F33
		public void CancelBusyStanceSoft()
		{
			if (this.curStance is Stance_Warmup)
			{
				this.SetStance(new Stance_Mobile());
			}
		}

		// Token: 0x06001C91 RID: 7313 RVA: 0x00019D4D File Offset: 0x00017F4D
		public void CancelBusyStanceHard()
		{
			this.SetStance(new Stance_Mobile());
		}

		// Token: 0x06001C92 RID: 7314 RVA: 0x000F1488 File Offset: 0x000EF688
		public void SetStance(Stance newStance)
		{
			if (this.debugLog)
			{
				Log.Message(string.Concat(new object[]
				{
					Find.TickManager.TicksGame,
					" ",
					this.pawn,
					" SetStance ",
					this.curStance,
					" -> ",
					newStance
				}), false);
			}
			newStance.stanceTracker = this;
			this.curStance = newStance;
			if (this.pawn.jobs.curDriver != null)
			{
				this.pawn.jobs.curDriver.Notify_StanceChanged();
			}
		}

		// Token: 0x06001C93 RID: 7315 RVA: 0x00006A05 File Offset: 0x00004C05
		public void Notify_DamageTaken(DamageInfo dinfo)
		{
		}

		// Token: 0x0400146F RID: 5231
		public Pawn pawn;

		// Token: 0x04001470 RID: 5232
		public Stance curStance = new Stance_Mobile();

		// Token: 0x04001471 RID: 5233
		private int staggerUntilTick = -1;

		// Token: 0x04001472 RID: 5234
		public StunHandler stunner;

		// Token: 0x04001473 RID: 5235
		public const int StaggerMeleeAttackTicks = 95;

		// Token: 0x04001474 RID: 5236
		public const int StaggerBulletImpactTicks = 95;

		// Token: 0x04001475 RID: 5237
		public const int StaggerExplosionImpactTicks = 95;

		// Token: 0x04001476 RID: 5238
		public bool debugLog;
	}
}
