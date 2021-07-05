using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009FA RID: 2554
	public class CompProperties_DarklightOverlay : CompProperties_FireOverlay
	{
		// Token: 0x06003ECE RID: 16078 RVA: 0x001574F6 File Offset: 0x001556F6
		public CompProperties_DarklightOverlay()
		{
			this.compClass = typeof(CompDarklightOverlay);
		}

		// Token: 0x06003ECF RID: 16079 RVA: 0x0015750E File Offset: 0x0015570E
		public override void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude, Thing thing = null)
		{
			GhostUtility.GhostGraphicFor(CompDarklightOverlay.DarklightGraphic, thingDef, ghostCol, null).DrawFromDef(center.ToVector3ShiftedWithAltitude(drawAltitude), rot, thingDef, 0f);
		}
	}
}
