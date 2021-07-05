using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200119F RID: 4511
	public class IncidentWorker_AmbrosiaSprout : IncidentWorker
	{
		// Token: 0x06006368 RID: 25448 RVA: 0x001EF3E0 File Offset: 0x001ED5E0
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return map.weatherManager.growthSeasonMemory.GrowthSeasonOutdoorsNow && this.TryFindRootCell(map, out intVec);
		}

		// Token: 0x06006369 RID: 25449 RVA: 0x001EF424 File Offset: 0x001ED624
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			if (!this.TryFindRootCell(map, out intVec))
			{
				return false;
			}
			Thing thing = null;
			int randomInRange = IncidentWorker_AmbrosiaSprout.CountRange.RandomInRange;
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < randomInRange; i++)
			{
				IntVec3 root = intVec;
				Map map2 = map;
				int radius = 6;
				Predicate<IntVec3> extraValidator;
				if ((extraValidator = <>9__0) == null)
				{
					extraValidator = (<>9__0 = ((IntVec3 x) => this.CanSpawnAt(x, map)));
				}
				IntVec3 intVec2;
				if (!CellFinder.TryRandomClosewalkCellNear(root, map2, radius, out intVec2, extraValidator))
				{
					break;
				}
				Plant plant = intVec2.GetPlant(map);
				if (plant != null)
				{
					plant.Destroy(DestroyMode.Vanish);
				}
				Thing thing2 = GenSpawn.Spawn(ThingDefOf.Plant_Ambrosia, intVec2, map, WipeMode.Vanish);
				if (thing == null)
				{
					thing = thing2;
				}
			}
			if (thing == null)
			{
				return false;
			}
			base.SendStandardLetter(parms, thing, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x0600636A RID: 25450 RVA: 0x001EF504 File Offset: 0x001ED704
		private bool TryFindRootCell(Map map, out IntVec3 cell)
		{
			return CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => this.CanSpawnAt(x, map) && x.GetRoom(map, RegionType.Set_Passable).CellCount >= 64, map, out cell);
		}

		// Token: 0x0600636B RID: 25451 RVA: 0x001EF540 File Offset: 0x001ED740
		private bool CanSpawnAt(IntVec3 c, Map map)
		{
			if (!c.Standable(map) || c.Fogged(map) || map.fertilityGrid.FertilityAt(c) < ThingDefOf.Plant_Ambrosia.plant.fertilityMin || !c.GetRoom(map, RegionType.Set_Passable).PsychologicallyOutdoors || c.GetEdifice(map) != null || !PlantUtility.GrowthSeasonNow(c, map, false))
			{
				return false;
			}
			Plant plant = c.GetPlant(map);
			if (plant != null && plant.def.plant.growDays > 10f)
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def == ThingDefOf.Plant_Ambrosia)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400428B RID: 17035
		private static readonly IntRange CountRange = new IntRange(10, 20);

		// Token: 0x0400428C RID: 17036
		private const int MinRoomCells = 64;

		// Token: 0x0400428D RID: 17037
		private const int SpawnRadius = 6;
	}
}
