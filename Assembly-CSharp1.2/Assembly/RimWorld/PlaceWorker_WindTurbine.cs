using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DCD RID: 7629
	public class PlaceWorker_WindTurbine : PlaceWorker
	{
		// Token: 0x0600A5C2 RID: 42434 RVA: 0x0006DC49 File Offset: 0x0006BE49
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			GenDraw.DrawFieldEdges(WindTurbineUtility.CalculateWindCells(center, rot, def.size).ToList<IntVec3>());
		}
	}
}
