using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001552 RID: 5458
	public class PlaceWorker_ShowExplosionRadius : PlaceWorker
	{
		// Token: 0x0600817F RID: 33151 RVA: 0x002DCC18 File Offset: 0x002DAE18
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CompProperties_Explosive compProperties = def.GetCompProperties<CompProperties_Explosive>();
			if (compProperties == null)
			{
				return;
			}
			GenDraw.DrawRadiusRing(center, compProperties.explosiveRadius);
		}
	}
}
