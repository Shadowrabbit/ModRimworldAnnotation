using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001558 RID: 5464
	public class PlaceWorker_PreventInteractionSpotOverlap : PlaceWorker
	{
		// Token: 0x06008192 RID: 33170 RVA: 0x002DD150 File Offset: 0x002DB350
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thingToPlace = null)
		{
			ThingDef thingDef = checkingDef as ThingDef;
			if (thingDef == null || !thingDef.hasInteractionCell)
			{
				return true;
			}
			IntVec3 intVec = ThingUtility.InteractionCellWhenAt(thingDef, loc, rot, map);
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					IntVec3 c = intVec;
					c.x += i;
					c.z += j;
					if (c.InBounds(map))
					{
						foreach (Thing thing in map.thingGrid.ThingsListAtFast(c))
						{
							if (thing != thingToIgnore)
							{
								ThingDef thingDef2 = thing.def;
								if (thing.def.entityDefToBuild != null)
								{
									thingDef2 = (thing.def.entityDefToBuild as ThingDef);
								}
								if (thingDef2 != null && thingDef2.hasInteractionCell && ThingUtility.InteractionCellWhenAt(thingDef2, thing.Position, thing.Rotation, thing.Map) == intVec)
								{
									return new AcceptanceReport(((thing.def.entityDefToBuild == null) ? "InteractionSpotOverlaps" : "InteractionSpotWillOverlap").Translate(thing.LabelNoCount, thing));
								}
							}
						}
					}
				}
			}
			return true;
		}
	}
}
