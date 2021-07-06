using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DDC RID: 7644
	public class PlaceWorker_ShowTradeBeaconRadius : PlaceWorker
	{
		// Token: 0x0600A5E7 RID: 42471 RVA: 0x00302308 File Offset: 0x00300508
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			GenDraw.DrawFieldEdges(Building_OrbitalTradeBeacon.TradeableCellsAround(center, currentMap));
		}
	}
}
