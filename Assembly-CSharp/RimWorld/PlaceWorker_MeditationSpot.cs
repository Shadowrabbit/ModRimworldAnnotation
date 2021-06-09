using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DEA RID: 7658
	public class PlaceWorker_MeditationSpot : PlaceWorker
	{
		// Token: 0x0600A612 RID: 42514 RVA: 0x0006DD69 File Offset: 0x0006BF69
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			MeditationUtility.DrawMeditationSpotOverlay(center, Find.CurrentMap);
		}
	}
}
