using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A34 RID: 2612
	public class MentalState_FireStartingSpree : MentalState
	{
		// Token: 0x06003E50 RID: 15952 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
