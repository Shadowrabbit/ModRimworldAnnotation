using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002F4 RID: 756
	public class Pawn_StanceTracker : IExposable
	{
		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x060015F5 RID: 5621 RVA: 0x000801CF File Offset: 0x0007E3CF
		public bool FullBodyBusy
		{
			get
			{
				return this.stunner.Stunned || this.curStance.StanceBusy;
			}
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x060015F6 RID: 5622 RVA: 0x000801EB File Offset: 0x0007E3EB
		public bool Staggered
		{
			get
			{
				return Find.TickManager.TicksGame < this.staggerUntilTick;
			}
		}

		// Token: 0x060015F7 RID: 5623 RVA: 0x000801FF File Offset: 0x0007E3FF
		public Pawn_StanceTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.stunner = new StunHandler(this.pawn);
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x00080231 File Offset: 0x0007E431
		public void StanceTrackerTick()
		{
			this.stunner.StunHandlerTick();
			if (!this.stunner.Stunned)
			{
				this.curStance.StanceTick();
			}
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x00080256 File Offset: 0x0007E456
		public void StanceTrackerDraw()
		{
			this.curStance.StanceDraw();
		}

		// Token: 0x060015FA RID: 5626 RVA: 0x00080264 File Offset: 0x0007E464
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

		// Token: 0x060015FB RID: 5627 RVA: 0x000802D3 File Offset: 0x0007E4D3
		public void StaggerFor(int ticks)
		{
			this.staggerUntilTick = Find.TickManager.TicksGame + ticks;
		}

		// Token: 0x060015FC RID: 5628 RVA: 0x000802E7 File Offset: 0x0007E4E7
		public void CancelBusyStanceSoft()
		{
			if (this.curStance is Stance_Warmup)
			{
				this.SetStance(new Stance_Mobile());
			}
		}

		// Token: 0x060015FD RID: 5629 RVA: 0x00080301 File Offset: 0x0007E501
		public void CancelBusyStanceHard()
		{
			this.SetStance(new Stance_Mobile());
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x00080310 File Offset: 0x0007E510
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
				}));
			}
			newStance.stanceTracker = this;
			this.curStance = newStance;
			if (this.pawn.jobs.curDriver != null)
			{
				this.pawn.jobs.curDriver.Notify_StanceChanged();
			}
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x0000313F File Offset: 0x0000133F
		public void Notify_DamageTaken(DamageInfo dinfo)
		{
		}

		// Token: 0x04000F41 RID: 3905
		public Pawn pawn;

		// Token: 0x04000F42 RID: 3906
		public Stance curStance = new Stance_Mobile();

		// Token: 0x04000F43 RID: 3907
		private int staggerUntilTick = -1;

		// Token: 0x04000F44 RID: 3908
		public StunHandler stunner;

		// Token: 0x04000F45 RID: 3909
		public const int StaggerMeleeAttackTicks = 95;

		// Token: 0x04000F46 RID: 3910
		public const int StaggerBulletImpactTicks = 95;

		// Token: 0x04000F47 RID: 3911
		public const int StaggerExplosionImpactTicks = 95;

		// Token: 0x04000F48 RID: 3912
		public bool debugLog;
	}
}
