using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005ED RID: 1517
	public class MentalState_WanderPsychotic : MentalState
	{
		// Token: 0x06002BB7 RID: 11191 RVA: 0x0001276E File Offset: 0x0001096E
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
