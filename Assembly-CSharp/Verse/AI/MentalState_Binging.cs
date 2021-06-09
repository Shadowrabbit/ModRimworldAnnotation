using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A2B RID: 2603
	public class MentalState_Binging : MentalState
	{
		// Token: 0x06003E32 RID: 15922 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
