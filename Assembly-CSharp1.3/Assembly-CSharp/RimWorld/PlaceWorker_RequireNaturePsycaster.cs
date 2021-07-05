using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200155F RID: 5471
	public class PlaceWorker_RequireNaturePsycaster : PlaceWorker
	{
		// Token: 0x060081A1 RID: 33185 RVA: 0x002DD478 File Offset: 0x002DB678
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
