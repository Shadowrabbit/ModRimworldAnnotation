using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DC8 RID: 7624
	public class PlaceWorker_NotUnderRoof : PlaceWorker
	{
		// Token: 0x0600A5B7 RID: 42423 RVA: 0x0006DC13 File Offset: 0x0006BE13
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			if (map.roofGrid.Roofed(loc))
			{
				return new AcceptanceReport("MustPlaceUnroofed".Translate());
			}
			return true;
		}
	}
}
