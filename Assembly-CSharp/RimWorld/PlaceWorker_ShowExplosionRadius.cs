using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DDF RID: 7647
	public class PlaceWorker_ShowExplosionRadius : PlaceWorker
	{
		// Token: 0x0600A5ED RID: 42477 RVA: 0x00302380 File Offset: 0x00300580
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
