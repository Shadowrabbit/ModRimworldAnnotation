using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017D2 RID: 6098
	public class Settlement_TraderTracker : IThingHolder, IExposable
	{
		// Token: 0x17001720 RID: 5920
		// (get) Token: 0x06008DD9 RID: 36313 RVA: 0x0015D6EE File Offset: 0x0015B8EE
		protected virtual int RegenerateStockEveryDays
		{
			get
			{
				return 30;
			}
		}

		// Token: 0x17001721 RID: 5921
		// (get) Token: 0x06008DDA RID: 36314 RVA: 0x0032F8CC File Offset: 0x0032DACC
		public IThingHolder ParentHolder
		{
			get
			{
				return this.settlement;
			}
		}

		// Token: 0x17001722 RID: 5922
		// (get) Token: 0x06008DDB RID: 36315 RVA: 0x0032F8D4 File Offset: 0x0032DAD4
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

		// Token: 0x17001723 RID: 5923
		// (get) Token: 0x06008DDC RID: 36316 RVA: 0x0032F8F0 File Offset: 0x0032DAF0
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

		// Token: 0x17001724 RID: 5924
		// (get) Token: 0x06008DDD RID: 36317 RVA: 0x0032F93C File Offset: 0x0032DB3C
		public int RandomPriceFactorSeed
		{
			get
			{
				return Gen.HashCombineInt(this.settlement.ID, 1933327354);
			}
		}

		// Token: 0x17001725 RID: 5925
		// (get) Token: 0x06008DDE RID: 36318 RVA: 0x0032F953 File Offset: 0x0032DB53
		public bool EverVisited
		{
			get
			{
				return this.everGeneratedStock;
			}
		}

		// Token: 0x17001726 RID: 5926
		// (get) Token: 0x06008DDF RID: 36319 RVA: 0x0032F95B File Offset: 0x0032DB5B
		public bool RestockedSinceLastVisit
		{
			get
			{
				return this.everGeneratedStock && this.stock == null;
			}
		}

		// Token: 0x17001727 RID: 5927
		// (get) Token: 0x06008DE0 RID: 36320 RVA: 0x0032F970 File Offset: 0x0032DB70
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

		// Token: 0x17001728 RID: 5928
		// (get) Token: 0x06008DE1 RID: 36321 RVA: 0x0032F9A4 File Offset: 0x0032DBA4
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

		// Token: 0x17001729 RID: 5929
		// (get) Token: 0x06008DE2 RID: 36322 RVA: 0x0032F9FE File Offset: 0x0032DBFE
		public virtual bool CanTradeNow
		{
			get
			{
				return this.TraderKind != null && (this.stock == null || this.stock.InnerListForReading.Any((Thing x) => this.TraderKind.WillTrade(x.def)));
			}
		}

		// Token: 0x1700172A RID: 5930
		// (get) Token: 0x06008DE3 RID: 36323 RVA: 0x0032FA30 File Offset: 0x0032DC30
		public virtual float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				return 0.02f;
			}
		}

		// Token: 0x06008DE4 RID: 36324 RVA: 0x0032FA37 File Offset: 0x0032DC37
		public Settlement_TraderTracker(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x06008DE5 RID: 36325 RVA: 0x0032FA58 File Offset: 0x0032DC58
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

		// Token: 0x06008DE6 RID: 36326 RVA: 0x0032FB5F File Offset: 0x0032DD5F
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

		// Token: 0x06008DE7 RID: 36327 RVA: 0x0032FB70 File Offset: 0x0032DD70
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

		// Token: 0x06008DE8 RID: 36328 RVA: 0x0032FBFC File Offset: 0x0032DDFC
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
				Log.Error("Could not find any pawn to give sold thing to.");
				thing.Destroy(DestroyMode.Vanish);
				return;
			}
			if (!pawn2.inventory.innerContainer.TryAdd(thing, true))
			{
				Log.Error("Could not add sold thing to inventory.");
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06008DE9 RID: 36329 RVA: 0x0032FC84 File Offset: 0x0032DE84
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
						Log.Error("Faction base has non-world-pawns in its stock. Removing...");
						this.stock.Remove(pawn2);
					}
				}
			}
		}

		// Token: 0x06008DEA RID: 36330 RVA: 0x0032FD50 File Offset: 0x0032DF50
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

		// Token: 0x06008DEB RID: 36331 RVA: 0x0032FDB5 File Offset: 0x0032DFB5
		public bool ContainsPawn(Pawn p)
		{
			return this.stock != null && this.stock.Contains(p);
		}

		// Token: 0x06008DEC RID: 36332 RVA: 0x0032FDD0 File Offset: 0x0032DFD0
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

		// Token: 0x06008DED RID: 36333 RVA: 0x0032FEB7 File Offset: 0x0032E0B7
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.stock;
		}

		// Token: 0x06008DEE RID: 36334 RVA: 0x0032FEBF File Offset: 0x0032E0BF
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x040059A4 RID: 22948
		public Settlement settlement;

		// Token: 0x040059A5 RID: 22949
		private ThingOwner<Thing> stock;

		// Token: 0x040059A6 RID: 22950
		private int lastStockGenerationTicks = -1;

		// Token: 0x040059A7 RID: 22951
		private bool everGeneratedStock;

		// Token: 0x040059A8 RID: 22952
		private const float DefaultTradePriceImprovement = 0.02f;

		// Token: 0x040059A9 RID: 22953
		private List<Pawn> tmpSavedPawns = new List<Pawn>();
	}
}
