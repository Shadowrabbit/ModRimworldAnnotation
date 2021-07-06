using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020020CF RID: 8399
	[StaticConstructorOnStartup]
	public class Caravan : WorldObject, IThingHolder, IIncidentTarget, ILoadReferenceable, ITrader
	{
		// Token: 0x17001A46 RID: 6726
		// (get) Token: 0x0600B20F RID: 45583 RVA: 0x00073B60 File Offset: 0x00071D60
		public List<Pawn> PawnsListForReading
		{
			get
			{
				return this.pawns.InnerListForReading;
			}
		}

		// Token: 0x17001A47 RID: 6727
		// (get) Token: 0x0600B210 RID: 45584 RVA: 0x00339C68 File Offset: 0x00337E68
		public override Material Material
		{
			get
			{
				if (this.cachedMat == null)
				{
					Color color;
					if (base.Faction == null)
					{
						color = Color.white;
					}
					else if (base.Faction.IsPlayer)
					{
						color = Caravan.PlayerCaravanColor;
					}
					else
					{
						color = base.Faction.Color;
					}
					this.cachedMat = MaterialPool.MatFrom(this.def.texture, ShaderDatabase.WorldOverlayTransparentLit, color, WorldMaterials.DynamicObjectRenderQueue);
				}
				return this.cachedMat;
			}
		}

		// Token: 0x17001A48 RID: 6728
		// (get) Token: 0x0600B211 RID: 45585 RVA: 0x00073B6D File Offset: 0x00071D6D
		// (set) Token: 0x0600B212 RID: 45586 RVA: 0x00073B75 File Offset: 0x00071D75
		public string Name
		{
			get
			{
				return this.nameInt;
			}
			set
			{
				this.nameInt = value;
			}
		}

		// Token: 0x17001A49 RID: 6729
		// (get) Token: 0x0600B213 RID: 45587 RVA: 0x00073B7E File Offset: 0x00071D7E
		public override Vector3 DrawPos
		{
			get
			{
				return this.tweener.TweenedPos;
			}
		}

		// Token: 0x17001A4A RID: 6730
		// (get) Token: 0x0600B214 RID: 45588 RVA: 0x00073B8B File Offset: 0x00071D8B
		public bool IsPlayerControlled
		{
			get
			{
				return base.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x17001A4B RID: 6731
		// (get) Token: 0x0600B215 RID: 45589 RVA: 0x00339CDC File Offset: 0x00337EDC
		public bool ImmobilizedByMass
		{
			get
			{
				if (Find.TickManager.TicksGame - this.cachedImmobilizedForTicks < 60)
				{
					return this.cachedImmobilized;
				}
				this.cachedImmobilized = (this.MassUsage > this.MassCapacity);
				this.cachedImmobilizedForTicks = Find.TickManager.TicksGame;
				return this.cachedImmobilized;
			}
		}

		// Token: 0x17001A4C RID: 6732
		// (get) Token: 0x0600B216 RID: 45590 RVA: 0x00339D30 File Offset: 0x00337F30
		public Pair<float, float> DaysWorthOfFood
		{
			get
			{
				if (Find.TickManager.TicksGame - this.cachedDaysWorthOfFoodForTicks < 3000)
				{
					return this.cachedDaysWorthOfFood;
				}
				this.cachedDaysWorthOfFood = new Pair<float, float>(DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this), DaysUntilRotCalculator.ApproxDaysUntilRot(this));
				this.cachedDaysWorthOfFoodForTicks = Find.TickManager.TicksGame;
				return this.cachedDaysWorthOfFood;
			}
		}

		// Token: 0x17001A4D RID: 6733
		// (get) Token: 0x0600B217 RID: 45591 RVA: 0x00073B9A File Offset: 0x00071D9A
		public bool CantMove
		{
			get
			{
				return this.NightResting || this.AllOwnersHaveMentalBreak || this.AllOwnersDowned || this.ImmobilizedByMass;
			}
		}

		// Token: 0x17001A4E RID: 6734
		// (get) Token: 0x0600B218 RID: 45592 RVA: 0x00073BBC File Offset: 0x00071DBC
		public float MassCapacity
		{
			get
			{
				return CollectionsMassCalculator.Capacity<Pawn>(this.PawnsListForReading, null);
			}
		}

		// Token: 0x17001A4F RID: 6735
		// (get) Token: 0x0600B219 RID: 45593 RVA: 0x00339D8C File Offset: 0x00337F8C
		public string MassCapacityExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				CollectionsMassCalculator.Capacity<Pawn>(this.PawnsListForReading, stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17001A50 RID: 6736
		// (get) Token: 0x0600B21A RID: 45594 RVA: 0x00073BCA File Offset: 0x00071DCA
		public float MassUsage
		{
			get
			{
				return CollectionsMassCalculator.MassUsage<Pawn>(this.PawnsListForReading, IgnorePawnsInventoryMode.DontIgnore, false, false);
			}
		}

		// Token: 0x17001A51 RID: 6737
		// (get) Token: 0x0600B21B RID: 45595 RVA: 0x00339DB4 File Offset: 0x00337FB4
		public bool AllOwnersDowned
		{
			get
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.IsOwner(this.pawns[i]) && !this.pawns[i].Downed)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17001A52 RID: 6738
		// (get) Token: 0x0600B21C RID: 45596 RVA: 0x00339E04 File Offset: 0x00338004
		public bool AllOwnersHaveMentalBreak
		{
			get
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.IsOwner(this.pawns[i]) && !this.pawns[i].InMentalState)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17001A53 RID: 6739
		// (get) Token: 0x0600B21D RID: 45597 RVA: 0x00339E54 File Offset: 0x00338054
		public bool NightResting
		{
			get
			{
				return base.Spawned && (!this.pather.Moving || this.pather.nextTile != this.pather.Destination || !Caravan_PathFollower.IsValidFinalPushDestination(this.pather.Destination) || Mathf.CeilToInt(this.pather.nextTileCostLeft / 1f) > 10000) && CaravanNightRestUtility.RestingNowAt(base.Tile);
			}
		}

		// Token: 0x17001A54 RID: 6740
		// (get) Token: 0x0600B21E RID: 45598 RVA: 0x00073BDA File Offset: 0x00071DDA
		public int LeftRestTicks
		{
			get
			{
				if (!this.NightResting)
				{
					return 0;
				}
				return CaravanNightRestUtility.LeftRestTicksAt(base.Tile);
			}
		}

		// Token: 0x17001A55 RID: 6741
		// (get) Token: 0x0600B21F RID: 45599 RVA: 0x00073BF1 File Offset: 0x00071DF1
		public int LeftNonRestTicks
		{
			get
			{
				if (this.NightResting)
				{
					return 0;
				}
				return CaravanNightRestUtility.LeftNonRestTicksAt(base.Tile);
			}
		}

		// Token: 0x17001A56 RID: 6742
		// (get) Token: 0x0600B220 RID: 45600 RVA: 0x00073C08 File Offset: 0x00071E08
		public override string Label
		{
			get
			{
				if (this.nameInt != null)
				{
					return this.nameInt;
				}
				return base.Label;
			}
		}

		// Token: 0x17001A57 RID: 6743
		// (get) Token: 0x0600B221 RID: 45601 RVA: 0x00073C1F File Offset: 0x00071E1F
		public override bool HasName
		{
			get
			{
				return !this.nameInt.NullOrEmpty();
			}
		}

		// Token: 0x17001A58 RID: 6744
		// (get) Token: 0x0600B222 RID: 45602 RVA: 0x00073C2F File Offset: 0x00071E2F
		public int TicksPerMove
		{
			get
			{
				return CaravanTicksPerMoveUtility.GetTicksPerMove(this, null);
			}
		}

		// Token: 0x17001A59 RID: 6745
		// (get) Token: 0x0600B223 RID: 45603 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AppendFactionToInspectString
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001A5A RID: 6746
		// (get) Token: 0x0600B224 RID: 45604 RVA: 0x00073C38 File Offset: 0x00071E38
		public float Visibility
		{
			get
			{
				return CaravanVisibilityCalculator.Visibility(this, null);
			}
		}

		// Token: 0x17001A5B RID: 6747
		// (get) Token: 0x0600B225 RID: 45605 RVA: 0x00339ECC File Offset: 0x003380CC
		public string VisibilityExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				CaravanVisibilityCalculator.Visibility(this, stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17001A5C RID: 6748
		// (get) Token: 0x0600B226 RID: 45606 RVA: 0x00339EF0 File Offset: 0x003380F0
		public string TicksPerMoveExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				CaravanTicksPerMoveUtility.GetTicksPerMove(this, stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17001A5D RID: 6749
		// (get) Token: 0x0600B227 RID: 45607 RVA: 0x00073C41 File Offset: 0x00071E41
		public IEnumerable<Thing> AllThings
		{
			get
			{
				return CaravanInventoryUtility.AllInventoryItems(this).Concat(this.pawns);
			}
		}

		// Token: 0x17001A5E RID: 6750
		// (get) Token: 0x0600B228 RID: 45608 RVA: 0x00073C54 File Offset: 0x00071E54
		public int ConstantRandSeed
		{
			get
			{
				return this.uniqueId ^ 728241121;
			}
		}

		// Token: 0x17001A5F RID: 6751
		// (get) Token: 0x0600B229 RID: 45609 RVA: 0x00073C62 File Offset: 0x00071E62
		public StoryState StoryState
		{
			get
			{
				return this.storyState;
			}
		}

		// Token: 0x17001A60 RID: 6752
		// (get) Token: 0x0600B22A RID: 45610 RVA: 0x00073C6A File Offset: 0x00071E6A
		public GameConditionManager GameConditionManager
		{
			get
			{
				Log.ErrorOnce("Attempted to retrieve condition manager directly from caravan", 13291050, false);
				return null;
			}
		}

		// Token: 0x17001A61 RID: 6753
		// (get) Token: 0x0600B22B RID: 45611 RVA: 0x00339F14 File Offset: 0x00338114
		public float PlayerWealthForStoryteller
		{
			get
			{
				if (!this.IsPlayerControlled)
				{
					return 0f;
				}
				float num = 0f;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					num += WealthWatcher.GetEquipmentApparelAndInventoryWealth(this.pawns[i]);
					if (this.pawns[i].Faction == Faction.OfPlayer)
					{
						num += this.pawns[i].MarketValue;
					}
				}
				return num * 0.7f;
			}
		}

		// Token: 0x17001A62 RID: 6754
		// (get) Token: 0x0600B22C RID: 45612 RVA: 0x00073C7D File Offset: 0x00071E7D
		public IEnumerable<Pawn> PlayerPawnsForStoryteller
		{
			get
			{
				if (!this.IsPlayerControlled)
				{
					return Enumerable.Empty<Pawn>();
				}
				return from x in this.PawnsListForReading
				where x.Faction == Faction.OfPlayer
				select x;
			}
		}

		// Token: 0x17001A63 RID: 6755
		// (get) Token: 0x0600B22D RID: 45613 RVA: 0x00073CB7 File Offset: 0x00071EB7
		public FloatRange IncidentPointsRandomFactorRange
		{
			get
			{
				return StorytellerUtility.CaravanPointsRandomFactorRange;
			}
		}

		// Token: 0x0600B22E RID: 45614 RVA: 0x00339F94 File Offset: 0x00338194
		public void SetUniqueId(int newId)
		{
			if (this.uniqueId != -1 || newId < 0)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to set caravan with uniqueId ",
					this.uniqueId,
					" to have uniqueId ",
					newId
				}), false);
			}
			this.uniqueId = newId;
		}

		// Token: 0x17001A64 RID: 6756
		// (get) Token: 0x0600B22F RID: 45615 RVA: 0x00073CBE File Offset: 0x00071EBE
		public TraderKindDef TraderKind
		{
			get
			{
				return this.trader.TraderKind;
			}
		}

		// Token: 0x17001A65 RID: 6757
		// (get) Token: 0x0600B230 RID: 45616 RVA: 0x00073CCB File Offset: 0x00071ECB
		public IEnumerable<Thing> Goods
		{
			get
			{
				return this.trader.Goods;
			}
		}

		// Token: 0x17001A66 RID: 6758
		// (get) Token: 0x0600B231 RID: 45617 RVA: 0x00073CD8 File Offset: 0x00071ED8
		public int RandomPriceFactorSeed
		{
			get
			{
				return this.trader.RandomPriceFactorSeed;
			}
		}

		// Token: 0x17001A67 RID: 6759
		// (get) Token: 0x0600B232 RID: 45618 RVA: 0x00073CE5 File Offset: 0x00071EE5
		public string TraderName
		{
			get
			{
				return this.trader.TraderName;
			}
		}

		// Token: 0x17001A68 RID: 6760
		// (get) Token: 0x0600B233 RID: 45619 RVA: 0x00073CF2 File Offset: 0x00071EF2
		public bool CanTradeNow
		{
			get
			{
				return this.trader.CanTradeNow;
			}
		}

		// Token: 0x17001A69 RID: 6761
		// (get) Token: 0x0600B234 RID: 45620 RVA: 0x00016647 File Offset: 0x00014847
		public float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17001A6A RID: 6762
		// (get) Token: 0x0600B235 RID: 45621 RVA: 0x00073CFF File Offset: 0x00071EFF
		public TradeCurrency TradeCurrency
		{
			get
			{
				return this.TraderKind.tradeCurrency;
			}
		}

		// Token: 0x0600B236 RID: 45622 RVA: 0x00073D0C File Offset: 0x00071F0C
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			return this.trader.ColonyThingsWillingToBuy(playerNegotiator);
		}

		// Token: 0x0600B237 RID: 45623 RVA: 0x00073D1A File Offset: 0x00071F1A
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToTrader(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x0600B238 RID: 45624 RVA: 0x00073D2A File Offset: 0x00071F2A
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToPlayer(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x0600B239 RID: 45625 RVA: 0x00339FF0 File Offset: 0x003381F0
		public Caravan()
		{
			this.pawns = new ThingOwner<Pawn>(this, false, LookMode.Reference);
			this.pather = new Caravan_PathFollower(this);
			this.gotoMote = new Caravan_GotoMoteRenderer();
			this.tweener = new Caravan_Tweener(this);
			this.trader = new Caravan_TraderTracker(this);
			this.forage = new Caravan_ForageTracker(this);
			this.needs = new Caravan_NeedsTracker(this);
			this.carryTracker = new Caravan_CarryTracker(this);
			this.beds = new Caravan_BedsTracker(this);
			this.storyState = new StoryState(this);
		}

		// Token: 0x0600B23A RID: 45626 RVA: 0x0033A09C File Offset: 0x0033829C
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
			}
			Scribe_Values.Look<int>(ref this.uniqueId, "uniqueId", 0, false);
			Scribe_Values.Look<string>(ref this.nameInt, "name", null, false);
			Scribe_Deep.Look<ThingOwner<Pawn>>(ref this.pawns, "pawns", new object[]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.autoJoinable, "autoJoinable", false, false);
			Scribe_Deep.Look<Caravan_PathFollower>(ref this.pather, "pather", new object[]
			{
				this
			});
			Scribe_Deep.Look<Caravan_TraderTracker>(ref this.trader, "trader", new object[]
			{
				this
			});
			Scribe_Deep.Look<Caravan_ForageTracker>(ref this.forage, "forage", new object[]
			{
				this
			});
			Scribe_Deep.Look<Caravan_NeedsTracker>(ref this.needs, "needs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Caravan_CarryTracker>(ref this.carryTracker, "carryTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Caravan_BedsTracker>(ref this.beds, "beds", new object[]
			{
				this
			});
			Scribe_Deep.Look<StoryState>(ref this.storyState, "storyState", new object[]
			{
				this
			});
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600B23B RID: 45627 RVA: 0x00073D3A File Offset: 0x00071F3A
		public override void PostAdd()
		{
			base.PostAdd();
			this.carryTracker.Notify_CaravanSpawned();
			this.beds.Notify_CaravanSpawned();
			Find.ColonistBar.MarkColonistsDirty();
		}

		// Token: 0x0600B23C RID: 45628 RVA: 0x00073D62 File Offset: 0x00071F62
		public override void PostRemove()
		{
			base.PostRemove();
			this.pather.StopDead();
			Find.ColonistBar.MarkColonistsDirty();
		}

		// Token: 0x0600B23D RID: 45629 RVA: 0x0033A1F0 File Offset: 0x003383F0
		public override void Tick()
		{
			base.Tick();
			this.CheckAnyNonWorldPawns();
			this.pather.PatherTick();
			this.tweener.TweenerTick();
			this.forage.ForageTrackerTick();
			this.carryTracker.CarryTrackerTick();
			this.beds.BedsTrackerTick();
			this.needs.NeedsTrackerTick();
			CaravanDrugPolicyUtility.CheckTakeScheduledDrugs(this);
			CaravanTendUtility.CheckTend(this);
		}

		// Token: 0x0600B23E RID: 45630 RVA: 0x00073D7F File Offset: 0x00071F7F
		public override void SpawnSetup()
		{
			base.SpawnSetup();
			this.tweener.ResetTweenedPosToRoot();
		}

		// Token: 0x0600B23F RID: 45631 RVA: 0x00073D92 File Offset: 0x00071F92
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.IsPlayerControlled && this.pather.curPath != null)
			{
				this.pather.curPath.DrawPath(this);
			}
			this.gotoMote.RenderMote();
		}

		// Token: 0x0600B240 RID: 45632 RVA: 0x0033A258 File Offset: 0x00338458
		public void AddPawn(Pawn p, bool addCarriedPawnToWorldPawnsIfAny)
		{
			if (p == null)
			{
				Log.Warning("Tried to add a null pawn to " + this, false);
				return;
			}
			if (p.Dead)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to add ",
					p,
					" to ",
					this,
					", but this pawn is dead."
				}), false);
				return;
			}
			Pawn pawn = p.carryTracker.CarriedThing as Pawn;
			if (pawn != null)
			{
				p.carryTracker.innerContainer.Remove(pawn);
			}
			if (p.Spawned)
			{
				p.DeSpawn(DestroyMode.Vanish);
			}
			if (this.pawns.TryAdd(p, true))
			{
				if (this.ShouldAutoCapture(p))
				{
					p.guest.CapturedBy(base.Faction, null);
				}
				if (pawn != null)
				{
					if (this.ShouldAutoCapture(pawn))
					{
						pawn.guest.CapturedBy(base.Faction, p);
					}
					this.AddPawn(pawn, addCarriedPawnToWorldPawnsIfAny);
					if (addCarriedPawnToWorldPawnsIfAny)
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
						return;
					}
				}
			}
			else
			{
				Log.Error("Couldn't add pawn " + p + " to caravan.", false);
			}
		}

		// Token: 0x0600B241 RID: 45633 RVA: 0x0033A360 File Offset: 0x00338560
		public void AddPawnOrItem(Thing thing, bool addCarriedPawnToWorldPawnsIfAny)
		{
			if (thing == null)
			{
				Log.Warning("Tried to add a null thing to " + this, false);
				return;
			}
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				this.AddPawn(pawn, addCarriedPawnToWorldPawnsIfAny);
				return;
			}
			CaravanInventoryUtility.GiveThing(this, thing);
		}

		// Token: 0x0600B242 RID: 45634 RVA: 0x00073DCB File Offset: 0x00071FCB
		public bool ContainsPawn(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x0600B243 RID: 45635 RVA: 0x00073DD9 File Offset: 0x00071FD9
		public void RemovePawn(Pawn p)
		{
			this.pawns.Remove(p);
		}

		// Token: 0x0600B244 RID: 45636 RVA: 0x00073DE8 File Offset: 0x00071FE8
		public void RemoveAllPawns()
		{
			this.pawns.Clear();
		}

		// Token: 0x0600B245 RID: 45637 RVA: 0x00073DF5 File Offset: 0x00071FF5
		public bool IsOwner(Pawn p)
		{
			return this.pawns.Contains(p) && CaravanUtility.IsOwner(p, base.Faction);
		}

		// Token: 0x0600B246 RID: 45638 RVA: 0x0033A39C File Offset: 0x0033859C
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (stringBuilder.Length != 0)
			{
				stringBuilder.AppendLine();
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			for (int i = 0; i < this.pawns.Count; i++)
			{
				if (this.pawns[i].IsColonist)
				{
					num++;
				}
				else if (this.pawns[i].RaceProps.Animal)
				{
					num2++;
				}
				else if (this.pawns[i].IsPrisoner)
				{
					num3++;
				}
				if (this.pawns[i].Downed)
				{
					num4++;
				}
				if (this.pawns[i].InMentalState)
				{
					num5++;
				}
			}
			stringBuilder.Append("CaravanColonistsCount".Translate(num, (num == 1) ? Faction.OfPlayer.def.pawnSingular : Faction.OfPlayer.def.pawnsPlural));
			if (num2 == 1)
			{
				stringBuilder.Append(", " + "CaravanAnimal".Translate());
			}
			else if (num2 > 1)
			{
				stringBuilder.Append(", " + "CaravanAnimalsCount".Translate(num2));
			}
			if (num3 == 1)
			{
				stringBuilder.Append(", " + "CaravanPrisoner".Translate());
			}
			else if (num3 > 1)
			{
				stringBuilder.Append(", " + "CaravanPrisonersCount".Translate(num3));
			}
			stringBuilder.AppendLine();
			if (num5 > 0)
			{
				stringBuilder.Append("CaravanPawnsInMentalState".Translate(num5));
			}
			if (num4 > 0)
			{
				if (num5 > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("CaravanPawnsDowned".Translate(num4));
			}
			if (num5 > 0 || num4 > 0)
			{
				stringBuilder.AppendLine();
			}
			if (this.pather.Moving)
			{
				if (this.pather.ArrivalAction != null)
				{
					stringBuilder.Append(this.pather.ArrivalAction.ReportString);
				}
				else
				{
					stringBuilder.Append("CaravanTraveling".Translate());
				}
			}
			else
			{
				Settlement settlement = CaravanVisitUtility.SettlementVisitedNow(this);
				if (settlement != null)
				{
					stringBuilder.Append("CaravanVisiting".Translate(settlement.Label));
				}
				else
				{
					stringBuilder.Append("CaravanWaiting".Translate());
				}
			}
			if (this.pather.Moving)
			{
				float num6 = (float)CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this, true) / 60000f;
				stringBuilder.AppendLine();
				stringBuilder.Append("CaravanEstimatedTimeToDestination".Translate(num6.ToString("0.#")));
			}
			if (this.AllOwnersDowned)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("AllCaravanMembersDowned".Translate());
			}
			else if (this.AllOwnersHaveMentalBreak)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("AllCaravanMembersMentalBreak".Translate());
			}
			else if (this.ImmobilizedByMass)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("CaravanImmobilizedByMass".Translate());
			}
			string text;
			if (this.needs.AnyPawnOutOfFood(out text))
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("CaravanOutOfFood".Translate());
				if (!text.NullOrEmpty())
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(text);
					stringBuilder.Append(".");
				}
			}
			if (!this.pather.MovingNow)
			{
				int usedBedCount = this.beds.GetUsedBedCount();
				stringBuilder.AppendLine();
				stringBuilder.Append(CaravanBedUtility.AppendUsingBedsLabel("CaravanResting".Translate(), usedBedCount));
			}
			else
			{
				string inspectStringLine = this.carryTracker.GetInspectStringLine();
				if (!inspectStringLine.NullOrEmpty())
				{
					stringBuilder.AppendLine();
					stringBuilder.Append(inspectStringLine);
				}
				string inBedForMedicalReasonsInspectStringLine = this.beds.GetInBedForMedicalReasonsInspectStringLine();
				if (!inBedForMedicalReasonsInspectStringLine.NullOrEmpty())
				{
					stringBuilder.AppendLine();
					stringBuilder.Append(inBedForMedicalReasonsInspectStringLine);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600B247 RID: 45639 RVA: 0x00073E13 File Offset: 0x00072013
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (Find.WorldSelector.SingleSelectedObject == this)
			{
				yield return new Gizmo_CaravanInfo(this);
			}
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.IsPlayerControlled)
			{
				List<Pawn>.Enumerator enumerator2;
				if (Find.WorldSelector.SingleSelectedObject == this)
				{
					yield return SettleInEmptyTileUtility.SettleCommand(this);
					foreach (Pawn p in this.pawns)
					{
						if (p.royalty != null)
						{
							foreach (FactionPermit factionPermit in p.royalty.AllFactionPermits)
							{
								IEnumerable<Gizmo> caravanGizmos = factionPermit.Permit.Worker.GetCaravanGizmos(p, factionPermit.Faction);
								if (caravanGizmos != null)
								{
									foreach (Gizmo gizmo2 in caravanGizmos)
									{
										yield return gizmo2;
									}
									enumerator = null;
								}
							}
							List<FactionPermit>.Enumerator enumerator3 = default(List<FactionPermit>.Enumerator);
						}
						p = null;
					}
					enumerator2 = default(List<Pawn>.Enumerator);
				}
				if (Find.WorldSelector.SingleSelectedObject == this)
				{
					if (this.PawnsListForReading.Count((Pawn x) => x.IsColonist) >= 2)
					{
						yield return new Command_Action
						{
							defaultLabel = "CommandSplitCaravan".Translate(),
							defaultDesc = "CommandSplitCaravanDesc".Translate(),
							icon = Caravan.SplitCommand,
							hotKey = KeyBindingDefOf.Misc5,
							action = delegate()
							{
								Find.WindowStack.Add(new Dialog_SplitCaravan(this));
							}
						};
					}
				}
				if (this.pather.Moving)
				{
					yield return new Command_Toggle
					{
						hotKey = KeyBindingDefOf.Misc1,
						isActive = (() => this.pather.Paused),
						toggleAction = delegate()
						{
							if (!this.pather.Moving)
							{
								return;
							}
							this.pather.Paused = !this.pather.Paused;
						},
						defaultDesc = "CommandToggleCaravanPauseDesc".Translate(2f.ToString("0.#"), 0.3f.ToStringPercent()),
						icon = TexCommand.PauseCaravan,
						defaultLabel = "CommandPauseCaravan".Translate()
					};
				}
				if (CaravanMergeUtility.ShouldShowMergeCommand)
				{
					yield return CaravanMergeUtility.MergeCommand(this);
				}
				foreach (Gizmo gizmo3 in this.forage.GetGizmos())
				{
					yield return gizmo3;
				}
				enumerator = null;
				foreach (WorldObject worldObject in Find.WorldObjects.ObjectsAt(base.Tile))
				{
					foreach (Gizmo gizmo4 in worldObject.GetCaravanGizmos(this))
					{
						yield return gizmo4;
					}
					enumerator = null;
				}
				IEnumerator<WorldObject> enumerator4 = null;
				foreach (Pawn pawn in this.pawns)
				{
					if (pawn.abilities != null && !pawn.Downed && !pawn.IsPrisoner)
					{
						foreach (Ability ability in pawn.abilities.abilities)
						{
							if (ability.def.showGizmoOnWorldView)
							{
								foreach (Command command in ability.GetGizmos())
								{
									yield return command;
								}
								IEnumerator<Command> enumerator6 = null;
							}
						}
						List<Ability>.Enumerator enumerator5 = default(List<Ability>.Enumerator);
					}
				}
				enumerator2 = default(List<Pawn>.Enumerator);
			}
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "Dev: Mental break",
					action = delegate()
					{
						Pawn pawn2;
						if ((from x in this.PawnsListForReading
						where x.RaceProps.Humanlike && !x.InMentalState
						select x).TryRandomElement(out pawn2))
						{
							pawn2.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_Sad, null, false, false, null, false);
						}
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Dev: Make random pawn hungry",
					action = delegate()
					{
						Pawn pawn2;
						if ((from x in this.PawnsListForReading
						where x.needs.food != null
						select x).TryRandomElement(out pawn2))
						{
							pawn2.needs.food.CurLevelPercentage = 0f;
						}
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Dev: Kill random pawn",
					action = delegate()
					{
						Pawn pawn2;
						if (this.PawnsListForReading.TryRandomElement(out pawn2))
						{
							pawn2.Kill(null, null);
							Messages.Message("Dev: Killed " + pawn2.LabelShort, this, MessageTypeDefOf.TaskCompletion, false);
						}
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Dev: Harm random pawn",
					action = delegate()
					{
						Pawn pawn2;
						if (this.PawnsListForReading.TryRandomElement(out pawn2))
						{
							DamageInfo dinfo = new DamageInfo(DamageDefOf.Scratch, 10f, 999f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
							pawn2.TakeDamage(dinfo);
						}
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Dev: Down random pawn",
					action = delegate()
					{
						Pawn pawn2;
						if ((from x in this.PawnsListForReading
						where !x.Downed
						select x).TryRandomElement(out pawn2))
						{
							HealthUtility.DamageUntilDowned(pawn2, true);
							Messages.Message("Dev: Downed " + pawn2.LabelShort, this, MessageTypeDefOf.TaskCompletion, false);
						}
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Dev: Plague on random pawn",
					action = delegate()
					{
						Pawn pawn2;
						if ((from x in this.PawnsListForReading
						where !x.Downed
						select x).TryRandomElement(out pawn2))
						{
							Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.Plague, pawn2, null);
							hediff.Severity = HediffDefOf.Plague.stages[1].minSeverity - 0.001f;
							pawn2.health.AddHediff(hediff, null, null, null);
							Messages.Message("Dev: Gave advanced plague to " + pawn2.LabelShort, this, MessageTypeDefOf.TaskCompletion, false);
						}
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Dev: Teleport to destination",
					action = delegate()
					{
						base.Tile = this.pather.Destination;
						this.pather.StopDead();
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Dev: +20% psyfocus",
					action = delegate()
					{
						for (int i = 0; i < this.PawnsListForReading.Count; i++)
						{
							Pawn_PsychicEntropyTracker psychicEntropy = this.PawnsListForReading[i].psychicEntropy;
							if (psychicEntropy != null)
							{
								psychicEntropy.OffsetPsyfocusDirectly(0.2f);
							}
						}
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600B248 RID: 45640 RVA: 0x00073E23 File Offset: 0x00072023
		public override IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetTransportPodsFloatMenuOptions(pods, representative))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			foreach (FloatMenuOption floatMenuOption2 in TransportPodsArrivalAction_GiveToCaravan.GetFloatMenuOptions(representative, pods, this))
			{
				yield return floatMenuOption2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600B249 RID: 45641 RVA: 0x00073E41 File Offset: 0x00072041
		public void RecacheImmobilizedNow()
		{
			this.cachedImmobilizedForTicks = -99999;
		}

		// Token: 0x0600B24A RID: 45642 RVA: 0x00073E4E File Offset: 0x0007204E
		public void RecacheDaysWorthOfFood()
		{
			this.cachedDaysWorthOfFoodForTicks = -99999;
		}

		// Token: 0x0600B24B RID: 45643 RVA: 0x0033A808 File Offset: 0x00338A08
		public virtual void Notify_MemberDied(Pawn member)
		{
			if (!base.Spawned)
			{
				Log.Error("Caravan member died in an unspawned caravan. Unspawned caravans shouldn't be kept for more than a single frame.", false);
			}
			if (!this.PawnsListForReading.Any((Pawn x) => x != member && this.IsOwner(x)))
			{
				this.RemovePawn(member);
				if (base.Faction == Faction.OfPlayer)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelAllCaravanColonistsDied".Translate(), "LetterAllCaravanColonistsDied".Translate(this.Name).CapitalizeFirst(), LetterDefOf.NegativeEvent, new GlobalTargetInfo(base.Tile), null, null, null, null);
				}
				this.pawns.Clear();
				this.Destroy();
				return;
			}
			member.Strip();
			this.RemovePawn(member);
		}

		// Token: 0x0600B24C RID: 45644 RVA: 0x00073E5B File Offset: 0x0007205B
		public virtual void Notify_Merged(List<Caravan> group)
		{
			this.notifiedOutOfFood = false;
		}

		// Token: 0x0600B24D RID: 45645 RVA: 0x00073E5B File Offset: 0x0007205B
		public virtual void Notify_StartedTrading()
		{
			this.notifiedOutOfFood = false;
		}

		// Token: 0x0600B24E RID: 45646 RVA: 0x0033A8E4 File Offset: 0x00338AE4
		private void CheckAnyNonWorldPawns()
		{
			for (int i = this.pawns.Count - 1; i >= 0; i--)
			{
				if (!this.pawns[i].IsWorldPawn())
				{
					Log.Error("Caravan member " + this.pawns[i] + " is not a world pawn. Removing...", false);
					this.pawns.Remove(this.pawns[i]);
				}
			}
		}

		// Token: 0x0600B24F RID: 45647 RVA: 0x00073E64 File Offset: 0x00072064
		private bool ShouldAutoCapture(Pawn p)
		{
			return CaravanUtility.ShouldAutoCapture(p, base.Faction);
		}

		// Token: 0x0600B250 RID: 45648 RVA: 0x00073E72 File Offset: 0x00072072
		public void Notify_PawnRemoved(Pawn p)
		{
			Find.ColonistBar.MarkColonistsDirty();
			this.RecacheImmobilizedNow();
			this.RecacheDaysWorthOfFood();
			this.carryTracker.Notify_PawnRemoved();
			this.beds.Notify_PawnRemoved();
		}

		// Token: 0x0600B251 RID: 45649 RVA: 0x00073EA0 File Offset: 0x000720A0
		public void Notify_PawnAdded(Pawn p)
		{
			Find.ColonistBar.MarkColonistsDirty();
			this.RecacheImmobilizedNow();
			this.RecacheDaysWorthOfFood();
		}

		// Token: 0x0600B252 RID: 45650 RVA: 0x00073EB8 File Offset: 0x000720B8
		public void Notify_DestinationOrPauseStatusChanged()
		{
			this.RecacheDaysWorthOfFood();
		}

		// Token: 0x0600B253 RID: 45651 RVA: 0x00073EC0 File Offset: 0x000720C0
		public void Notify_Teleported()
		{
			this.tweener.ResetTweenedPosToRoot();
			this.pather.Notify_Teleported_Int();
		}

		// Token: 0x0600B254 RID: 45652 RVA: 0x00073ED8 File Offset: 0x000720D8
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.pawns;
		}

		// Token: 0x0600B255 RID: 45653 RVA: 0x00073EE0 File Offset: 0x000720E0
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x04007A8F RID: 31375
		private int uniqueId = -1;

		// Token: 0x04007A90 RID: 31376
		private string nameInt;

		// Token: 0x04007A91 RID: 31377
		public ThingOwner<Pawn> pawns;

		// Token: 0x04007A92 RID: 31378
		public bool autoJoinable;

		// Token: 0x04007A93 RID: 31379
		public Caravan_PathFollower pather;

		// Token: 0x04007A94 RID: 31380
		public Caravan_GotoMoteRenderer gotoMote;

		// Token: 0x04007A95 RID: 31381
		public Caravan_Tweener tweener;

		// Token: 0x04007A96 RID: 31382
		public Caravan_TraderTracker trader;

		// Token: 0x04007A97 RID: 31383
		public Caravan_ForageTracker forage;

		// Token: 0x04007A98 RID: 31384
		public Caravan_NeedsTracker needs;

		// Token: 0x04007A99 RID: 31385
		public Caravan_CarryTracker carryTracker;

		// Token: 0x04007A9A RID: 31386
		public Caravan_BedsTracker beds;

		// Token: 0x04007A9B RID: 31387
		public StoryState storyState;

		// Token: 0x04007A9C RID: 31388
		private Material cachedMat;

		// Token: 0x04007A9D RID: 31389
		private bool cachedImmobilized;

		// Token: 0x04007A9E RID: 31390
		private int cachedImmobilizedForTicks = -99999;

		// Token: 0x04007A9F RID: 31391
		private Pair<float, float> cachedDaysWorthOfFood;

		// Token: 0x04007AA0 RID: 31392
		private int cachedDaysWorthOfFoodForTicks = -99999;

		// Token: 0x04007AA1 RID: 31393
		public bool notifiedOutOfFood;

		// Token: 0x04007AA2 RID: 31394
		private const int ImmobilizedCacheDuration = 60;

		// Token: 0x04007AA3 RID: 31395
		private const int DaysWorthOfFoodCacheDuration = 3000;

		// Token: 0x04007AA4 RID: 31396
		private static readonly Texture2D SplitCommand = ContentFinder<Texture2D>.Get("UI/Commands/SplitCaravan", true);

		// Token: 0x04007AA5 RID: 31397
		private static readonly Color PlayerCaravanColor = new Color(1f, 0.863f, 0.33f);
	}
}
