using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200155A RID: 5466
	public class PlaceWorker_ShipLandingBeacon : PlaceWorker
	{
		// Token: 0x06008196 RID: 33174 RVA: 0x002DD2F8 File Offset: 0x002DB4F8
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
