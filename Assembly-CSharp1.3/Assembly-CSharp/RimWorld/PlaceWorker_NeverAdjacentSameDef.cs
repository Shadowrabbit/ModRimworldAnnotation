using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200154D RID: 5453
	public class PlaceWorker_NeverAdjacentSameDef : PlaceWorker
	{
		// Token: 0x06008174 RID: 33140 RVA: 0x002DC9FC File Offset: 0x002DABFC
		public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			foreach (IntVec3 c in GenAdj.OccupiedRect(center, rot, def.Size).ExpandedBy(1))
			{
				if (c.InBounds(map))
				{
					List<Thing> list = map.thingGrid.ThingsListAt(c);
					for (int i = 0; i < list.Count; i++)
					{
						Thing thing2 = list[i];
						ThingDef thingDef;
						if (thing2 != thingToIgnore && ((thing2.def.category == ThingCategory.Building && thing2.def == def) || ((thing2.def.IsBlueprint || thing2.def.IsFrame) && (thingDef = (thing2.def.entityDefToBuild as ThingDef)) != null && thingDef == def)))
						{
							return "CannotPlaceAdjacentSameDef".Translate();
						}
					}
				}
			}
			return true;
		}
	}
}
