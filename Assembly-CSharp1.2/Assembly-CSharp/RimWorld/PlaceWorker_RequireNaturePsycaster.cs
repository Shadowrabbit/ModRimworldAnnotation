using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DED RID: 7661
	public class PlaceWorker_RequireNaturePsycaster : PlaceWorker
	{
		// Token: 0x0600A619 RID: 42521 RVA: 0x00302BC4 File Offset: 0x00300DC4
		public override bool IsBuildDesignatorVisible(BuildableDef def)
		{
			foreach (Pawn p in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
			{
				if (MeditationFocusDefOf.Natural.CanPawnUse(p))
				{
					return true;
				}
			}
			return false;
		}
	}
}
