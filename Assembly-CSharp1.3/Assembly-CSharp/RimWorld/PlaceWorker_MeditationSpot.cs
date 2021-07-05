using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200155B RID: 5467
	public class PlaceWorker_MeditationSpot : PlaceWorker
	{
		// Token: 0x06008198 RID: 33176 RVA: 0x002DD328 File Offset: 0x002DB528
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			MeditationUtility.DrawMeditationSpotOverlay(center, Find.CurrentMap);
		}
	}
}
