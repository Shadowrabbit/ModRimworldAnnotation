using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001549 RID: 5449
	public class PlaceWorker_Cooler : PlaceWorker
	{
		// Token: 0x06008168 RID: 33128 RVA: 0x002DC404 File Offset: 0x002DA604
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			IntVec3 intVec = center + IntVec3.South.RotatedBy(rot);
			IntVec3 intVec2 = center + IntVec3.North.RotatedBy(rot);
			GenDraw.DrawFieldEdges(new List<IntVec3>
			{
				intVec
			}, GenTemperature.ColorSpotCold, null);
			GenDraw.DrawFieldEdges(new List<IntVec3>
			{
				intVec2
			}, GenTemperature.ColorSpotHot, null);
			Room room = intVec2.GetRoom(currentMap);
			Room room2 = intVec.GetRoom(currentMap);
			if (room != null && room2 != null)
			{
				if (room == room2 && !room.UsesOutdoorTemperature)
				{
					GenDraw.DrawFieldEdges(room.Cells.ToList<IntVec3>(), new Color(1f, 0.7f, 0f, 0.5f), null);
					return;
				}
				if (!room.UsesOutdoorTemperature)
				{
					GenDraw.DrawFieldEdges(room.Cells.ToList<IntVec3>(), GenTemperature.ColorRoomHot, null);
				}
				if (!room2.UsesOutdoorTemperature)
				{
					GenDraw.DrawFieldEdges(room2.Cells.ToList<IntVec3>(), GenTemperature.ColorRoomCold, null);
				}
			}
		}

		// Token: 0x06008169 RID: 33129 RVA: 0x002DC52C File Offset: 0x002DA72C
		public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			IntVec3 c = center + IntVec3.South.RotatedBy(rot);
			IntVec3 c2 = center + IntVec3.North.RotatedBy(rot);
			if (c.Impassable(map) || c2.Impassable(map))
			{
				return "MustPlaceCoolerWithFreeSpaces".Translate();
			}
			Frame firstThing = c.GetFirstThing(map);
			Frame firstThing2 = c2.GetFirstThing(map);
			if ((firstThing != null && firstThing.def.entityDefToBuild != null && firstThing.def.entityDefToBuild.passability == Traversability.Impassable) || (firstThing2 != null && firstThing2.def.entityDefToBuild != null && firstThing2.def.entityDefToBuild.passability == Traversability.Impassable))
			{
				return "MustPlaceCoolerWithFreeSpaces".Translate();
			}
			Blueprint firstThing3 = c.GetFirstThing(map);
			Blueprint firstThing4 = c2.GetFirstThing(map);
			if ((firstThing3 != null && firstThing3.def.entityDefToBuild != null && firstThing3.def.entityDefToBuild.passability == Traversability.Impassable) || (firstThing4 != null && firstThing4.def.entityDefToBuild != null && firstThing4.def.entityDefToBuild.passability == Traversability.Impassable))
			{
				return "MustPlaceCoolerWithFreeSpaces".Translate();
			}
			return true;
		}
	}
}
