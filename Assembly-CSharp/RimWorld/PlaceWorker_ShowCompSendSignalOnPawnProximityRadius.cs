using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DDD RID: 7645
	public class PlaceWorker_ShowCompSendSignalOnPawnProximityRadius : PlaceWorker
	{
		// Token: 0x0600A5E9 RID: 42473 RVA: 0x00302328 File Offset: 0x00300528
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
