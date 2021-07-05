using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005E5 RID: 1509
	public class MentalState_Binging : MentalState
	{
		// Token: 0x06002B9B RID: 11163 RVA: 0x0001276E File Offset: 0x0001096E
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
