using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200192C RID: 6444
	public static class TradeUtility
	{
		// Token: 0x06008EC5 RID: 36549 RVA: 0x002923C0 File Offset: 0x002905C0
		public static bool EverPlayerSellable(ThingDef def)
		{
			return def.tradeability.PlayerCanSell() && def.GetStatValueAbstract(StatDefOf.MarketValue, null) > 0f && (def.category == ThingCategory.Item || def.category == ThingCategory.Pawn || def.category == ThingCategory.Building) && (def.category != ThingCategory.Building || def.Minifiable);
		}

		// Token: 0x06008EC6 RID: 36550 RVA: 0x00292424 File Offset: 0x00290624
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
			if (EquipmentUtility.IsBiocoded(t))
			{
				return false;
			}
			Pawn pawn = t as Pawn;
			return pawn == null || ((pawn.GetExtraHostFaction(null) == null || pawn.GetExtraHostFaction(null) != trader.Faction) && (!pawn.IsQuestLodger() || pawn.GetExtraHomeFaction(null) != trader.Faction));
		}

		// Token: 0x06008EC7 RID: 36551 RVA: 0x002924AC File Offset: 0x002906AC
		public static void SpawnDropPod(IntVec3 dropSpot, Map map, Thing t)
		{
			DropPodUtility.MakeDropPodAt(dropSpot, map, new ActiveDropPodInfo
			{
				SingleContainedThing = t,
				leaveSlag = false
			});
		}

		// Token: 0x06008EC8 RID: 36552 RVA: 0x0005F9ED File Offset: 0x0005DBED
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

		// Token: 0x06008EC9 RID: 36553 RVA: 0x0005FA04 File Offset: 0x0005DC04
		public static IEnumerable<Pawn> AllSellableColonyPawns(Map map)
		{
			foreach (Pawn pawn in map.mapPawns.PrisonersOfColonySpawned)
			{
				if (pawn.guest.PrisonerIsSecure)
				{
					yield return pawn;
				}
			}
			List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
			foreach (Pawn pawn2 in map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer))
			{
				if (pawn2.RaceProps.Animal && pawn2.HostFaction == null && !pawn2.InMentalState && !pawn2.Downed && map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(pawn2.def))
				{
					yield return pawn2;
				}
			}
			enumerator = default(List<Pawn>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06008ECA RID: 36554 RVA: 0x002924D8 File Offset: 0x002906D8
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

		// Token: 0x06008ECB RID: 36555 RVA: 0x00292550 File Offset: 0x00290750
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
					Log.Error("Could not find any " + resDef + " to transfer to trader.", false);
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

		// Token: 0x06008ECC RID: 36556 RVA: 0x0005FA14 File Offset: 0x0005DC14
		public static void LaunchSilver(Map map, int fee)
		{
			TradeUtility.LaunchThingsOfType(ThingDefOf.Silver, fee, map, null);
		}

		// Token: 0x06008ECD RID: 36557 RVA: 0x00292674 File Offset: 0x00290874
		public static Map PlayerHomeMapWithMostLaunchableSilver()
		{
			return (from x in Find.Maps
			where x.IsPlayerHome
			select x).MaxBy((Map x) => (from t in TradeUtility.AllLaunchableThingsForTrade(x, null)
			where t.def == ThingDefOf.Silver
			select t).Sum((Thing t) => t.stackCount));
		}

		// Token: 0x06008ECE RID: 36558 RVA: 0x002926D0 File Offset: 0x002908D0
		public static bool ColonyHasEnoughSilver(Map map, int fee)
		{
			return (from t in TradeUtility.AllLaunchableThingsForTrade(map, null)
			where t.def == ThingDefOf.Silver
			select t).Sum((Thing t) => t.stackCount) >= fee;
		}

		// Token: 0x06008ECF RID: 36559 RVA: 0x00292734 File Offset: 0x00290934
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

		// Token: 0x06008ED0 RID: 36560 RVA: 0x00292770 File Offset: 0x00290970
		public static float GetPricePlayerSell(Thing thing, float priceFactorSell_TraderPriceType, float priceGain_PlayerNegotiator, float priceGain_FactionBase, TradeCurrency currency = TradeCurrency.Silver)
		{
			if (currency == TradeCurrency.Favor)
			{
				return thing.RoyalFavorValue;
			}
			float statValue = thing.GetStatValue(StatDefOf.SellPriceFactor, true);
			float num = thing.MarketValue * 0.6f * priceFactorSell_TraderPriceType * statValue * (1f - Find.Storyteller.difficultyValues.tradePriceFactorLoss);
			num *= 1f + priceGain_PlayerNegotiator + priceGain_FactionBase;
			num = Mathf.Max(num, 0.01f);
			if (num > 99.5f)
			{
				num = Mathf.Round(num);
			}
			return num;
		}

		// Token: 0x06008ED1 RID: 36561 RVA: 0x002927E8 File Offset: 0x002909E8
		public static float GetPricePlayerBuy(Thing thing, float priceFactorBuy_TraderPriceType, float priceGain_PlayerNegotiator, float priceGain_FactionBase)
		{
			float num = thing.MarketValue * 1.4f * priceFactorBuy_TraderPriceType * (1f + Find.Storyteller.difficultyValues.tradePriceFactorLoss);
			num *= 1f - priceGain_PlayerNegotiator - priceGain_FactionBase;
			num = Mathf.Max(num, 0.5f);
			if (num > 99.5f)
			{
				num = Mathf.Round(num);
			}
			return num;
		}

		// Token: 0x04005B04 RID: 23300
		public const float MinimumBuyPrice = 0.5f;

		// Token: 0x04005B05 RID: 23301
		public const float MinimumSellPrice = 0.01f;

		// Token: 0x04005B06 RID: 23302
		public const float PriceFactorBuy_Global = 1.4f;

		// Token: 0x04005B07 RID: 23303
		public const float PriceFactorSell_Global = 0.6f;
	}
}
