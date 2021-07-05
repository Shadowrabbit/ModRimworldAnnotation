using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001230 RID: 4656
	public class TradeShip : PassingShip, ITrader, IThingHolder
	{
		// Token: 0x1700135E RID: 4958
		// (get) Token: 0x06006F93 RID: 28563 RVA: 0x0025364D File Offset: 0x0025184D
		public override string FullTitle
		{
			get
			{
				return this.name + " (" + this.def.label + ")";
			}
		}

		// Token: 0x1700135F RID: 4959
		// (get) Token: 0x06006F94 RID: 28564 RVA: 0x0025366F File Offset: 0x0025186F
		public int Silver
		{
			get
			{
				return this.CountHeldOf(ThingDefOf.Silver, null);
			}
		}

		// Token: 0x17001360 RID: 4960
		// (get) Token: 0x06006F95 RID: 28565 RVA: 0x0025367D File Offset: 0x0025187D
		public TradeCurrency TradeCurrency
		{
			get
			{
				return this.def.tradeCurrency;
			}
		}

		// Token: 0x17001361 RID: 4961
		// (get) Token: 0x06006F96 RID: 28566 RVA: 0x0025368A File Offset: 0x0025188A
		public IThingHolder ParentHolder
		{
			get
			{
				return base.Map;
			}
		}

		// Token: 0x17001362 RID: 4962
		// (get) Token: 0x06006F97 RID: 28567 RVA: 0x00253692 File Offset: 0x00251892
		public TraderKindDef TraderKind
		{
			get
			{
				return this.def;
			}
		}

		// Token: 0x17001363 RID: 4963
		// (get) Token: 0x06006F98 RID: 28568 RVA: 0x0025369A File Offset: 0x0025189A
		public int RandomPriceFactorSeed
		{
			get
			{
				return this.randomPriceFactorSeed;
			}
		}

		// Token: 0x17001364 RID: 4964
		// (get) Token: 0x06006F99 RID: 28569 RVA: 0x00251CBF File Offset: 0x0024FEBF
		public string TraderName
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17001365 RID: 4965
		// (get) Token: 0x06006F9A RID: 28570 RVA: 0x002536A2 File Offset: 0x002518A2
		public bool CanTradeNow
		{
			get
			{
				return !base.Departed;
			}
		}

		// Token: 0x17001366 RID: 4966
		// (get) Token: 0x06006F9B RID: 28571 RVA: 0x000682C5 File Offset: 0x000664C5
		public float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17001367 RID: 4967
		// (get) Token: 0x06006F9C RID: 28572 RVA: 0x002536AD File Offset: 0x002518AD
		public IEnumerable<Thing> Goods
		{
			get
			{
				int num;
				for (int i = 0; i < this.things.Count; i = num + 1)
				{
					Pawn pawn = this.things[i] as Pawn;
					if (pawn == null || !this.soldPrisoners.Contains(pawn))
					{
						yield return this.things[i];
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x06006F9D RID: 28573 RVA: 0x002536BD File Offset: 0x002518BD
		public TradeShip()
		{
		}

		// Token: 0x06006F9E RID: 28574 RVA: 0x002536D8 File Offset: 0x002518D8
		public TradeShip(TraderKindDef def, Faction faction = null) : base(faction)
		{
			this.def = def;
			this.things = new ThingOwner<Thing>(this);
			TradeShip.tmpExtantNames.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				TradeShip.tmpExtantNames.AddRange(from x in maps[i].passingShipManager.passingShips
				select x.name);
			}
			this.name = NameGenerator.GenerateName(RulePackDefOf.NamerTraderGeneral, TradeShip.tmpExtantNames, false, null);
			if (faction != null)
			{
				this.name = string.Format("{0} {1} {2}", this.name, "OfLower".Translate(), faction.Name);
			}
			this.randomPriceFactorSeed = Rand.RangeInclusive(1, 10000000);
			this.loadID = Find.UniqueIDsManager.GetNextPassingShipID();
		}

		// Token: 0x06006F9F RID: 28575 RVA: 0x002537D7 File Offset: 0x002519D7
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			foreach (Thing thing in TradeUtility.AllLaunchableThingsForTrade(base.Map, this))
			{
				yield return thing;
			}
			IEnumerator<Thing> enumerator = null;
			foreach (Pawn pawn in TradeUtility.AllSellableColonyPawns(base.Map, false))
			{
				yield return pawn;
			}
			IEnumerator<Pawn> enumerator2 = null;
			yield break;
			yield break;
		}

		// Token: 0x06006FA0 RID: 28576 RVA: 0x002537E8 File Offset: 0x002519E8
		public void GenerateThings()
		{
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.traderDef = this.def;
			parms.tile = new int?(base.Map.Tile);
			this.things.TryAddRangeOrTransfer(ThingSetMakerDefOf.TraderStock.root.Generate(parms), true, false);
		}

		// Token: 0x06006FA1 RID: 28577 RVA: 0x00253840 File Offset: 0x00251A40
		public override void PassingShipTick()
		{
			base.PassingShipTick();
			for (int i = this.things.Count - 1; i >= 0; i--)
			{
				Pawn pawn = this.things[i] as Pawn;
				if (pawn != null)
				{
					pawn.Tick();
					if (pawn.Dead)
					{
						this.things.Remove(pawn);
					}
				}
			}
		}

		// Token: 0x06006FA2 RID: 28578 RVA: 0x0025389C File Offset: 0x00251A9C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<TraderKindDef>(ref this.def, "def");
			Scribe_Deep.Look<ThingOwner>(ref this.things, "things", new object[]
			{
				this
			});
			Scribe_Collections.Look<Pawn>(ref this.soldPrisoners, "soldPrisoners", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.randomPriceFactorSeed, "randomPriceFactorSeed", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.soldPrisoners.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06006FA3 RID: 28579 RVA: 0x00253934 File Offset: 0x00251B34
		public override void TryOpenComms(Pawn negotiator)
		{
			if (!this.CanTradeNow)
			{
				return;
			}
			Find.WindowStack.Add(new Dialog_Trade(negotiator, this, false));
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.BuildOrbitalTradeBeacon, OpportunityType.Critical);
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(this.Goods.OfType<Pawn>(), "LetterRelatedPawnsTradeShip".Translate(Faction.OfPlayer.def.pawnsPlural), LetterDefOf.NeutralEvent, false, true);
			TutorUtility.DoModalDialogIfNotKnown(ConceptDefOf.TradeGoodsMustBeNearBeacon, Array.Empty<string>());
		}

		// Token: 0x06006FA4 RID: 28580 RVA: 0x002539B0 File Offset: 0x00251BB0
		public override void Depart()
		{
			base.Depart();
			this.things.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
			this.soldPrisoners.Clear();
		}

		// Token: 0x06006FA5 RID: 28581 RVA: 0x0025364D File Offset: 0x0025184D
		public override string GetCallLabel()
		{
			return this.name + " (" + this.def.label + ")";
		}

		// Token: 0x06006FA6 RID: 28582 RVA: 0x002539D0 File Offset: 0x00251BD0
		protected override AcceptanceReport CanCommunicateWith(Pawn negotiator)
		{
			AcceptanceReport result = base.CanCommunicateWith(negotiator);
			if (!result.Accepted)
			{
				return result;
			}
			return negotiator.CanTradeWith(base.Faction, this.TraderKind).Accepted;
		}

		// Token: 0x06006FA7 RID: 28583 RVA: 0x00253A10 File Offset: 0x00251C10
		public int CountHeldOf(ThingDef thingDef, ThingDef stuffDef = null)
		{
			Thing thing = this.HeldThingMatching(thingDef, stuffDef);
			if (thing != null)
			{
				return thing.stackCount;
			}
			return 0;
		}

		// Token: 0x06006FA8 RID: 28584 RVA: 0x00253A34 File Offset: 0x00251C34
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerSells, playerNegotiator, this);
			Thing thing2 = TradeUtility.ThingFromStockToMergeWith(this, thing);
			if (thing2 != null)
			{
				if (!thing2.TryAbsorbStack(thing, false))
				{
					thing.Destroy(DestroyMode.Vanish);
					return;
				}
			}
			else
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && pawn.RaceProps.Humanlike)
				{
					this.soldPrisoners.Add(pawn);
				}
				this.things.TryAdd(thing, false);
			}
		}

		// Token: 0x06006FA9 RID: 28585 RVA: 0x00253AA0 File Offset: 0x00251CA0
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this);
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				this.soldPrisoners.Remove(pawn);
			}
			TradeUtility.SpawnDropPod(DropCellFinder.TradeDropSpot(base.Map), base.Map, thing);
		}

		// Token: 0x06006FAA RID: 28586 RVA: 0x00253AEC File Offset: 0x00251CEC
		private Thing HeldThingMatching(ThingDef thingDef, ThingDef stuffDef)
		{
			for (int i = 0; i < this.things.Count; i++)
			{
				if (this.things[i].def == thingDef && this.things[i].Stuff == stuffDef)
				{
					return this.things[i];
				}
			}
			return null;
		}

		// Token: 0x06006FAB RID: 28587 RVA: 0x00253B45 File Offset: 0x00251D45
		public void ChangeCountHeldOf(ThingDef thingDef, ThingDef stuffDef, int count)
		{
			Thing thing = this.HeldThingMatching(thingDef, stuffDef);
			if (thing == null)
			{
				Log.Error("Changing count of thing trader doesn't have: " + thingDef);
			}
			thing.stackCount += count;
		}

		// Token: 0x06006FAC RID: 28588 RVA: 0x00251CC7 File Offset: 0x0024FEC7
		public override string ToString()
		{
			return this.FullTitle;
		}

		// Token: 0x06006FAD RID: 28589 RVA: 0x00253B6F File Offset: 0x00251D6F
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.things;
		}

		// Token: 0x06006FAE RID: 28590 RVA: 0x00253B77 File Offset: 0x00251D77
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x04003DA3 RID: 15779
		public TraderKindDef def;

		// Token: 0x04003DA4 RID: 15780
		private ThingOwner things;

		// Token: 0x04003DA5 RID: 15781
		private List<Pawn> soldPrisoners = new List<Pawn>();

		// Token: 0x04003DA6 RID: 15782
		private int randomPriceFactorSeed = -1;

		// Token: 0x04003DA7 RID: 15783
		private static List<string> tmpExtantNames = new List<string>();
	}
}
