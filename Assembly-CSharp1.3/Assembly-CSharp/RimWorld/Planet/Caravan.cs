using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200179B RID: 6043
	[StaticConstructorOnStartup]
	public class Caravan : WorldObject, IThingHolder, IIncidentTarget, ILoadReferenceable, ITrader
	{
		// Token: 0x170016C6 RID: 5830
		// (get) Token: 0x06008BBC RID: 35772 RVA: 0x0032275D File Offset: 0x0032095D
		public List<Pawn> PawnsListForReading
		{
			get
			{
				return this.pawns.InnerListForReading;
			}
		}

		// Token: 0x170016C7 RID: 5831
		// (get) Token: 0x06008BBD RID: 35773 RVA: 0x0032276C File Offset: 0x0032096C
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

		// Token: 0x170016C8 RID: 5832
		// (get) Token: 0x06008BBE RID: 35774 RVA: 0x003227DF File Offset: 0x003209DF
		// (set) Token: 0x06008BBF RID: 35775 RVA: 0x003227E7 File Offset: 0x003209E7
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

		// Token: 0x170016C9 RID: 5833
		// (get) Token: 0x06008BC0 RID: 35776 RVA: 0x003227F0 File Offset: 0x003209F0
		public override Vector3 DrawPos
		{
			get
			{
				return this.tweener.TweenedPos;
			}
		}

		// Token: 0x170016CA RID: 5834
		// (get) Token: 0x06008BC1 RID: 35777 RVA: 0x003227FD File Offset: 0x003209FD
		public bool IsPlayerControlled
		{
			get
			{
				return base.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x170016CB RID: 5835
		// (get) Token: 0x06008BC2 RID: 35778 RVA: 0x0032280C File Offset: 0x00320A0C
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

		// Token: 0x170016CC RID: 5836
		// (get) Token: 0x06008BC3 RID: 35779 RVA: 0x00322860 File Offset: 0x00320A60
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

		// Token: 0x170016CD RID: 5837
		// (get) Token: 0x06008BC4 RID: 35780 RVA: 0x003228B9 File Offset: 0x00320AB9
		public bool CantMove
		{
			get
			{
				return this.NightResting || this.AllOwnersHaveMentalBreak || this.AllOwnersDowned || this.ImmobilizedByMass;
			}
		}

		// Token: 0x170016CE RID: 5838
		// (get) Token: 0x06008BC5 RID: 35781 RVA: 0x003228DB File Offset: 0x00320ADB
		public float MassCapacity
		{
			get
			{
				return CollectionsMassCalculator.Capacity<Pawn>(this.PawnsListForReading, null);
			}
		}

		// Token: 0x170016CF RID: 5839
		// (get) Token: 0x06008BC6 RID: 35782 RVA: 0x003228EC File Offset: 0x00320AEC
		public string MassCapacityExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				CollectionsMassCalculator.Capacity<Pawn>(this.PawnsListForReading, stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170016D0 RID: 5840
		// (get) Token: 0x06008BC7 RID: 35783 RVA: 0x00322912 File Offset: 0x00320B12
		public float MassUsage
		{
			get
			{
				return CollectionsMassCalculator.MassUsage<Pawn>(this.PawnsListForReading, IgnorePawnsInventoryMode.DontIgnore, false, false);
			}
		}

		// Token: 0x170016D1 RID: 5841
		// (get) Token: 0x06008BC8 RID: 35784 RVA: 0x00322924 File Offset: 0x00320B24
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

		// Token: 0x170016D2 RID: 5842
		// (get) Token: 0x06008BC9 RID: 35785 RVA: 0x00322974 File Offset: 0x00320B74
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

		// Token: 0x170016D3 RID: 5843
		// (get) Token: 0x06008BCA RID: 35786 RVA: 0x003229C4 File Offset: 0x00320BC4
		public bool NightResting
		{
			get
			{
				return base.Spawned && (!this.pather.Moving || this.pather.nextTile != this.pather.Destination || !Caravan_PathFollower.IsValidFinalPushDestination(this.pather.Destination) || Mathf.CeilToInt(this.pather.nextTileCostLeft / 1f) > 10000) && CaravanNightRestUtility.RestingNowAt(base.Tile);
			}
		}

		// Token: 0x170016D4 RID: 5844
		// (get) Token: 0x06008BCB RID: 35787 RVA: 0x00322A3C File Offset: 0x00320C3C
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

		// Token: 0x170016D5 RID: 5845
		// (get) Token: 0x06008BCC RID: 35788 RVA: 0x00322A53 File Offset: 0x00320C53
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

		// Token: 0x170016D6 RID: 5846
		// (get) Token: 0x06008BCD RID: 35789 RVA: 0x00322A6A File Offset: 0x00320C6A
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

		// Token: 0x170016D7 RID: 5847
		// (get) Token: 0x06008BCE RID: 35790 RVA: 0x00322A81 File Offset: 0x00320C81
		public override bool HasName
		{
			get
			{
				return !this.nameInt.NullOrEmpty();
			}
		}

		// Token: 0x170016D8 RID: 5848
		// (get) Token: 0x06008BCF RID: 35791 RVA: 0x00322A91 File Offset: 0x00320C91
		public int TicksPerMove
		{
			get
			{
				return CaravanTicksPerMoveUtility.GetTicksPerMove(this, null);
			}
		}

		// Token: 0x170016D9 RID: 5849
		// (get) Token: 0x06008BD0 RID: 35792 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AppendFactionToInspectString
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170016DA RID: 5850
		// (get) Token: 0x06008BD1 RID: 35793 RVA: 0x00322A9A File Offset: 0x00320C9A
		public float Visibility
		{
			get
			{
				return CaravanVisibilityCalculator.Visibility(this, null);
			}
		}

		// Token: 0x170016DB RID: 5851
		// (get) Token: 0x06008BD2 RID: 35794 RVA: 0x00322AA4 File Offset: 0x00320CA4
		public string VisibilityExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				CaravanVisibilityCalculator.Visibility(this, stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170016DC RID: 5852
		// (get) Token: 0x06008BD3 RID: 35795 RVA: 0x00322AC8 File Offset: 0x00320CC8
		public string TicksPerMoveExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				CaravanTicksPerMoveUtility.GetTicksPerMove(this, stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170016DD RID: 5853
		// (get) Token: 0x06008BD4 RID: 35796 RVA: 0x00322AE9 File Offset: 0x00320CE9
		public IEnumerable<Thing> AllThings
		{
			get
			{
				return CaravanInventoryUtility.AllInventoryItems(this).Concat(this.pawns);
			}
		}

		// Token: 0x170016DE RID: 5854
		// (get) Token: 0x06008BD5 RID: 35797 RVA: 0x00322AFC File Offset: 0x00320CFC
		public int ConstantRandSeed
		{
			get
			{
				return this.uniqueId ^ 728241121;
			}
		}

		// Token: 0x170016DF RID: 5855
		// (get) Token: 0x06008BD6 RID: 35798 RVA: 0x00322B0A File Offset: 0x00320D0A
		public StoryState StoryState
		{
			get
			{
				return this.storyState;
			}
		}

		// Token: 0x170016E0 RID: 5856
		// (get) Token: 0x06008BD7 RID: 35799 RVA: 0x00322B12 File Offset: 0x00320D12
		public GameConditionManager GameConditionManager
		{
			get
			{
				Log.ErrorOnce("Attempted to retrieve condition manager directly from caravan", 13291050);
				return null;
			}
		}

		// Token: 0x170016E1 RID: 5857
		// (get) Token: 0x06008BD8 RID: 35800 RVA: 0x00322B24 File Offset: 0x00320D24
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

		// Token: 0x170016E2 RID: 5858
		// (get) Token: 0x06008BD9 RID: 35801 RVA: 0x00322BA2 File Offset: 0x00320DA2
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

		// Token: 0x170016E3 RID: 5859
		// (get) Token: 0x06008BDA RID: 35802 RVA: 0x00322BDC File Offset: 0x00320DDC
		public FloatRange IncidentPointsRandomFactorRange
		{
			get
			{
				return StorytellerUtility.CaravanPointsRandomFactorRange;
			}
		}

		// Token: 0x06008BDB RID: 35803 RVA: 0x00322BE4 File Offset: 0x00320DE4
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
				}));
			}
			this.uniqueId = newId;
		}

		// Token: 0x170016E4 RID: 5860
		// (get) Token: 0x06008BDC RID: 35804 RVA: 0x00322C3C File Offset: 0x00320E3C
		public TraderKindDef TraderKind
		{
			get
			{
				return this.trader.TraderKind;
			}
		}

		// Token: 0x170016E5 RID: 5861
		// (get) Token: 0x06008BDD RID: 35805 RVA: 0x00322C49 File Offset: 0x00320E49
		public IEnumerable<Thing> Goods
		{
			get
			{
				return this.trader.Goods;
			}
		}

		// Token: 0x170016E6 RID: 5862
		// (get) Token: 0x06008BDE RID: 35806 RVA: 0x00322C56 File Offset: 0x00320E56
		public int RandomPriceFactorSeed
		{
			get
			{
				return this.trader.RandomPriceFactorSeed;
			}
		}

		// Token: 0x170016E7 RID: 5863
		// (get) Token: 0x06008BDF RID: 35807 RVA: 0x00322C63 File Offset: 0x00320E63
		public string TraderName
		{
			get
			{
				return this.trader.TraderName;
			}
		}

		// Token: 0x170016E8 RID: 5864
		// (get) Token: 0x06008BE0 RID: 35808 RVA: 0x00322C70 File Offset: 0x00320E70
		public bool CanTradeNow
		{
			get
			{
				return this.trader.CanTradeNow;
			}
		}

		// Token: 0x170016E9 RID: 5865
		// (get) Token: 0x06008BE1 RID: 35809 RVA: 0x000682C5 File Offset: 0x000664C5
		public float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170016EA RID: 5866
		// (get) Token: 0x06008BE2 RID: 35810 RVA: 0x00322C7D File Offset: 0x00320E7D
		public TradeCurrency TradeCurrency
		{
			get
			{
				return this.TraderKind.tradeCurrency;
			}
		}

		// Token: 0x06008BE3 RID: 35811 RVA: 0x00322C8A File Offset: 0x00320E8A
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			return this.trader.ColonyThingsWillingToBuy(playerNegotiator);
		}

		// Token: 0x06008BE4 RID: 35812 RVA: 0x00322C98 File Offset: 0x00320E98
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToTrader(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x06008BE5 RID: 35813 RVA: 0x00322CA8 File Offset: 0x00320EA8
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToPlayer(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x06008BE6 RID: 35814 RVA: 0x00322CB8 File Offset: 0x00320EB8
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

		// Token: 0x06008BE7 RID: 35815 RVA: 0x00322D64 File Offset: 0x00320F64
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

		// Token: 0x06008BE8 RID: 35816 RVA: 0x00322EB6 File Offset: 0x003210B6
		public override void PostAdd()
		{
			base.PostAdd();
			this.carryTracker.Notify_CaravanSpawned();
			this.beds.Notify_CaravanSpawned();
			Find.ColonistBar.MarkColonistsDirty();
		}

		// Token: 0x06008BE9 RID: 35817 RVA: 0x00322EDE File Offset: 0x003210DE
		public override void PostRemove()
		{
			base.PostRemove();
			this.pather.StopDead();
			Find.ColonistBar.MarkColonistsDirty();
		}

		// Token: 0x06008BEA RID: 35818 RVA: 0x00322EFC File Offset: 0x003210FC
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

		// Token: 0x06008BEB RID: 35819 RVA: 0x00322F63 File Offset: 0x00321163
		public override void SpawnSetup()
		{
			base.SpawnSetup();
			this.tweener.ResetTweenedPosToRoot();
		}

		// Token: 0x06008BEC RID: 35820 RVA: 0x00322F76 File Offset: 0x00321176
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.IsPlayerControlled && this.pather.curPath != null)
			{
				this.pather.curPath.DrawPath(this);
			}
			this.gotoMote.RenderMote();
		}

		// Token: 0x06008BED RID: 35821 RVA: 0x00322FB0 File Offset: 0x003211B0
		public void AddPawn(Pawn p, bool addCarriedPawnToWorldPawnsIfAny)
		{
			if (p == null)
			{
				Log.Warning("Tried to add a null pawn to " + this);
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
				}));
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
				Log.Error("Couldn't add pawn " + p + " to caravan.");
			}
		}

		// Token: 0x06008BEE RID: 35822 RVA: 0x003230B4 File Offset: 0x003212B4
		public void AddPawnOrItem(Thing thing, bool addCarriedPawnToWorldPawnsIfAny)
		{
			if (thing == null)
			{
				Log.Warning("Tried to add a null thing to " + this);
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

		// Token: 0x06008BEF RID: 35823 RVA: 0x003230EF File Offset: 0x003212EF
		public bool ContainsPawn(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x06008BF0 RID: 35824 RVA: 0x003230FD File Offset: 0x003212FD
		public void RemovePawn(Pawn p)
		{
			this.pawns.Remove(p);
		}

		// Token: 0x06008BF1 RID: 35825 RVA: 0x0032310C File Offset: 0x0032130C
		public void RemoveAllPawns()
		{
			this.pawns.Clear();
		}

		// Token: 0x06008BF2 RID: 35826 RVA: 0x00323119 File Offset: 0x00321319
		public bool IsOwner(Pawn p)
		{
			return this.pawns.Contains(p) && CaravanUtility.IsOwner(p, base.Faction);
		}

		// Token: 0x06008BF3 RID: 35827 RVA: 0x00323138 File Offset: 0x00321338
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

		// Token: 0x06008BF4 RID: 35828 RVA: 0x003235A1 File Offset: 0x003217A1
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
							pawn2.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_Sad, null, false, false, null, false, false, false);
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
				if (ModsConfig.IdeologyActive)
				{
					yield return new Command_Action
					{
						defaultLabel = "Dev: Kill all non-slave pawns",
						action = delegate()
						{
							for (int i = this.PawnsListForReading.Count - 1; i >= 0; i--)
							{
								Pawn pawn2 = this.PawnsListForReading[i];
								if (!pawn2.IsSlave)
								{
									pawn2.Kill(null, null);
									Messages.Message("Dev: Killed " + pawn2.LabelShort, this, MessageTypeDefOf.TaskCompletion, false);
								}
							}
						}
					};
				}
				yield return new Command_Action
				{
					defaultLabel = "Dev: Harm random pawn",
					action = delegate()
					{
						Pawn pawn2;
						if (this.PawnsListForReading.TryRandomElement(out pawn2))
						{
							DamageInfo dinfo = new DamageInfo(DamageDefOf.Scratch, 10f, 999f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true);
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

		// Token: 0x06008BF5 RID: 35829 RVA: 0x003235B1 File Offset: 0x003217B1
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

		// Token: 0x06008BF6 RID: 35830 RVA: 0x003235CF File Offset: 0x003217CF
		public void RecacheImmobilizedNow()
		{
			this.cachedImmobilizedForTicks = -99999;
		}

		// Token: 0x06008BF7 RID: 35831 RVA: 0x003235DC File Offset: 0x003217DC
		public void RecacheDaysWorthOfFood()
		{
			this.cachedDaysWorthOfFoodForTicks = -99999;
		}

		// Token: 0x06008BF8 RID: 35832 RVA: 0x003235EC File Offset: 0x003217EC
		public virtual void Notify_MemberDied(Pawn member)
		{
			if (!base.Spawned)
			{
				Log.Error("Caravan member died in an unspawned caravan. Unspawned caravans shouldn't be kept for more than a single frame.");
			}
			if (!this.PawnsListForReading.Any((Pawn x) => x != member && this.IsOwner(x)))
			{
				this.RemovePawn(member);
				if (base.Faction == Faction.OfPlayer)
				{
					if (ModsConfig.IdeologyActive && this.PawnsListForReading.Any((Pawn x) => x != member && x.IsSlave))
					{
						Find.LetterStack.ReceiveLetter("LetterLabelAllCaravanColonistsDied".Translate(), "LetterOnlySlaveCaravanColonistsLeft".Translate(this.Name).CapitalizeFirst(), LetterDefOf.NegativeEvent, new GlobalTargetInfo(base.Tile), null, null, null, null);
					}
					else
					{
						Find.LetterStack.ReceiveLetter("LetterLabelAllCaravanColonistsDied".Translate(), "LetterAllCaravanColonistsDied".Translate(this.Name).CapitalizeFirst(), LetterDefOf.NegativeEvent, new GlobalTargetInfo(base.Tile), null, null, null, null);
					}
				}
				this.pawns.Clear();
				this.Destroy();
				return;
			}
			member.Strip();
			this.RemovePawn(member);
		}

		// Token: 0x06008BF9 RID: 35833 RVA: 0x00323736 File Offset: 0x00321936
		public virtual void Notify_Merged(List<Caravan> group)
		{
			this.notifiedOutOfFood = false;
		}

		// Token: 0x06008BFA RID: 35834 RVA: 0x00323736 File Offset: 0x00321936
		public virtual void Notify_StartedTrading()
		{
			this.notifiedOutOfFood = false;
		}

		// Token: 0x06008BFB RID: 35835 RVA: 0x00323740 File Offset: 0x00321940
		private void CheckAnyNonWorldPawns()
		{
			for (int i = this.pawns.Count - 1; i >= 0; i--)
			{
				if (!this.pawns[i].IsWorldPawn())
				{
					Log.Error("Caravan member " + this.pawns[i] + " is not a world pawn. Removing...");
					this.pawns.Remove(this.pawns[i]);
				}
			}
		}

		// Token: 0x06008BFC RID: 35836 RVA: 0x003237B0 File Offset: 0x003219B0
		private bool ShouldAutoCapture(Pawn p)
		{
			return CaravanUtility.ShouldAutoCapture(p, base.Faction);
		}

		// Token: 0x06008BFD RID: 35837 RVA: 0x003237BE File Offset: 0x003219BE
		public void Notify_PawnRemoved(Pawn p)
		{
			Find.ColonistBar.MarkColonistsDirty();
			this.RecacheImmobilizedNow();
			this.RecacheDaysWorthOfFood();
			this.carryTracker.Notify_PawnRemoved();
			this.beds.Notify_PawnRemoved();
		}

		// Token: 0x06008BFE RID: 35838 RVA: 0x003237EC File Offset: 0x003219EC
		public void Notify_PawnAdded(Pawn p)
		{
			Find.ColonistBar.MarkColonistsDirty();
			this.RecacheImmobilizedNow();
			this.RecacheDaysWorthOfFood();
		}

		// Token: 0x06008BFF RID: 35839 RVA: 0x00323804 File Offset: 0x00321A04
		public void Notify_DestinationOrPauseStatusChanged()
		{
			this.RecacheDaysWorthOfFood();
		}

		// Token: 0x06008C00 RID: 35840 RVA: 0x0032380C File Offset: 0x00321A0C
		public void Notify_Teleported()
		{
			this.tweener.ResetTweenedPosToRoot();
			this.pather.Notify_Teleported_Int();
		}

		// Token: 0x06008C01 RID: 35841 RVA: 0x00323824 File Offset: 0x00321A24
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.pawns;
		}

		// Token: 0x06008C02 RID: 35842 RVA: 0x0032382C File Offset: 0x00321A2C
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x040058E4 RID: 22756
		private int uniqueId = -1;

		// Token: 0x040058E5 RID: 22757
		private string nameInt;

		// Token: 0x040058E6 RID: 22758
		public ThingOwner<Pawn> pawns;

		// Token: 0x040058E7 RID: 22759
		public bool autoJoinable;

		// Token: 0x040058E8 RID: 22760
		public Caravan_PathFollower pather;

		// Token: 0x040058E9 RID: 22761
		public Caravan_GotoMoteRenderer gotoMote;

		// Token: 0x040058EA RID: 22762
		public Caravan_Tweener tweener;

		// Token: 0x040058EB RID: 22763
		public Caravan_TraderTracker trader;

		// Token: 0x040058EC RID: 22764
		public Caravan_ForageTracker forage;

		// Token: 0x040058ED RID: 22765
		public Caravan_NeedsTracker needs;

		// Token: 0x040058EE RID: 22766
		public Caravan_CarryTracker carryTracker;

		// Token: 0x040058EF RID: 22767
		public Caravan_BedsTracker beds;

		// Token: 0x040058F0 RID: 22768
		public StoryState storyState;

		// Token: 0x040058F1 RID: 22769
		private Material cachedMat;

		// Token: 0x040058F2 RID: 22770
		private bool cachedImmobilized;

		// Token: 0x040058F3 RID: 22771
		private int cachedImmobilizedForTicks = -99999;

		// Token: 0x040058F4 RID: 22772
		private Pair<float, float> cachedDaysWorthOfFood;

		// Token: 0x040058F5 RID: 22773
		private int cachedDaysWorthOfFoodForTicks = -99999;

		// Token: 0x040058F6 RID: 22774
		public bool notifiedOutOfFood;

		// Token: 0x040058F7 RID: 22775
		private const int ImmobilizedCacheDuration = 60;

		// Token: 0x040058F8 RID: 22776
		private const int DaysWorthOfFoodCacheDuration = 3000;

		// Token: 0x040058F9 RID: 22777
		private static readonly Texture2D SplitCommand = ContentFinder<Texture2D>.Get("UI/Commands/SplitCaravan", true);

		// Token: 0x040058FA RID: 22778
		private static readonly Color PlayerCaravanColor = new Color(1f, 0.863f, 0.33f);
	}
}
