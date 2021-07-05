using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200154E RID: 5454
	public class PlaceWorker_ShowFacilitiesConnections : PlaceWorker
	{
		// Token: 0x06008176 RID: 33142 RVA: 0x002DCB10 File Offset: 0x002DAD10
		public override void DrawPlaceMouseAttachments(float curX, ref float curY, BuildableDef bdef, IntVec3 center, Rot4 rot)
		{
			base.DrawPlaceMouseAttachments(curX, ref curY, bdef, center, rot);
			ThingDef thingDef;
			if ((thingDef = (bdef as ThingDef)) != null)
			{
				Map currentMap = Find.CurrentMap;
				if (thingDef.HasComp(typeof(CompAffectedByFacilities)))
				{
					CompAffectedByFacilities.DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo(curX, ref curY, thingDef, center, rot, currentMap);
					return;
				}
				CompFacility.DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo(curX, ref curY, thingDef, center, rot, currentMap);
			}
		}

		// Token: 0x06008177 RID: 33143 RVA: 0x002DCB68 File Offset: 0x002DAD68
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
