using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DCF RID: 7631
	public class PlaceWorker_ShowDeepResources : PlaceWorker
	{
		// Token: 0x0600A5C6 RID: 42438 RVA: 0x00301B80 File Offset: 0x002FFD80
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			if (currentMap.deepResourceGrid.AnyActiveDeepScannersOnMap())
			{
				currentMap.deepResourceGrid.MarkForDraw();
			}
		}
	}
}
