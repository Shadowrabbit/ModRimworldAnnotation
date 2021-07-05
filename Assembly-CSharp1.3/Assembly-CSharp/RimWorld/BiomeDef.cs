using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A53 RID: 2643
	public class BiomeDef : Def
	{
		// Token: 0x17000B17 RID: 2839
		// (get) Token: 0x06003FA1 RID: 16289 RVA: 0x00159609 File Offset: 0x00157809
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

		// Token: 0x17000B18 RID: 2840
		// (get) Token: 0x06003FA2 RID: 16290 RVA: 0x00159630 File Offset: 0x00157830
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

		// Token: 0x17000B19 RID: 2841
		// (get) Token: 0x06003FA3 RID: 16291 RVA: 0x001596CC File Offset: 0x001578CC
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

		// Token: 0x17000B1A RID: 2842
		// (get) Token: 0x06003FA4 RID: 16292 RVA: 0x00159754 File Offset: 0x00157954
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

		// Token: 0x17000B1B RID: 2843
		// (get) Token: 0x06003FA5 RID: 16293 RVA: 0x00159844 File Offset: 0x00157A44
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

		// Token: 0x17000B1C RID: 2844
		// (get) Token: 0x06003FA6 RID: 16294 RVA: 0x00159926 File Offset: 0x00157B26
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

		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x06003FA7 RID: 16295 RVA: 0x00159936 File Offset: 0x00157B36
		public float PlantCommonalitiesSum
		{
			get
			{
				this.CachePlantCommonalitiesIfShould();
				return this.cachedPlantCommonalitiesSum;
			}
		}

		// Token: 0x17000B1E RID: 2846
		// (get) Token: 0x06003FA8 RID: 16296 RVA: 0x00159944 File Offset: 0x00157B44
		public Dictionary<ThingDef, float> PlantCommonalities
		{
			get
			{
				this.CachePlantCommonalitiesIfShould();
				return this.cachedPlantCommonalities;
			}
		}

		// Token: 0x17000B1F RID: 2847
		// (get) Token: 0x06003FA9 RID: 16297 RVA: 0x00159954 File Offset: 0x00157B54
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

		// Token: 0x17000B20 RID: 2848
		// (get) Token: 0x06003FAA RID: 16298 RVA: 0x001599F4 File Offset: 0x00157BF4
		public int TreeSightingsPerHourFromCaravan
		{
			get
			{
				return Mathf.FloorToInt(this.TreeDensity * 25f);
			}
		}

		// Token: 0x06003FAB RID: 16299 RVA: 0x00159A08 File Offset: 0x00157C08
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

		// Token: 0x06003FAC RID: 16300 RVA: 0x00159B28 File Offset: 0x00157D28
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

		// Token: 0x06003FAD RID: 16301 RVA: 0x00159B52 File Offset: 0x00157D52
		public float CommonalityPctOfPlant(ThingDef plantDef)
		{
			return this.CommonalityOfPlant(plantDef) / this.PlantCommonalitiesSum;
		}

		// Token: 0x06003FAE RID: 16302 RVA: 0x00159B64 File Offset: 0x00157D64
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

		// Token: 0x06003FAF RID: 16303 RVA: 0x00159C80 File Offset: 0x00157E80
		public bool IsPackAnimalAllowed(ThingDef pawn)
		{
			return this.allowedPackAnimals.Contains(pawn);
		}

		// Token: 0x06003FB0 RID: 16304 RVA: 0x00159C8E File Offset: 0x00157E8E
		public static BiomeDef Named(string defName)
		{
			return DefDatabase<BiomeDef>.GetNamed(defName, true);
		}

		// Token: 0x06003FB1 RID: 16305 RVA: 0x00159C98 File Offset: 0x00157E98
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

		// Token: 0x06003FB2 RID: 16306 RVA: 0x00159DE4 File Offset: 0x00157FE4
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

		// Token: 0x04002330 RID: 9008
		public Type workerClass = typeof(BiomeWorker);

		// Token: 0x04002331 RID: 9009
		public bool implemented = true;

		// Token: 0x04002332 RID: 9010
		public bool canBuildBase = true;

		// Token: 0x04002333 RID: 9011
		public bool canAutoChoose = true;

		// Token: 0x04002334 RID: 9012
		public bool allowRoads = true;

		// Token: 0x04002335 RID: 9013
		public bool allowRivers = true;

		// Token: 0x04002336 RID: 9014
		public bool allowFarmingCamps = true;

		// Token: 0x04002337 RID: 9015
		public float animalDensity;

		// Token: 0x04002338 RID: 9016
		public float plantDensity;

		// Token: 0x04002339 RID: 9017
		public float diseaseMtbDays = 60f;

		// Token: 0x0400233A RID: 9018
		public float settlementSelectionWeight = 1f;

		// Token: 0x0400233B RID: 9019
		public float campSelectionWeight = 1f;

		// Token: 0x0400233C RID: 9020
		public bool impassable;

		// Token: 0x0400233D RID: 9021
		public bool hasVirtualPlants = true;

		// Token: 0x0400233E RID: 9022
		public float forageability;

		// Token: 0x0400233F RID: 9023
		public ThingDef foragedFood;

		// Token: 0x04002340 RID: 9024
		public bool wildPlantsCareAboutLocalFertility = true;

		// Token: 0x04002341 RID: 9025
		public float wildPlantRegrowDays = 25f;

		// Token: 0x04002342 RID: 9026
		public float movementDifficulty = 1f;

		// Token: 0x04002343 RID: 9027
		public List<WeatherCommonalityRecord> baseWeatherCommonalities = new List<WeatherCommonalityRecord>();

		// Token: 0x04002344 RID: 9028
		public List<TerrainThreshold> terrainsByFertility = new List<TerrainThreshold>();

		// Token: 0x04002345 RID: 9029
		public List<SoundDef> soundsAmbient = new List<SoundDef>();

		// Token: 0x04002346 RID: 9030
		public List<TerrainPatchMaker> terrainPatchMakers = new List<TerrainPatchMaker>();

		// Token: 0x04002347 RID: 9031
		private List<BiomePlantRecord> wildPlants = new List<BiomePlantRecord>();

		// Token: 0x04002348 RID: 9032
		private List<BiomeAnimalRecord> wildAnimals = new List<BiomeAnimalRecord>();

		// Token: 0x04002349 RID: 9033
		private List<BiomeDiseaseRecord> diseases = new List<BiomeDiseaseRecord>();

		// Token: 0x0400234A RID: 9034
		private List<ThingDef> allowedPackAnimals = new List<ThingDef>();

		// Token: 0x0400234B RID: 9035
		public bool hasBedrock = true;

		// Token: 0x0400234C RID: 9036
		public bool isExtremeBiome;

		// Token: 0x0400234D RID: 9037
		[NoTranslate]
		public string texture;

		// Token: 0x0400234E RID: 9038
		[Unsaved(false)]
		private Dictionary<PawnKindDef, float> cachedAnimalCommonalities;

		// Token: 0x0400234F RID: 9039
		[Unsaved(false)]
		private Dictionary<ThingDef, float> cachedPlantCommonalities;

		// Token: 0x04002350 RID: 9040
		[Unsaved(false)]
		private Dictionary<IncidentDef, float> cachedDiseaseCommonalities;

		// Token: 0x04002351 RID: 9041
		[Unsaved(false)]
		private Material cachedMat;

		// Token: 0x04002352 RID: 9042
		[Unsaved(false)]
		private List<ThingDef> cachedWildPlants;

		// Token: 0x04002353 RID: 9043
		[Unsaved(false)]
		private int? cachedMaxWildPlantsClusterRadius;

		// Token: 0x04002354 RID: 9044
		[Unsaved(false)]
		private float cachedPlantCommonalitiesSum;

		// Token: 0x04002355 RID: 9045
		[Unsaved(false)]
		private float? cachedLowestWildPlantOrder;

		// Token: 0x04002356 RID: 9046
		[Unsaved(false)]
		private BiomeWorker workerInt;
	}
}
