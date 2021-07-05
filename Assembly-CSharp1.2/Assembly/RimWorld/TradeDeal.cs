using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001921 RID: 6433
	public class TradeDeal
	{
		// Token: 0x17001678 RID: 5752
		// (get) Token: 0x06008E79 RID: 36473 RVA: 0x0005F74E File Offset: 0x0005D94E
		public int TradeableCount
		{
			get
			{
				return this.tradeables.Count;
			}
		}

		// Token: 0x17001679 RID: 5753
		// (get) Token: 0x06008E7A RID: 36474 RVA: 0x00291664 File Offset: 0x0028F864
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

		// Token: 0x1700167A RID: 5754
		// (get) Token: 0x06008E7B RID: 36475 RVA: 0x0005F75B File Offset: 0x0005D95B
		public List<Tradeable> AllTradeables
		{
			get
			{
				return this.tradeables;
			}
		}

		// Token: 0x06008E7C RID: 36476 RVA: 0x0005F763 File Offset: 0x0005D963
		public TradeDeal()
		{
			this.Reset();
		}

		// Token: 0x06008E7D RID: 36477 RVA: 0x0005F787 File Offset: 0x0005D987
		public void Reset()
		{
			this.tradeables.Clear();
			this.cannotSellReasons.Clear();
			this.AddAllTradeables();
		}

		// Token: 0x06008E7E RID: 36478 RVA: 0x002916CC File Offset: 0x0028F8CC
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

		// Token: 0x06008E7F RID: 36479 RVA: 0x00291818 File Offset: 0x0028FA18
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
			Room room = t.GetRoom(RegionType.Set_Passable);
			if (room != null)
			{
				int num = GenRadial.NumCellsInRadius(6.9f);
				for (int i = 0; i < num; i++)
				{
					IntVec3 intVec = t.Position + GenRadial.RadialPattern[i];
					if (intVec.InBounds(t.Map) && intVec.GetRoom(t.Map, RegionType.Set_Passable) == room)
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

		// Token: 0x06008E80 RID: 36480 RVA: 0x002918E0 File Offset: 0x0028FAE0
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

		// Token: 0x06008E81 RID: 36481 RVA: 0x00291928 File Offset: 0x0028FB28
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

		// Token: 0x06008E82 RID: 36482 RVA: 0x00291998 File Offset: 0x0028FB98
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

		// Token: 0x06008E83 RID: 36483 RVA: 0x0005F7A5 File Offset: 0x0005D9A5
		public bool DoesTraderHaveEnoughSilver()
		{
			return TradeSession.giftMode || (this.CurrencyTradeable != null && this.CurrencyTradeable.CountPostDealFor(Transactor.Trader) >= 0);
		}

		// Token: 0x06008E84 RID: 36484 RVA: 0x00291B2C File Offset: 0x0028FD2C
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

		// Token: 0x04005ADE RID: 23262
		private List<Tradeable> tradeables = new List<Tradeable>();

		// Token: 0x04005ADF RID: 23263
		public List<string> cannotSellReasons = new List<string>();
	}
}
