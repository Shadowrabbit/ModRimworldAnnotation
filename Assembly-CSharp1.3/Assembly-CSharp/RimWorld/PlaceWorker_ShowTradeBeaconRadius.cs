using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200154F RID: 5455
	public class PlaceWorker_ShowTradeBeaconRadius : PlaceWorker
	{
		// Token: 0x06008179 RID: 33145 RVA: 0x002DCBA0 File Offset: 0x002DADA0
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			GenDraw.DrawFieldEdges(Building_OrbitalTradeBeacon.TradeableCellsAround(center, currentMap));
		}
	}
}
