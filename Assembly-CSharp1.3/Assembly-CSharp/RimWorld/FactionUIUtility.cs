using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001307 RID: 4871
	public static class FactionUIUtility
	{
		// Token: 0x06007516 RID: 29974 RVA: 0x00280154 File Offset: 0x0027E354
		public static void DoWindowContents(Rect fillRect, ref Vector2 scrollPosition, ref float scrollViewHeight, Faction scrollToFaction = null)
		{
			Rect rect = new Rect(0f, 0f, fillRect.width, fillRect.height);
			GUI.BeginGroup(rect);
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			if (Prefs.DevMode)
			{
				Widgets.CheckboxLabeled(new Rect(rect.width - 120f, 0f, 120f, 24f), "Dev: Show all", ref FactionUIUtility.showAll, false, null, null, false);
			}
			else
			{
				FactionUIUtility.showAll = false;
			}
			Rect outRect = new Rect(0f, 50f, rect.width, rect.height - 50f);
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, scrollViewHeight);
			FactionUIUtility.visibleFactions.Clear();
			foreach (Faction faction in Find.FactionManager.AllFactionsInViewOrder)
			{
				if ((!faction.IsPlayer && !faction.Hidden) || FactionUIUtility.showAll)
				{
					FactionUIUtility.visibleFactions.Add(faction);
				}
			}
			if (FactionUIUtility.visibleFactions.Count > 0)
			{
				Widgets.Label(new Rect(564f, 50f, 200f, 100f), "EnemyOf".Translate());
				outRect.yMin += Text.LineHeight;
				Widgets.BeginScrollView(outRect, ref scrollPosition, rect2, true);
				float num = 0f;
				foreach (Faction faction2 in FactionUIUtility.visibleFactions)
				{
					if ((!faction2.IsPlayer && !faction2.Hidden) || FactionUIUtility.showAll)
					{
						GUI.color = new Color(1f, 1f, 1f, 0.2f);
						Widgets.DrawLineHorizontal(0f, num, rect2.width);
						GUI.color = Color.white;
						if (faction2 == scrollToFaction)
						{
							scrollPosition.y = num;
						}
						num += FactionUIUtility.DrawFactionRow(faction2, num, rect2);
					}
				}
				if (Event.current.type == EventType.Layout)
				{
					scrollViewHeight = num;
				}
				Widgets.EndScrollView();
			}
			else
			{
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect, "NoFactions".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
			}
			GUI.EndGroup();
		}

		// Token: 0x06007517 RID: 29975 RVA: 0x002803D0 File Offset: 0x0027E5D0
		private static float DrawFactionRow(Faction faction, float rowY, Rect fillRect)
		{
			float num = fillRect.width - 250f - 40f - 70f - 54f - 16f - 120f;
			Faction[] array = (from f in Find.FactionManager.AllFactionsInViewOrder
			where f != faction && f.HostileTo(faction) && ((!f.IsPlayer && !f.Hidden) || FactionUIUtility.showAll)
			select f).ToArray<Faction>();
			Rect rect = new Rect(90f, rowY, 250f, 80f);
			Rect r = new Rect(24f, rowY + 4f, 42f, 42f);
			float num2 = 62f;
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			Widgets.DrawHighlightIfMouseover(new Rect(0f, rowY, fillRect.width, 80f));
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			FactionUIUtility.DrawFactionIconWithTooltip(r, faction);
			string label = faction.Name.CapitalizeFirst() + "\n" + faction.def.LabelCap + "\n" + ((faction.leader != null) ? (faction.LeaderTitle.CapitalizeFirst() + ": " + faction.leader.Name.ToStringFull) : "");
			Widgets.Label(rect, label);
			Rect rect2 = new Rect(rect.xMax, rowY, 40f, 80f);
			Widgets.InfoCardButton(rect2.x, rect2.y, faction);
			Rect rect3 = new Rect(rect2.xMax, rowY, 60f, 80f);
			if (ModsConfig.IdeologyActive && faction.ideos != null)
			{
				float num3 = rect3.x;
				float num4 = rect3.y;
				if (faction.ideos.PrimaryIdeo != null)
				{
					if (num3 + 40f > rect3.xMax)
					{
						num3 = rect3.x;
						num4 += 45f;
					}
					Rect rect4 = new Rect(num3, num4, 40f, 40f);
					IdeoUIUtility.DoIdeoIcon(rect4, faction.ideos.PrimaryIdeo, true, null);
					num3 += rect4.width + 5f;
					num3 = rect3.x;
					num4 += 45f;
				}
				List<Ideo> ideosMinorListForReading = faction.ideos.IdeosMinorListForReading;
				for (int i = 0; i < ideosMinorListForReading.Count; i++)
				{
					if (num3 + 22f > rect3.xMax)
					{
						num3 = rect3.x;
						num4 += 27f;
					}
					if (num4 + 22f > rect3.yMax)
					{
						break;
					}
					Rect rect5 = new Rect(num3, num4, 22f, 22f);
					IdeoUIUtility.DoIdeoIcon(rect5, ideosMinorListForReading[i], true, null);
					num3 += rect5.width + 5f;
				}
			}
			Rect rect6 = new Rect(rect3.xMax, rowY, 70f, 80f);
			if (!faction.IsPlayer)
			{
				string text = faction.PlayerRelationKind.GetLabelCap();
				if (faction.defeated)
				{
					text += "\n(" + "DefeatedLower".Translate() + ")";
				}
				GUI.color = faction.PlayerRelationKind.GetColor();
				Text.Anchor = TextAnchor.UpperCenter;
				Widgets.Label(rect6, text);
				if (faction.HasGoodwill && !faction.def.permanentEnemy)
				{
					Text.Font = GameFont.Medium;
					Widgets.Label(new Rect(rect6.x, rect6.y + 20f, rect6.width, rect6.height), faction.PlayerGoodwill.ToStringWithSign());
					Text.Font = GameFont.Small;
				}
				GenUI.ResetLabelAlign();
				GUI.color = Color.white;
				if (Mouse.IsOver(rect6))
				{
					TaggedString taggedString = "";
					if (faction.def.permanentEnemy)
					{
						taggedString = "CurrentGoodwillTip_PermanentEnemy".Translate();
					}
					else if (faction.HasGoodwill)
					{
						taggedString = "Goodwill".Translate().Colorize(ColoredText.TipSectionTitleColor) + ": " + (faction.PlayerGoodwill.ToStringWithSign() + ", " + faction.PlayerRelationKind.GetLabel()).Colorize(faction.PlayerRelationKind.GetColor());
						TaggedString ongoingEvents = FactionUIUtility.GetOngoingEvents(faction);
						if (!ongoingEvents.NullOrEmpty())
						{
							taggedString += "\n\n" + "OngoingEvents".Translate().Colorize(ColoredText.TipSectionTitleColor) + ":\n" + ongoingEvents;
						}
						TaggedString recentEvents = FactionUIUtility.GetRecentEvents(faction);
						if (!recentEvents.NullOrEmpty())
						{
							taggedString += "\n\n" + "RecentEvents".Translate().Colorize(ColoredText.TipSectionTitleColor) + ":\n" + recentEvents;
						}
						string s = "";
						switch (faction.PlayerRelationKind)
						{
						case FactionRelationKind.Hostile:
							s = "CurrentGoodwillTip_Hostile".Translate(0.ToString("F0"));
							break;
						case FactionRelationKind.Neutral:
							s = "CurrentGoodwillTip_Neutral".Translate(-75.ToString("F0"), 75.ToString("F0"));
							break;
						case FactionRelationKind.Ally:
							s = "CurrentGoodwillTip_Ally".Translate(0.ToString("F0"));
							break;
						}
						taggedString += "\n\n" + s.Colorize(ColoredText.SubtleGrayColor);
					}
					if (taggedString != "")
					{
						TooltipHandler.TipRegion(rect6, taggedString);
					}
					Widgets.DrawHighlight(rect6);
				}
			}
			Rect rect7 = new Rect(rect6.xMax, rowY, 54f, 80f);
			if (!faction.IsPlayer && faction.HasGoodwill && !faction.def.permanentEnemy)
			{
				FactionRelationKind relationKindForGoodwill = FactionUIUtility.GetRelationKindForGoodwill(faction.NaturalGoodwill);
				GUI.color = relationKindForGoodwill.GetColor();
				Rect rect8 = rect7.ContractedBy(7f);
				rect8.height = 20f;
				Text.Anchor = TextAnchor.UpperCenter;
				Widgets.DrawRectFast(rect8, Color.black, null);
				Widgets.Label(rect8, faction.NaturalGoodwill.ToStringWithSign());
				GenUI.ResetLabelAlign();
				GUI.color = Color.white;
				if (Mouse.IsOver(rect7))
				{
					TaggedString taggedString2 = "NaturalGoodwill".Translate().Colorize(ColoredText.TipSectionTitleColor) + ": " + faction.NaturalGoodwill.ToStringWithSign().Colorize(relationKindForGoodwill.GetColor());
					int goodwill = Mathf.Clamp(faction.NaturalGoodwill - 50, -100, 100);
					int goodwill2 = Mathf.Clamp(faction.NaturalGoodwill + 50, -100, 100);
					taggedString2 += string.Concat(new string[]
					{
						"\n",
						"NaturalGoodwillRange".Translate().Colorize(ColoredText.TipSectionTitleColor),
						": ",
						goodwill.ToString().Colorize(FactionUIUtility.GetRelationKindForGoodwill(goodwill).GetColor()),
						" "
					}) + "RangeTo".Translate() + " " + goodwill2.ToString().Colorize(FactionUIUtility.GetRelationKindForGoodwill(goodwill2).GetColor());
					TaggedString naturalGoodwillExplanation = FactionUIUtility.GetNaturalGoodwillExplanation(faction);
					if (!naturalGoodwillExplanation.NullOrEmpty())
					{
						taggedString2 += "\n\n" + "AffectedBy".Translate().Colorize(ColoredText.TipSectionTitleColor) + "\n" + naturalGoodwillExplanation;
					}
					taggedString2 += "\n\n" + "NaturalGoodwillDescription".Translate(1.25f.ToStringPercent()).Colorize(ColoredText.SubtleGrayColor);
					TooltipHandler.TipRegion(rect7, taggedString2);
					Widgets.DrawHighlight(rect7);
				}
			}
			float num5 = rect7.xMax;
			for (int j = 0; j < array.Length; j++)
			{
				if (num5 >= rect7.xMax + num)
				{
					num5 = rect7.xMax;
					rowY += 27f;
					num2 += 27f;
				}
				FactionUIUtility.DrawFactionIconWithTooltip(new Rect(num5, rowY + 4f, 22f, 22f), array[j]);
				num5 += 27f;
			}
			return Mathf.Max(80f, num2);
		}

		// Token: 0x06007518 RID: 29976 RVA: 0x00280D20 File Offset: 0x0027EF20
		public static void DrawFactionIconWithTooltip(Rect r, Faction faction)
		{
			GUI.color = faction.Color;
			GUI.DrawTexture(r, faction.def.FactionIcon);
			GUI.color = Color.white;
			if (Mouse.IsOver(r))
			{
				TipSignal tip = new TipSignal(() => string.Concat(new string[]
				{
					faction.Name.Colorize(ColoredText.TipSectionTitleColor),
					"\n",
					faction.def.LabelCap.Resolve(),
					"\n\n",
					faction.def.description
				}), faction.loadID ^ 1938473043);
				TooltipHandler.TipRegion(r, tip);
				Widgets.DrawHighlight(r);
			}
			if (Widgets.ButtonInvisible(r, false))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(faction));
			}
		}

		// Token: 0x06007519 RID: 29977 RVA: 0x00280DC4 File Offset: 0x0027EFC4
		public static void DrawRelatedFactionInfo(Rect rect, Faction faction, ref float curY)
		{
			Text.Anchor = TextAnchor.LowerRight;
			curY += 10f;
			FactionRelationKind playerRelationKind = faction.PlayerRelationKind;
			string text = faction.Name.CapitalizeFirst() + "\n" + "goodwill".Translate().CapitalizeFirst() + ": " + faction.PlayerGoodwill.ToStringWithSign();
			GUI.color = Color.gray;
			Rect rect2 = new Rect(rect.x, curY, rect.width, Text.CalcHeight(text, rect.width));
			Widgets.Label(rect2, text);
			curY += rect2.height;
			GUI.color = playerRelationKind.GetColor();
			Rect rect3 = new Rect(rect2.x, curY - 7f, rect2.width, 25f);
			Widgets.Label(rect3, playerRelationKind.GetLabelCap());
			curY += rect3.height;
			GUI.color = Color.white;
			GenUI.ResetLabelAlign();
		}

		// Token: 0x0600751A RID: 29978 RVA: 0x00280ECC File Offset: 0x0027F0CC
		private static TaggedString GetRecentEvents(Faction other)
		{
			StringBuilder stringBuilder = new StringBuilder();
			List<HistoryEventDef> allDefsListForReading = DefDatabase<HistoryEventDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				int recentCountWithinTicks = Find.HistoryEventsManager.GetRecentCountWithinTicks(allDefsListForReading[i], 3600000, other);
				if (recentCountWithinTicks > 0)
				{
					Find.HistoryEventsManager.GetRecent(allDefsListForReading[i], 3600000, FactionUIUtility.tmpTicks, FactionUIUtility.tmpCustomGoodwill, other);
					int num = 0;
					for (int j = 0; j < FactionUIUtility.tmpTicks.Count; j++)
					{
						num += FactionUIUtility.tmpCustomGoodwill[j];
					}
					if (num != 0)
					{
						string text = "- " + allDefsListForReading[i].LabelCap;
						if (recentCountWithinTicks != 1)
						{
							text = text + " x" + recentCountWithinTicks;
						}
						text = text + ": " + num.ToStringWithSign().Colorize((num >= 0) ? FactionRelationKind.Ally.GetColor() : FactionRelationKind.Hostile.GetColor());
						stringBuilder.AppendInNewLine(text);
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600751B RID: 29979 RVA: 0x00280FE2 File Offset: 0x0027F1E2
		private static FactionRelationKind GetRelationKindForGoodwill(int goodwill)
		{
			if (goodwill <= -75)
			{
				return FactionRelationKind.Hostile;
			}
			if (goodwill >= 75)
			{
				return FactionRelationKind.Ally;
			}
			return FactionRelationKind.Neutral;
		}

		// Token: 0x0600751C RID: 29980 RVA: 0x00280FF4 File Offset: 0x0027F1F4
		private static TaggedString GetOngoingEvents(Faction other)
		{
			StringBuilder stringBuilder = new StringBuilder();
			List<GoodwillSituationManager.CachedSituation> situations = Find.GoodwillSituationManager.GetSituations(other);
			for (int i = 0; i < situations.Count; i++)
			{
				if (situations[i].maxGoodwill < 100)
				{
					string text = "- " + situations[i].def.Worker.GetPostProcessedLabelCap(other);
					text = text + ": " + (situations[i].maxGoodwill.ToStringWithSign() + " " + "max".Translate()).Colorize(FactionRelationKind.Hostile.GetColor());
					stringBuilder.AppendInNewLine(text);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600751D RID: 29981 RVA: 0x002810AC File Offset: 0x0027F2AC
		private static TaggedString GetNaturalGoodwillExplanation(Faction other)
		{
			StringBuilder stringBuilder = new StringBuilder();
			List<GoodwillSituationManager.CachedSituation> situations = Find.GoodwillSituationManager.GetSituations(other);
			for (int i = 0; i < situations.Count; i++)
			{
				if (situations[i].naturalGoodwillOffset != 0)
				{
					string text = "- " + situations[i].def.Worker.GetPostProcessedLabelCap(other);
					text = text + ": " + situations[i].naturalGoodwillOffset.ToStringWithSign().Colorize((situations[i].naturalGoodwillOffset >= 0) ? FactionRelationKind.Ally.GetColor() : FactionRelationKind.Hostile.GetColor());
					stringBuilder.AppendInNewLine(text);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040040A1 RID: 16545
		private static bool showAll;

		// Token: 0x040040A2 RID: 16546
		private static List<Faction> visibleFactions = new List<Faction>();

		// Token: 0x040040A3 RID: 16547
		private const float FactionIconRectSize = 42f;

		// Token: 0x040040A4 RID: 16548
		private const float FactionIconRectGapX = 24f;

		// Token: 0x040040A5 RID: 16549
		private const float FactionIconRectGapY = 4f;

		// Token: 0x040040A6 RID: 16550
		private const float RowMinHeight = 80f;

		// Token: 0x040040A7 RID: 16551
		private const float LabelRowHeight = 50f;

		// Token: 0x040040A8 RID: 16552
		private const float NameLeftMargin = 15f;

		// Token: 0x040040A9 RID: 16553
		private const float FactionIconSpacing = 5f;

		// Token: 0x040040AA RID: 16554
		private const float IdeoIconSpacing = 5f;

		// Token: 0x040040AB RID: 16555
		private const float BasicsColumnWidth = 250f;

		// Token: 0x040040AC RID: 16556
		private const float InfoColumnWidth = 40f;

		// Token: 0x040040AD RID: 16557
		private const float IdeosColumnWidth = 60f;

		// Token: 0x040040AE RID: 16558
		private const float RelationsColumnWidth = 70f;

		// Token: 0x040040AF RID: 16559
		private const float NaturalGoodwillColumnWidth = 54f;

		// Token: 0x040040B0 RID: 16560
		private static List<int> tmpTicks = new List<int>();

		// Token: 0x040040B1 RID: 16561
		private static List<int> tmpCustomGoodwill = new List<int>();
	}
}
