using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A2E RID: 2606
	public class MentalState_GiveUpExit : MentalState
	{
		// Token: 0x06003E3F RID: 15935 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
