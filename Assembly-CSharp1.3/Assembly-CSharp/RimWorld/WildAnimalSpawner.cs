using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D0D RID: 3341
	public class WildAnimalSpawner
	{
		// Token: 0x17000D7B RID: 3451
		// (get) Token: 0x06004E10 RID: 19984 RVA: 0x001A2E9C File Offset: 0x001A109C
		private float DesiredAnimalDensity
		{
			get
			{
				float num = this.map.Biome.animalDensity;
				float num2 = 0f;
				float num3 = 0f;
				foreach (PawnKindDef pawnKindDef in this.map.Biome.AllWildAnimals)
				{
					float num4 = this.map.Biome.CommonalityOfAnimal(pawnKindDef);
					num3 += num4;
					if (this.map.mapTemperature.SeasonAcceptableFor(pawnKindDef.race))
					{
						num2 += num4;
					}
				}
				num *= num2 / num3;
				num *= this.map.gameConditionManager.AggregateAnimalDensityFactor(this.map);
				return num;
			}
		}

		// Token: 0x17000D7C RID: 3452
		// (get) Token: 0x06004E11 RID: 19985 RVA: 0x001A2F64 File Offset: 0x001A1164
		private float DesiredTotalAnimalWeight
		{
			get
			{
				float desiredAnimalDensity = this.DesiredAnimalDensity;
				if (desiredAnimalDensity == 0f)
				{
					return 0f;
				}
				float num = 10000f / desiredAnimalDensity;
				return (float)this.map.Area / num;
			}
		}

		// Token: 0x17000D7D RID: 3453
		// (get) Token: 0x06004E12 RID: 19986 RVA: 0x001A2F9C File Offset: 0x001A119C
		private float CurrentTotalAnimalWeight
		{
			get
			{
				float num = 0f;
				List<Pawn> allPawnsSpawned = this.map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].Faction == null)
					{
						num += allPawnsSpawned[i].kindDef.ecoSystemWeight;
					}
				}
				return num;
			}
		}

		// Token: 0x17000D7E RID: 3454
		// (get) Token: 0x06004E13 RID: 19987 RVA: 0x001A2FF4 File Offset: 0x001A11F4
		public bool AnimalEcosystemFull
		{
			get
			{
				return this.CurrentTotalAnimalWeight >= this.DesiredTotalAnimalWeight;
			}
		}

		// Token: 0x06004E14 RID: 19988 RVA: 0x001A3007 File Offset: 0x001A1207
		public WildAnimalSpawner(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004E15 RID: 19989 RVA: 0x001A3018 File Offset: 0x001A1218
		public void WildAnimalSpawnerTick()
		{
			if (Find.TickManager.TicksGame % 1213 == 0 && !this.AnimalEcosystemFull && Rand.Chance(0.026955556f * this.DesiredAnimalDensity))
			{
				TraverseParms traverseParms = TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false).WithFenceblocked(true);
				IntVec3 loc;
				if (RCellFinder.TryFindRandomPawnEntryCell(out loc, this.map, CellFinder.EdgeRoadChance_Animal, true, (IntVec3 cell) => this.map.reachability.CanReachMapEdge(cell, traverseParms)))
				{
					this.SpawnRandomWildAnimalAt(loc);
				}
			}
		}

		// Token: 0x06004E16 RID: 19990 RVA: 0x001A30A4 File Offset: 0x001A12A4
		public bool SpawnRandomWildAnimalAt(IntVec3 loc)
		{
			PawnKindDef pawnKindDef = (from a in this.map.Biome.AllWildAnimals
			where this.map.mapTemperature.SeasonAcceptableFor(a.race)
			select a).RandomElementByWeight((PawnKindDef def) => this.map.Biome.CommonalityOfAnimal(def) / def.wildGroupSize.Average);
			if (pawnKindDef == null)
			{
				Log.Error("No spawnable animals right now.");
				return false;
			}
			int randomInRange = pawnKindDef.wildGroupSize.RandomInRange;
			int radius = Mathf.CeilToInt(Mathf.Sqrt((float)pawnKindDef.wildGroupSize.max));
			for (int i = 0; i < randomInRange; i++)
			{
				IntVec3 loc2 = CellFinder.RandomClosewalkCellNear(loc, this.map, radius, null);
				GenSpawn.Spawn(PawnGenerator.GeneratePawn(pawnKindDef, null), loc2, this.map, WipeMode.Vanish);
			}
			return true;
		}

		// Token: 0x06004E17 RID: 19991 RVA: 0x001A314C File Offset: 0x001A134C
		public string DebugString()
		{
			return string.Concat(new object[]
			{
				"DesiredTotalAnimalWeight: ",
				this.DesiredTotalAnimalWeight,
				"\nCurrentTotalAnimalWeight: ",
				this.CurrentTotalAnimalWeight,
				"\nDesiredAnimalDensity: ",
				this.DesiredAnimalDensity
			});
		}

		// Token: 0x04002F1E RID: 12062
		private Map map;

		// Token: 0x04002F1F RID: 12063
		private const int AnimalCheckInterval = 1213;

		// Token: 0x04002F20 RID: 12064
		private const float BaseAnimalSpawnChancePerInterval = 0.026955556f;
	}
}
