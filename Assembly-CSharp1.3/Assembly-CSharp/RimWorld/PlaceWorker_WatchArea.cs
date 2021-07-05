using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001553 RID: 5459
	public class PlaceWorker_WatchArea : PlaceWorker
	{
		// Token: 0x06008181 RID: 33153 RVA: 0x002DCC3C File Offset: 0x002DAE3C
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			GenDraw.DrawFieldEdges(WatchBuildingUtility.CalculateWatchCells(def, center, rot, currentMap).ToList<IntVec3>());
		}
	}
}
