using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200171F RID: 5919
	public static class PlantUtility
	{
		// Token: 0x06008295 RID: 33429 RVA: 0x0026B8B4 File Offset: 0x00269AB4
		public static bool GrowthSeasonNow(IntVec3 c, Map map, bool forSowing = false)
		{
			Room roomOrAdjacent = c.GetRoomOrAdjacent(map, RegionType.Set_All);
			if (roomOrAdjacent == null)
			{
				return false;
			}
			if (!roomOrAdjacent.UsesOutdoorTemperature)
			{
				float temperature = c.GetTemperature(map);
				return temperature > 0f && temperature < 58f;
			}
			if (forSowing)
			{
				return map.weatherManager.growthSeasonMemory.GrowthSeasonOutdoorsNowForSowing;
			}
			return map.weatherManager.growthSeasonMemory.GrowthSeasonOutdoorsNow;
		}

		// Token: 0x06008296 RID: 33430 RVA: 0x00057B3D File Offset: 0x00055D3D
		public static bool SnowAllowsPlanting(IntVec3 c, Map map)
		{
			return c.GetSnowDepth(map) < 0.2f;
		}

		// Token: 0x06008297 RID: 33431 RVA: 0x00057B4D File Offset: 0x00055D4D
		public static bool CanEverPlantAt(this ThingDef plantDef, IntVec3 c, Map map)
		{
			return plantDef.CanEverPlantAt_NewTemp(c, map, false);
		}

		// Token: 0x06008298 RID: 33432 RVA: 0x0026B918 File Offset: 0x00269B18
		public static bool CanEverPlantAt_NewTemp(this ThingDef plantDef, IntVec3 c, Map map, bool canWipePlantsExceptTree = false)
		{
			if (plantDef.category != ThingCategory.Plant)
			{
				Log.Error("Checking CanGrowAt with " + plantDef + " which is not a plant.", false);
			}
			if (!c.InBounds(map))
			{
				return false;
			}
			if (map.fertilityGrid.FertilityAt(c) < plantDef.plant.fertilityMin)
			{
				return false;
			}
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (thing.def.BlocksPlanting(canWipePlantsExceptTree))
				{
					return false;
				}
				if (plantDef.passability == Traversability.Impassable)
				{
					if (thing.def.category == ThingCategory.Pawn || thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Building)
					{
						return false;
					}
					if (thing.def.category == ThingCategory.Plant && canWipePlantsExceptTree && thing.def.plant.IsTree)
					{
						return false;
					}
				}
			}
			if (plantDef.passability == Traversability.Impassable)
			{
				for (int j = 0; j < 4; j++)
				{
					IntVec3 c2 = c + GenAdj.CardinalDirections[j];
					if (c2.InBounds(map))
					{
						Building edifice = c2.GetEdifice(map);
						if (edifice != null && edifice.def.IsDoor)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06008299 RID: 33433 RVA: 0x0026BA4C File Offset: 0x00269C4C
		public static void LogPlantProportions()
		{
			Dictionary<ThingDef, float> dictionary = new Dictionary<ThingDef, float>();
			foreach (ThingDef key in Find.CurrentMap.Biome.AllWildPlants)
			{
				dictionary.Add(key, 0f);
			}
			float num = 0f;
			foreach (IntVec3 c in Find.CurrentMap.AllCells)
			{
				Plant plant = c.GetPlant(Find.CurrentMap);
				if (plant != null && dictionary.ContainsKey(plant.def))
				{
					Dictionary<ThingDef, float> dictionary2 = dictionary;
					ThingDef key2 = plant.def;
					float num2 = dictionary2[key2];
					dictionary2[key2] = num2 + 1f;
					num += 1f;
				}
			}
			foreach (ThingDef thingDef in Find.CurrentMap.Biome.AllWildPlants)
			{
				Dictionary<ThingDef, float> dictionary3 = dictionary;
				ThingDef key2 = thingDef;
				dictionary3[key2] /= num;
			}
			Dictionary<ThingDef, float> dictionary4 = PlantUtility.CalculateDesiredPlantProportions(Find.CurrentMap.Biome);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("PLANT           EXPECTED             FOUND");
			foreach (ThingDef thingDef2 in Find.CurrentMap.Biome.AllWildPlants)
			{
				stringBuilder.AppendLine(thingDef2.LabelCap + "       " + dictionary4[thingDef2].ToStringPercent() + "        " + dictionary[thingDef2].ToStringPercent());
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x0600829A RID: 33434 RVA: 0x0026BC60 File Offset: 0x00269E60
		private static Dictionary<ThingDef, float> CalculateDesiredPlantProportions(BiomeDef biome)
		{
			Dictionary<ThingDef, float> dictionary = new Dictionary<ThingDef, float>();
			float num = 0f;
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.plant != null)
				{
					float num2 = biome.CommonalityOfPlant(thingDef);
					dictionary.Add(thingDef, num2);
					num += num2;
				}
			}
			foreach (ThingDef thingDef2 in biome.AllWildPlants)
			{
				Dictionary<ThingDef, float> dictionary2 = dictionary;
				ThingDef key = thingDef2;
				dictionary2[key] /= num;
			}
			return dictionary;
		}

		// Token: 0x0600829B RID: 33435 RVA: 0x00057B58 File Offset: 0x00055D58
		public static IEnumerable<ThingDef> ValidPlantTypesForGrowers(List<IPlantToGrowSettable> sel)
		{
			using (IEnumerator<ThingDef> enumerator = (from def in DefDatabase<ThingDef>.AllDefs
			where def.category == ThingCategory.Plant
			select def).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef plantDef = enumerator.Current;
					if (sel.TrueForAll((IPlantToGrowSettable x) => PlantUtility.CanSowOnGrower(plantDef, x)))
					{
						yield return plantDef;
					}
				}
			}
			IEnumerator<ThingDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600829C RID: 33436 RVA: 0x0026BD28 File Offset: 0x00269F28
		public static bool CanSowOnGrower(ThingDef plantDef, object obj)
		{
			if (obj is Zone)
			{
				return plantDef.plant.sowTags.Contains("Ground");
			}
			Thing thing = obj as Thing;
			return thing != null && thing.def.building != null && plantDef.plant.sowTags.Contains(thing.def.building.sowTag);
		}

		// Token: 0x0600829D RID: 33437 RVA: 0x0026BD8C File Offset: 0x00269F8C
		public static Thing AdjacentSowBlocker(ThingDef plantDef, IntVec3 c, Map map)
		{
			for (int i = 0; i < 8; i++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCells[i];
				if (c2.InBounds(map))
				{
					Plant plant = c2.GetPlant(map);
					if (plant != null && (plant.def.plant.blockAdjacentSow || (plantDef.plant.blockAdjacentSow && plant.sown)))
					{
						return plant;
					}
				}
			}
			return null;
		}

		// Token: 0x0600829E RID: 33438 RVA: 0x00057B68 File Offset: 0x00055D68
		public static byte GetWindExposure(Plant plant)
		{
			return (byte)Mathf.Min(255f * plant.def.plant.topWindExposure, 255f);
		}

		// Token: 0x0600829F RID: 33439 RVA: 0x0026BDF8 File Offset: 0x00269FF8
		public static void SetWindExposureColors(Color32[] colors, Plant plant)
		{
			colors[1].a = (colors[2].a = PlantUtility.GetWindExposure(plant));
			colors[0].a = (colors[3].a = 0);
		}

		// Token: 0x060082A0 RID: 33440 RVA: 0x0026BE44 File Offset: 0x0026A044
		public static void LogFallColorForYear()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Fall color amounts for each latitude and each day of the year");
			stringBuilder.AppendLine("---------------------------------------");
			stringBuilder.Append("Lat".PadRight(6));
			for (int i = -90; i <= 90; i += 10)
			{
				stringBuilder.Append((i.ToString() + "d").PadRight(6));
			}
			stringBuilder.AppendLine();
			for (int j = 0; j < 60; j += 5)
			{
				stringBuilder.Append(j.ToString().PadRight(6));
				for (int k = -90; k <= 90; k += 10)
				{
					stringBuilder.Append(PlantFallColors.GetFallColorFactor((float)k, j).ToString("F3").PadRight(6));
				}
				stringBuilder.AppendLine();
			}
			Log.Message(stringBuilder.ToString(), false);
		}
	}
}
