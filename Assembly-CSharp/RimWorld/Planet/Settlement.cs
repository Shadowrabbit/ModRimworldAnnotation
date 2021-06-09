using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002140 RID: 8512
	[StaticConstructorOnStartup]
	public class Settlement : MapParent, ITrader, ITraderRestockingInfoProvider
	{
		// Token: 0x17001AAA RID: 6826
		// (get) Token: 0x0600B507 RID: 46343 RVA: 0x00075888 File Offset: 0x00073A88
		// (set) Token: 0x0600B508 RID: 46344 RVA: 0x00075890 File Offset: 0x00073A90
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

		// Token: 0x17001AAB RID: 6827
		// (get) Token: 0x0600B509 RID: 46345 RVA: 0x00075899 File Offset: 0x00073A99
		public override Texture2D ExpandingIcon
		{
			get
			{
				return base.Faction.def.FactionIcon;
			}
		}

		// Token: 0x17001AAC RID: 6828
		// (get) Token: 0x0600B50A RID: 46346 RVA: 0x000758AB File Offset: 0x00073AAB
		public override string Label
		{
			get
			{
				if (this.nameInt == null)
				{
					return base.Label;
				}
				return this.nameInt;
			}
		}

		// Token: 0x17001AAD RID: 6829
		// (get) Token: 0x0600B50B RID: 46347 RVA: 0x000758C2 File Offset: 0x00073AC2
		public override bool HasName
		{
			get
			{
				return !this.nameInt.NullOrEmpty();
			}
		}

		// Token: 0x17001AAE RID: 6830
		// (get) Token: 0x0600B50C RID: 46348 RVA: 0x000758D2 File Offset: 0x00073AD2
		protected override bool UseGenericEnterMapFloatMenuOption
		{
			get
			{
				return !this.Attackable;
			}
		}

		// Token: 0x17001AAF RID: 6831
		// (get) Token: 0x0600B50D RID: 46349 RVA: 0x000758DD File Offset: 0x00073ADD
		public virtual bool Visitable
		{
			get
			{
				return base.Faction != Faction.OfPlayer && (base.Faction == null || !base.Faction.HostileTo(Faction.OfPlayer));
			}
		}

		// Token: 0x17001AB0 RID: 6832
		// (get) Token: 0x0600B50E RID: 46350 RVA: 0x0007590B File Offset: 0x00073B0B
		public virtual bool Attackable
		{
			get
			{
				return base.Faction != Faction.OfPlayer;
			}
		}

		// Token: 0x17001AB1 RID: 6833
		// (get) Token: 0x0600B50F RID: 46351 RVA: 0x0007590B File Offset: 0x00073B0B
		public override bool ShowRelatedQuests
		{
			get
			{
				return base.Faction != Faction.OfPlayer;
			}
		}

		// Token: 0x17001AB2 RID: 6834
		// (get) Token: 0x0600B510 RID: 46352 RVA: 0x003472EC File Offset: 0x003454EC
		public override Material Material
		{
			get
			{
				if (this.cachedMat == null)
				{
					this.cachedMat = MaterialPool.MatFrom(base.Faction.def.settlementTexturePath, ShaderDatabase.WorldOverlayTransparentLit, base.Faction.Color, WorldMaterials.WorldObjectRenderQueue);
				}
				return this.cachedMat;
			}
		}

		// Token: 0x17001AB3 RID: 6835
		// (get) Token: 0x0600B511 RID: 46353 RVA: 0x0007591D File Offset: 0x00073B1D
		public override MapGeneratorDef MapGeneratorDef
		{
			get
			{
				if (base.Faction == Faction.OfPlayer)
				{
					return MapGeneratorDefOf.Base_Player;
				}
				return MapGeneratorDefOf.Base_Faction;
			}
		}

		// Token: 0x17001AB4 RID: 6836
		// (get) Token: 0x0600B512 RID: 46354 RVA: 0x00075937 File Offset: 0x00073B37
		public TraderKindDef TraderKind
		{
			get
			{
				if (this.trader == null)
				{
					return null;
				}
				return this.trader.TraderKind;
			}
		}

		// Token: 0x17001AB5 RID: 6837
		// (get) Token: 0x0600B513 RID: 46355 RVA: 0x0007594E File Offset: 0x00073B4E
		public IEnumerable<Thing> Goods
		{
			get
			{
				if (this.trader == null)
				{
					return null;
				}
				return this.trader.StockListForReading;
			}
		}

		// Token: 0x17001AB6 RID: 6838
		// (get) Token: 0x0600B514 RID: 46356 RVA: 0x00075965 File Offset: 0x00073B65
		public int RandomPriceFactorSeed
		{
			get
			{
				if (this.trader == null)
				{
					return 0;
				}
				return this.trader.RandomPriceFactorSeed;
			}
		}

		// Token: 0x17001AB7 RID: 6839
		// (get) Token: 0x0600B515 RID: 46357 RVA: 0x0007597C File Offset: 0x00073B7C
		public string TraderName
		{
			get
			{
				if (this.trader == null)
				{
					return null;
				}
				return this.trader.TraderName;
			}
		}

		// Token: 0x17001AB8 RID: 6840
		// (get) Token: 0x0600B516 RID: 46358 RVA: 0x00075993 File Offset: 0x00073B93
		public bool CanTradeNow
		{
			get
			{
				return this.trader != null && this.trader.CanTradeNow;
			}
		}

		// Token: 0x17001AB9 RID: 6841
		// (get) Token: 0x0600B517 RID: 46359 RVA: 0x000759AA File Offset: 0x00073BAA
		public float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				if (this.trader == null)
				{
					return 0f;
				}
				return this.trader.TradePriceImprovementOffsetForPlayer;
			}
		}

		// Token: 0x17001ABA RID: 6842
		// (get) Token: 0x0600B518 RID: 46360 RVA: 0x000759C5 File Offset: 0x00073BC5
		public TradeCurrency TradeCurrency
		{
			get
			{
				return this.TraderKind.tradeCurrency;
			}
		}

		// Token: 0x0600B519 RID: 46361 RVA: 0x000759D2 File Offset: 0x00073BD2
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			if (this.trader == null)
			{
				return null;
			}
			return this.trader.ColonyThingsWillingToBuy(playerNegotiator);
		}

		// Token: 0x0600B51A RID: 46362 RVA: 0x000759EA File Offset: 0x00073BEA
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToTrader(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x0600B51B RID: 46363 RVA: 0x000759FA File Offset: 0x00073BFA
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToPlayer(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x17001ABB RID: 6843
		// (get) Token: 0x0600B51C RID: 46364 RVA: 0x00075A0A File Offset: 0x00073C0A
		public bool EverVisited
		{
			get
			{
				return this.trader.EverVisited;
			}
		}

		// Token: 0x17001ABC RID: 6844
		// (get) Token: 0x0600B51D RID: 46365 RVA: 0x00075A17 File Offset: 0x00073C17
		public bool RestockedSinceLastVisit
		{
			get
			{
				return this.trader.RestockedSinceLastVisit;
			}
		}

		// Token: 0x17001ABD RID: 6845
		// (get) Token: 0x0600B51E RID: 46366 RVA: 0x00075A24 File Offset: 0x00073C24
		public int NextRestockTick
		{
			get
			{
				return this.trader.NextRestockTick;
			}
		}

		// Token: 0x0600B51F RID: 46367 RVA: 0x00075A31 File Offset: 0x00073C31
		public Settlement()
		{
			this.trader = new Settlement_TraderTracker(this);
		}

		// Token: 0x0600B520 RID: 46368 RVA: 0x00075A50 File Offset: 0x00073C50
		public override IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			foreach (IncidentTargetTagDef incidentTargetTagDef in base.IncidentTargetTags())
			{
				yield return incidentTargetTagDef;
			}
			IEnumerator<IncidentTargetTagDef> enumerator = null;
			if (base.Faction == Faction.OfPlayer)
			{
				yield return IncidentTargetTagDefOf.Map_PlayerHome;
			}
			else
			{
				yield return IncidentTargetTagDefOf.Map_Misc;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600B521 RID: 46369 RVA: 0x00347340 File Offset: 0x00345540
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.previouslyGeneratedInhabitants, "previouslyGeneratedInhabitants", LookMode.Reference, Array.Empty<object>());
			Scribe_Deep.Look<Settlement_TraderTracker>(ref this.trader, "trader", new object[]
			{
				this
			});
			Scribe_Values.Look<string>(ref this.nameInt, "nameInt", null, false);
			Scribe_Values.Look<bool>(ref this.namedByPlayer, "namedByPlayer", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.previouslyGeneratedInhabitants.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600B522 RID: 46370 RVA: 0x00075A60 File Offset: 0x00073C60
		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			if (!base.Map.IsPlayerHome)
			{
				base.GetComponent<TimedDetectionRaids>().StartDetectionCountdown(240000, -1);
			}
		}

		// Token: 0x0600B523 RID: 46371 RVA: 0x00075A86 File Offset: 0x00073C86
		public override void Tick()
		{
			base.Tick();
			if (this.trader != null)
			{
				this.trader.TraderTrackerTick();
			}
			SettlementDefeatUtility.CheckDefeated(this);
		}

		// Token: 0x0600B524 RID: 46372 RVA: 0x003473DC File Offset: 0x003455DC
		public override void Notify_MyMapRemoved(Map map)
		{
			base.Notify_MyMapRemoved(map);
			for (int i = this.previouslyGeneratedInhabitants.Count - 1; i >= 0; i--)
			{
				Pawn pawn = this.previouslyGeneratedInhabitants[i];
				if (pawn.DestroyedOrNull() || !pawn.IsWorldPawn())
				{
					this.previouslyGeneratedInhabitants.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600B525 RID: 46373 RVA: 0x00075AA7 File Offset: 0x00073CA7
		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			return !base.Map.IsPlayerHome && !base.Map.mapPawns.AnyPawnBlockingMapRemoval;
		}

		// Token: 0x0600B526 RID: 46374 RVA: 0x00075ACE File Offset: 0x00073CCE
		public override void PostRemove()
		{
			base.PostRemove();
			if (this.trader != null)
			{
				this.trader.TryDestroyStock();
			}
		}

		// Token: 0x0600B527 RID: 46375 RVA: 0x00347434 File Offset: 0x00345634
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (base.Faction != Faction.OfPlayer)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += base.Faction.PlayerRelationKind.GetLabel();
				if (!base.Faction.Hidden)
				{
					text = text + " (" + base.Faction.PlayerGoodwill.ToStringWithSign() + ")";
				}
				TraderKindDef traderKind = this.TraderKind;
				RoyalTitleDef royalTitleDef = (traderKind != null) ? traderKind.TitleRequiredToTrade : null;
				if (royalTitleDef != null)
				{
					text += "\n" + "RequiresTradePermission".Translate(royalTitleDef.GetLabelCapForBothGenders());
				}
			}
			return text;
		}

		// Token: 0x0600B528 RID: 46376 RVA: 0x00075AE9 File Offset: 0x00073CE9
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.TraderKind != null)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandShowSellableItems".Translate(),
					defaultDesc = "CommandShowSellableItemsDesc".Translate(),
					icon = Settlement.ShowSellableItemsCommand,
					action = delegate()
					{
						Find.WindowStack.Add(new Dialog_SellableItems(this));
						RoyalTitleDef titleRequiredToTrade = this.TraderKind.TitleRequiredToTrade;
						if (titleRequiredToTrade != null)
						{
							TutorUtility.DoModalDialogIfNotKnown(ConceptDefOf.TradingRequiresPermit, new string[]
							{
								titleRequiredToTrade.GetLabelCapForBothGenders()
							});
						}
					}
				};
			}
			if (base.Faction != Faction.OfPlayer && !PlayerKnowledgeDatabase.IsComplete(ConceptDefOf.FormCaravan))
			{
				Command_Action command_Action = new Command_Action();
				command_Action.defaultLabel = "CommandFormCaravan".Translate();
				command_Action.defaultDesc = "CommandFormCaravanDesc".Translate();
				command_Action.icon = Settlement.FormCaravanCommand;
				command_Action.action = delegate()
				{
					Find.Tutor.learningReadout.TryActivateConcept(ConceptDefOf.FormCaravan);
					Messages.Message("MessageSelectOwnBaseToFormCaravan".Translate(), MessageTypeDefOf.RejectInput, false);
				};
				yield return command_Action;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600B529 RID: 46377 RVA: 0x00075AF9 File Offset: 0x00073CF9
		public override IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			if (this.CanTradeNow && CaravanVisitUtility.SettlementVisitedNow(caravan) == this)
			{
				yield return CaravanVisitUtility.TradeCommand(caravan, base.Faction, this.TraderKind);
			}
			if (CaravanArrivalAction_OfferGifts.CanOfferGiftsTo(caravan, this))
			{
				yield return FactionGiftUtility.OfferGiftsCommand(caravan, this);
			}
			foreach (Gizmo gizmo in base.GetCaravanGizmos(caravan))
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.Attackable)
			{
				yield return new Command_Action
				{
					icon = Settlement.AttackCommand,
					defaultLabel = "CommandAttackSettlement".Translate(),
					defaultDesc = "CommandAttackSettlementDesc".Translate(),
					action = delegate()
					{
						SettlementUtility.Attack(caravan, this);
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600B52A RID: 46378 RVA: 0x00075B10 File Offset: 0x00073D10
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(caravan))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (CaravanVisitUtility.SettlementVisitedNow(caravan) != this)
			{
				foreach (FloatMenuOption floatMenuOption2 in CaravanArrivalAction_VisitSettlement.GetFloatMenuOptions(caravan, this))
				{
					yield return floatMenuOption2;
				}
				enumerator = null;
			}
			foreach (FloatMenuOption floatMenuOption3 in CaravanArrivalAction_Trade.GetFloatMenuOptions(caravan, this))
			{
				yield return floatMenuOption3;
			}
			enumerator = null;
			foreach (FloatMenuOption floatMenuOption4 in CaravanArrivalAction_OfferGifts.GetFloatMenuOptions(caravan, this))
			{
				yield return floatMenuOption4;
			}
			enumerator = null;
			foreach (FloatMenuOption floatMenuOption5 in CaravanArrivalAction_AttackSettlement.GetFloatMenuOptions(caravan, this))
			{
				yield return floatMenuOption5;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600B52B RID: 46379 RVA: 0x00075B27 File Offset: 0x00073D27
		public override IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetTransportPodsFloatMenuOptions(pods, representative))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			foreach (FloatMenuOption floatMenuOption2 in TransportPodsArrivalAction_VisitSettlement.GetFloatMenuOptions(representative, pods, this))
			{
				yield return floatMenuOption2;
			}
			enumerator = null;
			foreach (FloatMenuOption floatMenuOption3 in TransportPodsArrivalAction_GiveGift.GetFloatMenuOptions(representative, pods, this))
			{
				yield return floatMenuOption3;
			}
			enumerator = null;
			if (!base.HasMap)
			{
				foreach (FloatMenuOption floatMenuOption4 in TransportPodsArrivalAction_AttackSettlement.GetFloatMenuOptions(representative, pods, this))
				{
					yield return floatMenuOption4;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600B52C RID: 46380 RVA: 0x00075B45 File Offset: 0x00073D45
		public override IEnumerable<FloatMenuOption> GetShuttleFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<int, TransportPodsArrivalAction> launchAction)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetShuttleFloatMenuOptions(pods, launchAction))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (TransportPodsArrivalAction_GiveGift.CanGiveGiftTo(pods, this))
			{
				Action <>9__2;
				yield return new FloatMenuOption("GiveGiftViaTransportPods".Translate(base.Faction.Name, FactionGiftUtility.GetGoodwillChange(pods, this).ToStringWithSign()), delegate()
				{
					TradeRequestComp tradeReqComp = this.GetComponent<TradeRequestComp>();
					if (tradeReqComp.ActiveRequest && pods.Any((IThingHolder p) => p.GetDirectlyHeldThings().Contains(tradeReqComp.requestThingDef)))
					{
						WindowStack windowStack = Find.WindowStack;
						TaggedString text = "GiveGiftViaTransportPodsTradeRequestWarning".Translate();
						string buttonAText = "Yes".Translate();
						Action buttonAAction;
						if ((buttonAAction = <>9__2) == null)
						{
							buttonAAction = (<>9__2 = delegate()
							{
								launchAction(this.Tile, new TransportPodsArrivalAction_GiveGift(this));
							});
						}
						windowStack.Add(new Dialog_MessageBox(text, buttonAText, buttonAAction, "No".Translate(), null, null, false, null, null));
						return;
					}
					launchAction(this.Tile, new TransportPodsArrivalAction_GiveGift(this));
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			if (!base.HasMap)
			{
				Func<FloatMenuAcceptanceReport> <>9__3;
				Func<FloatMenuAcceptanceReport> acceptanceReportGetter;
				if ((acceptanceReportGetter = <>9__3) == null)
				{
					acceptanceReportGetter = (<>9__3 = (() => TransportPodsArrivalAction_AttackSettlement.CanAttack(pods, this)));
				}
				Func<TransportPodsArrivalAction_Shuttle> <>9__4;
				Func<TransportPodsArrivalAction_Shuttle> arrivalActionGetter;
				if ((arrivalActionGetter = <>9__4) == null)
				{
					arrivalActionGetter = (<>9__4 = (() => new TransportPodsArrivalAction_Shuttle(this)));
				}
				foreach (FloatMenuOption floatMenuOption2 in TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_Shuttle>(acceptanceReportGetter, arrivalActionGetter, "AttackShuttle".Translate(this.Label), launchAction, base.Tile))
				{
					yield return floatMenuOption2;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600B52D RID: 46381 RVA: 0x00075B63 File Offset: 0x00073D63
		public override void GetChildHolders(List<IThingHolder> outChildren)
		{
			base.GetChildHolders(outChildren);
			if (this.trader != null)
			{
				outChildren.Add(this.trader);
			}
		}

		// Token: 0x04007C31 RID: 31793
		public Settlement_TraderTracker trader;

		// Token: 0x04007C32 RID: 31794
		public List<Pawn> previouslyGeneratedInhabitants = new List<Pawn>();

		// Token: 0x04007C33 RID: 31795
		private string nameInt;

		// Token: 0x04007C34 RID: 31796
		public bool namedByPlayer;

		// Token: 0x04007C35 RID: 31797
		private Material cachedMat;

		// Token: 0x04007C36 RID: 31798
		public static readonly Texture2D ShowSellableItemsCommand = ContentFinder<Texture2D>.Get("UI/Commands/SellableItems", true);

		// Token: 0x04007C37 RID: 31799
		public static readonly Texture2D FormCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/FormCaravan", true);

		// Token: 0x04007C38 RID: 31800
		public static readonly Texture2D AttackCommand = ContentFinder<Texture2D>.Get("UI/Commands/AttackSettlement", true);
	}
}
