using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005EB RID: 1515
	public class MentalState_WanderConfused : MentalState
	{
		// Token: 0x06002BB3 RID: 11187 RVA: 0x0001276E File Offset: 0x0001096E
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
