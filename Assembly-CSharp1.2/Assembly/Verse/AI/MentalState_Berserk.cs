using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A2A RID: 2602
	public class MentalState_Berserk : MentalState
	{
		// Token: 0x06003E2E RID: 15918 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool ForceHostileTo(Thing t)
		{
			return true;
		}

		// Token: 0x06003E2F RID: 15919 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool ForceHostileTo(Faction f)
		{
			return true;
		}

		// Token: 0x06003E30 RID: 15920 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
