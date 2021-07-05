using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017CE RID: 6094
	[StaticConstructorOnStartup]
	public class Settlement : MapParent, ITrader, ITraderRestockingInfoProvider
	{
		// Token: 0x1700170C RID: 5900
		// (get) Token: 0x06008D9C RID: 36252 RVA: 0x0032EAF7 File Offset: 0x0032CCF7
		// (set) Token: 0x06008D9D RID: 36253 RVA: 0x0032EAFF File Offset: 0x0032CCFF
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

		// Token: 0x1700170D RID: 5901
		// (get) Token: 0x06008D9E RID: 36254 RVA: 0x0032EB08 File Offset: 0x0032CD08
		public override Texture2D ExpandingIcon
		{
			get
			{
				return base.Faction.def.FactionIcon;
			}
		}

		// Token: 0x1700170E RID: 5902
		// (get) Token: 0x06008D9F RID: 36255 RVA: 0x0032EB1A File Offset: 0x0032CD1A
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

		// Token: 0x1700170F RID: 5903
		// (get) Token: 0x06008DA0 RID: 36256 RVA: 0x0032EB31 File Offset: 0x0032CD31
		public override bool HasName
		{
			get
			{
				return !this.nameInt.NullOrEmpty();
			}
		}

		// Token: 0x17001710 RID: 5904
		// (get) Token: 0x06008DA1 RID: 36257 RVA: 0x0032EB41 File Offset: 0x0032CD41
		protected override bool UseGenericEnterMapFloatMenuOption
		{
			get
			{
				return !this.Attackable;
			}
		}

		// Token: 0x17001711 RID: 5905
		// (get) Token: 0x06008DA2 RID: 36258 RVA: 0x0032EB4C File Offset: 0x0032CD4C
		public virtual bool Visitable
		{
			get
			{
				return base.Faction != Faction.OfPlayer && (base.Faction == null || !base.Faction.HostileTo(Faction.OfPlayer));
			}
		}

		// Token: 0x17001712 RID: 5906
		// (get) Token: 0x06008DA3 RID: 36259 RVA: 0x0032EB7A File Offset: 0x0032CD7A
		public virtual bool Attackable
		{
			get
			{
				return base.Faction != Faction.OfPlayer;
			}
		}

		// Token: 0x17001713 RID: 5907
		// (get) Token: 0x06008DA4 RID: 36260 RVA: 0x0032EB7A File Offset: 0x0032CD7A
		public override bool ShowRelatedQuests
		{
			get
			{
				return base.Faction != Faction.OfPlayer;
			}
		}

		// Token: 0x17001714 RID: 5908
		// (get) Token: 0x06008DA5 RID: 36261 RVA: 0x0032EB8C File Offset: 0x0032CD8C
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

		// Token: 0x17001715 RID: 5909
		// (get) Token: 0x06008DA6 RID: 36262 RVA: 0x0032EBDD File Offset: 0x0032CDDD
		public override MapGeneratorDef MapGeneratorDef
		{
			get
			{
				if (this.def.mapGenerator != null)
				{
					return this.def.mapGenerator;
				}
				if (base.Faction == Faction.OfPlayer)
				{
					return MapGeneratorDefOf.Base_Player;
				}
				return MapGeneratorDefOf.Base_Faction;
			}
		}

		// Token: 0x17001716 RID: 5910
		// (get) Token: 0x06008DA7 RID: 36263 RVA: 0x0032EC10 File Offset: 0x0032CE10
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

		// Token: 0x17001717 RID: 5911
		// (get) Token: 0x06008DA8 RID: 36264 RVA: 0x0032EC27 File Offset: 0x0032CE27
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

		// Token: 0x17001718 RID: 5912
		// (get) Token: 0x06008DA9 RID: 36265 RVA: 0x0032EC3E File Offset: 0x0032CE3E
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

		// Token: 0x17001719 RID: 5913
		// (get) Token: 0x06008DAA RID: 36266 RVA: 0x0032EC55 File Offset: 0x0032CE55
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

		// Token: 0x1700171A RID: 5914
		// (get) Token: 0x06008DAB RID: 36267 RVA: 0x0032EC6C File Offset: 0x0032CE6C
		public bool CanTradeNow
		{
			get
			{
				return this.trader != null && this.trader.CanTradeNow;
			}
		}

		// Token: 0x1700171B RID: 5915
		// (get) Token: 0x06008DAC RID: 36268 RVA: 0x0032EC83 File Offset: 0x0032CE83
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

		// Token: 0x1700171C RID: 5916
		// (get) Token: 0x06008DAD RID: 36269 RVA: 0x0032EC9E File Offset: 0x0032CE9E
		public TradeCurrency TradeCurrency
		{
			get
			{
				return this.TraderKind.tradeCurrency;
			}
		}

		// Token: 0x06008DAE RID: 36270 RVA: 0x0032ECAB File Offset: 0x0032CEAB
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			if (this.trader == null)
			{
				return null;
			}
			return this.trader.ColonyThingsWillingToBuy(playerNegotiator);
		}

		// Token: 0x06008DAF RID: 36271 RVA: 0x0032ECC3 File Offset: 0x0032CEC3
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToTrader(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x06008DB0 RID: 36272 RVA: 0x0032ECD3 File Offset: 0x0032CED3
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToPlayer(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x1700171D RID: 5917
		// (get) Token: 0x06008DB1 RID: 36273 RVA: 0x0032ECE3 File Offset: 0x0032CEE3
		public bool EverVisited
		{
			get
			{
				return this.trader.EverVisited;
			}
		}

		// Token: 0x1700171E RID: 5918
		// (get) Token: 0x06008DB2 RID: 36274 RVA: 0x0032ECF0 File Offset: 0x0032CEF0
		public bool RestockedSinceLastVisit
		{
			get
			{
				return this.trader.RestockedSinceLastVisit;
			}
		}

		// Token: 0x1700171F RID: 5919
		// (get) Token: 0x06008DB3 RID: 36275 RVA: 0x0032ECFD File Offset: 0x0032CEFD
		public int NextRestockTick
		{
			get
			{
				return this.trader.NextRestockTick;
			}
		}

		// Token: 0x06008DB4 RID: 36276 RVA: 0x0032ED0A File Offset: 0x0032CF0A
		public Settlement()
		{
			this.trader = new Settlement_TraderTracker(this);
		}

		// Token: 0x06008DB5 RID: 36277 RVA: 0x0032ED29 File Offset: 0x0032CF29
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

		// Token: 0x06008DB6 RID: 36278 RVA: 0x0032ED3C File Offset: 0x0032CF3C
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

		// Token: 0x06008DB7 RID: 36279 RVA: 0x0032EDD6 File Offset: 0x0032CFD6
		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			if (!base.Map.IsPlayerHome)
			{
				base.GetComponent<TimedDetectionRaids>().StartDetectionCountdown(240000, -1);
			}
		}

		// Token: 0x06008DB8 RID: 36280 RVA: 0x0032EDFC File Offset: 0x0032CFFC
		public override void Tick()
		{
			base.Tick();
			if (this.trader != null)
			{
				this.trader.TraderTrackerTick();
			}
			SettlementDefeatUtility.CheckDefeated(this);
		}

		// Token: 0x06008DB9 RID: 36281 RVA: 0x0032EE20 File Offset: 0x0032D020
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

		// Token: 0x06008DBA RID: 36282 RVA: 0x0032EE75 File Offset: 0x0032D075
		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			return !base.Map.IsPlayerHome && !base.Map.mapPawns.AnyPawnBlockingMapRemoval;
		}

		// Token: 0x06008DBB RID: 36283 RVA: 0x0032EE9C File Offset: 0x0032D09C
		public override void PostRemove()
		{
			base.PostRemove();
			if (this.trader != null)
			{
				this.trader.TryDestroyStock();
			}
		}

		// Token: 0x06008DBC RID: 36284 RVA: 0x0032EEB8 File Offset: 0x0032D0B8
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (base.Faction != Faction.OfPlayer)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += base.Faction.PlayerRelationKind.GetLabelCap();
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

		// Token: 0x06008DBD RID: 36285 RVA: 0x0032EF77 File Offset: 0x0032D177
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

		// Token: 0x06008DBE RID: 36286 RVA: 0x0032EF87 File Offset: 0x0032D187
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

		// Token: 0x06008DBF RID: 36287 RVA: 0x0032EF9E File Offset: 0x0032D19E
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

		// Token: 0x06008DC0 RID: 36288 RVA: 0x0032EFB5 File Offset: 0x0032D1B5
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

		// Token: 0x06008DC1 RID: 36289 RVA: 0x0032EFD3 File Offset: 0x0032D1D3
		public override IEnumerable<FloatMenuOption> GetShuttleFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<int, TransportPodsArrivalAction> launchAction)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetShuttleFloatMenuOptions(pods, launchAction))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (TransportPodsArrivalAction_Trade.CanTradeWith(pods, this))
			{
				yield return new FloatMenuOption("TradeWith".Translate(this.Label), delegate()
				{
					launchAction(this.Tile, new TransportPodsArrivalAction_Trade(this));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (TransportPodsArrivalAction_GiveGift.CanGiveGiftTo(pods, this))
			{
				Action <>9__3;
				yield return new FloatMenuOption("GiveGiftViaTransportPods".Translate(base.Faction.Name, FactionGiftUtility.GetGoodwillChange(pods, this).ToStringWithSign()), delegate()
				{
					TradeRequestComp tradeReqComp = this.GetComponent<TradeRequestComp>();
					if (tradeReqComp.ActiveRequest && pods.Any((IThingHolder p) => p.GetDirectlyHeldThings().Contains(tradeReqComp.requestThingDef)))
					{
						WindowStack windowStack = Find.WindowStack;
						TaggedString text = "GiveGiftViaTransportPodsTradeRequestWarning".Translate();
						string buttonAText = "Yes".Translate();
						Action buttonAAction;
						if ((buttonAAction = <>9__3) == null)
						{
							buttonAAction = (<>9__3 = delegate()
							{
								launchAction(this.Tile, new TransportPodsArrivalAction_GiveGift(this));
							});
						}
						windowStack.Add(new Dialog_MessageBox(text, buttonAText, buttonAAction, "No".Translate(), null, null, false, null, null));
						return;
					}
					launchAction(this.Tile, new TransportPodsArrivalAction_GiveGift(this));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			CompTransporter firstPod;
			if (!base.HasMap && (firstPod = (pods.FirstOrDefault<IThingHolder>() as CompTransporter)) != null && firstPod.Shuttle.shipParent != null)
			{
				Func<FloatMenuAcceptanceReport> <>9__4;
				Func<FloatMenuAcceptanceReport> acceptanceReportGetter;
				if ((acceptanceReportGetter = <>9__4) == null)
				{
					acceptanceReportGetter = (<>9__4 = (() => TransportPodsArrivalAction_AttackSettlement.CanAttack(pods, this)));
				}
				Func<TransportPodsArrivalAction_TransportShip> <>9__5;
				Func<TransportPodsArrivalAction_TransportShip> arrivalActionGetter;
				if ((arrivalActionGetter = <>9__5) == null)
				{
					arrivalActionGetter = (<>9__5 = (() => new TransportPodsArrivalAction_TransportShip(this, firstPod.Shuttle.shipParent)));
				}
				foreach (FloatMenuOption floatMenuOption2 in TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_TransportShip>(acceptanceReportGetter, arrivalActionGetter, "AttackShuttle".Translate(this.Label), launchAction, base.Tile))
				{
					yield return floatMenuOption2;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06008DC2 RID: 36290 RVA: 0x0032EFF1 File Offset: 0x0032D1F1
		public override void GetChildHolders(List<IThingHolder> outChildren)
		{
			base.GetChildHolders(outChildren);
			if (this.trader != null)
			{
				outChildren.Add(this.trader);
			}
		}

		// Token: 0x0400599B RID: 22939
		public Settlement_TraderTracker trader;

		// Token: 0x0400599C RID: 22940
		public List<Pawn> previouslyGeneratedInhabitants = new List<Pawn>();

		// Token: 0x0400599D RID: 22941
		private string nameInt;

		// Token: 0x0400599E RID: 22942
		public bool namedByPlayer;

		// Token: 0x0400599F RID: 22943
		private Material cachedMat;

		// Token: 0x040059A0 RID: 22944
		public static readonly Texture2D ShowSellableItemsCommand = ContentFinder<Texture2D>.Get("UI/Commands/SellableItems", true);

		// Token: 0x040059A1 RID: 22945
		public static readonly Texture2D FormCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/FormCaravan", true);

		// Token: 0x040059A2 RID: 22946
		public static readonly Texture2D AttackCommand = ContentFinder<Texture2D>.Get("UI/Commands/AttackSettlement", true);
	}
}
