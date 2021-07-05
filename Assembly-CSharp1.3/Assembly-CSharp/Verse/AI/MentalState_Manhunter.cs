using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005E9 RID: 1513
	public class MentalState_Manhunter : MentalState
	{
		// Token: 0x06002BAA RID: 11178 RVA: 0x00104506 File Offset: 0x00102706
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.AnimalsDontAttackDoors, OpportunityType.Critical);
		}

		// Token: 0x06002BAB RID: 11179 RVA: 0x0010451C File Offset: 0x0010271C
		public override bool ForceHostileTo(Thing t)
		{
			Pawn pawn;
			return ((pawn = (t as Pawn)) == null || !pawn.RaceProps.Roamer) && t.Faction != null && this.ForceHostileTo(t.Faction);
		}

		// Token: 0x06002BAC RID: 11180 RVA: 0x00104558 File Offset: 0x00102758
		public override bool ForceHostileTo(Faction f)
		{
			return f.def.humanlikeFaction || f == Faction.OfMechanoids;
		}

		// Token: 0x06002BAD RID: 11181 RVA: 0x0001276E File Offset: 0x0001096E
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
