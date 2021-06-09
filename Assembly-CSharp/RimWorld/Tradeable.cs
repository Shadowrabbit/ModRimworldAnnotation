using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200191B RID: 6427
	public class Tradeable : Transferable
	{
		// Token: 0x17001658 RID: 5720
		// (get) Token: 0x06008E34 RID: 36404 RVA: 0x0005F314 File Offset: 0x0005D514
		// (set) Token: 0x06008E35 RID: 36405 RVA: 0x0005F31C File Offset: 0x0005D51C
		public override int CountToTransfer
		{
			get
			{
				return this.countToTransfer;
			}
			protected set
			{
				this.countToTransfer = value;
				base.EditBuffer = value.ToStringCached();
			}
		}

		// Token: 0x17001659 RID: 5721
		// (get) Token: 0x06008E36 RID: 36406 RVA: 0x0005F331 File Offset: 0x0005D531
		public Thing FirstThingColony
		{
			get
			{
				if (this.thingsColony.Count == 0)
				{
					return null;
				}
				return this.thingsColony[0];
			}
		}

		// Token: 0x1700165A RID: 5722
		// (get) Token: 0x06008E37 RID: 36407 RVA: 0x0005F34E File Offset: 0x0005D54E
		public Thing FirstThingTrader
		{
			get
			{
				if (this.thingsTrader.Count == 0)
				{
					return null;
				}
				return this.thingsTrader[0];
			}
		}

		// Token: 0x1700165B RID: 5723
		// (get) Token: 0x06008E38 RID: 36408 RVA: 0x0005F36B File Offset: 0x0005D56B
		public override string Label
		{
			get
			{
				return this.AnyThing.LabelNoCount;
			}
		}

		// Token: 0x1700165C RID: 5724
		// (get) Token: 0x06008E39 RID: 36409 RVA: 0x0005F378 File Offset: 0x0005D578
		public virtual float BaseMarketValue
		{
			get
			{
				return this.AnyThing.MarketValue;
			}
		}

		// Token: 0x1700165D RID: 5725
		// (get) Token: 0x06008E3A RID: 36410 RVA: 0x0005F385 File Offset: 0x0005D585
		public override bool Interactive
		{
			get
			{
				return !this.IsCurrency || (TradeSession.Active && TradeSession.giftMode);
			}
		}

		// Token: 0x1700165E RID: 5726
		// (get) Token: 0x06008E3B RID: 36411 RVA: 0x0005F39F File Offset: 0x0005D59F
		public virtual bool TraderWillTrade
		{
			get
			{
				return TradeSession.trader.TraderKind.WillTrade(this.ThingDef);
			}
		}

		// Token: 0x1700165F RID: 5727
		// (get) Token: 0x06008E3C RID: 36412 RVA: 0x0005F3B6 File Offset: 0x0005D5B6
		public override bool HasAnyThing
		{
			get
			{
				return this.FirstThingColony != null || this.FirstThingTrader != null;
			}
		}

		// Token: 0x17001660 RID: 5728
		// (get) Token: 0x06008E3D RID: 36413 RVA: 0x00290C04 File Offset: 0x0028EE04
		public override Thing AnyThing
		{
			get
			{
				if (this.FirstThingColony != null)
				{
					return this.FirstThingColony.GetInnerIfMinified();
				}
				if (this.FirstThingTrader != null)
				{
					return this.FirstThingTrader.GetInnerIfMinified();
				}
				Log.Error(base.GetType() + " lacks AnyThing.", false);
				return null;
			}
		}

		// Token: 0x17001661 RID: 5729
		// (get) Token: 0x06008E3E RID: 36414 RVA: 0x0005F3CB File Offset: 0x0005D5CB
		public override ThingDef ThingDef
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return null;
				}
				return this.AnyThing.def;
			}
		}

		// Token: 0x17001662 RID: 5730
		// (get) Token: 0x06008E3F RID: 36415 RVA: 0x0005F3E2 File Offset: 0x0005D5E2
		public ThingDef StuffDef
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return null;
				}
				return this.AnyThing.Stuff;
			}
		}

		// Token: 0x17001663 RID: 5731
		// (get) Token: 0x06008E40 RID: 36416 RVA: 0x0005F3F9 File Offset: 0x0005D5F9
		public override string TipDescription
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return "";
				}
				return this.AnyThing.DescriptionDetailed;
			}
		}

		// Token: 0x17001664 RID: 5732
		// (get) Token: 0x06008E41 RID: 36417 RVA: 0x0005F414 File Offset: 0x0005D614
		public TradeAction ActionToDo
		{
			get
			{
				if (this.CountToTransfer == 0)
				{
					return TradeAction.None;
				}
				if (base.CountToTransferToDestination > 0)
				{
					return TradeAction.PlayerSells;
				}
				return TradeAction.PlayerBuys;
			}
		}

		// Token: 0x17001665 RID: 5733
		// (get) Token: 0x06008E42 RID: 36418 RVA: 0x0005F42C File Offset: 0x0005D62C
		public virtual bool IsCurrency
		{
			get
			{
				return !this.Bugged && this.ThingDef == ThingDefOf.Silver;
			}
		}

		// Token: 0x17001666 RID: 5734
		// (get) Token: 0x06008E43 RID: 36419 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool IsFavor
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008E44 RID: 36420 RVA: 0x0005F445 File Offset: 0x0005D645
		public virtual int CostToInt(float cost)
		{
			return Mathf.RoundToInt(cost);
		}

		// Token: 0x17001667 RID: 5735
		// (get) Token: 0x06008E45 RID: 36421 RVA: 0x0005F44D File Offset: 0x0005D64D
		public override TransferablePositiveCountDirection PositiveCountDirection
		{
			get
			{
				if (TradeSession.Active && TradeSession.giftMode)
				{
					return TransferablePositiveCountDirection.Destination;
				}
				return TransferablePositiveCountDirection.Source;
			}
		}

		// Token: 0x17001668 RID: 5736
		// (get) Token: 0x06008E46 RID: 36422 RVA: 0x0005F460 File Offset: 0x0005D660
		public float CurTotalCurrencyCostForSource
		{
			get
			{
				if (this.ActionToDo == TradeAction.None)
				{
					return 0f;
				}
				return (float)base.CountToTransferToSource * this.GetPriceFor(this.ActionToDo);
			}
		}

		// Token: 0x17001669 RID: 5737
		// (get) Token: 0x06008E47 RID: 36423 RVA: 0x0005F484 File Offset: 0x0005D684
		public float CurTotalCurrencyCostForDestination
		{
			get
			{
				if (this.ActionToDo == TradeAction.None)
				{
					return 0f;
				}
				return (float)base.CountToTransferToDestination * this.GetPriceFor(this.ActionToDo);
			}
		}

		// Token: 0x1700166A RID: 5738
		// (get) Token: 0x06008E48 RID: 36424 RVA: 0x0005F4A8 File Offset: 0x0005D6A8
		public virtual Window NewInfoDialog
		{
			get
			{
				return new Dialog_InfoCard(this.ThingDef);
			}
		}

		// Token: 0x1700166B RID: 5739
		// (get) Token: 0x06008E49 RID: 36425 RVA: 0x0005F4B5 File Offset: 0x0005D6B5
		private bool Bugged
		{
			get
			{
				if (!this.HasAnyThing)
				{
					Log.ErrorOnce(this.ToString() + " is bugged. There will be no more logs about this.", 162112, false);
					return true;
				}
				return false;
			}
		}

		// Token: 0x06008E4A RID: 36426 RVA: 0x0005F4DD File Offset: 0x0005D6DD
		public Tradeable()
		{
		}

		// Token: 0x06008E4B RID: 36427 RVA: 0x00290C50 File Offset: 0x0028EE50
		public Tradeable(Thing thingColony, Thing thingTrader)
		{
			this.thingsColony.Add(thingColony);
			this.thingsTrader.Add(thingTrader);
		}

		// Token: 0x06008E4C RID: 36428 RVA: 0x0005F511 File Offset: 0x0005D711
		public void AddThing(Thing t, Transactor trans)
		{
			if (trans == Transactor.Colony)
			{
				this.thingsColony.Add(t);
			}
			if (trans == Transactor.Trader)
			{
				this.thingsTrader.Add(t);
			}
		}

		// Token: 0x06008E4D RID: 36429 RVA: 0x0005F532 File Offset: 0x0005D732
		public PriceType PriceTypeFor(TradeAction action)
		{
			return TradeSession.trader.TraderKind.PriceTypeFor(this.ThingDef, action);
		}

		// Token: 0x06008E4E RID: 36430 RVA: 0x00290CA8 File Offset: 0x0028EEA8
		private void InitPriceDataIfNeeded()
		{
			if (this.pricePlayerBuy > 0f)
			{
				return;
			}
			if (this.IsCurrency)
			{
				this.pricePlayerBuy = this.BaseMarketValue;
				this.pricePlayerSell = this.BaseMarketValue;
				return;
			}
			this.priceFactorBuy_TraderPriceType = this.PriceTypeFor(TradeAction.PlayerBuys).PriceMultiplier();
			this.priceFactorSell_TraderPriceType = this.PriceTypeFor(TradeAction.PlayerSells).PriceMultiplier();
			this.priceGain_PlayerNegotiator = TradeSession.playerNegotiator.GetStatValue(StatDefOf.TradePriceImprovement, true);
			this.priceGain_Settlement = TradeSession.trader.TradePriceImprovementOffsetForPlayer;
			this.priceFactorSell_ItemSellPriceFactor = this.AnyThing.GetStatValue(StatDefOf.SellPriceFactor, true);
			this.pricePlayerBuy = TradeUtility.GetPricePlayerBuy(this.AnyThing, this.priceFactorBuy_TraderPriceType, this.priceGain_PlayerNegotiator, this.priceGain_Settlement);
			this.pricePlayerSell = TradeUtility.GetPricePlayerSell(this.AnyThing, this.priceFactorSell_TraderPriceType, this.priceGain_PlayerNegotiator, this.priceGain_Settlement, TradeSession.TradeCurrency);
			if (this.pricePlayerSell >= this.pricePlayerBuy)
			{
				this.pricePlayerSell = this.pricePlayerBuy;
			}
		}

		// Token: 0x06008E4F RID: 36431 RVA: 0x00290DAC File Offset: 0x0028EFAC
		public string GetPriceTooltip(TradeAction action)
		{
			if (!this.HasAnyThing)
			{
				return "";
			}
			this.InitPriceDataIfNeeded();
			string text = (action == TradeAction.PlayerBuys) ? "BuyPriceDesc".Translate() : "SellPriceDesc".Translate();
			if (TradeSession.TradeCurrency != TradeCurrency.Silver)
			{
				return text;
			}
			text += "\n\n";
			text += StatDefOf.MarketValue.LabelCap + ": " + this.BaseMarketValue.ToStringMoney(null);
			if (action == TradeAction.PlayerBuys)
			{
				text += "\n  x " + 1.4f.ToString("F2") + " (" + "Buying".Translate() + ")";
				if (this.priceFactorBuy_TraderPriceType != 1f)
				{
					text += "\n  x " + this.priceFactorBuy_TraderPriceType.ToString("F2") + " (" + "TraderTypePrice".Translate() + ")";
				}
				if (Find.Storyteller.difficultyValues.tradePriceFactorLoss != 0f)
				{
					text += "\n  x " + (1f + Find.Storyteller.difficultyValues.tradePriceFactorLoss).ToString("F2") + " (" + "DifficultyLevel".Translate() + ")";
				}
				text += "\n";
				text += "\n" + "YourNegotiatorBonus".Translate() + ": -" + this.priceGain_PlayerNegotiator.ToStringPercent();
				if (this.priceGain_Settlement != 0f)
				{
					text += "\n" + "TradeWithFactionBaseBonus".Translate() + ": -" + this.priceGain_Settlement.ToStringPercent();
				}
			}
			else
			{
				text += "\n  x " + 0.6f.ToString("F2") + " (" + "Selling".Translate() + ")";
				if (this.priceFactorSell_TraderPriceType != 1f)
				{
					text += "\n  x " + this.priceFactorSell_TraderPriceType.ToString("F2") + " (" + "TraderTypePrice".Translate() + ")";
				}
				if (this.priceFactorSell_ItemSellPriceFactor != 1f)
				{
					text += "\n  x " + this.priceFactorSell_ItemSellPriceFactor.ToString("F2") + " (" + "ItemSellPriceFactor".Translate() + ")";
				}
				if (Find.Storyteller.difficultyValues.tradePriceFactorLoss != 0f)
				{
					text += "\n  x " + (1f - Find.Storyteller.difficultyValues.tradePriceFactorLoss).ToString("F2") + " (" + "DifficultyLevel".Translate() + ")";
				}
				text += "\n";
				text += "\n" + "YourNegotiatorBonus".Translate() + ": " + this.priceGain_PlayerNegotiator.ToStringPercent();
				if (this.priceGain_Settlement != 0f)
				{
					text += "\n" + "TradeWithFactionBaseBonus".Translate() + ": " + this.priceGain_Settlement.ToStringPercent();
				}
			}
			text += "\n\n";
			float priceFor = this.GetPriceFor(action);
			text += "FinalPrice".Translate() + ": " + priceFor.ToStringMoney(null);
			if ((action == TradeAction.PlayerBuys && priceFor <= 0.5f) || (action == TradeAction.PlayerBuys && priceFor <= 0.01f))
			{
				text += " (" + "minimum".Translate() + ")";
			}
			return text;
		}

		// Token: 0x06008E50 RID: 36432 RVA: 0x0005F54A File Offset: 0x0005D74A
		public virtual float GetPriceFor(TradeAction action)
		{
			this.InitPriceDataIfNeeded();
			if (action == TradeAction.PlayerBuys)
			{
				return this.pricePlayerBuy;
			}
			return this.pricePlayerSell;
		}

		// Token: 0x06008E51 RID: 36433 RVA: 0x0005F563 File Offset: 0x0005D763
		public override int GetMinimumToTransfer()
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Destination)
			{
				return -this.CountHeldBy(Transactor.Trader);
			}
			return -this.CountHeldBy(Transactor.Colony);
		}

		// Token: 0x06008E52 RID: 36434 RVA: 0x0005F57F File Offset: 0x0005D77F
		public override int GetMaximumToTransfer()
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Destination)
			{
				return this.CountHeldBy(Transactor.Colony);
			}
			return this.CountHeldBy(Transactor.Trader);
		}

		// Token: 0x06008E53 RID: 36435 RVA: 0x0005F599 File Offset: 0x0005D799
		public override AcceptanceReport UnderflowReport()
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Destination)
			{
				return new AcceptanceReport("TraderHasNoMore".Translate());
			}
			return new AcceptanceReport("ColonyHasNoMore".Translate());
		}

		// Token: 0x06008E54 RID: 36436 RVA: 0x0005F5CD File Offset: 0x0005D7CD
		public override AcceptanceReport OverflowReport()
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Destination)
			{
				return new AcceptanceReport("ColonyHasNoMore".Translate());
			}
			return new AcceptanceReport("TraderHasNoMore".Translate());
		}

		// Token: 0x06008E55 RID: 36437 RVA: 0x0005F601 File Offset: 0x0005D801
		private List<Thing> TransactorThings(Transactor trans)
		{
			if (trans == Transactor.Colony)
			{
				return this.thingsColony;
			}
			return this.thingsTrader;
		}

		// Token: 0x06008E56 RID: 36438 RVA: 0x00291238 File Offset: 0x0028F438
		public virtual int CountHeldBy(Transactor trans)
		{
			List<Thing> list = this.TransactorThings(trans);
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				num += list[i].stackCount;
			}
			return num;
		}

		// Token: 0x06008E57 RID: 36439 RVA: 0x0005F613 File Offset: 0x0005D813
		public int CountPostDealFor(Transactor trans)
		{
			if (trans == Transactor.Colony)
			{
				return this.CountHeldBy(trans) + base.CountToTransferToSource;
			}
			return this.CountHeldBy(trans) + base.CountToTransferToDestination;
		}

		// Token: 0x06008E58 RID: 36440 RVA: 0x00291270 File Offset: 0x0028F470
		public virtual void ResolveTrade()
		{
			if (this.ActionToDo == TradeAction.PlayerSells)
			{
				TransferableUtility.TransferNoSplit(this.thingsColony, base.CountToTransferToDestination, delegate(Thing thing, int countToTransfer)
				{
					TradeSession.trader.GiveSoldThingToTrader(thing, countToTransfer, TradeSession.playerNegotiator);
				}, true, true);
				return;
			}
			if (this.ActionToDo == TradeAction.PlayerBuys)
			{
				TransferableUtility.TransferNoSplit(this.thingsTrader, base.CountToTransferToSource, delegate(Thing thing, int countToTransfer)
				{
					this.CheckTeachOpportunity(thing, countToTransfer);
					TradeSession.trader.GiveSoldThingToPlayer(thing, countToTransfer, TradeSession.playerNegotiator);
				}, true, true);
			}
		}

		// Token: 0x06008E59 RID: 36441 RVA: 0x002912E4 File Offset: 0x0028F4E4
		private void CheckTeachOpportunity(Thing boughtThing, int boughtCount)
		{
			Building building = boughtThing as Building;
			if (building == null)
			{
				MinifiedThing minifiedThing = boughtThing as MinifiedThing;
				if (minifiedThing != null)
				{
					building = (minifiedThing.InnerThing as Building);
				}
			}
			if (building != null && building.def.building != null && building.def.building.boughtConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(building.def.building.boughtConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
		}

		// Token: 0x06008E5A RID: 36442 RVA: 0x0029134C File Offset: 0x0028F54C
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				base.GetType(),
				"(",
				this.ThingDef,
				", countToTransfer=",
				this.CountToTransfer,
				")"
			});
		}

		// Token: 0x06008E5B RID: 36443 RVA: 0x0005F635 File Offset: 0x0005D835
		public override int GetHashCode()
		{
			return this.AnyThing.GetHashCode();
		}

		// Token: 0x06008E5C RID: 36444 RVA: 0x0029139C File Offset: 0x0028F59C
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.thingsColony.RemoveAll((Thing x) => x.Destroyed);
				this.thingsTrader.RemoveAll((Thing x) => x.Destroyed);
			}
			Scribe_Values.Look<int>(ref this.countToTransfer, "countToTransfer", 0, false);
			Scribe_Collections.Look<Thing>(ref this.thingsColony, "thingsColony", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.thingsTrader, "thingsTrader", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				base.EditBuffer = this.countToTransfer.ToStringCached();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.thingsColony.RemoveAll((Thing x) => x == null) == 0)
				{
					if (this.thingsTrader.RemoveAll((Thing x) => x == null) == 0)
					{
						return;
					}
				}
				Log.Warning("Some of the things were null after loading.", false);
			}
		}

		// Token: 0x04005AC8 RID: 23240
		public List<Thing> thingsColony = new List<Thing>();

		// Token: 0x04005AC9 RID: 23241
		public List<Thing> thingsTrader = new List<Thing>();

		// Token: 0x04005ACA RID: 23242
		private int countToTransfer;

		// Token: 0x04005ACB RID: 23243
		private float pricePlayerBuy = -1f;

		// Token: 0x04005ACC RID: 23244
		private float pricePlayerSell = -1f;

		// Token: 0x04005ACD RID: 23245
		private float priceFactorBuy_TraderPriceType;

		// Token: 0x04005ACE RID: 23246
		private float priceFactorSell_TraderPriceType;

		// Token: 0x04005ACF RID: 23247
		private float priceFactorSell_ItemSellPriceFactor;

		// Token: 0x04005AD0 RID: 23248
		private float priceGain_PlayerNegotiator;

		// Token: 0x04005AD1 RID: 23249
		private float priceGain_Settlement;
	}
}
