using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005E8 RID: 1512
	public class MentalState_GiveUpExit : MentalState
	{
		// Token: 0x06002BA8 RID: 11176 RVA: 0x0001276E File Offset: 0x0001096E
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
