using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BB2 RID: 7090
	[StaticConstructorOnStartup]
	public static class TradeUI
	{
		// Token: 0x06009C31 RID: 39985 RVA: 0x002DD4AC File Offset: 0x002DB6AC
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
			if (trad.TraderWillTrade)
			{
				bool flash = Time.time - Dialog_Trade.lastCurrencyFlashTime < 1f && trad.IsCurrency;
				TransferableUIUtility.DoCountAdjustInterface(rect5, trad, index, trad.GetMinimumToTransfer(), trad.GetMaximumToTransfer(), flash, null, false);
			}
			else
			{
				TradeUI.DrawWillNotTradeIndication(rect5, trad);
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
			Rect idRect = new Rect(0f, 0f, num, rect.height);
			TransferableUIUtility.DrawTransferableInfo(trad, idRect, trad.TraderWillTrade ? Color.white : TradeUI.NoTradeColor);
			GenUI.ResetLabelAlign();
			GUI.EndGroup();
		}

		// Token: 0x06009C32 RID: 39986 RVA: 0x002DD71C File Offset: 0x002DB91C
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

		// Token: 0x06009C33 RID: 39987 RVA: 0x002DD9B0 File Offset: 0x002DBBB0
		private static void DrawWillNotTradeIndication(Rect rect, Tradeable trad)
		{
			rect = rect.Rounded();
			GUI.color = TradeUI.NoTradeColor;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, "TraderWillNotTrade".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
		}

		// Token: 0x040063AF RID: 25519
		public const float CountColumnWidth = 75f;

		// Token: 0x040063B0 RID: 25520
		public const float PriceColumnWidth = 100f;

		// Token: 0x040063B1 RID: 25521
		public const float AdjustColumnWidth = 240f;

		// Token: 0x040063B2 RID: 25522
		public const float TotalNumbersColumnsWidths = 590f;

		// Token: 0x040063B3 RID: 25523
		public static readonly Color NoTradeColor = new Color(0.5f, 0.5f, 0.5f);
	}
}
