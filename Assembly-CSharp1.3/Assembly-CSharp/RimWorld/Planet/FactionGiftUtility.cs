using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld.Planet
{
	// Token: 0x02001732 RID: 5938
	[StaticConstructorOnStartup]
	public static class FactionGiftUtility
	{
		// Token: 0x060088F4 RID: 35060 RVA: 0x00313898 File Offset: 0x00311A98
		public static Command OfferGiftsCommand(Caravan caravan, Settlement settlement)
		{
			return new Command_Action
			{
				defaultLabel = "CommandOfferGifts".Translate(),
				defaultDesc = "CommandOfferGiftsDesc".Translate(),
				icon = FactionGiftUtility.OfferGiftsCommandTex,
				action = delegate()
				{
					Pawn playerNegotiator = BestCaravanPawnUtility.FindBestNegotiator(caravan, null, null);
					Find.WindowStack.Add(new Dialog_Trade(playerNegotiator, settlement, true));
				}
			};
		}

		// Token: 0x060088F5 RID: 35061 RVA: 0x00313908 File Offset: 0x00311B08
		public static void GiveGift(List<Tradeable> tradeables, Faction giveTo, GlobalTargetInfo lookTarget)
		{
			int goodwillChange = FactionGiftUtility.GetGoodwillChange(tradeables, giveTo);
			for (int i = 0; i < tradeables.Count; i++)
			{
				if (tradeables[i].ActionToDo == TradeAction.PlayerSells)
				{
					tradeables[i].ResolveTrade();
				}
			}
			if (giveTo.PlayerGoodwill == 100)
			{
				FactionGiftUtility.SendGiftNotAppreciatedMessage(giveTo, lookTarget);
			}
			Faction.OfPlayer.TryAffectGoodwillWith(giveTo, goodwillChange, true, true, HistoryEventDefOf.GaveGift, null);
		}

		// Token: 0x060088F6 RID: 35062 RVA: 0x00313978 File Offset: 0x00311B78
		public static void GiveGift(List<ActiveDropPodInfo> pods, Settlement giveTo)
		{
			int goodwillChange = FactionGiftUtility.GetGoodwillChange(pods.Cast<IThingHolder>(), giveTo);
			for (int i = 0; i < pods.Count; i++)
			{
				ThingOwner innerContainer = pods[i].innerContainer;
				for (int j = innerContainer.Count - 1; j >= 0; j--)
				{
					FactionGiftUtility.GiveGiftInternal(innerContainer[j], innerContainer[j].stackCount, giveTo.Faction);
					if (j < innerContainer.Count)
					{
						innerContainer.RemoveAt(j);
					}
				}
			}
			if (giveTo.Faction.PlayerGoodwill == 100)
			{
				FactionGiftUtility.SendGiftNotAppreciatedMessage(giveTo.Faction, giveTo);
			}
			Faction.OfPlayer.TryAffectGoodwillWith(giveTo.Faction, goodwillChange, true, true, HistoryEventDefOf.GaveGift, null);
		}

		// Token: 0x060088F7 RID: 35063 RVA: 0x00313A34 File Offset: 0x00311C34
		private static void GiveGiftInternal(Thing thing, int count, Faction giveTo)
		{
			Thing thing2 = thing.SplitOff(count);
			Pawn pawn;
			if ((pawn = (thing2 as Pawn)) != null)
			{
				pawn.SetFaction(giveTo, null);
				Pawn_GuestTracker guest = pawn.guest;
				if (guest != null)
				{
					guest.SetGuestStatus(null, GuestStatus.Guest);
				}
			}
			thing2.DestroyOrPassToWorld(DestroyMode.Vanish);
		}

		// Token: 0x060088F8 RID: 35064 RVA: 0x00313A74 File Offset: 0x00311C74
		public static bool CheckCanCarryGift(List<Tradeable> tradeables, ITrader trader)
		{
			Pawn pawn = trader as Pawn;
			if (pawn == null)
			{
				return true;
			}
			float num = 0f;
			float num2 = 0f;
			Lord lord = pawn.GetLord();
			if (lord != null)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn pawn2 = lord.ownedPawns[i];
					TraderCaravanRole traderCaravanRole = pawn2.GetTraderCaravanRole();
					if ((pawn2.RaceProps.Humanlike && traderCaravanRole != TraderCaravanRole.Guard) || traderCaravanRole == TraderCaravanRole.Carrier)
					{
						num += MassUtility.Capacity(pawn2, null);
						num2 += MassUtility.GearAndInventoryMass(pawn2);
					}
				}
			}
			else
			{
				num = MassUtility.Capacity(pawn, null);
				num2 = MassUtility.GearAndInventoryMass(pawn);
			}
			float num3 = 0f;
			for (int j = 0; j < tradeables.Count; j++)
			{
				if (tradeables[j].ActionToDo == TradeAction.PlayerSells)
				{
					int num4 = Mathf.Min(tradeables[j].CountToTransferToDestination, tradeables[j].CountHeldBy(Transactor.Colony));
					if (num4 > 0)
					{
						num3 += tradeables[j].AnyThing.GetStatValue(StatDefOf.Mass, true) * (float)num4;
					}
				}
			}
			if (num2 + num3 <= num)
			{
				return true;
			}
			float num5 = num - num2;
			if (num5 <= 0f)
			{
				Messages.Message("MessageCantGiveGiftBecauseCantCarryEncumbered".Translate(), MessageTypeDefOf.RejectInput, false);
			}
			else
			{
				Messages.Message("MessageCantGiveGiftBecauseCantCarry".Translate(num3.ToStringMass(), num5.ToStringMass()), MessageTypeDefOf.RejectInput, false);
			}
			return false;
		}

		// Token: 0x060088F9 RID: 35065 RVA: 0x00313BF0 File Offset: 0x00311DF0
		public static int GetGoodwillChange(IEnumerable<IThingHolder> pods, Settlement giveTo)
		{
			float num = 0f;
			foreach (IThingHolder thingHolder in pods)
			{
				ThingOwner directlyHeldThings = thingHolder.GetDirectlyHeldThings();
				for (int i = 0; i < directlyHeldThings.Count; i++)
				{
					float singlePrice;
					if (directlyHeldThings[i].def == ThingDefOf.Silver)
					{
						singlePrice = directlyHeldThings[i].MarketValue;
					}
					else
					{
						float priceFactorSell_TraderPriceType = (giveTo.TraderKind != null) ? giveTo.TraderKind.PriceTypeFor(directlyHeldThings[i].def, TradeAction.PlayerSells).PriceMultiplier() : 1f;
						float tradePriceImprovementOffsetForPlayer = giveTo.TradePriceImprovementOffsetForPlayer;
						singlePrice = TradeUtility.GetPricePlayerSell(directlyHeldThings[i], priceFactorSell_TraderPriceType, 1f, 1f, tradePriceImprovementOffsetForPlayer, 1f, 1f, TradeCurrency.Silver);
					}
					num += FactionGiftUtility.GetBaseGoodwillChange(directlyHeldThings[i], directlyHeldThings[i].stackCount, singlePrice, giveTo.Faction);
				}
			}
			return FactionGiftUtility.PostProcessedGoodwillChange(num, giveTo.Faction);
		}

		// Token: 0x060088FA RID: 35066 RVA: 0x00313D08 File Offset: 0x00311F08
		public static int GetGoodwillChange(List<Tradeable> tradeables, Faction theirFaction)
		{
			float num = 0f;
			for (int i = 0; i < tradeables.Count; i++)
			{
				if (tradeables[i].ActionToDo == TradeAction.PlayerSells)
				{
					int count = Mathf.Min(tradeables[i].CountToTransferToDestination, tradeables[i].CountHeldBy(Transactor.Colony));
					num += FactionGiftUtility.GetBaseGoodwillChange(tradeables[i].AnyThing, count, tradeables[i].GetPriceFor(TradeAction.PlayerSells), theirFaction);
				}
			}
			return FactionGiftUtility.PostProcessedGoodwillChange(num, theirFaction);
		}

		// Token: 0x060088FB RID: 35067 RVA: 0x00313D84 File Offset: 0x00311F84
		private static float GetBaseGoodwillChange(Thing anyThing, int count, float singlePrice, Faction theirFaction)
		{
			if (count <= 0)
			{
				return 0f;
			}
			float num = singlePrice * (float)count;
			Pawn pawn = anyThing as Pawn;
			if (pawn != null && pawn.IsPrisoner && pawn.Faction == theirFaction)
			{
				num *= 2f;
			}
			return num / 40f;
		}

		// Token: 0x060088FC RID: 35068 RVA: 0x00313DCC File Offset: 0x00311FCC
		private static int PostProcessedGoodwillChange(float goodwillChange, Faction theirFaction)
		{
			float num = (float)theirFaction.PlayerGoodwill;
			float num2 = 0f;
			SimpleCurve giftGoodwillFactorRelationsCurve = DiplomacyTuning.GiftGoodwillFactorRelationsCurve;
			while (goodwillChange >= 0.25f)
			{
				num2 += 0.25f * giftGoodwillFactorRelationsCurve.Evaluate(Mathf.Min(num + num2, 100f));
				goodwillChange -= 0.25f;
				if (num2 >= 200f)
				{
					break;
				}
			}
			if (num2 < 200f)
			{
				num2 += goodwillChange * giftGoodwillFactorRelationsCurve.Evaluate(Mathf.Min(num + num2, 100f));
			}
			return (int)Mathf.Min(num2, 200f);
		}

		// Token: 0x060088FD RID: 35069 RVA: 0x00313E50 File Offset: 0x00312050
		private static void SendGiftNotAppreciatedMessage(Faction giveTo, GlobalTargetInfo lookTarget)
		{
			Messages.Message("MessageGiftGivenButNotAppreciated".Translate(giveTo.Name), lookTarget, MessageTypeDefOf.NegativeEvent, true);
		}

		// Token: 0x040056FA RID: 22266
		private static readonly Texture2D OfferGiftsCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/OfferGifts", true);
	}
}
