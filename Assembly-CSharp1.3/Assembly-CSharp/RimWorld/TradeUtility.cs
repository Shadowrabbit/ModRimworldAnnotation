using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001233 RID: 4659
	public static class TradeUtility
	{
		// Token: 0x06006FB2 RID: 28594 RVA: 0x00253BAC File Offset: 0x00251DAC
		public static bool EverPlayerSellable(ThingDef def)
		{
			return def.tradeability.PlayerCanSell() && def.GetStatValueAbstract(StatDefOf.MarketValue, null) > 0f && (def.category == ThingCategory.Item || def.category == ThingCategory.Pawn || def.category == ThingCategory.Building) && (def.category != ThingCategory.Building || def.Minifiable);
		}

		// Token: 0x06006FB3 RID: 28595 RVA: 0x00253C10 File Offset: 0x00251E10
		public static bool PlayerSellableNow(Thing t, ITrader trader)
		{
			t = t.GetInnerIfMinified();
			if (!TradeUtility.EverPlayerSellable(t.def))
			{
				return false;
			}
			if (t.IsNotFresh())
			{
				return false;
			}
			Apparel apparel = t as Apparel;
			if (apparel != null && apparel.WornByCorpse)
			{
				return false;
			}
			if (CompBiocodable.IsBiocoded(t))
			{
				return false;
			}
			Pawn pawn = t as Pawn;
			return pawn == null || ((pawn.GetExtraHostFaction(null) == null || pawn.GetExtraHostFaction(null) != trader.Faction) && (!pawn.IsQuestLodger() || pawn.GetExtraHomeFaction(null) != trader.Faction) && (pawn.HomeFaction == null || pawn.HomeFaction != trader.Faction || pawn.IsSlave || pawn.IsPrisoner));
		}

		// Token: 0x06006FB4 RID: 28596 RVA: 0x00253CC0 File Offset: 0x00251EC0
		public static void SpawnDropPod(IntVec3 dropSpot, Map map, Thing t)
		{
			DropPodUtility.MakeDropPodAt(dropSpot, map, new ActiveDropPodInfo
			{
				SingleContainedThing = t,
				leaveSlag = false
			});
		}

		// Token: 0x06006FB5 RID: 28597 RVA: 0x00253CE9 File Offset: 0x00251EE9
		public static IEnumerable<Thing> AllLaunchableThingsForTrade(Map map, ITrader trader = null)
		{
			HashSet<Thing> yieldedThings = new HashSet<Thing>();
			foreach (Building_OrbitalTradeBeacon building_OrbitalTradeBeacon in Building_OrbitalTradeBeacon.AllPowered(map))
			{
				foreach (IntVec3 c in building_OrbitalTradeBeacon.TradeableCells)
				{
					List<Thing> thingList = c.GetThingList(map);
					int num;
					for (int i = 0; i < thingList.Count; i = num + 1)
					{
						Thing thing = thingList[i];
						if (thing.def.category == ThingCategory.Item && TradeUtility.PlayerSellableNow(thing, trader) && !yieldedThings.Contains(thing))
						{
							yieldedThings.Add(thing);
							yield return thing;
						}
						num = i;
					}
					thingList = null;
				}
				IEnumerator<IntVec3> enumerator2 = null;
			}
			IEnumerator<Building_OrbitalTradeBeacon> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06006FB6 RID: 28598 RVA: 0x00253D00 File Offset: 0x00251F00
		public static IEnumerable<Pawn> AllSellableColonyPawns(Map map, bool checkAcceptableTemperatureOfAnimals = true)
		{
			foreach (Pawn pawn in map.mapPawns.PrisonersOfColonySpawned)
			{
				if (pawn.guest.PrisonerIsSecure)
				{
					yield return pawn;
				}
			}
			List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
			foreach (Pawn pawn2 in map.mapPawns.SlavesOfColonySpawned)
			{
				yield return pawn2;
			}
			enumerator = default(List<Pawn>.Enumerator);
			foreach (Pawn pawn3 in map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer))
			{
				if (pawn3.RaceProps.Animal && pawn3.HostFaction == null && !pawn3.InMentalState && !pawn3.Downed && (!checkAcceptableTemperatureOfAnimals || map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(pawn3.def)))
				{
					yield return pawn3;
				}
			}
			enumerator = default(List<Pawn>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06006FB7 RID: 28599 RVA: 0x00253D18 File Offset: 0x00251F18
		public static Thing ThingFromStockToMergeWith(ITrader trader, Thing thing)
		{
			if (thing is Pawn)
			{
				return null;
			}
			foreach (Thing thing2 in trader.Goods)
			{
				if (TransferableUtility.TransferAsOne(thing2, thing, TransferAsOneMode.Normal) && thing2.CanStackWith(thing) && thing2.def.stackLimit != 1)
				{
					return thing2;
				}
			}
			return null;
		}

		// Token: 0x06006FB8 RID: 28600 RVA: 0x00253D90 File Offset: 0x00251F90
		public static void LaunchThingsOfType(ThingDef resDef, int debt, Map map, TradeShip trader)
		{
			while (debt > 0)
			{
				Thing thing = null;
				foreach (Building_OrbitalTradeBeacon building_OrbitalTradeBeacon in Building_OrbitalTradeBeacon.AllPowered(map))
				{
					foreach (IntVec3 c in building_OrbitalTradeBeacon.TradeableCells)
					{
						foreach (Thing thing2 in map.thingGrid.ThingsAt(c))
						{
							if (thing2.def == resDef)
							{
								thing = thing2;
								goto IL_9D;
							}
						}
					}
				}
				IL_9D:
				if (thing == null)
				{
					Log.Error("Could not find any " + resDef + " to transfer to trader.");
					return;
				}
				int num = Math.Min(debt, thing.stackCount);
				if (trader != null)
				{
					trader.GiveSoldThingToTrader(thing, num, TradeSession.playerNegotiator);
				}
				else
				{
					thing.SplitOff(num).Destroy(DestroyMode.Vanish);
				}
				debt -= num;
			}
		}

		// Token: 0x06006FB9 RID: 28601 RVA: 0x00253EB4 File Offset: 0x002520B4
		public static void LaunchSilver(Map map, int fee)
		{
			TradeUtility.LaunchThingsOfType(ThingDefOf.Silver, fee, map, null);
		}

		// Token: 0x06006FBA RID: 28602 RVA: 0x00253EC4 File Offset: 0x002520C4
		public static Map PlayerHomeMapWithMostLaunchableSilver()
		{
			return (from x in Find.Maps
			where x.IsPlayerHome
			select x).MaxBy((Map x) => (from t in TradeUtility.AllLaunchableThingsForTrade(x, null)
			where t.def == ThingDefOf.Silver
			select t).Sum((Thing t) => t.stackCount));
		}

		// Token: 0x06006FBB RID: 28603 RVA: 0x00253F20 File Offset: 0x00252120
		public static bool ColonyHasEnoughSilver(Map map, int fee)
		{
			return (from t in TradeUtility.AllLaunchableThingsForTrade(map, null)
			where t.def == ThingDefOf.Silver
			select t).Sum((Thing t) => t.stackCount) >= fee;
		}

		// Token: 0x06006FBC RID: 28604 RVA: 0x00253F84 File Offset: 0x00252184
		public static void CheckInteractWithTradersTeachOpportunity(Pawn pawn)
		{
			if (pawn.Dead)
			{
				return;
			}
			Lord lord = pawn.GetLord();
			if (lord != null && lord.CurLordToil is LordToil_DefendTraderCaravan)
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.InteractingWithTraders, pawn, OpportunityType.Important);
			}
		}

		// Token: 0x06006FBD RID: 28605 RVA: 0x00253FC0 File Offset: 0x002521C0
		public static float GetPricePlayerSell(Thing thing, float priceFactorSell_TraderPriceType, float priceFactorSell_HumanPawn, float priceGain_PlayerNegotiator, float priceGain_FactionBase, float priceGain_DrugBonus, float priceGain_AnimalProduceBonus, TradeCurrency currency = TradeCurrency.Silver)
		{
			if (currency == TradeCurrency.Favor)
			{
				return thing.RoyalFavorValue;
			}
			float statValue = thing.GetStatValue(StatDefOf.SellPriceFactor, true);
			float num = thing.MarketValue * 0.6f * priceFactorSell_TraderPriceType * statValue * priceFactorSell_HumanPawn * (1f - Find.Storyteller.difficulty.tradePriceFactorLoss);
			num *= 1f + priceGain_PlayerNegotiator + priceGain_DrugBonus + priceGain_FactionBase + priceGain_AnimalProduceBonus;
			num = Mathf.Max(num, 0.01f);
			if (num > 99.5f)
			{
				num = Mathf.Round(num);
			}
			return num;
		}

		// Token: 0x06006FBE RID: 28606 RVA: 0x00254040 File Offset: 0x00252240
		public static float GetPricePlayerBuy(Thing thing, float priceFactorBuy_TraderPriceType, float priceFactorBuy_JoinAs, float priceGain_PlayerNegotiator, float priceGain_FactionBase)
		{
			float num = thing.MarketValue * 1.4f * priceFactorBuy_TraderPriceType * priceFactorBuy_JoinAs * (1f + Find.Storyteller.difficulty.tradePriceFactorLoss);
			num *= 1f - priceGain_PlayerNegotiator - priceGain_FactionBase;
			num = Mathf.Max(num, 0.5f);
			if (num > 99.5f)
			{
				num = Mathf.Round(num);
			}
			return num;
		}

		// Token: 0x06006FBF RID: 28607 RVA: 0x002540A0 File Offset: 0x002522A0
		public static float TotalMarketValue(IEnumerable<Thing> things)
		{
			if (things == null)
			{
				return 0f;
			}
			float num = 0f;
			foreach (Thing thing in things)
			{
				num += thing.MarketValue * (float)thing.stackCount;
			}
			return num;
		}

		// Token: 0x04003DAD RID: 15789
		public const float MinimumBuyPrice = 0.5f;

		// Token: 0x04003DAE RID: 15790
		public const float MinimumSellPrice = 0.01f;

		// Token: 0x04003DAF RID: 15791
		public const float PriceFactorBuy_Global = 1.4f;

		// Token: 0x04003DB0 RID: 15792
		public const float PriceFactorSell_Global = 0.6f;
	}
}
