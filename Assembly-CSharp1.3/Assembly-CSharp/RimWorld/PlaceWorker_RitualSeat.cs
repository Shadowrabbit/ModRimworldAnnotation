using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001565 RID: 5477
	public class PlaceWorker_RitualSeat : PlaceWorker
	{
		// Token: 0x060081B7 RID: 33207 RVA: 0x002DDA2C File Offset: 0x002DBC2C
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			ThingDef thingDef = (def.entityDefToBuild != null) ? ((ThingDef)def.entityDefToBuild) : def;
			List<Thing> list = Find.CurrentMap.listerThings.ThingsOfDef(ThingDefOf.Lectern);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing2 = list[i];
				if (GatheringsUtility.InGatheringArea(center, thing2.Position, Find.CurrentMap) && SpectatorCellFinder.IsCorrectlyRotatedChair(center, rot, thingDef, thing2.OccupiedRect()))
				{
					GenDraw.DrawLineBetween(GenThing.TrueCenter(center, rot, thingDef.size, thingDef.Altitude), thing2.TrueCenter(), SimpleColor.Yellow, 0.2f);
					foreach (Thing thing3 in PlaceWorker_RitualPosition.GetRitualFocusInRange(thing2.Position, thing2))
					{
						if (GatheringsUtility.InGatheringArea(center, thing3.Position, Find.CurrentMap) && SpectatorCellFinder.IsCorrectlyRotatedChair(center, rot, thingDef, thing3.OccupiedRect()))
						{
							GenDraw.DrawLineBetween(thing2.TrueCenter(), thing3.TrueCenter(), SimpleColor.Green, 0.2f);
						}
					}
				}
			}
		}
	}
}
