using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200210E RID: 8462
	public static class DaysWorthOfFoodCalculator
	{
		// Token: 0x0600B3B0 RID: 46000 RVA: 0x003420A4 File Offset: 0x003402A4
		private static float ApproxDaysWorthOfFood(List<Pawn> pawns, List<ThingDefCount> extraFood, int tile, IgnorePawnsInventoryMode ignoreInventory, Faction faction, WorldPath path = null, float nextTileCostLeft = 0f, int caravanTicksPerMove = 3300, bool assumeCaravanMoving = true)
		{
			if (!DaysWorthOfFoodCalculator.AnyFoodEatingPawn(pawns))
			{
				return 600f;
			}
			if (!assumeCaravanMoving)
			{
				path = null;
			}
			DaysWorthOfFoodCalculator.tmpFood.Clear();
			if (extraFood != null)
			{
				int i = 0;
				int count = extraFood.Count;
				while (i < count)
				{
					ThingDefCount item = extraFood[i];
					if (item.ThingDef.IsNutritionGivingIngestible && item.Count > 0)
					{
						DaysWorthOfFoodCalculator.tmpFood.Add(item);
					}
					i++;
				}
			}
			int j = 0;
			int count2 = pawns.Count;
			while (j < count2)
			{
				Pawn pawn = pawns[j];
				if (!InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, ignoreInventory))
				{
					ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
					int k = 0;
					int count3 = innerContainer.Count;
					while (k < count3)
					{
						Thing thing = innerContainer[k];
						if (thing.def.IsNutritionGivingIngestible)
						{
							DaysWorthOfFoodCalculator.tmpFood.Add(new ThingDefCount(thing.def, thing.stackCount));
						}
						k++;
					}
				}
				j++;
			}
			DaysWorthOfFoodCalculator.tmpFood2.Clear();
			DaysWorthOfFoodCalculator.tmpFood2.AddRange(DaysWorthOfFoodCalculator.tmpFood);
			DaysWorthOfFoodCalculator.tmpFood.Clear();
			int l = 0;
			int count4 = DaysWorthOfFoodCalculator.tmpFood2.Count;
			while (l < count4)
			{
				ThingDefCount item2 = DaysWorthOfFoodCalculator.tmpFood2[l];
				bool flag = false;
				int m = 0;
				int count5 = DaysWorthOfFoodCalculator.tmpFood.Count;
				while (m < count5)
				{
					ThingDefCount thingDefCount = DaysWorthOfFoodCalculator.tmpFood[m];
					if (thingDefCount.ThingDef == item2.ThingDef)
					{
						DaysWorthOfFoodCalculator.tmpFood[m] = thingDefCount.WithCount(thingDefCount.Count + item2.Count);
						flag = true;
						break;
					}
					m++;
				}
				if (!flag)
				{
					DaysWorthOfFoodCalculator.tmpFood.Add(item2);
				}
				l++;
			}
			DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn.Clear();
			int n = 0;
			int count6 = pawns.Count;
			while (n < count6)
			{
				DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn.Add(0f);
				n++;
			}
			int ticksAbs = Find.TickManager.TicksAbs;
			DaysWorthOfFoodCalculator.tmpTicksToArrive.Clear();
			if (path != null && path.Found)
			{
				CaravanArrivalTimeEstimator.EstimatedTicksToArriveToEvery(tile, path.LastNode, path, nextTileCostLeft, caravanTicksPerMove, ticksAbs, DaysWorthOfFoodCalculator.tmpTicksToArrive);
			}
			DaysWorthOfFoodCalculator.cachedNutritionBetweenHungryAndFed.Clear();
			DaysWorthOfFoodCalculator.cachedTicksUntilHungryWhenFed.Clear();
			DaysWorthOfFoodCalculator.cachedMaxFoodLevel.Clear();
			int num = 0;
			int count7 = pawns.Count;
			while (num < count7)
			{
				Pawn pawn2 = pawns[num];
				if (pawn2.RaceProps.EatsFood)
				{
					Need_Food food = pawn2.needs.food;
					DaysWorthOfFoodCalculator.cachedNutritionBetweenHungryAndFed.Add(food.NutritionBetweenHungryAndFed);
					DaysWorthOfFoodCalculator.cachedTicksUntilHungryWhenFed.Add(food.TicksUntilHungryWhenFedIgnoringMalnutrition);
					DaysWorthOfFoodCalculator.cachedMaxFoodLevel.Add(food.MaxLevel);
				}
				else
				{
					DaysWorthOfFoodCalculator.cachedNutritionBetweenHungryAndFed.Add(0f);
					DaysWorthOfFoodCalculator.cachedTicksUntilHungryWhenFed.Add(0);
					DaysWorthOfFoodCalculator.cachedMaxFoodLevel.Add(0f);
				}
				num++;
			}
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			bool flag2 = false;
			WorldGrid worldGrid = Find.WorldGrid;
			bool flag3;
			do
			{
				flag3 = false;
				int num5 = ticksAbs + (int)(num3 * 60000f);
				int num6 = (path != null) ? CaravanArrivalTimeEstimator.TileIllBeInAt(num5, DaysWorthOfFoodCalculator.tmpTicksToArrive, ticksAbs) : tile;
				bool flag4 = CaravanNightRestUtility.WouldBeRestingAt(num6, (long)num5);
				float progressPerTick = ForagedFoodPerDayCalculator.GetProgressPerTick(assumeCaravanMoving && !flag4, flag4, null);
				float num7 = 1f / progressPerTick;
				bool flag5 = VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsAt(num6, num5);
				float num8 = num3 - num2;
				if (num8 > 0f)
				{
					num4 += num8 * 60000f;
					if (num4 >= num7)
					{
						BiomeDef biome = worldGrid[num6].biome;
						int num9 = Mathf.RoundToInt(ForagedFoodPerDayCalculator.GetForagedFoodCountPerInterval(pawns, biome, faction, null));
						ThingDef foragedFood = biome.foragedFood;
						while (num4 >= num7)
						{
							num4 -= num7;
							if (num9 > 0)
							{
								bool flag6 = false;
								for (int num10 = DaysWorthOfFoodCalculator.tmpFood.Count - 1; num10 >= 0; num10--)
								{
									ThingDefCount thingDefCount2 = DaysWorthOfFoodCalculator.tmpFood[num10];
									if (thingDefCount2.ThingDef == foragedFood)
									{
										DaysWorthOfFoodCalculator.tmpFood[num10] = thingDefCount2.WithCount(thingDefCount2.Count + num9);
										flag6 = true;
										break;
									}
								}
								if (!flag6)
								{
									DaysWorthOfFoodCalculator.tmpFood.Add(new ThingDefCount(foragedFood, num9));
								}
							}
						}
					}
				}
				num2 = num3;
				int num11 = 0;
				int count8 = pawns.Count;
				while (num11 < count8)
				{
					Pawn pawn3 = pawns[num11];
					if (pawn3.RaceProps.EatsFood)
					{
						if (flag5 && VirtualPlantsUtility.CanEverEatVirtualPlants(pawn3))
						{
							if (DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn[num11] < num3)
							{
								DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn[num11] = num3;
							}
							else
							{
								List<float> list = DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn;
								int index = num11;
								list[index] += 0.45f;
							}
							flag3 = true;
						}
						else
						{
							float num12 = DaysWorthOfFoodCalculator.cachedNutritionBetweenHungryAndFed[num11];
							int num13 = DaysWorthOfFoodCalculator.cachedTicksUntilHungryWhenFed[num11];
							for (;;)
							{
								int num14 = DaysWorthOfFoodCalculator.BestEverEdibleFoodIndexFor(pawn3, DaysWorthOfFoodCalculator.tmpFood);
								if (num14 < 0)
								{
									break;
								}
								ThingDefCount thingDefCount3 = DaysWorthOfFoodCalculator.tmpFood[num14];
								float num15 = Mathf.Min(thingDefCount3.ThingDef.ingestible.CachedNutrition, num12);
								float num16 = num15 / num12 * (float)num13 / 60000f;
								int num17 = Mathf.Min(Mathf.CeilToInt(Mathf.Min(0.2f, DaysWorthOfFoodCalculator.cachedMaxFoodLevel[num11]) / num15), thingDefCount3.Count);
								List<float> list = DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn;
								int index = num11;
								list[index] += num16 * (float)num17;
								DaysWorthOfFoodCalculator.tmpFood[num14] = thingDefCount3.WithCount(thingDefCount3.Count - num17);
								flag3 = true;
								if (DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn[num11] >= num3)
								{
									goto IL_5BA;
								}
							}
							if (DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn[num11] < num3)
							{
								flag2 = true;
							}
						}
						IL_5BA:
						if (flag2)
						{
							break;
						}
						num3 = Mathf.Max(num3, DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn[num11]);
					}
					num11++;
				}
			}
			while (flag3 && !flag2 && num3 <= 601f);
			float num18 = 600f;
			int num19 = 0;
			int count9 = pawns.Count;
			while (num19 < count9)
			{
				if (pawns[num19].RaceProps.EatsFood)
				{
					num18 = Mathf.Min(num18, DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn[num19]);
				}
				num19++;
			}
			return num18;
		}

		// Token: 0x0600B3B1 RID: 46001 RVA: 0x003426F4 File Offset: 0x003408F4
		public static float ApproxDaysWorthOfFood(Caravan caravan)
		{
			return DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(caravan.PawnsListForReading, null, caravan.Tile, IgnorePawnsInventoryMode.DontIgnore, caravan.Faction, caravan.pather.curPath, caravan.pather.nextTileCostLeft, caravan.TicksPerMove, caravan.pather.Moving && !caravan.pather.Paused);
		}

		// Token: 0x0600B3B2 RID: 46002 RVA: 0x00342754 File Offset: 0x00340954
		public static float ApproxDaysWorthOfFood(List<TransferableOneWay> transferables, int tile, IgnorePawnsInventoryMode ignoreInventory, Faction faction, WorldPath path = null, float nextTileCostLeft = 0f, int caravanTicksPerMove = 3300)
		{
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			DaysWorthOfFoodCalculator.tmpPawns.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing)
				{
					if (transferableOneWay.AnyThing is Pawn)
					{
						for (int j = 0; j < transferableOneWay.CountToTransfer; j++)
						{
							Pawn pawn = (Pawn)transferableOneWay.things[j];
							if (pawn.RaceProps.EatsFood)
							{
								DaysWorthOfFoodCalculator.tmpPawns.Add(pawn);
							}
						}
					}
					else
					{
						DaysWorthOfFoodCalculator.tmpThingDefCounts.Add(new ThingDefCount(transferableOneWay.ThingDef, transferableOneWay.CountToTransfer));
					}
				}
			}
			float result = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(DaysWorthOfFoodCalculator.tmpPawns, DaysWorthOfFoodCalculator.tmpThingDefCounts, tile, ignoreInventory, faction, path, nextTileCostLeft, caravanTicksPerMove, true);
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			DaysWorthOfFoodCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x0600B3B3 RID: 46003 RVA: 0x0034282C File Offset: 0x00340A2C
		public static float ApproxDaysWorthOfFoodLeftAfterTransfer(List<TransferableOneWay> transferables, int tile, IgnorePawnsInventoryMode ignoreInventory, Faction faction, WorldPath path = null, float nextTileCostLeft = 0f, int caravanTicksPerMove = 3300)
		{
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			DaysWorthOfFoodCalculator.tmpPawns.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing)
				{
					if (transferableOneWay.AnyThing is Pawn)
					{
						for (int j = transferableOneWay.things.Count - 1; j >= transferableOneWay.CountToTransfer; j--)
						{
							Pawn pawn = (Pawn)transferableOneWay.things[j];
							if (pawn.RaceProps.EatsFood)
							{
								DaysWorthOfFoodCalculator.tmpPawns.Add(pawn);
							}
						}
					}
					else
					{
						DaysWorthOfFoodCalculator.tmpThingDefCounts.Add(new ThingDefCount(transferableOneWay.ThingDef, transferableOneWay.MaxCount - transferableOneWay.CountToTransfer));
					}
				}
			}
			float result = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(DaysWorthOfFoodCalculator.tmpPawns, DaysWorthOfFoodCalculator.tmpThingDefCounts, tile, ignoreInventory, faction, path, nextTileCostLeft, caravanTicksPerMove, true);
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			DaysWorthOfFoodCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x0600B3B4 RID: 46004 RVA: 0x0034291C File Offset: 0x00340B1C
		public static float ApproxDaysWorthOfFood(List<Pawn> pawns, List<Thing> potentiallyFood, int tile, IgnorePawnsInventoryMode ignoreInventory, Faction faction)
		{
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			DaysWorthOfFoodCalculator.tmpPawns.Clear();
			for (int i = 0; i < pawns.Count; i++)
			{
				Pawn pawn = pawns[i];
				if (pawn.RaceProps.EatsFood)
				{
					DaysWorthOfFoodCalculator.tmpPawns.Add(pawn);
				}
			}
			for (int j = 0; j < potentiallyFood.Count; j++)
			{
				DaysWorthOfFoodCalculator.tmpThingDefCounts.Add(new ThingDefCount(potentiallyFood[j].def, potentiallyFood[j].stackCount));
			}
			float result = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(DaysWorthOfFoodCalculator.tmpPawns, DaysWorthOfFoodCalculator.tmpThingDefCounts, tile, ignoreInventory, faction, null, 0f, 3300, true);
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			DaysWorthOfFoodCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x0600B3B5 RID: 46005 RVA: 0x003429DC File Offset: 0x00340BDC
		public static float ApproxDaysWorthOfFood(List<Pawn> pawns, List<ThingCount> potentiallyFood, int tile, IgnorePawnsInventoryMode ignoreInventory, Faction faction)
		{
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			for (int i = 0; i < potentiallyFood.Count; i++)
			{
				if (potentiallyFood[i].Count > 0)
				{
					DaysWorthOfFoodCalculator.tmpThingDefCounts.Add(new ThingDefCount(potentiallyFood[i].Thing.def, potentiallyFood[i].Count));
				}
			}
			float result = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(pawns, DaysWorthOfFoodCalculator.tmpThingDefCounts, tile, ignoreInventory, faction, null, 0f, 3300, true);
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			return result;
		}

		// Token: 0x0600B3B6 RID: 46006 RVA: 0x00342A70 File Offset: 0x00340C70
		public static float ApproxDaysWorthOfFoodLeftAfterTradeableTransfer(List<Thing> allCurrentThings, List<Tradeable> tradeables, int tile, IgnorePawnsInventoryMode ignoreInventory, Faction faction)
		{
			DaysWorthOfFoodCalculator.tmpThingCounts.Clear();
			TransferableUtility.SimulateTradeableTransfer(allCurrentThings, tradeables, DaysWorthOfFoodCalculator.tmpThingCounts);
			DaysWorthOfFoodCalculator.tmpPawns.Clear();
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			for (int i = DaysWorthOfFoodCalculator.tmpThingCounts.Count - 1; i >= 0; i--)
			{
				if (DaysWorthOfFoodCalculator.tmpThingCounts[i].Count > 0)
				{
					Pawn pawn = DaysWorthOfFoodCalculator.tmpThingCounts[i].Thing as Pawn;
					if (pawn != null)
					{
						if (pawn.RaceProps.EatsFood)
						{
							DaysWorthOfFoodCalculator.tmpPawns.Add(pawn);
						}
					}
					else
					{
						DaysWorthOfFoodCalculator.tmpThingDefCounts.Add(new ThingDefCount(DaysWorthOfFoodCalculator.tmpThingCounts[i].Thing.def, DaysWorthOfFoodCalculator.tmpThingCounts[i].Count));
					}
				}
			}
			DaysWorthOfFoodCalculator.tmpThingCounts.Clear();
			float result = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(DaysWorthOfFoodCalculator.tmpPawns, DaysWorthOfFoodCalculator.tmpThingDefCounts, tile, ignoreInventory, faction, null, 0f, 3300, true);
			DaysWorthOfFoodCalculator.tmpPawns.Clear();
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			return result;
		}

		// Token: 0x0600B3B7 RID: 46007 RVA: 0x00342B88 File Offset: 0x00340D88
		public static bool AnyFoodEatingPawn(List<Pawn> pawns)
		{
			int i = 0;
			int count = pawns.Count;
			while (i < count)
			{
				if (pawns[i].RaceProps.EatsFood)
				{
					return true;
				}
				i++;
			}
			return false;
		}

		// Token: 0x0600B3B8 RID: 46008 RVA: 0x00342BC0 File Offset: 0x00340DC0
		private static int BestEverEdibleFoodIndexFor(Pawn pawn, List<ThingDefCount> food)
		{
			int num = -1;
			float num2 = 0f;
			int i = 0;
			int count = food.Count;
			while (i < count)
			{
				if (food[i].Count > 0)
				{
					ThingDef thingDef = food[i].ThingDef;
					if (CaravanPawnsNeedsUtility.CanEatForNutritionEver(thingDef, pawn))
					{
						float foodScore = CaravanPawnsNeedsUtility.GetFoodScore(thingDef, pawn, thingDef.ingestible.CachedNutrition);
						if (num < 0 || foodScore > num2)
						{
							num = i;
							num2 = foodScore;
						}
					}
				}
				i++;
			}
			return num;
		}

		// Token: 0x04007B82 RID: 31618
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x04007B83 RID: 31619
		private static List<ThingDefCount> tmpThingDefCounts = new List<ThingDefCount>();

		// Token: 0x04007B84 RID: 31620
		private static List<ThingCount> tmpThingCounts = new List<ThingCount>();

		// Token: 0x04007B85 RID: 31621
		public const float InfiniteDaysWorthOfFood = 600f;

		// Token: 0x04007B86 RID: 31622
		private static List<float> tmpDaysWorthOfFoodForPawn = new List<float>();

		// Token: 0x04007B87 RID: 31623
		private static List<ThingDefCount> tmpFood = new List<ThingDefCount>();

		// Token: 0x04007B88 RID: 31624
		private static List<ThingDefCount> tmpFood2 = new List<ThingDefCount>();

		// Token: 0x04007B89 RID: 31625
		private static List<Pair<int, int>> tmpTicksToArrive = new List<Pair<int, int>>();

		// Token: 0x04007B8A RID: 31626
		private static List<float> cachedNutritionBetweenHungryAndFed = new List<float>();

		// Token: 0x04007B8B RID: 31627
		private static List<int> cachedTicksUntilHungryWhenFed = new List<int>();

		// Token: 0x04007B8C RID: 31628
		private static List<float> cachedMaxFoodLevel = new List<float>();
	}
}
