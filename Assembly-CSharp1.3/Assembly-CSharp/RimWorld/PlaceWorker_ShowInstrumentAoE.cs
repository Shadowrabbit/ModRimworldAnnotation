using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001546 RID: 5446
	public class PlaceWorker_ShowInstrumentAoE : PlaceWorker
	{
		// Token: 0x06008161 RID: 33121 RVA: 0x002DC2D0 File Offset: 0x002DA4D0
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			ThingDef thingDef = (ThingDef)checkingDef;
			PlaceWorker_ShowInstrumentAoE.tmpCells.Clear();
			int num = GenRadial.NumCellsInRadius(thingDef.building.instrumentRange);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = loc + GenRadial.RadialPattern[i];
				if (Building_MusicalInstrument.IsAffectedByInstrument(thingDef, loc, intVec, map))
				{
					PlaceWorker_ShowInstrumentAoE.tmpCells.Add(intVec);
				}
			}
			GenDraw.DrawFieldEdges(PlaceWorker_ShowInstrumentAoE.tmpCells);
			return true;
		}

		// Token: 0x040050AD RID: 20653
		private static List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
