using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A2F RID: 2607
	public class MentalState_Manhunter : MentalState
	{
		// Token: 0x06003E41 RID: 15937 RVA: 0x0002ED4A File Offset: 0x0002CF4A
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.AnimalsDontAttackDoors, OpportunityType.Critical);
		}

		// Token: 0x06003E42 RID: 15938 RVA: 0x0002ED5E File Offset: 0x0002CF5E
		public override bool ForceHostileTo(Thing t)
		{
			return t.Faction != null && this.ForceHostileTo(t.Faction);
		}

		// Token: 0x06003E43 RID: 15939 RVA: 0x0002ED76 File Offset: 0x0002CF76
		public override bool ForceHostileTo(Faction f)
		{
			return f.def.humanlikeFaction || f == Faction.OfMechanoids;
		}

		// Token: 0x06003E44 RID: 15940 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
