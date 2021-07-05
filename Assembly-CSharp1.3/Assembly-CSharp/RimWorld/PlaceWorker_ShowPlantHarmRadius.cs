using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001551 RID: 5457
	public class PlaceWorker_ShowPlantHarmRadius : PlaceWorker
	{
		// Token: 0x0600817D RID: 33149 RVA: 0x002DCBE4 File Offset: 0x002DADE4
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
