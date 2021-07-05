using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005E7 RID: 1511
	public class MentalState_PanicFlee : MentalState
	{
		// Token: 0x06002BA6 RID: 11174 RVA: 0x0001276E File Offset: 0x0001096E
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
