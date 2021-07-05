using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A33 RID: 2611
	public class MentalState_WanderPsychotic : MentalState
	{
		// Token: 0x06003E4E RID: 15950 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
