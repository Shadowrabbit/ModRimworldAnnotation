using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DE8 RID: 7656
	public class PlaceWorker_ShowProjectileInterceptorRadius : PlaceWorker
	{
		// Token: 0x0600A60E RID: 42510 RVA: 0x00302A60 File Offset: 0x00300C60
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CompProperties_ProjectileInterceptor compProperties = def.GetCompProperties<CompProperties_ProjectileInterceptor>();
			if (compProperties != null)
			{
				GenDraw.DrawCircleOutline(center.ToVector3Shifted(), compProperties.radius);
			}
		}
	}
}
