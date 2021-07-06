using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A22 RID: 2594
	public abstract class MentalState_InsultingSpree : MentalState
	{
		// Token: 0x06003DF0 RID: 15856 RVA: 0x0002EA11 File Offset: 0x0002CC11
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
			Scribe_Values.Look<bool>(ref this.insultedTargetAtLeastOnce, "insultedTargetAtLeastOnce", false, false);
			Scribe_Values.Look<int>(ref this.lastInsultTicks, "lastInsultTicks", 0, false);
		}

		// Token: 0x06003DF1 RID: 15857 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x04002ADC RID: 10972
		public Pawn target;

		// Token: 0x04002ADD RID: 10973
		public bool insultedTargetAtLeastOnce;

		// Token: 0x04002ADE RID: 10974
		public int lastInsultTicks = -999999;
	}
}
