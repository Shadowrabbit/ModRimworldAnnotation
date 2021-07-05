using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020013B0 RID: 5040
	[StaticConstructorOnStartup]
	public class Dialog_Trade : Window
	{
		// Token: 0x17001574 RID: 5492
		// (get) Token: 0x06007AA2 RID: 31394 RVA: 0x00273565 File Offset: 0x00271765
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1024f, (float)UI.screenHeight);
			}
		}

		// Token: 0x17001575 RID: 5493
		// (get) Token: 0x06007AA3 RID: 31395 RVA: 0x002B4E9A File Offset: 0x002B309A
		private int Tile
		{
			get
			{
				return TradeSession.playerNegotiator.Tile;
			}
		}

		// Token: 0x17001576 RID: 5494
		// (get) Token: 0x06007AA4 RID: 31396 RVA: 0x002B4EA6 File Offset: 0x002B30A6
		private BiomeDef Biome
		{
			get
			{
				return Find.WorldGrid[this.Tile].biome;
			}
		}

		// Token: 0x17001577 RID: 5495
		// (get) Token: 0x06007AA5 RID: 31397 RVA: 0x002B4EC0 File Offset: 0x002B30C0
		private float MassUsage
		{
			get
			{
				if (this.massUsageDirty)
				{
					this.massUsageDirty = false;
					TradeSession.deal.UpdateCurrencyCount();
					if (this.cachedCurrencyTradeable != null)
					{
						this.cachedTradeables.Add(this.cachedCurrencyTradeable);
					}
					this.cachedMassUsage = CollectionsMassCalculator.MassUsageLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, IgnorePawnsInventoryMode.Ignore, false, false);
					if (this.cachedCurrencyTradeable != null)
					{
						this.cachedTradeables.RemoveLast<Tradeable>();
					}
				}
				return this.cachedMassUsage;
			}
		}

		// Token: 0x17001578 RID: 5496
		// (get) Token: 0x06007AA6 RID: 31398 RVA: 0x002B4F34 File Offset: 0x002B3134
		private float MassCapacity
		{
			get
			{
				if (this.massCapacityDirty)
				{
					this.massCapacityDirty = false;
					TradeSession.deal.UpdateCurrencyCount();
					if (this.cachedCurrencyTradeable != null)
					{
						this.cachedTradeables.Add(this.cachedCurrencyTradeable);
					}
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedMassCapacity = CollectionsMassCalculator.CapacityLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, stringBuilder);
					this.cachedMassCapacityExplanation = stringBuilder.ToString();
					if (this.cachedCurrencyTradeable != null)
					{
						this.cachedTradeables.RemoveLast<Tradeable>();
					}
				}
				return this.cachedMassCapacity;
			}
		}

		// Token: 0x17001579 RID: 5497
		// (get) Token: 0x06007AA7 RID: 31399 RVA: 0x002B4FB8 File Offset: 0x002B31B8
		private float TilesPerDay
		{
			get
			{
				if (this.tilesPerDayDirty)
				{
					this.tilesPerDayDirty = false;
					TradeSession.deal.UpdateCurrencyCount();
					Caravan caravan = TradeSession.playerNegotiator.GetCaravan();
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedTilesPerDay = TilesPerDayCalculator.ApproxTilesPerDayLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, this.MassUsage, this.MassCapacity, this.Tile, (caravan != null && caravan.pather.Moving) ? caravan.pather.nextTile : -1, stringBuilder);
					this.cachedTilesPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedTilesPerDay;
			}
		}

		// Token: 0x1700157A RID: 5498
		// (get) Token: 0x06007AA8 RID: 31400 RVA: 0x002B504C File Offset: 0x002B324C
		private Pair<float, float> DaysWorthOfFood
		{
			get
			{
				if (this.daysWorthOfFoodDirty)
				{
					this.daysWorthOfFoodDirty = false;
					TradeSession.deal.UpdateCurrencyCount();
					float first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFoodLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, this.Tile, IgnorePawnsInventoryMode.Ignore, Faction.OfPlayer);
					this.cachedDaysWorthOfFood = new Pair<float, float>(first, DaysUntilRotCalculator.ApproxDaysUntilRotLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, this.Tile, IgnorePawnsInventoryMode.Ignore));
				}
				return this.cachedDaysWorthOfFood;
			}
		}

		// Token: 0x1700157B RID: 5499
		// (get) Token: 0x06007AA9 RID: 31401 RVA: 0x002B50BC File Offset: 0x002B32BC
		private Pair<ThingDef, float> ForagedFoodPerDay
		{
			get
			{
				if (this.foragedFoodPerDayDirty)
				{
					this.foragedFoodPerDayDirty = false;
					TradeSession.deal.UpdateCurrencyCount();
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedForagedFoodPerDay = ForagedFoodPerDayCalculator.ForagedFoodPerDayLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, this.Biome, Faction.OfPlayer, stringBuilder);
					this.cachedForagedFoodPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedForagedFoodPerDay;
			}
		}

		// Token: 0x1700157C RID: 5500
		// (get) Token: 0x06007AAA RID: 31402 RVA: 0x002B5120 File Offset: 0x002B3320
		private float Visibility
		{
			get
			{
				if (this.visibilityDirty)
				{
					this.visibilityDirty = false;
					TradeSession.deal.UpdateCurrencyCount();
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedVisibility = CaravanVisibilityCalculator.VisibilityLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, stringBuilder);
					this.cachedVisibilityExplanation = stringBuilder.ToString();
				}
				return this.cachedVisibility;
			}
		}

		// Token: 0x1700157D RID: 5501
		// (get) Token: 0x06007AAB RID: 31403 RVA: 0x002B5176 File Offset: 0x002B3376
		public override QuickSearchWidget CommonSearchWidget
		{
			get
			{
				return this.quickSearchWidget;
			}
		}

		// Token: 0x06007AAC RID: 31404 RVA: 0x002B5180 File Offset: 0x002B3380
		public Dialog_Trade(Pawn playerNegotiator, ITrader trader, bool giftsOnly = false)
		{
			this.giftsOnly = giftsOnly;
			TradeSession.SetupWith(trader, playerNegotiator, giftsOnly);
			this.SetupPlayerCaravanVariables();
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.soundAppear = SoundDefOf.CommsWindow_Open;
			this.soundClose = SoundDefOf.CommsWindow_Close;
			if (trader is PassingShip)
			{
				this.soundAmbient = SoundDefOf.RadioComms_Ambience;
			}
			this.commonSearchWidgetOffset.x = this.commonSearchWidgetOffset.x + 18f;
			this.commonSearchWidgetOffset.y = this.commonSearchWidgetOffset.y - 18f;
			this.sorter1 = TransferableSorterDefOf.Category;
			this.sorter2 = TransferableSorterDefOf.MarketValue;
		}

		// Token: 0x06007AAD RID: 31405 RVA: 0x002B525D File Offset: 0x002B345D
		public override void PreOpen()
		{
			base.PreOpen();
			this.quickSearchWidget.Reset();
		}

		// Token: 0x06007AAE RID: 31406 RVA: 0x002B5270 File Offset: 0x002B3470
		public override void PostOpen()
		{
			base.PostOpen();
			if (!this.giftsOnly && !this.playerIsCaravan)
			{
				Pawn playerNegotiator = TradeSession.playerNegotiator;
				float level = playerNegotiator.health.capacities.GetLevel(PawnCapacityDefOf.Talking);
				float level2 = playerNegotiator.health.capacities.GetLevel(PawnCapacityDefOf.Hearing);
				if (level < 0.95f || level2 < 0.95f)
				{
					TaggedString taggedString;
					if (level < 0.95f)
					{
						taggedString = "NegotiatorTalkingImpaired".Translate(playerNegotiator.LabelShort, playerNegotiator);
					}
					else
					{
						taggedString = "NegotiatorHearingImpaired".Translate(playerNegotiator.LabelShort, playerNegotiator);
					}
					taggedString += "\n\n" + "NegotiatorCapacityImpaired".Translate();
					Find.WindowStack.Add(new Dialog_MessageBox(taggedString, null, null, null, null, null, false, null, null));
				}
			}
			this.CacheTradeables();
		}

		// Token: 0x06007AAF RID: 31407 RVA: 0x002B5358 File Offset: 0x002B3558
		private void CacheTradeables()
		{
			this.cachedCurrencyTradeable = TradeSession.deal.AllTradeables.FirstOrDefault((Tradeable x) => x.IsCurrency && (TradeSession.TradeCurrency != TradeCurrency.Favor || x.IsFavor));
			this.cachedTradeables = (from tr in TradeSession.deal.AllTradeables
			where !tr.IsCurrency && (tr.TraderWillTrade || !TradeSession.trader.TraderKind.hideThingsNotWillingToTrade)
			where this.quickSearchWidget.filter.Matches(tr.Label)
			select tr).OrderByDescending(delegate(Tradeable tr)
			{
				if (!tr.TraderWillTrade)
				{
					return -1;
				}
				return 0;
			}).ThenBy((Tradeable tr) => tr, this.sorter1.Comparer).ThenBy((Tradeable tr) => tr, this.sorter2.Comparer).ThenBy((Tradeable tr) => TransferableUIUtility.DefaultListOrderPriority(tr)).ThenBy((Tradeable tr) => tr.ThingDef.label).ThenBy(delegate(Tradeable tr)
			{
				QualityCategory result;
				if (tr.AnyThing.TryGetQuality(out result))
				{
					return (int)result;
				}
				return -1;
			}).ThenBy((Tradeable tr) => tr.AnyThing.HitPoints).ToList<Tradeable>();
			this.quickSearchWidget.noResultsMatched = !this.cachedTradeables.Any<Tradeable>();
		}

		// Token: 0x06007AB0 RID: 31408 RVA: 0x002B5510 File Offset: 0x002B3710
		public override void DoWindowContents(Rect inRect)
		{
			if (this.playerIsCaravan)
			{
				CaravanUIUtility.DrawCaravanInfo(new CaravanUIUtility.CaravanInfo(this.MassUsage, this.MassCapacity, this.cachedMassCapacityExplanation, this.TilesPerDay, this.cachedTilesPerDayExplanation, this.DaysWorthOfFood, this.ForagedFoodPerDay, this.cachedForagedFoodPerDayExplanation, this.Visibility, this.cachedVisibilityExplanation, -1f, -1f, null), null, this.Tile, null, -9999f, new Rect(12f, 0f, inRect.width - 24f, 40f), true, null, false);
				inRect.yMin += 52f;
			}
			TradeSession.deal.UpdateCurrencyCount();
			GUI.BeginGroup(inRect);
			inRect = inRect.AtZero();
			TransferableUIUtility.DoTransferableSorters(this.sorter1, this.sorter2, delegate(TransferableSorterDef x)
			{
				this.sorter1 = x;
				this.CacheTradeables();
			}, delegate(TransferableSorterDef x)
			{
				this.sorter2 = x;
				this.CacheTradeables();
			});
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.Label(new Rect(0f, 27f, inRect.width / 2f, inRect.height / 2f), "NegotiatorTradeDialogInfo".Translate(TradeSession.playerNegotiator.Name.ToStringFull, TradeSession.playerNegotiator.GetStatValue(StatDefOf.TradePriceImprovement, true).ToStringPercent()));
			float num = inRect.width - 590f;
			Rect position = new Rect(num, 0f, inRect.width - num, 58f);
			GUI.BeginGroup(position);
			Text.Font = GameFont.Medium;
			Rect rect = new Rect(0f, 0f, position.width / 2f, position.height);
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.Label(rect, Faction.OfPlayer.Name.Truncate(rect.width, null));
			Rect rect2 = new Rect(position.width / 2f, 0f, position.width / 2f, position.height);
			Text.Anchor = TextAnchor.UpperRight;
			string text = TradeSession.trader.TraderName;
			if (Text.CalcSize(text).x > rect2.width)
			{
				Text.Font = GameFont.Small;
				text = text.Truncate(rect2.width, null);
			}
			Widgets.Label(rect2, text);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperRight;
			Widgets.Label(new Rect(position.width / 2f, 27f, position.width / 2f, position.height / 2f), TradeSession.trader.TraderKind.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			if (!TradeSession.giftMode)
			{
				GUI.color = new Color(1f, 1f, 1f, 0.6f);
				Text.Font = GameFont.Tiny;
				Rect rect3 = new Rect(position.width / 2f - 100f - 30f, 0f, 200f, position.height);
				Text.Anchor = TextAnchor.LowerCenter;
				Widgets.Label(rect3, "PositiveBuysNegativeSells".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
			}
			GUI.EndGroup();
			float num2 = 0f;
			if (this.cachedCurrencyTradeable != null)
			{
				float num3 = inRect.width - 16f;
				TradeUI.DrawTradeableRow(new Rect(0f, 58f, num3, 30f), this.cachedCurrencyTradeable, 1);
				GUI.color = Color.gray;
				Widgets.DrawLineHorizontal(0f, 87f, num3);
				GUI.color = Color.white;
				num2 = 30f;
			}
			Rect mainRect = new Rect(0f, 58f + num2, inRect.width, inRect.height - 58f - 38f - num2 - 20f);
			this.FillMainRect(mainRect);
			Text.Font = GameFont.Small;
			Rect rect4 = new Rect(inRect.width / 2f - Dialog_Trade.AcceptButtonSize.x / 2f, inRect.height - 55f, Dialog_Trade.AcceptButtonSize.x, Dialog_Trade.AcceptButtonSize.y);
			if (Widgets.ButtonText(rect4, TradeSession.giftMode ? ("OfferGifts".Translate() + " (" + FactionGiftUtility.GetGoodwillChange(TradeSession.deal.AllTradeables, TradeSession.trader.Faction).ToStringWithSign() + ")") : "AcceptButton".Translate(), true, true, true))
			{
				Action action = delegate()
				{
					bool flag;
					if (TradeSession.deal.TryExecute(out flag))
					{
						if (flag)
						{
							SoundDefOf.ExecuteTrade.PlayOneShotOnCamera(null);
							Caravan caravan = TradeSession.playerNegotiator.GetCaravan();
							if (caravan != null)
							{
								caravan.RecacheImmobilizedNow();
							}
							this.Close(false);
							return;
						}
						this.Close(true);
					}
				};
				if (TradeSession.deal.DoesTraderHaveEnoughSilver())
				{
					action();
				}
				else
				{
					this.FlashSilver();
					SoundDefOf.ClickReject.PlayOneShotOnCamera(null);
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmTraderShortFunds".Translate(), action, false, null));
				}
				Event.current.Use();
			}
			if (Widgets.ButtonText(new Rect(rect4.x - 10f - Dialog_Trade.OtherBottomButtonSize.x, rect4.y, Dialog_Trade.OtherBottomButtonSize.x, Dialog_Trade.OtherBottomButtonSize.y), "ResetButton".Translate(), true, true, true))
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				TradeSession.deal.Reset();
				this.CacheTradeables();
				this.CountToTransferChanged();
			}
			if (Widgets.ButtonText(new Rect(rect4.xMax + 10f, rect4.y, Dialog_Trade.OtherBottomButtonSize.x, Dialog_Trade.OtherBottomButtonSize.y), "CancelButton".Translate(), true, true, true))
			{
				this.Close(true);
				Event.current.Use();
			}
			float y = Dialog_Trade.OtherBottomButtonSize.y;
			Rect rect5 = new Rect(inRect.width - y, rect4.y, y, y);
			if (Widgets.ButtonImageWithBG(rect5, Dialog_Trade.ShowSellableItemsIcon, new Vector2?(new Vector2(32f, 32f))))
			{
				Find.WindowStack.Add(new Dialog_SellableItems(TradeSession.trader));
			}
			TooltipHandler.TipRegionByKey(rect5, "CommandShowSellableItemsDesc");
			Faction faction = TradeSession.trader.Faction;
			if (faction != null && !this.giftsOnly && !faction.def.permanentEnemy)
			{
				Rect rect6 = new Rect(rect5.x - y - 4f, rect4.y, y, y);
				if (TradeSession.giftMode)
				{
					if (Widgets.ButtonImageWithBG(rect6, Dialog_Trade.TradeModeIcon, new Vector2?(new Vector2(32f, 32f))))
					{
						TradeSession.giftMode = false;
						TradeSession.deal.Reset();
						this.CacheTradeables();
						this.CountToTransferChanged();
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}
					TooltipHandler.TipRegionByKey(rect6, "TradeModeTip");
				}
				else
				{
					if (Widgets.ButtonImageWithBG(rect6, Dialog_Trade.GiftModeIcon, new Vector2?(new Vector2(32f, 32f))))
					{
						TradeSession.giftMode = true;
						TradeSession.deal.Reset();
						this.CacheTradeables();
						this.CountToTransferChanged();
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}
					TooltipHandler.TipRegionByKey(rect6, "GiftModeTip", faction.Name);
				}
			}
			GUI.EndGroup();
		}

		// Token: 0x06007AB1 RID: 31409 RVA: 0x002B5C4C File Offset: 0x002B3E4C
		public override void Close(bool doCloseSound = true)
		{
			DragSliderManager.ForceStop();
			base.Close(doCloseSound);
		}

		// Token: 0x06007AB2 RID: 31410 RVA: 0x002B5C5C File Offset: 0x002B3E5C
		private void FillMainRect(Rect mainRect)
		{
			Text.Font = GameFont.Small;
			float height = 6f + (float)this.cachedTradeables.Count * 30f;
			Rect viewRect = new Rect(0f, 0f, mainRect.width - 16f, height);
			Widgets.BeginScrollView(mainRect, ref this.scrollPosition, viewRect, true);
			float num = 6f;
			float num2 = this.scrollPosition.y - 30f;
			float num3 = this.scrollPosition.y + mainRect.height;
			int num4 = 0;
			for (int i = 0; i < this.cachedTradeables.Count; i++)
			{
				if (num > num2 && num < num3)
				{
					Rect rect = new Rect(0f, num, viewRect.width, 30f);
					int countToTransfer = this.cachedTradeables[i].CountToTransfer;
					TradeUI.DrawTradeableRow(rect, this.cachedTradeables[i], num4);
					if (countToTransfer != this.cachedTradeables[i].CountToTransfer)
					{
						this.CountToTransferChanged();
					}
				}
				num += 30f;
				num4++;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x06007AB3 RID: 31411 RVA: 0x002B5D79 File Offset: 0x002B3F79
		public void FlashSilver()
		{
			Dialog_Trade.lastCurrencyFlashTime = Time.time;
		}

		// Token: 0x06007AB4 RID: 31412 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool CausesMessageBackground()
		{
			return true;
		}

		// Token: 0x06007AB5 RID: 31413 RVA: 0x002B5D88 File Offset: 0x002B3F88
		private void SetupPlayerCaravanVariables()
		{
			Caravan caravan = TradeSession.playerNegotiator.GetCaravan();
			if (caravan != null)
			{
				this.playerIsCaravan = true;
				this.playerCaravanAllPawnsAndItems = new List<Thing>();
				List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					this.playerCaravanAllPawnsAndItems.Add(pawnsListForReading[i]);
				}
				this.playerCaravanAllPawnsAndItems.AddRange(CaravanInventoryUtility.AllInventoryItems(caravan));
				caravan.Notify_StartedTrading();
				return;
			}
			this.playerIsCaravan = false;
		}

		// Token: 0x06007AB6 RID: 31414 RVA: 0x002B5DFE File Offset: 0x002B3FFE
		private void CountToTransferChanged()
		{
			this.massUsageDirty = true;
			this.massCapacityDirty = true;
			this.tilesPerDayDirty = true;
			this.daysWorthOfFoodDirty = true;
			this.foragedFoodPerDayDirty = true;
			this.visibilityDirty = true;
		}

		// Token: 0x06007AB7 RID: 31415 RVA: 0x002B5E2A File Offset: 0x002B402A
		public override void Notify_CommonSearchChanged()
		{
			this.CacheTradeables();
		}

		// Token: 0x040043F9 RID: 17401
		private bool giftsOnly;

		// Token: 0x040043FA RID: 17402
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x040043FB RID: 17403
		private QuickSearchWidget quickSearchWidget = new QuickSearchWidget();

		// Token: 0x040043FC RID: 17404
		public static float lastCurrencyFlashTime = -100f;

		// Token: 0x040043FD RID: 17405
		private List<Tradeable> cachedTradeables;

		// Token: 0x040043FE RID: 17406
		private Tradeable cachedCurrencyTradeable;

		// Token: 0x040043FF RID: 17407
		private TransferableSorterDef sorter1;

		// Token: 0x04004400 RID: 17408
		private TransferableSorterDef sorter2;

		// Token: 0x04004401 RID: 17409
		private bool playerIsCaravan;

		// Token: 0x04004402 RID: 17410
		private List<Thing> playerCaravanAllPawnsAndItems;

		// Token: 0x04004403 RID: 17411
		private bool massUsageDirty = true;

		// Token: 0x04004404 RID: 17412
		private float cachedMassUsage;

		// Token: 0x04004405 RID: 17413
		private bool massCapacityDirty = true;

		// Token: 0x04004406 RID: 17414
		private float cachedMassCapacity;

		// Token: 0x04004407 RID: 17415
		private string cachedMassCapacityExplanation;

		// Token: 0x04004408 RID: 17416
		private bool tilesPerDayDirty = true;

		// Token: 0x04004409 RID: 17417
		private float cachedTilesPerDay;

		// Token: 0x0400440A RID: 17418
		private string cachedTilesPerDayExplanation;

		// Token: 0x0400440B RID: 17419
		private bool daysWorthOfFoodDirty = true;

		// Token: 0x0400440C RID: 17420
		private Pair<float, float> cachedDaysWorthOfFood;

		// Token: 0x0400440D RID: 17421
		private bool foragedFoodPerDayDirty = true;

		// Token: 0x0400440E RID: 17422
		private Pair<ThingDef, float> cachedForagedFoodPerDay;

		// Token: 0x0400440F RID: 17423
		private string cachedForagedFoodPerDayExplanation;

		// Token: 0x04004410 RID: 17424
		private bool visibilityDirty = true;

		// Token: 0x04004411 RID: 17425
		private float cachedVisibility;

		// Token: 0x04004412 RID: 17426
		private string cachedVisibilityExplanation;

		// Token: 0x04004413 RID: 17427
		private const float TitleAreaHeight = 45f;

		// Token: 0x04004414 RID: 17428
		private const float TopAreaHeight = 58f;

		// Token: 0x04004415 RID: 17429
		private const float ColumnWidth = 120f;

		// Token: 0x04004416 RID: 17430
		private const float FirstCommodityY = 6f;

		// Token: 0x04004417 RID: 17431
		private const float RowInterval = 30f;

		// Token: 0x04004418 RID: 17432
		private const float SpaceBetweenTraderNameAndTraderKind = 27f;

		// Token: 0x04004419 RID: 17433
		private const float ShowSellableItemsIconSize = 32f;

		// Token: 0x0400441A RID: 17434
		private const float GiftModeIconSize = 32f;

		// Token: 0x0400441B RID: 17435
		private const float TradeModeIconSize = 32f;

		// Token: 0x0400441C RID: 17436
		protected static readonly Vector2 AcceptButtonSize = new Vector2(160f, 40f);

		// Token: 0x0400441D RID: 17437
		protected static readonly Vector2 OtherBottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x0400441E RID: 17438
		private static readonly Texture2D ShowSellableItemsIcon = ContentFinder<Texture2D>.Get("UI/Commands/SellableItems", true);

		// Token: 0x0400441F RID: 17439
		private static readonly Texture2D GiftModeIcon = ContentFinder<Texture2D>.Get("UI/Buttons/GiftMode", true);

		// Token: 0x04004420 RID: 17440
		private static readonly Texture2D TradeModeIcon = ContentFinder<Texture2D>.Get("UI/Buttons/TradeMode", true);
	}
}
