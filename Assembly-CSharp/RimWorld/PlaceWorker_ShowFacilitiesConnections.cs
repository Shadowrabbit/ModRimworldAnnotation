using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DDB RID: 7643
	public class PlaceWorker_ShowFacilitiesConnections : PlaceWorker
	{
		// Token: 0x0600A5E5 RID: 42469 RVA: 0x003022D0 File Offset: 0x003004D0
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			if (def.HasComp(typeof(CompAffectedByFacilities)))
			{
				CompAffectedByFacilities.DrawLinesToPotentialThingsToLinkTo(def, center, rot, currentMap);
				return;
			}
			CompFacility.DrawLinesToPotentialThingsToLinkTo(def, center, rot, currentMap);
		}
	}
}
