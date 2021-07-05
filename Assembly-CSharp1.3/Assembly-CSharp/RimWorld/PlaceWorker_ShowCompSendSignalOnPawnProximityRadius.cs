using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001550 RID: 5456
	public class PlaceWorker_ShowCompSendSignalOnPawnProximityRadius : PlaceWorker
	{
		// Token: 0x0600817B RID: 33147 RVA: 0x002DCBC0 File Offset: 0x002DADC0
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CompProperties_SendSignalOnPawnProximity compProperties = def.GetCompProperties<CompProperties_SendSignalOnPawnProximity>();
			if (compProperties == null)
			{
				return;
			}
			GenDraw.DrawRadiusRing(center, compProperties.radius);
		}
	}
}
