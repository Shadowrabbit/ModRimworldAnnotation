using System;

namespace Verse
{
	// Token: 0x020002F7 RID: 759
	public abstract class Stance_Busy : Stance
	{
		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06001607 RID: 5639 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool StanceBusy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001608 RID: 5640 RVA: 0x000803BF File Offset: 0x0007E5BF
		public Stance_Busy()
		{
			this.SetPieSizeFactor();
		}

		// Token: 0x06001609 RID: 5641 RVA: 0x000803D8 File Offset: 0x0007E5D8
		public Stance_Busy(int ticks, LocalTargetInfo focusTarg, Verb verb)
		{
			this.ticksLeft = ticks;
			this.focusTarg = focusTarg;
			this.verb = verb;
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x00080400 File Offset: 0x0007E600
		public Stance_Busy(int ticks) : this(ticks, null, null)
		{
		}

		// Token: 0x0600160B RID: 5643 RVA: 0x00080410 File Offset: 0x0007E610
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

		// Token: 0x0600160C RID: 5644 RVA: 0x00080450 File Offset: 0x0007E650
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
				Log.Warning(base.GetType() + " had a bugged verb after loading.");
			}
		}

		// Token: 0x0600160D RID: 5645 RVA: 0x000804EF File Offset: 0x0007E6EF
		public override void StanceTick()
		{
			this.ticksLeft--;
			if (this.ticksLeft <= 0)
			{
				this.Expire();
			}
		}

		// Token: 0x0600160E RID: 5646 RVA: 0x0008050E File Offset: 0x0007E70E
		protected virtual void Expire()
		{
			if (this.stanceTracker.curStance == this)
			{
				this.stanceTracker.SetStance(new Stance_Mobile());
			}
		}

		// Token: 0x04000F4A RID: 3914
		public int ticksLeft;

		// Token: 0x04000F4B RID: 3915
		public Verb verb;

		// Token: 0x04000F4C RID: 3916
		public LocalTargetInfo focusTarg;

		// Token: 0x04000F4D RID: 3917
		public bool neverAimWeapon;

		// Token: 0x04000F4E RID: 3918
		protected float pieSizeFactor = 1f;
	}
}
