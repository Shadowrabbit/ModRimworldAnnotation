using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000618 RID: 1560
	public class ThinkNode_ChancePerHour_Lovin : ThinkNode_ChancePerHour
	{
		// Token: 0x06002D16 RID: 11542 RVA: 0x0010F0D4 File Offset: 0x0010D2D4
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
