using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200154B RID: 5451
	public class PlaceWorker_NeverAdjacentUnstandable : PlaceWorker
	{
		// Token: 0x0600816E RID: 33134 RVA: 0x002DC7CC File Offset: 0x002DA9CC
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			GenDraw.DrawFieldEdges(GenAdj.OccupiedRect(center, rot, def.size).ExpandedBy(1).Cells.ToList<IntVec3>(), Color.white, null);
		}

		// Token: 0x0600816F RID: 33135 RVA: 0x002DC810 File Offset: 0x002DAA10
		public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			foreach (IntVec3 c in GenAdj.OccupiedRect(center, rot, def.Size).ExpandedBy(1))
			{
				List<Thing> list = map.thingGrid.ThingsListAt(c);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] != thingToIgnore && list[i].def.passability != Traversability.Standable)
					{
						return "MustPlaceAdjacentStandable".Translate();
					}
				}
			}
			return true;
		}
	}
}
