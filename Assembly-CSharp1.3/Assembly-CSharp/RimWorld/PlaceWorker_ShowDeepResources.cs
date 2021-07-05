using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001543 RID: 5443
	public class PlaceWorker_ShowDeepResources : PlaceWorker
	{
		// Token: 0x0600815B RID: 33115 RVA: 0x002DC1F8 File Offset: 0x002DA3F8
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
