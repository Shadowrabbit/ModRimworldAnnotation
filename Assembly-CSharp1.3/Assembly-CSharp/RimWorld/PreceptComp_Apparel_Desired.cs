using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EE8 RID: 3816
	public class PreceptComp_Apparel_Desired : PreceptComp_Apparel
	{
		// Token: 0x06005AA0 RID: 23200 RVA: 0x001F57CC File Offset: 0x001F39CC
		public override void Notify_MemberGenerated(Pawn pawn, Precept precept)
		{
			if (!base.AppliesToPawn(pawn, precept))
			{
				return;
			}
			Precept_Apparel precept_Apparel = (Precept_Apparel)precept;
			foreach (Apparel apparel in pawn.apparel.WornApparel)
			{
				if (!ApparelUtility.CanWearTogether(apparel.def, precept_Apparel.apparelDef, pawn.RaceProps.body) && (apparel.HasThingCategory(ThingCategoryDefOf.ArmorHeadgear) || apparel.HasThingCategory(ThingCategoryDefOf.ApparelArmor)))
				{
					return;
				}
			}
			base.GiveApparelToPawn(pawn, precept_Apparel);
		}
	}
}
