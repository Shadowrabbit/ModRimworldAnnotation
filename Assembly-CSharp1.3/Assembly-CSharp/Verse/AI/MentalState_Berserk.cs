using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005E4 RID: 1508
	public class MentalState_Berserk : MentalState
	{
		// Token: 0x06002B97 RID: 11159 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool ForceHostileTo(Thing t)
		{
			return true;
		}

		// Token: 0x06002B98 RID: 11160 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool ForceHostileTo(Faction f)
		{
			return true;
		}

		// Token: 0x06002B99 RID: 11161 RVA: 0x0001276E File Offset: 0x0001096E
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
