using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001779 RID: 6009
	public static class VirtualPlantsUtility
	{
		// Token: 0x06008A91 RID: 35473 RVA: 0x0031BB64 File Offset: 0x00319D64
		public static bool CanEverEatVirtualPlants(Pawn p)
		{
			return p.RaceProps.Eats(FoodTypeFlags.Plant);
		}

		// Token: 0x06008A92 RID: 35474 RVA: 0x0031BB73 File Offset: 0x00319D73
		public static bool CanEatVirtualPlantsNow(Pawn p)
		{
			return VirtualPlantsUtility.CanEatVirtualPlants(p, GenTicks.TicksAbs);
		}

		// Token: 0x06008A93 RID: 35475 RVA: 0x0031BB80 File Offset: 0x00319D80
		public static bool CanEatVirtualPlants(Pawn p, int ticksAbs)
		{
			return p.Tile >= 0 && !p.Dead && p.IsWorldPawn() && VirtualPlantsUtility.CanEverEatVirtualPlants(p) && VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsAt(p.Tile, ticksAbs);
		}

		// Token: 0x06008A94 RID: 35476 RVA: 0x0031BBB1 File Offset: 0x00319DB1
		public static bool EnvironmentAllowsEatingVirtualPlantsNowAt(int tile)
		{
			return VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsAt(tile, GenTicks.TicksAbs);
		}

		// Token: 0x06008A95 RID: 35477 RVA: 0x0031BBBE File Offset: 0x00319DBE
		public static bool EnvironmentAllowsEatingVirtualPlantsAt(int tile, int ticksAbs)
		{
			return Find.WorldGrid[tile].biome.hasVirtualPlants && GenTemperature.GetTemperatureFromSeasonAtTile(ticksAbs, tile) >= 0f;
		}

		// Token: 0x06008A96 RID: 35478 RVA: 0x0031BBEC File Offset: 0x00319DEC
		public static void EatVirtualPlants(Pawn p)
		{
			float num = ThingDefOf.Plant_Grass.GetStatValueAbstract(StatDefOf.Nutrition, null) * VirtualPlantsUtility.VirtualPlantNutritionRandomFactor.RandomInRange;
			p.needs.food.CurLevel += num;
		}

		// Token: 0x06008A97 RID: 35479 RVA: 0x0031BC30 File Offset: 0x00319E30
		public static string GetVirtualPlantsStatusExplanationAt(int tile, int ticksAbs)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (ticksAbs == GenTicks.TicksAbs)
			{
				stringBuilder.Append("AnimalsCanGrazeNow".Translate());
			}
			else if (ticksAbs > GenTicks.TicksAbs)
			{
				stringBuilder.Append("AnimalsWillBeAbleToGraze".Translate());
			}
			else
			{
				stringBuilder.Append("AnimalsCanGraze".Translate());
			}
			stringBuilder.Append(": ");
			bool flag = VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsAt(tile, ticksAbs);
			stringBuilder.Append(flag ? "Yes".Translate() : "No".Translate());
			if (flag)
			{
				float? approxDaysUntilPossibleToGraze = VirtualPlantsUtility.GetApproxDaysUntilPossibleToGraze(tile, ticksAbs, true);
				if (approxDaysUntilPossibleToGraze != null)
				{
					stringBuilder.Append("\n" + "PossibleToGrazeFor".Translate(approxDaysUntilPossibleToGraze.Value.ToString("0.#")));
				}
				else
				{
					stringBuilder.Append("\n" + "PossibleToGrazeForever".Translate());
				}
			}
			else
			{
				if (!Find.WorldGrid[tile].biome.hasVirtualPlants)
				{
					stringBuilder.Append("\n" + "CantGrazeBecauseOfBiome".Translate(Find.WorldGrid[tile].biome.label));
				}
				float? approxDaysUntilPossibleToGraze2 = VirtualPlantsUtility.GetApproxDaysUntilPossibleToGraze(tile, ticksAbs, false);
				if (approxDaysUntilPossibleToGraze2 != null)
				{
					stringBuilder.Append("\n" + "CantGrazeBecauseOfTemp".Translate(approxDaysUntilPossibleToGraze2.Value.ToString("0.#")));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06008A98 RID: 35480 RVA: 0x0031BDF0 File Offset: 0x00319FF0
		public static float? GetApproxDaysUntilPossibleToGraze(int tile, int ticksAbs, bool untilNoLongerPossibleToGraze = false)
		{
			if (!untilNoLongerPossibleToGraze && !Find.WorldGrid[tile].biome.hasVirtualPlants)
			{
				return null;
			}
			float num = 0f;
			for (int i = 0; i < Mathf.CeilToInt(133.33334f); i++)
			{
				bool flag = VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsAt(tile, ticksAbs + (int)(num * 60000f));
				if ((!untilNoLongerPossibleToGraze && flag) || (untilNoLongerPossibleToGraze && !flag))
				{
					return new float?(num);
				}
				num += 0.45f;
			}
			return null;
		}

		// Token: 0x0400582F RID: 22575
		private static readonly FloatRange VirtualPlantNutritionRandomFactor = new FloatRange(0.7f, 1f);
	}
}
