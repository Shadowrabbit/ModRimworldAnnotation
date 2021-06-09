using System;

namespace Verse
{
	// Token: 0x02000467 RID: 1127
	public abstract class Stance_Busy : Stance
	{
		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06001C9B RID: 7323 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool StanceBusy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001C9C RID: 7324 RVA: 0x00019D6F File Offset: 0x00017F6F
		public Stance_Busy()
		{
			this.SetPieSizeFactor();
		}

		// Token: 0x06001C9D RID: 7325 RVA: 0x00019D88 File Offset: 0x00017F88
		public Stance_Busy(int ticks, LocalTargetInfo focusTarg, Verb verb)
		{
			this.ticksLeft = ticks;
			this.focusTarg = focusTarg;
			this.verb = verb;
		}

		// Token: 0x06001C9E RID: 7326 RVA: 0x00019DB0 File Offset: 0x00017FB0
		public Stance_Busy(int ticks) : this(ticks, null, null)
		{
		}

		// Token: 0x06001C9F RID: 7327 RVA: 0x00019DC0 File Offset: 0x00017FC0
		private void SetPieSizeFactor()
		{
			if (this.ticksLeft < 300)
			{
				this.pieSizeFactor = 1f;
				return;
			}
			if (this.ticksLeft < 450)
			{
				this.pieSizeFactor = 0.75f;
				return;
			}
			this.pieSizeFactor = 0.5f;
		}

		// Token: 0x06001CA0 RID: 7328 RVA: 0x000F1524 File Offset: 0x000EF724
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
			Scribe_TargetInfo.Look(ref this.focusTarg, "focusTarg");
			Scribe_Values.Look<bool>(ref this.neverAimWeapon, "neverAimWeapon", false, false);
			Scribe_References.Look<Verb>(ref this.verb, "verb", false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.SetPieSizeFactor();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.verb != null && this.verb.BuggedAfterLoading)
			{
				this.verb = null;
				Log.Warning(base.GetType() + " had a bugged verb after loading.", false);
			}
		}

		// Token: 0x06001CA1 RID: 7329 RVA: 0x00019DFF File Offset: 0x00017FFF
		public override void StanceTick()
		{
			this.ticksLeft--;
			if (this.ticksLeft <= 0)
			{
				this.Expire();
			}
		}

		// Token: 0x06001CA2 RID: 7330 RVA: 0x00019E1E File Offset: 0x0001801E
		protected virtual void Expire()
		{
			if (this.stanceTracker.curStance == this)
			{
				this.stanceTracker.SetStance(new Stance_Mobile());
			}
		}

		// Token: 0x04001478 RID: 5240
		public int ticksLeft;

		// Token: 0x04001479 RID: 5241
		public Verb verb;

		// Token: 0x0400147A RID: 5242
		public LocalTargetInfo focusTarg;

		// Token: 0x0400147B RID: 5243
		public bool neverAimWeapon;

		// Token: 0x0400147C RID: 5244
		protected float pieSizeFactor = 1f;
	}
}
