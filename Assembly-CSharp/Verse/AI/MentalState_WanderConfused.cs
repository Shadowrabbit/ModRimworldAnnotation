using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A31 RID: 2609
	public class MentalState_WanderConfused : MentalState
	{
		// Token: 0x06003E4A RID: 15946 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
