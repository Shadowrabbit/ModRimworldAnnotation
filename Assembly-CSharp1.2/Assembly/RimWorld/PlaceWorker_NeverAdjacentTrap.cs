using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DDA RID: 7642
	public class PlaceWorker_NeverAdjacentTrap : PlaceWorker
	{
		// Token: 0x0600A5E2 RID: 42466 RVA: 0x00006A05 File Offset: 0x00004C05
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
		}

		// Token: 0x0600A5E3 RID: 42467 RVA: 0x003021A4 File Offset: 0x003003A4
		public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			foreach (IntVec3 c in GenAdj.OccupiedRect(center, rot, def.Size).ExpandedBy(1))
			{
				List<Thing> list = map.thingGrid.ThingsListAt(c);
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing2 = list[i];
					if (thing2 != thingToIgnore && ((thing2.def.category == ThingCategory.Building && thing2.def.building.isTrap) || ((thing2.def.IsBlueprint || thing2.def.IsFrame) && thing2.def.entityDefToBuild is ThingDef && ((ThingDef)thing2.def.entityDefToBuild).building.isTrap)))
					{
						return "CannotPlaceAdjacentTrap".Translate();
					}
				}
			}
			return true;
		}
	}
}
