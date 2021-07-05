using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DD3 RID: 7635
	public class PlaceWorker_ShowInstrumentAoE : PlaceWorker
	{
		// Token: 0x0600A5CF RID: 42447 RVA: 0x00301C2C File Offset: 0x002FFE2C
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

		// Token: 0x0400708E RID: 28814
		private static List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
