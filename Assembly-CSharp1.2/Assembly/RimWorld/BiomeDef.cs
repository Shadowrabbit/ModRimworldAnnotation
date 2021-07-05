using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F79 RID: 3961
	public class BiomeDef : Def
	{
		// Token: 0x17000D55 RID: 3413
		// (get) Token: 0x060056E2 RID: 22242 RVA: 0x0003C47F File Offset: 0x0003A67F
		public BiomeWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (BiomeWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x17000D56 RID: 3414
		// (get) Token: 0x060056E3 RID: 22243 RVA: 0x001CBAB8 File Offset: 0x001C9CB8
		public Material DrawMaterial
		{
			get
			{
				if (this.cachedMat == null)
				{
					if (this.texture.NullOrEmpty())
					{
						return null;
					}
					if (this == BiomeDefOf.Ocean || this == BiomeDefOf.Lake)
					{
						this.cachedMat = MaterialAllocator.Create(WorldMaterials.WorldOcean);
					}
					else if (!this.allowRoads && !this.allowRivers)
					{
						this.cachedMat = MaterialAllocator.Create(WorldMaterials.WorldIce);
					}
					else
					{
						this.cachedMat = MaterialAllocator.Create(WorldMaterials.WorldTerrain);
					}
					this.cachedMat.mainTexture = ContentFinder<Texture2D>.Get(this.texture, true);
				}
				return this.cachedMat;
			}
		}

		// Token: 0x17000D57 RID: 3415
		// (get) Token: 0x060056E4 RID: 22244 RVA: 0x001CBB54 File Offset: 0x001C9D54
		public List<ThingDef> AllWildPlants
		{
			get
			{
				if (this.cachedWildPlants == null)
				{
					this.cachedWildPlants = new List<ThingDef>();
					foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
					{
						if (thingDef.category == ThingCategory.Plant && this.CommonalityOfPlant(thingDef) > 0f)
						{
							this.cachedWildPlants.Add(thingDef);
						}
					}
				}
				return this.cachedWildPlants;
			}
		}

		// Token: 0x17000D58 RID: 3416
		// (get) Token: 0x060056E5 RID: 22245 RVA: 0x001CBBDC File Offset: 0x001C9DDC
		public int MaxWildAndCavePlantsClusterRadius
		{
			get
			{
				if (this.cachedMaxWildPlantsClusterRadius == null)
				{
					this.cachedMaxWildPlantsClusterRadius = new int?(0);
					List<ThingDef> allWildPlants = this.AllWildPlants;
					for (int i = 0; i < allWildPlants.Count; i++)
					{
						if (allWildPlants[i].plant.GrowsInClusters)
						{
							this.cachedMaxWildPlantsClusterRadius = new int?(Mathf.Max(this.cachedMaxWildPlantsClusterRadius.Value, allWildPlants[i].plant.wildClusterRadius));
						}
					}
					List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int j = 0; j < allDefsListForReading.Count; j++)
					{
						if (allDefsListForReading[j].category == ThingCategory.Plant && allDefsListForReading[j].plant.cavePlant)
						{
							this.cachedMaxWildPlantsClusterRadius = new int?(Mathf.Max(this.cachedMaxWildPlantsClusterRadius.Value, allDefsListForReading[j].plant.wildClusterRadius));
						}
					}
				}
				return this.cachedMaxWildPlantsClusterRadius.Value;
			}
		}

		// Token: 0x17000D59 RID: 3417
		// (get) Token: 0x060056E6 RID: 22246 RVA: 0x001CBCCC File Offset: 0x001C9ECC
		public float LowestWildAndCavePlantOrder
		{
			get
			{
				if (this.cachedLowestWildPlantOrder == null)
				{
					this.cachedLowestWildPlantOrder = new float?((float)int.MaxValue);
					List<ThingDef> allWildPlants = this.AllWildPlants;
					for (int i = 0; i < allWildPlants.Count; i++)
					{
						this.cachedLowestWildPlantOrder = new float?(Mathf.Min(this.cachedLowestWildPlantOrder.Value, allWildPlants[i].plant.wildOrder));
					}
					List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int j = 0; j < allDefsListForReading.Count; j++)
					{
						if (allDefsListForReading[j].category == ThingCategory.Plant && allDefsListForReading[j].plant.cavePlant)
						{
							this.cachedLowestWildPlantOrder = new float?(Mathf.Min(this.cachedLowestWildPlantOrder.Value, allDefsListForReading[j].plant.wildOrder));
						}
					}
				}
				return this.cachedLowestWildPlantOrder.Value;
			}
		}

		// Token: 0x17000D5A RID: 3418
		// (get) Token: 0x060056E7 RID: 22247 RVA: 0x0003C4A5 File Offset: 0x0003A6A5
		public IEnumerable<PawnKindDef> AllWildAnimals
		{
			get
			{
				foreach (PawnKindDef pawnKindDef in DefDatabase<PawnKindDef>.AllDefs)
				{
					if (this.CommonalityOfAnimal(pawnKindDef) > 0f)
					{
						yield return pawnKindDef;
					}
				}
				IEnumerator<PawnKindDef> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000D5B RID: 3419
		// (get) Token: 0x060056E8 RID: 22248 RVA: 0x0003C4B5 File Offset: 0x0003A6B5
		public float PlantCommonalitiesSum
		{
			get
			{
				this.CachePlantCommonalitiesIfShould();
				return this.cachedPlantCommonalitiesSum;
			}
		}

		// Token: 0x17000D5C RID: 3420
		// (get) Token: 0x060056E9 RID: 22249 RVA: 0x001CBDB0 File Offset: 0x001C9FB0
		public float TreeDensity
		{
			get
			{
				float num = 0f;
				float num2 = 0f;
				this.CachePlantCommonalitiesIfShould();
				foreach (KeyValuePair<ThingDef, float> keyValuePair in this.cachedPlantCommonalities)
				{
					num += keyValuePair.Value;
					if (keyValuePair.Key.plant.IsTree)
					{
						num2 += keyValuePair.Value;
					}
				}
				if (num == 0f)
				{
					return 0f;
				}
				return num2 / num * this.plantDensity;
			}
		}

		// Token: 0x060056EA RID: 22250 RVA: 0x001CBE50 File Offset: 0x001CA050
		public float CommonalityOfAnimal(PawnKindDef animalDef)
		{
			if (this.cachedAnimalCommonalities == null)
			{
				this.cachedAnimalCommonalities = new Dictionary<PawnKindDef, float>();
				for (int i = 0; i < this.wildAnimals.Count; i++)
				{
					this.cachedAnimalCommonalities.Add(this.wildAnimals[i].animal, this.wildAnimals[i].commonality);
				}
				foreach (PawnKindDef pawnKindDef in DefDatabase<PawnKindDef>.AllDefs)
				{
					if (pawnKindDef.RaceProps.wildBiomes != null)
					{
						for (int j = 0; j < pawnKindDef.RaceProps.wildBiomes.Count; j++)
						{
							if (pawnKindDef.RaceProps.wildBiomes[j].biome == this)
							{
								this.cachedAnimalCommonalities.Add(pawnKindDef, pawnKindDef.RaceProps.wildBiomes[j].commonality);
							}
						}
					}
				}
			}
			float result;
			if (this.cachedAnimalCommonalities.TryGetValue(animalDef, out result))
			{
				return result;
			}
			return 0f;
		}

		// Token: 0x060056EB RID: 22251 RVA: 0x001CBF70 File Offset: 0x001CA170
		public float CommonalityOfPlant(ThingDef plantDef)
		{
			this.CachePlantCommonalitiesIfShould();
			float result;
			if (this.cachedPlantCommonalities.TryGetValue(plantDef, out result))
			{
				return result;
			}
			return 0f;
		}

		// Token: 0x060056EC RID: 22252 RVA: 0x0003C4C3 File Offset: 0x0003A6C3
		public float CommonalityPctOfPlant(ThingDef plantDef)
		{
			return this.CommonalityOfPlant(plantDef) / this.PlantCommonalitiesSum;
		}

		// Token: 0x060056ED RID: 22253 RVA: 0x001CBF9C File Offset: 0x001CA19C
		public float CommonalityOfDisease(IncidentDef diseaseInc)
		{
			if (this.cachedDiseaseCommonalities == null)
			{
				this.cachedDiseaseCommonalities = new Dictionary<IncidentDef, float>();
				for (int i = 0; i < this.diseases.Count; i++)
				{
					this.cachedDiseaseCommonalities.Add(this.diseases[i].diseaseInc, this.diseases[i].commonality);
				}
				foreach (IncidentDef incidentDef in DefDatabase<IncidentDef>.AllDefs)
				{
					if (incidentDef.diseaseBiomeRecords != null)
					{
						for (int j = 0; j < incidentDef.diseaseBiomeRecords.Count; j++)
						{
							if (incidentDef.diseaseBiomeRecords[j].biome == this)
							{
								this.cachedDiseaseCommonalities.Add(incidentDef.diseaseBiomeRecords[j].diseaseInc, incidentDef.diseaseBiomeRecords[j].commonality);
							}
						}
					}
				}
			}
			float result;
			if (this.cachedDiseaseCommonalities.TryGetValue(diseaseInc, out result))
			{
				return result;
			}
			return 0f;
		}

		// Token: 0x060056EE RID: 22254 RVA: 0x0003C4D3 File Offset: 0x0003A6D3
		public bool IsPackAnimalAllowed(ThingDef pawn)
		{
			return this.allowedPackAnimals.Contains(pawn);
		}

		// Token: 0x060056EF RID: 22255 RVA: 0x0003C4E1 File Offset: 0x0003A6E1
		public static BiomeDef Named(string defName)
		{
			return DefDatabase<BiomeDef>.GetNamed(defName, true);
		}

		// Token: 0x060056F0 RID: 22256 RVA: 0x001CC0B8 File Offset: 0x001CA2B8
		private void CachePlantCommonalitiesIfShould()
		{
			if (this.cachedPlantCommonalities == null)
			{
				this.cachedPlantCommonalities = new Dictionary<ThingDef, float>();
				for (int i = 0; i < this.wildPlants.Count; i++)
				{
					if (this.wildPlants[i].plant != null)
					{
						this.cachedPlantCommonalities.Add(this.wildPlants[i].plant, this.wildPlants[i].commonality);
					}
				}
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
				{
					if (thingDef.plant != null && thingDef.plant.wildBiomes != null)
					{
						for (int j = 0; j < thingDef.plant.wildBiomes.Count; j++)
						{
							if (thingDef.plant.wildBiomes[j].biome == this)
							{
								this.cachedPlantCommonalities.Add(thingDef, thingDef.plant.wildBiomes[j].commonality);
							}
						}
					}
				}
				this.cachedPlantCommonalitiesSum = this.cachedPlantCommonalities.Sum((KeyValuePair<ThingDef, float> x) => x.Value);
			}
		}

		// Token: 0x060056F1 RID: 22257 RVA: 0x0003C4EA File Offset: 0x0003A6EA
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (Prefs.DevMode)
			{
				using (List<BiomeAnimalRecord>.Enumerator enumerator2 = this.wildAnimals.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						BiomeAnimalRecord wa = enumerator2.Current;
						if (this.wildAnimals.Count((BiomeAnimalRecord a) => a.animal == wa.animal) > 1)
						{
							yield return "Duplicate animal record: " + wa.animal.defName;
						}
					}
				}
				List<BiomeAnimalRecord>.Enumerator enumerator2 = default(List<BiomeAnimalRecord>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x04003833 RID: 14387
		public Type workerClass = typeof(BiomeWorker);

		// Token: 0x04003834 RID: 14388
		public bool implemented = true;

		// Token: 0x04003835 RID: 14389
		public bool canBuildBase = true;

		// Token: 0x04003836 RID: 14390
		public bool canAutoChoose = true;

		// Token: 0x04003837 RID: 14391
		public bool allowRoads = true;

		// Token: 0x04003838 RID: 14392
		public bool allowRivers = true;

		// Token: 0x04003839 RID: 14393
		public float animalDensity;

		// Token: 0x0400383A RID: 14394
		public float plantDensity;

		// Token: 0x0400383B RID: 14395
		public float diseaseMtbDays = 60f;

		// Token: 0x0400383C RID: 14396
		public float settlementSelectionWeight = 1f;

		// Token: 0x0400383D RID: 14397
		public bool impassable;

		// Token: 0x0400383E RID: 14398
		public bool hasVirtualPlants = true;

		// Token: 0x0400383F RID: 14399
		public float forageability;

		// Token: 0x04003840 RID: 14400
		public ThingDef foragedFood;

		// Token: 0x04003841 RID: 14401
		public bool wildPlantsCareAboutLocalFertility = true;

		// Token: 0x04003842 RID: 14402
		public float wildPlantRegrowDays = 25f;

		// Token: 0x04003843 RID: 14403
		public float movementDifficulty = 1f;

		// Token: 0x04003844 RID: 14404
		public List<WeatherCommonalityRecord> baseWeatherCommonalities = new List<WeatherCommonalityRecord>();

		// Token: 0x04003845 RID: 14405
		public List<TerrainThreshold> terrainsByFertility = new List<TerrainThreshold>();

		// Token: 0x04003846 RID: 14406
		public List<SoundDef> soundsAmbient = new List<SoundDef>();

		// Token: 0x04003847 RID: 14407
		public List<TerrainPatchMaker> terrainPatchMakers = new List<TerrainPatchMaker>();

		// Token: 0x04003848 RID: 14408
		private List<BiomePlantRecord> wildPlants = new List<BiomePlantRecord>();

		// Token: 0x04003849 RID: 14409
		private List<BiomeAnimalRecord> wildAnimals = new List<BiomeAnimalRecord>();

		// Token: 0x0400384A RID: 14410
		private List<BiomeDiseaseRecord> diseases = new List<BiomeDiseaseRecord>();

		// Token: 0x0400384B RID: 14411
		private List<ThingDef> allowedPackAnimals = new List<ThingDef>();

		// Token: 0x0400384C RID: 14412
		public bool hasBedrock = true;

		// Token: 0x0400384D RID: 14413
		public bool isExtremeBiome;

		// Token: 0x0400384E RID: 14414
		[NoTranslate]
		public string texture;

		// Token: 0x0400384F RID: 14415
		[Unsaved(false)]
		private Dictionary<PawnKindDef, float> cachedAnimalCommonalities;

		// Token: 0x04003850 RID: 14416
		[Unsaved(false)]
		private Dictionary<ThingDef, float> cachedPlantCommonalities;

		// Token: 0x04003851 RID: 14417
		[Unsaved(false)]
		private Dictionary<IncidentDef, float> cachedDiseaseCommonalities;

		// Token: 0x04003852 RID: 14418
		[Unsaved(false)]
		private Material cachedMat;

		// Token: 0x04003853 RID: 14419
		[Unsaved(false)]
		private List<ThingDef> cachedWildPlants;

		// Token: 0x04003854 RID: 14420
		[Unsaved(false)]
		private int? cachedMaxWildPlantsClusterRadius;

		// Token: 0x04003855 RID: 14421
		[Unsaved(false)]
		private float cachedPlantCommonalitiesSum;

		// Token: 0x04003856 RID: 14422
		[Unsaved(false)]
		private float? cachedLowestWildPlantOrder;

		// Token: 0x04003857 RID: 14423
		[Unsaved(false)]
		private BiomeWorker workerInt;
	}
}
