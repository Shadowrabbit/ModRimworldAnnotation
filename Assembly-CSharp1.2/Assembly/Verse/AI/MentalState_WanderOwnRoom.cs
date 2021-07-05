using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A30 RID: 2608
	public class MentalState_WanderOwnRoom : MentalState
	{
		// Token: 0x06003E46 RID: 15942 RVA: 0x001781F4 File Offset: 0x001763F4
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			if (this.pawn.ownership.OwnedBed != null)
			{
				this.target = this.pawn.ownership.OwnedBed.Position;
				return;
			}
			this.target = this.pawn.Position;
		}

		// Token: 0x06003E47 RID: 15943 RVA: 0x00178248 File Offset: 0x00176448
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.target, "target", default(IntVec3), false);
		}

		// Token: 0x06003E48 RID: 15944 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x04002AF8 RID: 11000
		public IntVec3 target;
	}
}
