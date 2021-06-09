using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DC9 RID: 7625
	public class PlaceWorker_OnSteamGeyser : PlaceWorker
	{
		// Token: 0x0600A5B9 RID: 42425 RVA: 0x0030197C File Offset: 0x002FFB7C
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			Thing thing2 = map.thingGrid.ThingAt(loc, ThingDefOf.SteamGeyser);
			if (thing2 == null || thing2.Position != loc)
			{
				return "MustPlaceOnSteamGeyser".Translate();
			}
			return true;
		}

		// Token: 0x0600A5BA RID: 42426 RVA: 0x0006DC3F File Offset: 0x0006BE3F
		public override bool ForceAllowPlaceOver(BuildableDef otherDef)
		{
			return otherDef == ThingDefOf.SteamGeyser;
		}
	}
}
