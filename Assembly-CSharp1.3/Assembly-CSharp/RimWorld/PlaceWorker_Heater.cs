using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001542 RID: 5442
	public class PlaceWorker_Heater : PlaceWorker
	{
		// Token: 0x06008159 RID: 33113 RVA: 0x002DC1B4 File Offset: 0x002DA3B4
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			Room room = center.GetRoom(currentMap);
			if (room != null && !room.UsesOutdoorTemperature)
			{
				GenDraw.DrawFieldEdges(room.Cells.ToList<IntVec3>(), GenTemperature.ColorRoomHot, null);
			}
		}
	}
}
