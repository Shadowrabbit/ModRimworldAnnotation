using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001548 RID: 5448
	public class PlaceWorker_CoolerSimple : PlaceWorker
	{
		// Token: 0x06008166 RID: 33126 RVA: 0x002DC3C0 File Offset: 0x002DA5C0
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			Room room = center.GetRoom(currentMap);
			if (room != null && !room.UsesOutdoorTemperature)
			{
				GenDraw.DrawFieldEdges(room.Cells.ToList<IntVec3>(), GenTemperature.ColorRoomCold, null);
			}
		}
	}
}
