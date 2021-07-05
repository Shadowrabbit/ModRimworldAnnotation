using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AD RID: 2477
	public class ThoughtWorker_SharedBed : ThoughtWorker
	{
		// Token: 0x06003DE8 RID: 15848 RVA: 0x00153B90 File Offset: 0x00151D90
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			return LovePartnerRelationUtility.GetMostDislikedNonPartnerBedOwner(p) != null;
		}
	}
}
