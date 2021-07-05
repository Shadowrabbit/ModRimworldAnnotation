using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200154A RID: 5450
	public class PlaceWorker_Vent : PlaceWorker
	{
		// Token: 0x0600816B RID: 33131 RVA: 0x002DC660 File Offset: 0x002DA860
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			IntVec3 intVec = center + IntVec3.South.RotatedBy(rot);
			IntVec3 intVec2 = center + IntVec3.North.RotatedBy(rot);
			GenDraw.DrawFieldEdges(new List<IntVec3>
			{
				intVec
			}, Color.white, null);
			GenDraw.DrawFieldEdges(new List<IntVec3>
			{
				intVec2
			}, Color.white, null);
			Room room = intVec2.GetRoom(currentMap);
			Room room2 = intVec.GetRoom(currentMap);
			if (room != null && room2 != null)
			{
				if (room == room2 && !room.UsesOutdoorTemperature)
				{
					GenDraw.DrawFieldEdges(room.Cells.ToList<IntVec3>(), Color.white, null);
					return;
				}
				if (!room.UsesOutdoorTemperature)
				{
					GenDraw.DrawFieldEdges(room.Cells.ToList<IntVec3>(), Color.white, null);
				}
				if (!room2.UsesOutdoorTemperature)
				{
					GenDraw.DrawFieldEdges(room2.Cells.ToList<IntVec3>(), Color.white, null);
				}
			}
		}

		// Token: 0x0600816C RID: 33132 RVA: 0x002DC770 File Offset: 0x002DA970
		public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			IntVec3 c = center + IntVec3.South.RotatedBy(rot);
			IntVec3 c2 = center + IntVec3.North.RotatedBy(rot);
			if (c.Impassable(map) || c2.Impassable(map))
			{
				return "MustPlaceVentWithFreeSpaces".Translate();
			}
			return true;
		}
	}
}
