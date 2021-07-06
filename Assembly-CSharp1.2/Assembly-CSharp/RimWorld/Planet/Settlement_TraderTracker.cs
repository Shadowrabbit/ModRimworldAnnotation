using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200214B RID: 8523
	public class Settlement_TraderTracker : IThingHolder, IExposable
	{
		// Token: 0x17001ACA RID: 6858
		// (get) Token: 0x0600B581 RID: 46465 RVA: 0x0003D335 File Offset: 0x0003B535
		protected virtual int RegenerateStockEveryDays
		{
			get
			{
				return 30;
			}
		}

		// Token: 0x17001ACB RID: 6859
		// (get) Token: 0x0600B582 RID: 46466 RVA: 0x00075E2F File Offset: 0x0007402F
		public IThingHolder ParentHolder
		{
			get
			{
				return this.settlement;
			}
		}

		// Token: 0x17001ACC RID: 6860
		// (get) Token: 0x0600B583 RID: 46467 RVA: 0x00075E37 File Offset: 0x00074037
		public List<Thing> StockListForReading
		{
			get
			{
				if (this.stock == null)
				{
					this.RegenerateStock();
				}
				return this.stock.InnerListForReading;
			}
		}

		// Token: 0x17001ACD RID: 6861
		// (get) Token: 0x0600B584 RID: 46468 RVA: 0x00348670 File Offset: 0x00346870
		public TraderKindDef TraderKind
		{
			get
			{
				List<TraderKindDef> baseTraderKinds = this.settlement.Faction.def.baseTraderKinds;
				if (baseTraderKinds.NullOrEmpty<TraderKindDef>())
				{
					return null;
				}
				int index = Mathf.Abs(this.settlement.HashOffset()) % baseTraderKinds.Count;
				return baseTraderKinds[index];
			}
		}

		// Token: 0x17001ACE RID: 6862
		// (get) Token: 0x0600B585 RID: 46469 RVA: 0x00075E52 File Offset: 0x00074052
		public int RandomPriceFactorSeed
		{
			get
			{
				return Gen.HashCombineInt(this.settlement.ID, 1933327354);
			}
		}

		// Token: 0x17001ACF RID: 6863
		// (get) Token: 0x0600B586 RID: 46470 RVA: 0x00075E69 File Offset: 0x00074069
		public bool EverVisited
		{
			get
			{
				return this.everGeneratedStock;
			}
		}

		// Token: 0x17001AD0 RID: 6864
		// (get) Token: 0x0600B587 RID: 46471 RVA: 0x00075E71 File Offset: 0x00074071
		public bool RestockedSinceLastVisit
		{
			get
			{
				return this.everGeneratedStock && this.stock == null;
			}
		}

		// Token: 0x17001AD1 RID: 6865
		// (get) Token: 0x0600B588 RID: 46472 RVA: 0x00075E86 File Offset: 0x00074086
		public int NextRestockTick
		{
			get
			{
				if (this.stock == null || !this.everGeneratedStock)
				{
					return -1;
				}
				return ((this.lastStockGenerationTicks == -1) ? 0 : this.lastStockGenerationTicks) + this.RegenerateStockEveryDays * 60000;
			}
		}

		// Token: 0x17001AD2 RID: 6866
		// (get) Token: 0x0600B589 RID: 46473 RVA: 0x003486BC File Offset: 0x003468BC
		public virtual string TraderName
		{
			get
			{
				if (this.settlement.Faction == null)
				{
					return this.settlement.LabelCap;
				}
				return "SettlementTrader".Translate(this.settlement.LabelCap, this.settlement.Faction.Name);
			}
		}

		// Token: 0x17001AD3 RID: 6867
		// (get) Token: 0x0600B58A RID: 46474 RVA: 0x00075EB9 File Offset: 0x000740B9
		public virtual bool CanTradeNow
		{
			get
			{
				return this.TraderKind != null && (this.stock == null || this.stock.InnerListForReading.Any((Thing x) => this.TraderKind.WillTrade(x.def)));
			}
		}

		// Token: 0x17001AD4 RID: 6868
		// (get) Token: 0x0600B58B RID: 46475 RVA: 0x00075EEB File Offset: 0x000740EB
		public virtual float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				return 0.02f;
			}
		}

		// Token: 0x0600B58C RID: 46476 RVA: 0x00075EF2 File Offset: 0x000740F2
		public Settlement_TraderTracker(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x0600B58D RID: 46477 RVA: 0x00348718 File Offset: 0x00346918
		public virtual void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.tmpSavedPawns.Clear();
				if (this.stock != null)
				{
					for (int i = this.stock.Count - 1; i >= 0; i--)
					{
						Pawn pawn = this.stock[i] as Pawn;
						if (pawn != null)
						{
							this.stock.Remove(pawn);
							this.tmpSavedPawns.Add(pawn);
						}
					}
				}
			}
			Scribe_Collections.Look<Pawn>(ref this.tmpSavedPawns, "tmpSavedPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.stock, "stock", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.lastStockGenerationTicks, "lastStockGenerationTicks", 0, false);
			Scribe_Values.Look<bool>(ref this.everGeneratedStock, "wasStockGeneratedYet", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit || Scribe.mode == LoadSaveMode.Saving)
			{
				for (int j = 0; j < this.tmpSavedPawns.Count; j++)
				{
					this.stock.TryAdd(this.tmpSavedPawns[j], false);
				}
				this.tmpSavedPawns.Clear();
			}
		}

		// Token: 0x0600B58E RID: 46478 RVA: 0x00075F13 File Offset: 0x00074113
		public virtual IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			Caravan caravan = playerNegotiator.GetCaravan();
			foreach (Thing thing in CaravanInventoryUtility.AllInventoryItems(caravan))
			{
				yield return thing;
			}
			List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			List<Pawn> pawns = caravan.PawnsListForReading;
			int num;
			for (int i = 0; i < pawns.Count; i = num + 1)
			{
				if (!caravan.IsOwner(pawns[i]))
				{
					yield return pawns[i];
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600B58F RID: 46479 RVA: 0x00348820 File Offset: 0x00346A20
		public virtual void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			if (this.stock == null)
			{
				this.RegenerateStock();
			}
			Caravan caravan = playerNegotiator.GetCaravan();
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerSells, playerNegotiator, this.settlement);
			Pawn pawn = toGive as Pawn;
			if (pawn != null)
			{
				CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(pawn, caravan.PawnsListForReading, null);
				if (pawn.RaceProps.Humanlike)
				{
					return;
				}
				if (!this.stock.TryAdd(pawn, false))
				{
					pawn.Destroy(DestroyMode.Vanish);
					return;
				}
			}
			else if (!this.stock.TryAdd(thing, false))
			{
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600B590 RID: 46480 RVA: 0x003488AC File Offset: 0x00346AAC
		public virtual void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			Caravan caravan = playerNegotiator.GetCaravan();
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this.settlement);
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				caravan.AddPawn(pawn, true);
				return;
			}
			Pawn pawn2 = CaravanInventoryUtility.FindPawnToMoveInventoryTo(thing, caravan.PawnsListForReading, null, null);
			if (pawn2 == null)
			{
				Log.Error("Could not find any pawn to give sold thing to.", false);
				thing.Destroy(DestroyMode.Vanish);
				return;
			}
			if (!pawn2.inventory.innerContainer.TryAdd(thing, true))
			{
				Log.Error("Could not add sold thing to inventory.", false);
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600B591 RID: 46481 RVA: 0x00348934 File Offset: 0x00346B34
		public virtual void TraderTrackerTick()
		{
			if (this.stock != null)
			{
				if (Find.TickManager.TicksGame - this.lastStockGenerationTicks > this.RegenerateStockEveryDays * 60000)
				{
					this.TryDestroyStock();
					return;
				}
				for (int i = this.stock.Count - 1; i >= 0; i--)
				{
					Pawn pawn = this.stock[i] as Pawn;
					if (pawn != null && pawn.Destroyed)
					{
						this.stock.Remove(pawn);
					}
				}
				for (int j = this.stock.Count - 1; j >= 0; j--)
				{
					Pawn pawn2 = this.stock[j] as Pawn;
					if (pawn2 != null && !pawn2.IsWorldPawn())
					{
						Log.Error("Faction base has non-world-pawns in its stock. Removing...", false);
						this.stock.Remove(pawn2);
					}
				}
			}
		}

		// Token: 0x0600B592 RID: 46482 RVA: 0x00348A04 File Offset: 0x00346C04
		public void TryDestroyStock()
		{
			if (this.stock != null)
			{
				for (int i = this.stock.Count - 1; i >= 0; i--)
				{
					Thing thing = this.stock[i];
					this.stock.Remove(thing);
					if (!(thing is Pawn) && !thing.Destroyed)
					{
						thing.Destroy(DestroyMode.Vanish);
					}
				}
				this.stock = null;
			}
		}

		// Token: 0x0600B593 RID: 46483 RVA: 0x00075F23 File Offset: 0x00074123
		public bool ContainsPawn(Pawn p)
		{
			return this.stock != null && this.stock.Contains(p);
		}

		// Token: 0x0600B594 RID: 46484 RVA: 0x00348A6C File Offset: 0x00346C6C
		protected virtual void RegenerateStock()
		{
			this.TryDestroyStock();
			this.stock = new ThingOwner<Thing>(this);
			this.everGeneratedStock = true;
			if (this.settlement.Faction == null || !this.settlement.Faction.IsPlayer)
			{
				ThingSetMakerParams parms = default(ThingSetMakerParams);
				parms.traderDef = this.TraderKind;
				parms.tile = new int?(this.settlement.Tile);
				parms.makingFaction = this.settlement.Faction;
				this.stock.TryAddRangeOrTransfer(ThingSetMakerDefOf.TraderStock.root.Generate(parms), true, false);
			}
			for (int i = 0; i < this.stock.Count; i++)
			{
				Pawn pawn = this.stock[i] as Pawn;
				if (pawn != null)
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
				}
			}
			this.lastStockGenerationTicks = Find.TickManager.TicksGame;
		}

		// Token: 0x0600B595 RID: 46485 RVA: 0x00075F3B File Offset: 0x0007413B
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.stock;
		}

		// Token: 0x0600B596 RID: 46486 RVA: 0x00075F43 File Offset: 0x00074143
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x04007C71 RID: 31857
		public Settlement settlement;

		// Token: 0x04007C72 RID: 31858
		private ThingOwner<Thing> stock;

		// Token: 0x04007C73 RID: 31859
		private int lastStockGenerationTicks = -1;

		// Token: 0x04007C74 RID: 31860
		private bool everGeneratedStock;

		// Token: 0x04007C75 RID: 31861
		private const float DefaultTradePriceImprovement = 0.02f;

		// Token: 0x04007C76 RID: 31862
		private List<Pawn> tmpSavedPawns = new List<Pawn>();
	}
}
