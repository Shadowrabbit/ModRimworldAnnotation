using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A77 RID: 2679
	public class ThinkNode_ChancePerHour_Lovin : ThinkNode_ChancePerHour
	{
		// Token: 0x06003FF7 RID: 16375 RVA: 0x00181C30 File Offset: 0x0017FE30
		protected override float MtbHours(Pawn pawn)
		{
			if (pawn.CurrentBed() == null)
			{
				return -1f;
			}
			Pawn partnerInMyBed = LovePartnerRelationUtility.GetPartnerInMyBed(pawn);
			if (partnerInMyBed == null)
			{
				return -1f;
			}
			return LovePartnerRelationUtility.GetLovinMtbHours(pawn, partnerInMyBed);
		}
	}
}
