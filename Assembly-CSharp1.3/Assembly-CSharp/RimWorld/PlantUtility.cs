using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010C0 RID: 4288
	public static class PlantUtility
	{
		// Token: 0x0600669C RID: 26268 RVA: 0x0022A724 File Offset: 0x00228924
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

		// Token: 0x0600669D RID: 26269 RVA: 0x0022A787 File Offset: 0x00228987
		public static bool SnowAllowsPlanting(IntVec3 c, Map map)
		{
			return c.GetSnowDepth(map) < 0.2f;
		}

		// Token: 0x0600669E RID: 26270 RVA: 0x0022A798 File Offset: 0x00228998
		public static bool CanNowPlantAt(this ThingDef plantDef, IntVec3 c, Map map, bool canWipePlantsExceptTree = false)
		{
			if (!plantDef.CanEverPlantAt(c, map, canWipePlantsExceptTree))
			{
				return false;
			}
			foreach (Thing thing in c.GetThingList(map))
			{
				if (map.designationManager.DesignationOn(thing, DesignationDefOf.Uninstall) != null)
				{
					return false;
				}
				if (map.designationManager.DesignationOn(thing, DesignationDefOf.Deconstruct) != null)
				{
					return false;
				}
				Building building;
				Blueprint_Install blueprint_Install;
				if ((building = (thing as Building)) != null && map.listerBuildings.TryGetReinstallBlueprint(building, out blueprint_Install))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600669F RID: 26271 RVA: 0x0022A844 File Offset: 0x00228A44
		public static AcceptanceReport CanEverPlantAt(this ThingDef plantDef, IntVec3 c, Map map, out Thing blockingThing, bool canWipePlantsExceptTree = false)
		{
			blockingThing = null;
			if (plantDef.category != ThingCategory.Plant)
			{
				Log.Error("Checking CanGrowAt with " + plantDef + " which is not a plant.");
			}
			if (!c.InBounds(map))
			{
				return "OutOfBounds".Translate();
			}
			if (map.fertilityGrid.FertilityAt(c) < plantDef.plant.fertilityMin)
			{
				return "MessageWarningNotEnoughFertility".Translate();
			}
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (thing.def.BlocksPlanting(canWipePlantsExceptTree))
				{
					blockingThing = thing;
					return "BlockedBy".Translate(thing);
				}
				if (plantDef.passability == Traversability.Impassable)
				{
					if (thing.def.category == ThingCategory.Pawn || thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Building)
					{
						blockingThing = thing;
						return "BlockedBy".Translate(thing);
					}
					if (thing.def.category == ThingCategory.Plant && canWipePlantsExceptTree && thing.def.plant.IsTree)
					{
						blockingThing = thing;
						return "BlockedBy".Translate(thing);
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
							blockingThing = edifice;
							return "BlockedBy".Translate(edifice);
						}
					}
				}
			}
			return true;
		}

		// Token: 0x060066A0 RID: 26272 RVA: 0x0022AA04 File Offset: 0x00228C04
		public static bool CanEverPlantAt(this ThingDef plantDef, IntVec3 c, Map map, bool canWipePlantsExceptTree = false)
		{
			Thing thing;
			return plantDef.CanEverPlantAt(c, map, out thing, canWipePlantsExceptTree).Accepted;
		}

		// Token: 0x060066A1 RID: 26273 RVA: 0x0022AA24 File Offset: 0x00228C24
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
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x060066A2 RID: 26274 RVA: 0x0022AC38 File Offset: 0x00228E38
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

		// Token: 0x060066A3 RID: 26275 RVA: 0x0022AD00 File Offset: 0x00228F00
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

		// Token: 0x060066A4 RID: 26276 RVA: 0x0022AD10 File Offset: 0x00228F10
		public static bool CanSowOnGrower(ThingDef plantDef, object obj)
		{
			if (obj is Zone)
			{
				return plantDef.plant.sowTags.Contains("Ground");
			}
			Thing thing = obj as Thing;
			return thing != null && thing.def.building != null && plantDef.plant.sowTags.Contains(thing.def.building.sowTag);
		}

		// Token: 0x060066A5 RID: 26277 RVA: 0x0022AD74 File Offset: 0x00228F74
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

		// Token: 0x060066A6 RID: 26278 RVA: 0x0022ADDD File Offset: 0x00228FDD
		public static byte GetWindExposure(Plant plant)
		{
			return (byte)Mathf.Min(255f * plant.def.plant.topWindExposure, 255f);
		}

		// Token: 0x060066A7 RID: 26279 RVA: 0x0022AE00 File Offset: 0x00229000
		public static void SetWindExposureColors(Color32[] colors, Plant plant)
		{
			colors[1].a = (colors[2].a = PlantUtility.GetWindExposure(plant));
			colors[0].a = (colors[3].a = 0);
		}

		// Token: 0x060066A8 RID: 26280 RVA: 0x0022AE4C File Offset: 0x0022904C
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
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x060066A9 RID: 26281 RVA: 0x0022AF22 File Offset: 0x00229122
		public static float GrowthRateFactorFor_Fertility(ThingDef def, float fertilityAtCell)
		{
			return fertilityAtCell * def.plant.fertilitySensitivity + (1f - def.plant.fertilitySensitivity);
		}

		// Token: 0x060066AA RID: 26282 RVA: 0x0022AF44 File Offset: 0x00229144
		public static float GrowthRateFactorFor_Light(ThingDef def, float glow)
		{
			if (def.plant.growMinGlow == def.plant.growOptimalGlow && glow == def.plant.growOptimalGlow)
			{
				return 1f;
			}
			return GenMath.InverseLerp(def.plant.growMinGlow, def.plant.growOptimalGlow, glow);
		}

		// Token: 0x060066AB RID: 26283 RVA: 0x0022AF99 File Offset: 0x00229199
		public static float GrowthRateFactorFor_Temperature(float cellTemp)
		{
			if (cellTemp < 6f)
			{
				return Mathf.InverseLerp(0f, 6f, cellTemp);
			}
			if (cellTemp > 42f)
			{
				return Mathf.InverseLerp(58f, 42f, cellTemp);
			}
			return 1f;
		}

		// Token: 0x060066AC RID: 26284 RVA: 0x0022AFD2 File Offset: 0x002291D2
		public static float NutritionFactorFromGrowth(ThingDef def, float plantGrowth)
		{
			if (def.plant.Sowable)
			{
				return plantGrowth;
			}
			return Mathf.Lerp(0.5f, 1f, plantGrowth);
		}

		// Token: 0x060066AD RID: 26285 RVA: 0x0022AFF4 File Offset: 0x002291F4
		public static bool PawnWillingToCutPlant_Job(Thing plant, Pawn pawn)
		{
			return !plant.def.plant.IsTree || !plant.def.plant.treeLoversCareIfChopped || new HistoryEvent(HistoryEventDefOf.CutTree, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job();
		}
	}
}
