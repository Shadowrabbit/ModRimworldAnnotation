using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200153D RID: 5437
	public class PlaceWorker_OnSteamGeyser : PlaceWorker
	{
		// Token: 0x0600814E RID: 33102 RVA: 0x002DBFC8 File Offset: 0x002DA1C8
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			Thing thing2 = map.thingGrid.ThingAt(loc, ThingDefOf.SteamGeyser);
			if (thing2 == null || thing2.Position != loc)
			{
				return "MustPlaceOnSteamGeyser".Translate();
			}
			return true;
		}

		// Token: 0x0600814F RID: 33103 RVA: 0x002DC00F File Offset: 0x002DA20F
		public override bool ForceAllowPlaceOver(BuildableDef otherDef)
		{
			return otherDef == ThingDefOf.SteamGeyser;
		}
	}
}
