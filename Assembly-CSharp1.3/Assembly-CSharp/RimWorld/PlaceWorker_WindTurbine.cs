using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001541 RID: 5441
	public class PlaceWorker_WindTurbine : PlaceWorker
	{
		// Token: 0x06008157 RID: 33111 RVA: 0x002DC19B File Offset: 0x002DA39B
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			GenDraw.DrawFieldEdges(WindTurbineUtility.CalculateWindCells(center, rot, def.size).ToList<IntVec3>());
		}
	}
}
