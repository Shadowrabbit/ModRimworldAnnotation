using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F0D RID: 3853
	public class CompProperties_FireOverlay : CompProperties
	{
		// Token: 0x06005540 RID: 21824 RVA: 0x0003B1D5 File Offset: 0x000393D5
		public CompProperties_FireOverlay()
		{
			this.compClass = typeof(CompFireOverlay);
		}

		// Token: 0x06005541 RID: 21825 RVA: 0x0003B1F8 File Offset: 0x000393F8
		public override void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude, Thing thing = null)
		{
			GhostUtility.GhostGraphicFor(CompFireOverlay.FireGraphic, thingDef, ghostCol).DrawFromDef(center.ToVector3ShiftedWithAltitude(drawAltitude), rot, thingDef, 0f);
		}

		// Token: 0x0400365B RID: 13915
		public float fireSize = 1f;

		// Token: 0x0400365C RID: 13916
		public Vector3 offset;
	}
}
