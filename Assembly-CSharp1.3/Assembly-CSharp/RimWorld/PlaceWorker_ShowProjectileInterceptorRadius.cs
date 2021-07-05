using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001559 RID: 5465
	public class PlaceWorker_ShowProjectileInterceptorRadius : PlaceWorker
	{
		// Token: 0x06008194 RID: 33172 RVA: 0x002DD2CC File Offset: 0x002DB4CC
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
