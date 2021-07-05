using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DDE RID: 7646
	public class PlaceWorker_ShowPlantHarmRadius : PlaceWorker
	{
		// Token: 0x0600A5EB RID: 42475 RVA: 0x0030234C File Offset: 0x0030054C
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			if (thing == null)
			{
				return;
			}
			CompPlantHarmRadius compPlantHarmRadius = thing.TryGetComp<CompPlantHarmRadius>();
			if (compPlantHarmRadius == null)
			{
				return;
			}
			float currentRadius = compPlantHarmRadius.CurrentRadius;
			if (currentRadius < 50f)
			{
				GenDraw.DrawRadiusRing(center, currentRadius);
			}
		}
	}
}
