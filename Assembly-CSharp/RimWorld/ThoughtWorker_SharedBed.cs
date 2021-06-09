using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EB7 RID: 3767
	public class ThoughtWorker_SharedBed : ThoughtWorker
	{
		// Token: 0x060053BF RID: 21439 RVA: 0x0003A477 File Offset: 0x00038677
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
