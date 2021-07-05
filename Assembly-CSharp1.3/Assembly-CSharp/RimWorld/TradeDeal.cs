using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200122E RID: 4654
	public class TradeDeal
	{
		// Token: 0x17001359 RID: 4953
		// (get) Token: 0x06006F83 RID: 28547 RVA: 0x00252F17 File Offset: 0x00251117
		public int TradeableCount
		{
			get
			{
				return this.tradeables.Count;
			}
		}

		// Token: 0x1700135A RID: 4954
		// (get) Token: 0x06006F84 RID: 28548 RVA: 0x00252F24 File Offset: 0x00251124
		public Tradeable CurrencyTradeable
		{
			get
			{
				for (int i = 0; i < this.tradeables.Count; i++)
				{
					if ((TradeSession.TradeCurrency == TradeCurrency.Favor) ? this.tradeables[i].IsFavor : (this.tradeables[i].ThingDef == ThingDefOf.Silver))
					{
						return this.tradeables[i];
					}
				}
				return null;
			}
		}

		// Token: 0x1700135B RID: 4955
		// (get) Token: 0x06006F85 RID: 28549 RVA: 0x00252F8A File Offset: 0x0025118A
		public List<Tradeable> AllTradeables
		{
			get
			{
				return this.tradeables;
			}
		}

		// Token: 0x06006F86 RID: 28550 RVA: 0x00252F92 File Offset: 0x00251192
		public TradeDeal()
		{
			this.Reset();
		}

		// Token: 0x06006F87 RID: 28551 RVA: 0x00252FB6 File Offset: 0x002511B6
		public void Reset()
		{
			this.tradeables.Clear();
			this.cannotSellReasons.Clear();
			this.AddAllTradeables();
		}

		// Token: 0x06006F88 RID: 28552 RVA: 0x00252FD4 File Offset: 0x002511D4
		private void AddAllTradeables()
		{
			foreach (Thing t in TradeSession.trader.ColonyThingsWillingToBuy(TradeSession.playerNegotiator))
			{
				if (TradeUtility.PlayerSellableNow(t, TradeSession.trader))
				{
					string text;
					if (!TradeSession.playerNegotiator.IsWorldPawn() && !this.InSellablePosition(t, out text))
					{
						if (text != null && !this.cannotSellReasons.Contains(text))
						{
							this.cannotSellReasons.Add(text);
						}
					}
					else
					{
						this.AddToTradeables(t, Transactor.Colony);
					}
				}
			}
			if (!TradeSession.giftMode)
			{
				foreach (Thing t2 in TradeSession.trader.Goods)
				{
					this.AddToTradeables(t2, Transactor.Trader);
				}
			}
			if (!TradeSession.giftMode)
			{
				if (this.tradeables.Find((Tradeable x) => x.IsCurrency) == null)
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.Silver, null);
					thing.stackCount = 0;
					this.AddToTradeables(thing, Transactor.Trader);
				}
			}
			if (TradeSession.TradeCurrency == TradeCurrency.Favor)
			{
				this.tradeables.Add(new Tradeable_RoyalFavor());
			}
		}

		// Token: 0x06006F89 RID: 28553 RVA: 0x00253120 File Offset: 0x00251320
		private bool InSellablePosition(Thing t, out string reason)
		{
			if (!t.Spawned)
			{
				reason = null;
				return false;
			}
			if (t.Position.Fogged(t.Map))
			{
				reason = null;
				return false;
			}
			Room room = t.GetRoom(RegionType.Set_All);
			if (room != null)
			{
				int num = GenRadial.NumCellsInRadius(6.9f);
				for (int i = 0; i < num; i++)
				{
					IntVec3 intVec = t.Position + GenRadial.RadialPattern[i];
					if (intVec.InBounds(t.Map) && intVec.GetRoom(t.Map) == room)
					{
						List<Thing> thingList = intVec.GetThingList(t.Map);
						for (int j = 0; j < thingList.Count; j++)
						{
							if (thingList[j].PreventPlayerSellingThingsNearby(out reason))
							{
								return false;
							}
						}
					}
				}
			}
			reason = null;
			return true;
		}

		// Token: 0x06006F8A RID: 28554 RVA: 0x002531E8 File Offset: 0x002513E8
		private void AddToTradeables(Thing t, Transactor trans)
		{
			Tradeable tradeable = TransferableUtility.TradeableMatching(t, this.tradeables);
			if (tradeable == null)
			{
				if (t is Pawn)
				{
					tradeable = new Tradeable_Pawn();
				}
				else
				{
					tradeable = new Tradeable();
				}
				this.tradeables.Add(tradeable);
			}
			tradeable.AddThing(t, trans);
		}

		// Token: 0x06006F8B RID: 28555 RVA: 0x00253230 File Offset: 0x00251430
		public void UpdateCurrencyCount()
		{
			if (this.CurrencyTradeable == null || TradeSession.giftMode)
			{
				return;
			}
			float num = 0f;
			for (int i = 0; i < this.tradeables.Count; i++)
			{
				Tradeable tradeable = this.tradeables[i];
				if (!tradeable.IsCurrency)
				{
					num += tradeable.CurTotalCurrencyCostForSource;
				}
			}
			this.CurrencyTradeable.ForceToSource(-this.CurrencyTradeable.CostToInt(num));
		}

		// Token: 0x06006F8C RID: 28556 RVA: 0x002532A0 File Offset: 0x002514A0
		public bool TryExecute(out bool actuallyTraded)
		{
			if (TradeSession.giftMode)
			{
				this.UpdateCurrencyCount();
				this.LimitCurrencyCountToFunds();
				int goodwillChange = FactionGiftUtility.GetGoodwillChange(this.tradeables, TradeSession.trader.Faction);
				FactionGiftUtility.GiveGift(this.tradeables, TradeSession.trader.Faction, TradeSession.playerNegotiator);
				actuallyTraded = ((float)goodwillChange > 0f);
				return true;
			}
			if (this.CurrencyTradeable == null || this.CurrencyTradeable.CountPostDealFor(Transactor.Colony) < 0)
			{
				Find.WindowStack.WindowOfType<Dialog_Trade>().FlashSilver();
				Messages.Message("MessageColonyCannotAfford".Translate(), MessageTypeDefOf.RejectInput, false);
				actuallyTraded = false;
				return false;
			}
			if (ModsConfig.IdeologyActive)
			{
				if (this.tradeables.Any((Tradeable x) => x.ActionToDo != TradeAction.None && x.ThingDef != null && x.ThingDef.IsNaturalOrgan))
				{
					HistoryEvent historyEvent = new HistoryEvent(HistoryEventDefOf.TradedOrgan, TradeSession.playerNegotiator.Named(HistoryEventArgsNames.Doer));
					if (!historyEvent.Notify_PawnAboutToDo("MessagePawnUnwillingToDoDueToIdeo"))
					{
						actuallyTraded = false;
						return false;
					}
					Find.HistoryEventsManager.RecordEvent(historyEvent, true);
				}
				if (this.tradeables.Any((Tradeable x) => x.ActionToDo == TradeAction.PlayerSells && x.ThingDef != null && x.ThingDef.IsNaturalOrgan))
				{
					HistoryEvent historyEvent2 = new HistoryEvent(HistoryEventDefOf.SoldOrgan, TradeSession.playerNegotiator.Named(HistoryEventArgsNames.Doer));
					if (!historyEvent2.Notify_PawnAboutToDo("MessagePawnUnwillingToDoDueToIdeo"))
					{
						actuallyTraded = false;
						return false;
					}
					Find.HistoryEventsManager.RecordEvent(historyEvent2, true);
				}
			}
			this.UpdateCurrencyCount();
			this.LimitCurrencyCountToFunds();
			actuallyTraded = false;
			float num = 0f;
			foreach (Tradeable tradeable in this.tradeables)
			{
				if (tradeable.ActionToDo != TradeAction.None)
				{
					actuallyTraded = true;
				}
				if (tradeable.ActionToDo == TradeAction.PlayerSells)
				{
					num += tradeable.CurTotalCurrencyCostForDestination;
				}
				tradeable.ResolveTrade();
			}
			this.Reset();
			if (TradeSession.trader.Faction != null)
			{
				TradeSession.trader.Faction.Notify_PlayerTraded(num, TradeSession.playerNegotiator);
			}
			Pawn pawn = TradeSession.trader as Pawn;
			if (pawn != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.TradedWith, new object[]
				{
					TradeSession.playerNegotiator,
					pawn
				});
			}
			if (actuallyTraded)
			{
				TradeSession.playerNegotiator.mindState.inspirationHandler.EndInspiration(InspirationDefOf.Inspired_Trade);
			}
			return true;
		}

		// Token: 0x06006F8D RID: 28557 RVA: 0x0025350C File Offset: 0x0025170C
		public bool DoesTraderHaveEnoughSilver()
		{
			return TradeSession.giftMode || (this.CurrencyTradeable != null && this.CurrencyTradeable.CountPostDealFor(Transactor.Trader) >= 0);
		}

		// Token: 0x06006F8E RID: 28558 RVA: 0x00253534 File Offset: 0x00251734
		private void LimitCurrencyCountToFunds()
		{
			if (this.CurrencyTradeable == null)
			{
				return;
			}
			if (this.CurrencyTradeable.CountToTransferToSource > this.CurrencyTradeable.CountHeldBy(Transactor.Trader))
			{
				this.CurrencyTradeable.ForceToSource(this.CurrencyTradeable.CountHeldBy(Transactor.Trader));
			}
			if (this.CurrencyTradeable.CountToTransferToDestination > this.CurrencyTradeable.CountHeldBy(Transactor.Colony))
			{
				this.CurrencyTradeable.ForceToDestination(this.CurrencyTradeable.CountHeldBy(Transactor.Colony));
			}
		}

		// Token: 0x04003D9D RID: 15773
		private List<Tradeable> tradeables = new List<Tradeable>();

		// Token: 0x04003D9E RID: 15774
		public List<string> cannotSellReasons = new List<string>();
	}
}
