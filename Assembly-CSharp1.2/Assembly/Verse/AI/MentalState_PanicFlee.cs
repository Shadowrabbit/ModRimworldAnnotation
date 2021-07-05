using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A2D RID: 2605
	public class MentalState_PanicFlee : MentalState
	{
		// Token: 0x06003E3D RID: 15933 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
