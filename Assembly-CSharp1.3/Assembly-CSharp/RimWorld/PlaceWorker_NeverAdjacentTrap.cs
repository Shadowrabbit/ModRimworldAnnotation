using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200154C RID: 5452
	public class PlaceWorker_NeverAdjacentTrap : PlaceWorker
	{
		// Token: 0x06008171 RID: 33137 RVA: 0x0000313F File Offset: 0x0000133F
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
		}

		// Token: 0x06008172 RID: 33138 RVA: 0x002DC8D0 File Offset: 0x002DAAD0
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
