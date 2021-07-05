using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001926 RID: 6438
	public class TradeShip : PassingShip, ITrader, IThingHolder
	{
		// Token: 0x1700167D RID: 5757
		// (get) Token: 0x06008E8F RID: 36495 RVA: 0x0005F810 File Offset: 0x0005DA10
		public override string FullTitle
		{
			get
			{
				return this.name + " (" + this.def.label + ")";
			}
		}

		// Token: 0x1700167E RID: 5758
		// (get) Token: 0x06008E90 RID: 36496 RVA: 0x0005F832 File Offset: 0x0005DA32
		public int Silver
		{
			get
			{
				return this.CountHeldOf(ThingDefOf.Silver, null);
			}
		}

		// Token: 0x1700167F RID: 5759
		// (get) Token: 0x06008E91 RID: 36497 RVA: 0x0005F840 File Offset: 0x0005DA40
		public TradeCurrency TradeCurrency
		{
			get
			{
				return this.def.tradeCurrency;
			}
		}

		// Token: 0x17001680 RID: 5760
		// (get) Token: 0x06008E92 RID: 36498 RVA: 0x0005F84D File Offset: 0x0005DA4D
		public IThingHolder ParentHolder
		{
			get
			{
				return base.Map;
			}
		}

		// Token: 0x17001681 RID: 5761
		// (get) Token: 0x06008E93 RID: 36499 RVA: 0x0005F855 File Offset: 0x0005DA55
		public TraderKindDef TraderKind
		{
			get
			{
				return this.def;
			}
		}

		// Token: 0x17001682 RID: 5762
		// (get) Token: 0x06008E94 RID: 36500 RVA: 0x0005F85D File Offset: 0x0005DA5D
		public int RandomPriceFactorSeed
		{
			get
			{
				return this.randomPriceFactorSeed;
			}
		}

		// Token: 0x17001683 RID: 5763
		// (get) Token: 0x06008E95 RID: 36501 RVA: 0x0005E946 File Offset: 0x0005CB46
		public string TraderName
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17001684 RID: 5764
		// (get) Token: 0x06008E96 RID: 36502 RVA: 0x0005F865 File Offset: 0x0005DA65
		public bool CanTradeNow
		{
			get
			{
				return !base.Departed;
			}
		}

		// Token: 0x17001685 RID: 5765
		// (get) Token: 0x06008E97 RID: 36503 RVA: 0x00016647 File Offset: 0x00014847
		public float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17001686 RID: 5766
		// (get) Token: 0x06008E98 RID: 36504 RVA: 0x0005F870 File Offset: 0x0005DA70
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

		// Token: 0x06008E99 RID: 36505 RVA: 0x0005F880 File Offset: 0x0005DA80
		public TradeShip()
		{
		}

		// Token: 0x06008E9A RID: 36506 RVA: 0x00291CD0 File Offset: 0x0028FED0
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

		// Token: 0x06008E9B RID: 36507 RVA: 0x0005F89A File Offset: 0x0005DA9A
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			foreach (Thing thing in TradeUtility.AllLaunchableThingsForTrade(base.Map, this))
			{
				yield return thing;
			}
			IEnumerator<Thing> enumerator = null;
			foreach (Pawn pawn in TradeUtility.AllSellableColonyPawns(base.Map))
			{
				yield return pawn;
			}
			IEnumerator<Pawn> enumerator2 = null;
			yield break;
			yield break;
		}

		// Token: 0x06008E9C RID: 36508 RVA: 0x00291DD0 File Offset: 0x0028FFD0
		public void GenerateThings()
		{
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.traderDef = this.def;
			parms.tile = new int?(base.Map.Tile);
			this.things.TryAddRangeOrTransfer(ThingSetMakerDefOf.TraderStock.root.Generate(parms), true, false);
		}

		// Token: 0x06008E9D RID: 36509 RVA: 0x00291E28 File Offset: 0x00290028
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

		// Token: 0x06008E9E RID: 36510 RVA: 0x00291E84 File Offset: 0x00290084
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

		// Token: 0x06008E9F RID: 36511 RVA: 0x00291F1C File Offset: 0x0029011C
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

		// Token: 0x06008EA0 RID: 36512 RVA: 0x0005F8AA File Offset: 0x0005DAAA
		public override void Depart()
		{
			base.Depart();
			this.things.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
			this.soldPrisoners.Clear();
		}

		// Token: 0x06008EA1 RID: 36513 RVA: 0x0005F810 File Offset: 0x0005DA10
		public override string GetCallLabel()
		{
			return this.name + " (" + this.def.label + ")";
		}

		// Token: 0x06008EA2 RID: 36514 RVA: 0x00291F98 File Offset: 0x00290198
		protected override AcceptanceReport CanCommunicateWith_NewTemp(Pawn negotiator)
		{
			AcceptanceReport result = base.CanCommunicateWith_NewTemp(negotiator);
			if (!result.Accepted)
			{
				return result;
			}
			return negotiator.CanTradeWith_NewTemp(base.Faction, this.TraderKind);
		}

		// Token: 0x06008EA3 RID: 36515 RVA: 0x0005F8C9 File Offset: 0x0005DAC9
		protected override bool CanCommunicateWith(Pawn negotiator)
		{
			return base.CanCommunicateWith(negotiator) && negotiator.CanTradeWith(base.Faction, this.TraderKind);
		}

		// Token: 0x06008EA4 RID: 36516 RVA: 0x00291FCC File Offset: 0x002901CC
		public int CountHeldOf(ThingDef thingDef, ThingDef stuffDef = null)
		{
			Thing thing = this.HeldThingMatching(thingDef, stuffDef);
			if (thing != null)
			{
				return thing.stackCount;
			}
			return 0;
		}

		// Token: 0x06008EA5 RID: 36517 RVA: 0x00291FF0 File Offset: 0x002901F0
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

		// Token: 0x06008EA6 RID: 36518 RVA: 0x0029205C File Offset: 0x0029025C
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

		// Token: 0x06008EA7 RID: 36519 RVA: 0x002920A8 File Offset: 0x002902A8
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

		// Token: 0x06008EA8 RID: 36520 RVA: 0x0005F8E8 File Offset: 0x0005DAE8
		public void ChangeCountHeldOf(ThingDef thingDef, ThingDef stuffDef, int count)
		{
			Thing thing = this.HeldThingMatching(thingDef, stuffDef);
			if (thing == null)
			{
				Log.Error("Changing count of thing trader doesn't have: " + thingDef, false);
			}
			thing.stackCount += count;
		}

		// Token: 0x06008EA9 RID: 36521 RVA: 0x0005E94E File Offset: 0x0005CB4E
		public override string ToString()
		{
			return this.FullTitle;
		}

		// Token: 0x06008EAA RID: 36522 RVA: 0x0005F913 File Offset: 0x0005DB13
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.things;
		}

		// Token: 0x06008EAB RID: 36523 RVA: 0x0005F91B File Offset: 0x0005DB1B
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x04005AEC RID: 23276
		public TraderKindDef def;

		// Token: 0x04005AED RID: 23277
		private ThingOwner things;

		// Token: 0x04005AEE RID: 23278
		private List<Pawn> soldPrisoners = new List<Pawn>();

		// Token: 0x04005AEF RID: 23279
		private int randomPriceFactorSeed = -1;

		// Token: 0x04005AF0 RID: 23280
		private static List<string> tmpExtantNames = new List<string>();
	}
}
