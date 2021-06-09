using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000F9B RID: 3995
	public class GatheringWorker_Concert : GatheringWorker
	{
		// Token: 0x06005798 RID: 22424 RVA: 0x0003CBBA File Offset: 0x0003ADBA
		protected override LordJob CreateLordJob(IntVec3 spot, Pawn organizer)
		{
			return new LordJob_Joinable_Concert(spot, organizer, this.def);
		}

		// Token: 0x06005799 RID: 22425 RVA: 0x001CDB30 File Offset: 0x001CBD30
		protected override bool TryFindGatherSpot(Pawn organizer, out IntVec3 spot)
		{
			bool enjoyableOutside = JoyUtility.EnjoyableOutsideNow(organizer, null);
			Map map = organizer.Map;
			IEnumerable<Building_MusicalInstrument> enumerable = organizer.Map.listerBuildings.AllBuildingsColonistOfClass<Building_MusicalInstrument>();
			bool result;
			try
			{
				int num = -1;
				foreach (Building_MusicalInstrument building_MusicalInstrument in enumerable)
				{
					if (GatheringsUtility.ValidateGatheringSpot_NewTemp(building_MusicalInstrument.InteractionCell, this.def, organizer, enjoyableOutside, false) && GatheringWorker_Concert.InstrumentAccessible(building_MusicalInstrument, organizer))
					{
						float instrumentRange = building_MusicalInstrument.def.building.instrumentRange;
						if ((float)num < instrumentRange)
						{
							GatheringWorker_Concert.tmpInstruments.Clear();
						}
						else if ((float)num > instrumentRange)
						{
							continue;
						}
						GatheringWorker_Concert.tmpInstruments.Add(building_MusicalInstrument);
					}
				}
				Building_MusicalInstrument building_MusicalInstrument2;
				if (!GatheringWorker_Concert.tmpInstruments.TryRandomElement(out building_MusicalInstrument2))
				{
					spot = IntVec3.Invalid;
					result = false;
				}
				else
				{
					spot = building_MusicalInstrument2.InteractionCell;
					result = true;
				}
			}
			finally
			{
				GatheringWorker_Concert.tmpInstruments.Clear();
			}
			return result;
		}

		// Token: 0x0600579A RID: 22426 RVA: 0x0003CBC9 File Offset: 0x0003ADC9
		public static bool InstrumentAccessible(Building_MusicalInstrument i, Pawn p)
		{
			return !i.IsBeingPlayed && p.CanReserveAndReach(i.InteractionCell, PathEndMode.OnCell, p.NormalMaxDanger(), 1, -1, null, false);
		}

		// Token: 0x04003943 RID: 14659
		private static List<Building_MusicalInstrument> tmpInstruments = new List<Building_MusicalInstrument>();
	}
}
