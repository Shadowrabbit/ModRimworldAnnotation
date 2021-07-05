using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200153C RID: 5436
	public class Placeworker_OnlyUnderThickRoof : PlaceWorker
	{
		// Token: 0x0600814C RID: 33100 RVA: 0x002DBF84 File Offset: 0x002DA184
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			RoofDef roofDef = map.roofGrid.RoofAt(loc);
			if (roofDef == null || !roofDef.isThickRoof)
			{
				return new AcceptanceReport("MustPlaceUnderThickRoof".Translate());
			}
			return true;
		}
	}
}
