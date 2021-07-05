using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000A6D RID: 2669
	public class GatheringWorker_Concert : GatheringWorker
	{
		// Token: 0x06004015 RID: 16405 RVA: 0x0015B27B File Offset: 0x0015947B
		protected override LordJob CreateLordJob(IntVec3 spot, Pawn organizer)
		{
			return new LordJob_Joinable_Concert(spot, organizer, this.def);
		}

		// Token: 0x06004016 RID: 16406 RVA: 0x0015B28C File Offset: 0x0015948C
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
					if (GatheringsUtility.ValidateGatheringSpot(building_MusicalInstrument.InteractionCell, this.def, organizer, enjoyableOutside, false) && GatheringWorker_Concert.InstrumentAccessible(building_MusicalInstrument, organizer))
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

		// Token: 0x06004017 RID: 16407 RVA: 0x0015B398 File Offset: 0x00159598
		public static bool InstrumentAccessible(Building_MusicalInstrument i, Pawn p)
		{
			return !i.IsBeingPlayed && p.CanReach(i.InteractionCell, PathEndMode.OnCell, p.NormalMaxDanger(), false, false, TraverseMode.ByPawn) && p.CanReserveSittableOrSpot(i.InteractionCell, false);
		}

		// Token: 0x04002453 RID: 9299
		private static List<Building_MusicalInstrument> tmpInstruments = new List<Building_MusicalInstrument>();
	}
}
