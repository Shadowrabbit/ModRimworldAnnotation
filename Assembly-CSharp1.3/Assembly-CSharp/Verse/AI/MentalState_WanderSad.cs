using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005EC RID: 1516
	public class MentalState_WanderSad : MentalState
	{
		// Token: 0x06002BB5 RID: 11189 RVA: 0x0001276E File Offset: 0x0001096E
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
