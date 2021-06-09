using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A32 RID: 2610
	public class MentalState_WanderSad : MentalState
	{
		// Token: 0x06003E4C RID: 15948 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
