using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005D9 RID: 1497
	public abstract class MentalState_InsultingSpree : MentalState
	{
		// Token: 0x06002B4D RID: 11085 RVA: 0x00102ED2 File Offset: 0x001010D2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
			Scribe_Values.Look<bool>(ref this.insultedTargetAtLeastOnce, "insultedTargetAtLeastOnce", false, false);
			Scribe_Values.Look<int>(ref this.lastInsultTicks, "lastInsultTicks", 0, false);
		}

		// Token: 0x06002B4E RID: 11086 RVA: 0x0001276E File Offset: 0x0001096E
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x04001A74 RID: 6772
		public Pawn target;

		// Token: 0x04001A75 RID: 6773
		public bool insultedTargetAtLeastOnce;

		// Token: 0x04001A76 RID: 6774
		public int lastInsultTicks = -999999;
	}
}
