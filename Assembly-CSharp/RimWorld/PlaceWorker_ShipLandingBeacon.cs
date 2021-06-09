using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DE9 RID: 7657
	public class PlaceWorker_ShipLandingBeacon : PlaceWorker
	{
		// Token: 0x0600A610 RID: 42512 RVA: 0x00302A8C File Offset: 0x00300C8C
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			if (def.HasComp(typeof(CompShipLandingBeacon)))
			{
				ShipLandingBeaconUtility.DrawLinesToNearbyBeacons(def, center, rot, currentMap, thing);
			}
		}
	}
}
