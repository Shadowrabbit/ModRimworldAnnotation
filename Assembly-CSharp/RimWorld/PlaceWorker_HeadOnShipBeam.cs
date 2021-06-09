using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DCB RID: 7627
	public class PlaceWorker_HeadOnShipBeam : PlaceWorker
	{
		// Token: 0x0600A5BE RID: 42430 RVA: 0x00301A5C File Offset: 0x002FFC5C
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			IntVec3 c = loc + rot.FacingCell * -1;
			if (!c.InBounds(map))
			{
				return false;
			}
			Building edifice = c.GetEdifice(map);
			if (edifice == null || edifice.def != ThingDefOf.Ship_Beam)
			{
				return "MustPlaceHeadOnShipBeam".Translate();
			}
			return true;
		}
	}
}
