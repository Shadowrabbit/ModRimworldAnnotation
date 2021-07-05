using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200153B RID: 5435
	public class PlaceWorker_NotUnderRoof : PlaceWorker
	{
		// Token: 0x0600814A RID: 33098 RVA: 0x002DBF58 File Offset: 0x002DA158
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
