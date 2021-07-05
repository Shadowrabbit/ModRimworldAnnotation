using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200153F RID: 5439
	public class PlaceWorker_HeadOnShipBeam : PlaceWorker
	{
		// Token: 0x06008153 RID: 33107 RVA: 0x002DC0B4 File Offset: 0x002DA2B4
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
