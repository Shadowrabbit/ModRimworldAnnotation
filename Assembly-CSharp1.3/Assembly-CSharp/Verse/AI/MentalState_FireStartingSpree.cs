using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005EE RID: 1518
	public class MentalState_FireStartingSpree : MentalState
	{
		// Token: 0x06002BB9 RID: 11193 RVA: 0x0001276E File Offset: 0x0001096E
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
