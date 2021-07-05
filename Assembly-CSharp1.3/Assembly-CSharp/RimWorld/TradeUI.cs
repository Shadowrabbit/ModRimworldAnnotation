using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013B3 RID: 5043
	[StaticConstructorOnStartup]
	public static class TradeUI
	{
		// Token: 0x06007ABF RID: 31423 RVA: 0x002B6000 File Offset: 0x002B4200
		public static void DrawTradeableRow(Rect rect, Tradeable trad, int index)
		{
			if (index % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			Text.Font = GameFont.Small;
			GUI.BeginGroup(rect);
			float num = rect.width;
			int num2 = trad.CountHeldBy(Transactor.Trader);
			if (num2 != 0 && trad.IsThing)
			{
				Rect rect2 = new Rect(num - 75f, 0f, 75f, rect.height);
				if (Mouse.IsOver(rect2))
				{
					Widgets.DrawHighlight(rect2);
				}
				Text.Anchor = TextAnchor.MiddleRight;
				Rect rect3 = rect2;
				rect3.xMin += 5f;
				rect3.xMax -= 5f;
				Widgets.Label(rect3, num2.ToStringCached());
				TooltipHandler.TipRegionByKey(rect2, "TraderCount");
				Rect rect4 = new Rect(rect2.x - 100f, 0f, 100f, rect.height);
				Text.Anchor = TextAnchor.MiddleRight;
				TradeUI.DrawPrice(rect4, trad, TradeAction.PlayerBuys);
			}
			num -= 175f;
			Rect rect5 = new Rect(num - 240f, 0f, 240f, rect.height);
			if (!trad.TraderWillTrade)
			{
				TradeUI.DrawWillNotTradeText(rect5, "TraderWillNotTrade".Translate());
			}
			else if (ModsConfig.IdeologyActive && TransferableUIUtility.TradeIsPlayerSellingToSlavery(trad, TradeSession.trader.Faction) && !new HistoryEvent(HistoryEventDefOf.SoldSlave, TradeSession.playerNegotiator.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo())
			{
				TradeUI.DrawWillNotTradeText(rect5, "NegotiatorWillNotTradeSlaves".Translate());
				if (Mouse.IsOver(rect5))
				{
					Widgets.DrawHighlight(rect5);
					TooltipHandler.TipRegion(rect5, "NegotiatorWillNotTradeSlavesTip".Translate(TradeSession.playerNegotiator, TradeSession.playerNegotiator.Ideo.name));
				}
			}
			else
			{
				bool flash = Time.time - Dialog_Trade.lastCurrencyFlashTime < 1f && trad.IsCurrency;
				TransferableUIUtility.DoCountAdjustInterface(rect5, trad, index, trad.GetMinimumToTransfer(), trad.GetMaximumToTransfer(), flash, null, false);
			}
			num -= 240f;
			int num3 = trad.CountHeldBy(Transactor.Colony);
			if (num3 != 0)
			{
				Rect rect6 = new Rect(num - 100f, 0f, 100f, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				TradeUI.DrawPrice(rect6, trad, TradeAction.PlayerSells);
				Rect rect7 = new Rect(rect6.x - 75f, 0f, 75f, rect.height);
				if (Mouse.IsOver(rect7))
				{
					Widgets.DrawHighlight(rect7);
				}
				Text.Anchor = TextAnchor.MiddleLeft;
				Rect rect8 = rect7;
				rect8.xMin += 5f;
				rect8.xMax -= 5f;
				Widgets.Label(rect8, num3.ToStringCached());
				TooltipHandler.TipRegionByKey(rect7, "ColonyCount");
			}
			num -= 175f;
			TransferableUIUtility.DoExtraAnimalIcons(trad, rect, ref num);
			if (ModsConfig.IdeologyActive)
			{
				TransferableUIUtility.DrawCaptiveTradeInfo(trad, TradeSession.trader, rect, ref num);
			}
			Rect idRect = new Rect(0f, 0f, num, rect.height);
			TransferableUIUtility.DrawTransferableInfo(trad, idRect, trad.TraderWillTrade ? Color.white : TradeUI.NoTradeColor);
			GenUI.ResetLabelAlign();
			GUI.EndGroup();
		}

		// Token: 0x06007AC0 RID: 31424 RVA: 0x002B6328 File Offset: 0x002B4528
		private static void DrawPrice(Rect rect, Tradeable trad, TradeAction action)
		{
			if (trad.IsCurrency || !trad.TraderWillTrade)
			{
				return;
			}
			rect = rect.Rounded();
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, new TipSignal(() => trad.GetPriceTooltip(action), trad.GetHashCode() * 297));
			}
			if (action == TradeAction.PlayerBuys)
			{
				switch (trad.PriceTypeFor(action))
				{
				case PriceType.VeryCheap:
					GUI.color = new Color(0f, 1f, 0f);
					break;
				case PriceType.Cheap:
					GUI.color = new Color(0.5f, 1f, 0.5f);
					break;
				case PriceType.Normal:
					GUI.color = Color.white;
					break;
				case PriceType.Expensive:
					GUI.color = new Color(1f, 0.5f, 0.5f);
					break;
				case PriceType.Exorbitant:
					GUI.color = new Color(1f, 0f, 0f);
					break;
				}
			}
			else
			{
				switch (trad.PriceTypeFor(action))
				{
				case PriceType.VeryCheap:
					GUI.color = new Color(1f, 0f, 0f);
					break;
				case PriceType.Cheap:
					GUI.color = new Color(1f, 0.5f, 0.5f);
					break;
				case PriceType.Normal:
					GUI.color = Color.white;
					break;
				case PriceType.Expensive:
					GUI.color = new Color(0.5f, 1f, 0.5f);
					break;
				case PriceType.Exorbitant:
					GUI.color = new Color(0f, 1f, 0f);
					break;
				}
			}
			float priceFor = trad.GetPriceFor(action);
			string label = (TradeSession.TradeCurrency == TradeCurrency.Silver) ? priceFor.ToStringMoney(null) : priceFor.ToString();
			Rect rect2 = new Rect(rect);
			rect2.xMax -= 5f;
			rect2.xMin += 5f;
			if (Text.Anchor == TextAnchor.MiddleLeft)
			{
				rect2.xMax += 300f;
			}
			if (Text.Anchor == TextAnchor.MiddleRight)
			{
				rect2.xMin -= 300f;
			}
			Widgets.Label(rect2, label);
			GUI.color = Color.white;
		}

		// Token: 0x06007AC1 RID: 31425 RVA: 0x002B65B9 File Offset: 0x002B47B9
		private static void DrawWillNotTradeText(Rect rect, string text)
		{
			rect = rect.Rounded();
			GUI.color = TradeUI.NoTradeColor;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, text);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
		}

		// Token: 0x04004428 RID: 17448
		public const float CountColumnWidth = 75f;

		// Token: 0x04004429 RID: 17449
		public const float PriceColumnWidth = 100f;

		// Token: 0x0400442A RID: 17450
		public const float AdjustColumnWidth = 240f;

		// Token: 0x0400442B RID: 17451
		public const float TotalNumbersColumnsWidths = 590f;

		// Token: 0x0400442C RID: 17452
		public static readonly Color NoTradeColor = new Color(0.5f, 0.5f, 0.5f);
	}
}
