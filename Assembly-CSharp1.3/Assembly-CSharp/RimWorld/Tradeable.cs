using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001235 RID: 4661
	public class Tradeable : Transferable
	{
		// Token: 0x17001368 RID: 4968
		// (get) Token: 0x06006FC0 RID: 28608 RVA: 0x00254104 File Offset: 0x00252304
		// (set) Token: 0x06006FC1 RID: 28609 RVA: 0x0025410C File Offset: 0x0025230C
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

		// Token: 0x17001369 RID: 4969
		// (get) Token: 0x06006FC2 RID: 28610 RVA: 0x00254121 File Offset: 0x00252321
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

		// Token: 0x1700136A RID: 4970
		// (get) Token: 0x06006FC3 RID: 28611 RVA: 0x0025413E File Offset: 0x0025233E
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

		// Token: 0x1700136B RID: 4971
		// (get) Token: 0x06006FC4 RID: 28612 RVA: 0x0025415B File Offset: 0x0025235B
		public override string Label
		{
			get
			{
				return this.AnyThing.LabelNoCount;
			}
		}

		// Token: 0x1700136C RID: 4972
		// (get) Token: 0x06006FC5 RID: 28613 RVA: 0x00254168 File Offset: 0x00252368
		public virtual float BaseMarketValue
		{
			get
			{
				return this.AnyThing.MarketValue;
			}
		}

		// Token: 0x1700136D RID: 4973
		// (get) Token: 0x06006FC6 RID: 28614 RVA: 0x00254175 File Offset: 0x00252375
		public override bool Interactive
		{
			get
			{
				return !this.IsCurrency || (TradeSession.Active && TradeSession.giftMode);
			}
		}

		// Token: 0x1700136E RID: 4974
		// (get) Token: 0x06006FC7 RID: 28615 RVA: 0x0025418F File Offset: 0x0025238F
		public virtual bool TraderWillTrade
		{
			get
			{
				return TradeSession.trader.TraderKind.WillTrade(this.ThingDef);
			}
		}

		// Token: 0x1700136F RID: 4975
		// (get) Token: 0x06006FC8 RID: 28616 RVA: 0x002541A6 File Offset: 0x002523A6
		public override bool HasAnyThing
		{
			get
			{
				return this.FirstThingColony != null || this.FirstThingTrader != null;
			}
		}

		// Token: 0x17001370 RID: 4976
		// (get) Token: 0x06006FC9 RID: 28617 RVA: 0x002541BB File Offset: 0x002523BB
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
				Log.Error(base.GetType() + " lacks AnyThing.");
				return null;
			}
		}

		// Token: 0x17001371 RID: 4977
		// (get) Token: 0x06006FCA RID: 28618 RVA: 0x002541FB File Offset: 0x002523FB
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

		// Token: 0x17001372 RID: 4978
		// (get) Token: 0x06006FCB RID: 28619 RVA: 0x00254212 File Offset: 0x00252412
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

		// Token: 0x17001373 RID: 4979
		// (get) Token: 0x06006FCC RID: 28620 RVA: 0x00254229 File Offset: 0x00252429
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

		// Token: 0x17001374 RID: 4980
		// (get) Token: 0x06006FCD RID: 28621 RVA: 0x00254244 File Offset: 0x00252444
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

		// Token: 0x17001375 RID: 4981
		// (get) Token: 0x06006FCE RID: 28622 RVA: 0x0025425C File Offset: 0x0025245C
		public virtual bool IsCurrency
		{
			get
			{
				return !this.Bugged && this.ThingDef == ThingDefOf.Silver;
			}
		}

		// Token: 0x17001376 RID: 4982
		// (get) Token: 0x06006FCF RID: 28623 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool IsFavor
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06006FD0 RID: 28624 RVA: 0x00254275 File Offset: 0x00252475
		public virtual int CostToInt(float cost)
		{
			return Mathf.RoundToInt(cost);
		}

		// Token: 0x17001377 RID: 4983
		// (get) Token: 0x06006FD1 RID: 28625 RVA: 0x0025427D File Offset: 0x0025247D
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

		// Token: 0x17001378 RID: 4984
		// (get) Token: 0x06006FD2 RID: 28626 RVA: 0x00254290 File Offset: 0x00252490
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

		// Token: 0x17001379 RID: 4985
		// (get) Token: 0x06006FD3 RID: 28627 RVA: 0x002542B4 File Offset: 0x002524B4
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

		// Token: 0x1700137A RID: 4986
		// (get) Token: 0x06006FD4 RID: 28628 RVA: 0x002542D8 File Offset: 0x002524D8
		public virtual Window NewInfoDialog
		{
			get
			{
				return new Dialog_InfoCard(this.ThingDef, null);
			}
		}

		// Token: 0x1700137B RID: 4987
		// (get) Token: 0x06006FD5 RID: 28629 RVA: 0x002542E6 File Offset: 0x002524E6
		private bool Bugged
		{
			get
			{
				if (!this.HasAnyThing)
				{
					Log.ErrorOnce(this.ToString() + " is bugged. There will be no more logs about this.", 162112);
					return true;
				}
				return false;
			}
		}

		// Token: 0x06006FD6 RID: 28630 RVA: 0x0025430D File Offset: 0x0025250D
		public Tradeable()
		{
		}

		// Token: 0x06006FD7 RID: 28631 RVA: 0x00254344 File Offset: 0x00252544
		public Tradeable(Thing thingColony, Thing thingTrader)
		{
			this.thingsColony.Add(thingColony);
			this.thingsTrader.Add(thingTrader);
		}

		// Token: 0x06006FD8 RID: 28632 RVA: 0x0025439B File Offset: 0x0025259B
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

		// Token: 0x06006FD9 RID: 28633 RVA: 0x002543BC File Offset: 0x002525BC
		public PriceType PriceTypeFor(TradeAction action)
		{
			return TradeSession.trader.TraderKind.PriceTypeFor(this.ThingDef, action);
		}

		// Token: 0x06006FDA RID: 28634 RVA: 0x002543D4 File Offset: 0x002525D4
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
			this.priceGain_Leader = ((TradeSession.playerNegotiator == Faction.OfPlayer.leader) ? 0.02f : 0f);
			this.priceGain_Settlement = TradeSession.trader.TradePriceImprovementOffsetForPlayer;
			this.priceFactorSell_ItemSellPriceFactor = this.AnyThing.GetStatValue(StatDefOf.SellPriceFactor, true);
			this.priceFactorBuy_JoinAs = 1f;
			this.priceFactorSell_HumanPawn = 1f;
			if (this.ThingDef.IsNonMedicalDrug)
			{
				this.priceGain_DrugBonus = TradeSession.playerNegotiator.GetStatValue(StatDefOf.DrugSellPriceImprovement, true);
			}
			if (ModsConfig.IdeologyActive)
			{
				if (this.ThingDef.IsLeather || this.ThingDef.IsMeat || this.ThingDef.IsWool)
				{
					this.priceGain_AnimalProduceBonus = TradeSession.playerNegotiator.GetStatValue(StatDefOf.AnimalProductsSellImprovement, true);
				}
				Pawn pawn;
				if ((pawn = (this.AnyThing as Pawn)) != null && pawn.RaceProps.Humanlike && pawn.guest != null)
				{
					if (pawn.guest.joinStatus == JoinStatus.JoinAsSlave)
					{
						this.priceFactorBuy_JoinAs = 0.6f;
					}
					this.priceFactorSell_HumanPawn = 0.6f;
				}
			}
			this.pricePlayerBuy = TradeUtility.GetPricePlayerBuy(this.AnyThing, this.priceFactorBuy_TraderPriceType, this.priceFactorBuy_JoinAs, this.priceGain_PlayerNegotiator, this.priceGain_Settlement);
			this.pricePlayerSell = TradeUtility.GetPricePlayerSell(this.AnyThing, this.priceFactorSell_TraderPriceType, this.priceFactorSell_HumanPawn, this.priceGain_PlayerNegotiator, this.priceGain_Settlement, this.priceGain_DrugBonus, this.priceGain_AnimalProduceBonus, TradeSession.TradeCurrency);
			if (this.pricePlayerSell >= this.pricePlayerBuy)
			{
				this.pricePlayerSell = this.pricePlayerBuy;
			}
		}

		// Token: 0x06006FDB RID: 28635 RVA: 0x002545DC File Offset: 0x002527DC
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
				if (Find.Storyteller.difficulty.tradePriceFactorLoss != 0f)
				{
					text += "\n  x " + (1f + Find.Storyteller.difficulty.tradePriceFactorLoss).ToString("F2") + " (" + "DifficultyLevel".Translate() + ")";
				}
				text += "\n";
				text += "\n" + "YourNegotiatorBonus".Translate() + ": -" + (this.priceGain_PlayerNegotiator - this.priceGain_Leader).ToStringPercent();
				if (ModsConfig.IdeologyActive)
				{
					if (this.priceGain_Leader != 0f)
					{
						text += "\n" + "YourLeaderTradeBonus".Translate() + ": -" + this.priceGain_Leader.ToStringPercent();
					}
					if (this.priceFactorBuy_JoinAs != 1f)
					{
						text += "\n" + "Slave".Translate() + ": x" + this.priceFactorBuy_JoinAs.ToStringPercent();
					}
				}
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
				if (Find.Storyteller.difficulty.tradePriceFactorLoss != 0f)
				{
					text += "\n  x " + (1f - Find.Storyteller.difficulty.tradePriceFactorLoss).ToString("F2") + " (" + "DifficultyLevel".Translate() + ")";
				}
				text += "\n";
				text += "\n" + "YourNegotiatorBonus".Translate() + ": " + (this.priceGain_PlayerNegotiator - this.priceGain_Leader).ToStringPercent();
				if (ModsConfig.IdeologyActive && this.priceGain_Leader != 0f)
				{
					text += "\n" + "YourLeaderTradeBonus".Translate() + ": -" + this.priceGain_Leader.ToStringPercent();
				}
				if (this.priceGain_Settlement != 0f)
				{
					text += "\n" + "TradeWithFactionBaseBonus".Translate() + ": " + this.priceGain_Settlement.ToStringPercent();
				}
				if (this.priceGain_DrugBonus != 0f)
				{
					text += "\n" + "TradingDrugsBonus".Translate() + ": " + this.priceGain_DrugBonus.ToStringPercent();
				}
				if (this.priceGain_AnimalProduceBonus != 0f)
				{
					text += "\n" + "TradingAnimalProduceBonus".Translate() + ": " + this.priceGain_AnimalProduceBonus.ToStringPercent();
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

		// Token: 0x06006FDC RID: 28636 RVA: 0x00254BE8 File Offset: 0x00252DE8
		public virtual float GetPriceFor(TradeAction action)
		{
			this.InitPriceDataIfNeeded();
			if (action == TradeAction.PlayerBuys)
			{
				return this.pricePlayerBuy;
			}
			return this.pricePlayerSell;
		}

		// Token: 0x06006FDD RID: 28637 RVA: 0x00254C01 File Offset: 0x00252E01
		public override int GetMinimumToTransfer()
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Destination)
			{
				return -this.CountHeldBy(Transactor.Trader);
			}
			return -this.CountHeldBy(Transactor.Colony);
		}

		// Token: 0x06006FDE RID: 28638 RVA: 0x00254C1D File Offset: 0x00252E1D
		public override int GetMaximumToTransfer()
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Destination)
			{
				return this.CountHeldBy(Transactor.Colony);
			}
			return this.CountHeldBy(Transactor.Trader);
		}

		// Token: 0x06006FDF RID: 28639 RVA: 0x00254C37 File Offset: 0x00252E37
		public override AcceptanceReport UnderflowReport()
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Destination)
			{
				return new AcceptanceReport("TraderHasNoMore".Translate());
			}
			return new AcceptanceReport("ColonyHasNoMore".Translate());
		}

		// Token: 0x06006FE0 RID: 28640 RVA: 0x00254C6B File Offset: 0x00252E6B
		public override AcceptanceReport OverflowReport()
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Destination)
			{
				return new AcceptanceReport("ColonyHasNoMore".Translate());
			}
			return new AcceptanceReport("TraderHasNoMore".Translate());
		}

		// Token: 0x06006FE1 RID: 28641 RVA: 0x00254C9F File Offset: 0x00252E9F
		private List<Thing> TransactorThings(Transactor trans)
		{
			if (trans == Transactor.Colony)
			{
				return this.thingsColony;
			}
			return this.thingsTrader;
		}

		// Token: 0x06006FE2 RID: 28642 RVA: 0x00254CB4 File Offset: 0x00252EB4
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

		// Token: 0x06006FE3 RID: 28643 RVA: 0x00254CEC File Offset: 0x00252EEC
		public int CountPostDealFor(Transactor trans)
		{
			if (trans == Transactor.Colony)
			{
				return this.CountHeldBy(trans) + base.CountToTransferToSource;
			}
			return this.CountHeldBy(trans) + base.CountToTransferToDestination;
		}

		// Token: 0x06006FE4 RID: 28644 RVA: 0x00254D10 File Offset: 0x00252F10
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

		// Token: 0x06006FE5 RID: 28645 RVA: 0x00254D84 File Offset: 0x00252F84
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

		// Token: 0x06006FE6 RID: 28646 RVA: 0x00254DEC File Offset: 0x00252FEC
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

		// Token: 0x06006FE7 RID: 28647 RVA: 0x00254E3C File Offset: 0x0025303C
		public override int GetHashCode()
		{
			return this.AnyThing.GetHashCode();
		}

		// Token: 0x06006FE8 RID: 28648 RVA: 0x00254E4C File Offset: 0x0025304C
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
				Log.Warning("Some of the things were null after loading.");
			}
		}

		// Token: 0x04003DB5 RID: 15797
		public List<Thing> thingsColony = new List<Thing>();

		// Token: 0x04003DB6 RID: 15798
		public List<Thing> thingsTrader = new List<Thing>();

		// Token: 0x04003DB7 RID: 15799
		private int countToTransfer;

		// Token: 0x04003DB8 RID: 15800
		public const float NegotiatorLeaderOffset = 0.02f;

		// Token: 0x04003DB9 RID: 15801
		private const float PriceFactor_JoinAsSlaveOrColonyPawn = 0.6f;

		// Token: 0x04003DBA RID: 15802
		private float pricePlayerBuy = -1f;

		// Token: 0x04003DBB RID: 15803
		private float pricePlayerSell = -1f;

		// Token: 0x04003DBC RID: 15804
		private float priceFactorBuy_TraderPriceType;

		// Token: 0x04003DBD RID: 15805
		private float priceFactorBuy_JoinAs;

		// Token: 0x04003DBE RID: 15806
		private float priceFactorSell_HumanPawn;

		// Token: 0x04003DBF RID: 15807
		private float priceFactorSell_TraderPriceType;

		// Token: 0x04003DC0 RID: 15808
		private float priceFactorSell_ItemSellPriceFactor;

		// Token: 0x04003DC1 RID: 15809
		private float priceGain_PlayerNegotiator;

		// Token: 0x04003DC2 RID: 15810
		private float priceGain_Leader;

		// Token: 0x04003DC3 RID: 15811
		private float priceGain_Settlement;

		// Token: 0x04003DC4 RID: 15812
		private float priceGain_DrugBonus;

		// Token: 0x04003DC5 RID: 15813
		private float priceGain_AnimalProduceBonus;
	}
}
