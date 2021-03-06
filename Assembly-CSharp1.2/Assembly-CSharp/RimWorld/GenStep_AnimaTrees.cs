using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001277 RID: 4727
	public class GenStep_AnimaTrees : GenStep
	{
		// Token: 0x17000FF3 RID: 4083
		// (get) Token: 0x0600670F RID: 26383 RVA: 0x000466CE File Offset: 0x000448CE
		public override int SeedPart
		{
			get
			{
				return 647816171;
			}
		}

		// Token: 0x06006710 RID: 26384 RVA: 0x001FB018 File Offset: 0x001F9218
		public override void Generate(Map map, GenStepParams parms)
		{
			if (map.Biome.isExtremeBiome)
			{
				return;
			}
			int i = GenStep_AnimaTrees.DesiredTreeCountForMap(map);
			int num = 0;
			Predicate<IntVec3> <>9__0;
			while (i > 0)
			{
				int minEdgeDistance = 25;
				Predicate<IntVec3> validator;
				if ((validator = <>9__0) == null)
				{
					validator = (<>9__0 = ((IntVec3 x) => GenStep_AnimaTrees.CanSpawnAt(x, map, 0, 50, 22, 10)));
				}
				IntVec3 cell;
				if (!CellFinderLoose.TryFindRandomNotEdgeCellWith(minEdgeDistance, validator, map, out cell))
				{
					break;
				}
				Thing thing;
				if (GenStep_AnimaTrees.TrySpawnAt(cell, map, GenStep_AnimaTrees.GrowthRange.RandomInRange, out thing))
				{
					i--;
				}
				num++;
				if (num > 1000)
				{
					Log.Error("Could not place anima tree; too many iterations.", false);
					return;
				}
			}
		}

		// Token: 0x06006711 RID: 26385 RVA: 0x001FB0C8 File Offset: 0x001F92C8
		public static bool TrySpawnAt(IntVec3 cell, Map map, float growth, out Thing plant)
		{
			Plant plant2 = cell.GetPlant(map);
			if (plant2 != null)
			{
				plant2.Destroy(DestroyMode.Vanish);
			}
			plant = GenSpawn.Spawn(ThingDefOf.Plant_TreeAnima, cell, map, WipeMode.Vanish);
			((Plant)plant).Growth = growth;
			return plant != null;
		}

		// Token: 0x06006712 RID: 26386 RVA: 0x001FB108 File Offset: 0x001F9308
		public static bool CanSpawnAt(IntVec3 c, Map map, int minProximityToArtificialStructures = 40, int minProximityToCenter = 0, int minFertileUnroofedCells = 22, int maxFertileUnroofedCellRadius = 10)
		{
			if (!c.Standable(map) || c.Fogged(map) || !c.GetRoom(map, RegionType.Set_Passable).PsychologicallyOutdoors)
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
				if (thingList[i].def == ThingDefOf.Plant_TreeAnima)
				{
					return false;
				}
			}
			if (minProximityToCenter > 0 && map.Center.InHorDistOf(c, (float)minProximityToCenter))
			{
				return false;
			}
			if (!map.reachability.CanReachFactionBase(c, map.ParentFaction))
			{
				return false;
			}
			TerrainDef terrain = c.GetTerrain(map);
			if (terrain.avoidWander || terrain.fertility <= 0f)
			{
				return false;
			}
			if (c.Roofed(map))
			{
				return false;
			}
			if (minProximityToArtificialStructures != 0 && GenRadial.RadialDistinctThingsAround(c, map, (float)minProximityToArtificialStructures, false).Any(new Func<Thing, bool>(MeditationUtility.CountsAsArtificialBuilding)))
			{
				return false;
			}
			int num = GenRadial.NumCellsInRadius((float)maxFertileUnroofedCellRadius);
			int num2 = 0;
			for (int j = 0; j < num; j++)
			{
				IntVec3 intVec = c + GenRadial.RadialPattern[j];
				if (WanderUtility.InSameRoom(intVec, c, map))
				{
					if (intVec.InBounds(map) && !intVec.Roofed(map) && intVec.GetTerrain(map).fertility > 0f)
					{
						num2++;
					}
					if (num2 >= minFertileUnroofedCells)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006713 RID: 26387 RVA: 0x000466D5 File Offset: 0x000448D5
		public static int DesiredTreeCountForMap(Map map)
		{
			return Mathf.Max(Mathf.RoundToInt(GenStep_AnimaTrees.Density * (float)map.Area), 1);
		}

		// Token: 0x0400447B RID: 17531
		public static readonly float Density = 1.25E-05f;

		// Token: 0x0400447C RID: 17532
		private const int MinDistanceToEdge = 25;

		// Token: 0x0400447D RID: 17533
		private static readonly FloatRange GrowthRange = new FloatRange(0.5f, 0.75f);
	}
}
