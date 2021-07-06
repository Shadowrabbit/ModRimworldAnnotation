using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001344 RID: 4932
	public class WildAnimalSpawner
	{
		// Token: 0x1700107E RID: 4222
		// (get) Token: 0x06006AFC RID: 27388 RVA: 0x00210548 File Offset: 0x0020E748
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

		// Token: 0x1700107F RID: 4223
		// (get) Token: 0x06006AFD RID: 27389 RVA: 0x00210610 File Offset: 0x0020E810
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

		// Token: 0x17001080 RID: 4224
		// (get) Token: 0x06006AFE RID: 27390 RVA: 0x00210648 File Offset: 0x0020E848
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

		// Token: 0x17001081 RID: 4225
		// (get) Token: 0x06006AFF RID: 27391 RVA: 0x00048D25 File Offset: 0x00046F25
		public bool AnimalEcosystemFull
		{
			get
			{
				return this.CurrentTotalAnimalWeight >= this.DesiredTotalAnimalWeight;
			}
		}

		// Token: 0x06006B00 RID: 27392 RVA: 0x00048D38 File Offset: 0x00046F38
		public WildAnimalSpawner(Map map)
		{
			this.map = map;
		}

		// Token: 0x06006B01 RID: 27393 RVA: 0x002106A0 File Offset: 0x0020E8A0
		public void WildAnimalSpawnerTick()
		{
			if (Find.TickManager.TicksGame % 1213 == 0 && !this.AnimalEcosystemFull && Rand.Chance(0.026955556f * this.DesiredAnimalDensity))
			{
				TraverseParms traverseParms = TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false);
				IntVec3 loc;
				if (RCellFinder.TryFindRandomPawnEntryCell(out loc, this.map, CellFinder.EdgeRoadChance_Animal, true, (IntVec3 cell) => this.map.reachability.CanReachMapEdge(cell, traverseParms)))
				{
					this.SpawnRandomWildAnimalAt(loc);
				}
			}
		}

		// Token: 0x06006B02 RID: 27394 RVA: 0x00210720 File Offset: 0x0020E920
		public bool SpawnRandomWildAnimalAt(IntVec3 loc)
		{
			PawnKindDef pawnKindDef = (from a in this.map.Biome.AllWildAnimals
			where this.map.mapTemperature.SeasonAcceptableFor(a.race)
			select a).RandomElementByWeight((PawnKindDef def) => this.map.Biome.CommonalityOfAnimal(def) / def.wildGroupSize.Average);
			if (pawnKindDef == null)
			{
				Log.Error("No spawnable animals right now.", false);
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

		// Token: 0x06006B03 RID: 27395 RVA: 0x002107C8 File Offset: 0x0020E9C8
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

		// Token: 0x0400472C RID: 18220
		private Map map;

		// Token: 0x0400472D RID: 18221
		private const int AnimalCheckInterval = 1213;

		// Token: 0x0400472E RID: 18222
		private const float BaseAnimalSpawnChancePerInterval = 0.026955556f;
	}
}
