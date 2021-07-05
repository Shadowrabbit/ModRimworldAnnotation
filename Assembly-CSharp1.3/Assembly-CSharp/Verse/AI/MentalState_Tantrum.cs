using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005F3 RID: 1523
	public abstract class MentalState_Tantrum : MentalState
	{
		// Token: 0x06002BD2 RID: 11218 RVA: 0x00104AF4 File Offset: 0x00102CF4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
			Scribe_Values.Look<bool>(ref this.hitTargetAtLeastOnce, "hitTargetAtLeastOnce", false, false);
		}

		// Token: 0x06002BD3 RID: 11219 RVA: 0x0001276E File Offset: 0x0001096E
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x06002BD4 RID: 11220 RVA: 0x00104B1F File Offset: 0x00102D1F
		public override void Notify_AttackedTarget(LocalTargetInfo hitTarget)
		{
			base.Notify_AttackedTarget(hitTarget);
			if (this.target != null && hitTarget.Thing == this.target)
			{
				this.hitTargetAtLeastOnce = true;
			}
		}

		// Token: 0x04001AA2 RID: 6818
		public Thing target;

		// Token: 0x04001AA3 RID: 6819
		protected bool hitTargetAtLeastOnce;
	}
}
