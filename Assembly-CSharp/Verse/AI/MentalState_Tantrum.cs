using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A38 RID: 2616
	public abstract class MentalState_Tantrum : MentalState
	{
		// Token: 0x06003E61 RID: 15969 RVA: 0x0002EE92 File Offset: 0x0002D092
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
			Scribe_Values.Look<bool>(ref this.hitTargetAtLeastOnce, "hitTargetAtLeastOnce", false, false);
		}

		// Token: 0x06003E62 RID: 15970 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x06003E63 RID: 15971 RVA: 0x0002EEBD File Offset: 0x0002D0BD
		public override void Notify_AttackedTarget(LocalTargetInfo hitTarget)
		{
			base.Notify_AttackedTarget(hitTarget);
			if (this.target != null && hitTarget.Thing == this.target)
			{
				this.hitTargetAtLeastOnce = true;
			}
		}

		// Token: 0x04002AFC RID: 11004
		public Thing target;

		// Token: 0x04002AFD RID: 11005
		protected bool hitTargetAtLeastOnce;
	}
}
