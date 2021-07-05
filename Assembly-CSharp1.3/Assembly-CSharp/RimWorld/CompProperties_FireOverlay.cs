using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A01 RID: 2561
	public class CompProperties_FireOverlay : CompProperties
	{
		// Token: 0x06003EEC RID: 16108 RVA: 0x00157BF8 File Offset: 0x00155DF8
		public CompProperties_FireOverlay()
		{
			this.compClass = typeof(CompFireOverlay);
		}

		// Token: 0x06003EED RID: 16109 RVA: 0x00157C31 File Offset: 0x00155E31
		public override void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude, Thing thing = null)
		{
			GhostUtility.GhostGraphicFor(CompFireOverlay.FireGraphic, thingDef, ghostCol, null).DrawFromDef(center.ToVector3ShiftedWithAltitude(drawAltitude), rot, thingDef, 0f);
		}

		// Token: 0x040021D7 RID: 8663
		public float fireSize = 1f;

		// Token: 0x040021D8 RID: 8664
		public float finalFireSize = 1f;

		// Token: 0x040021D9 RID: 8665
		public float fireGrowthDurationTicks = -1f;

		// Token: 0x040021DA RID: 8666
		public Vector3 offset;
	}
}
