using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02001816 RID: 6166
	public static class WorldFactionsUIUtility
	{
		// Token: 0x06009073 RID: 36979 RVA: 0x0033CE40 File Offset: 0x0033B040
		public static void DoWindowContents(Rect rect, ref Dictionary<FactionDef, int> factionCounts, bool isDefaultFactionCounts, out bool resetFactions)
		{
			resetFactions = false;
			Rect rect2 = new Rect(rect.x, rect.y, rect.width, Text.LineHeight);
			Widgets.Label(rect2, "Factions".Translate());
			TooltipHandler.TipRegion(rect2, () => "FactionSelectionDesc".Translate(12), 4534123);
			float num = Text.LineHeight + 4f;
			if (!isDefaultFactionCounts)
			{
				Rect rect3 = new Rect(rect.xMax - 200f - 16f, rect.y, 200f, Text.LineHeight);
				resetFactions = Widgets.ButtonText(rect3, "ResetFactionsToDefault".Translate(), true, true, true);
			}
			WorldFactionsUIUtility.factionList.Clear();
			foreach (KeyValuePair<FactionDef, int> keyValuePair in factionCounts)
			{
				WorldFactionsUIUtility.factionList.Add(keyValuePair.Key);
			}
			float num2 = 24f;
			float num3 = (float)WorldFactionsUIUtility.factionList.Count * (num2 + 8f);
			float b = rect.height - num - Text.LineHeight;
			Rect outRect = new Rect(rect.x, rect.y + num, rect.width, Mathf.Min(num3, b));
			Rect rect4 = new Rect(outRect.x, outRect.y, outRect.width - 16f, num3);
			Widgets.DrawBoxSolid(new Rect(outRect.x, outRect.y, outRect.width - 16f, outRect.height), WorldFactionsUIUtility.BackgroundColor);
			Widgets.BeginScrollView(outRect, ref WorldFactionsUIUtility.scrollPosition, rect4, true);
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = rect4.width;
			listing_Standard.Begin(rect4);
			foreach (FactionDef factionDef in WorldFactionsUIUtility.factionList)
			{
				listing_Standard.Gap(4f);
				WorldFactionsUIUtility.DoRow(listing_Standard.GetRect(num2), factionDef, ref factionCounts);
				listing_Standard.Gap(4f);
			}
			listing_Standard.End();
			Widgets.EndScrollView();
			float num4 = outRect.yMax + 4f;
			int num5 = 0;
			foreach (KeyValuePair<FactionDef, int> keyValuePair2 in factionCounts)
			{
				if (!keyValuePair2.Key.hidden)
				{
					num5 += keyValuePair2.Value;
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (num5 == 0)
			{
				stringBuilder.AppendLine("FactionsDisabledWarning".Translate());
			}
			else
			{
				if (num5 > 11)
				{
					stringBuilder.AppendLine("RecommendedFactionCount".Translate(11));
				}
				if (ModsConfig.RoyaltyActive && factionCounts.ContainsKey(FactionDefOf.Empire) && factionCounts[FactionDefOf.Empire] <= 0)
				{
					stringBuilder.AppendLine("Warning".Translate() + ": " + "FactionDisabledContentWarning".Translate(FactionDefOf.Empire.label));
				}
				if (factionCounts.ContainsKey(FactionDefOf.Mechanoid) && factionCounts[FactionDefOf.Mechanoid] <= 0)
				{
					stringBuilder.AppendLine("Warning".Translate() + ": " + "MechanoidsDisabledContentWarning".Translate(FactionDefOf.Mechanoid.label));
				}
				if (factionCounts.ContainsKey(FactionDefOf.Insect) && factionCounts[FactionDefOf.Insect] <= 0)
				{
					stringBuilder.AppendLine("Warning".Translate() + ": " + "InsectsDisabledContentWarning".Translate(FactionDefOf.Insect.label));
				}
			}
			if (stringBuilder.Length > 0)
			{
				Rect rect5 = new Rect(rect.x, num4, rect.width, rect.yMax - num4);
				GUI.color = Color.yellow;
				Text.Font = GameFont.Tiny;
				Text.WordWrap = true;
				Widgets.Label(rect5, stringBuilder.ToString());
				GUI.color = Color.white;
			}
		}

		// Token: 0x06009074 RID: 36980 RVA: 0x0033D2B8 File Offset: 0x0033B4B8
		public static void DoRow(Rect rect, FactionDef factionDef, ref Dictionary<FactionDef, int> factionCounts)
		{
			GUI.BeginGroup(rect);
			WidgetRow widgetRow = new WidgetRow(4f, 0f, UIDirection.RightThenUp, 99999f, 4f);
			bool flag = factionCounts[factionDef] == 0;
			if (!flag)
			{
				if (widgetRow.ButtonText(" - ", null, true, true, true, null) && TutorSystem.AllowAction("ConfiguringWorldFactions"))
				{
					Dictionary<FactionDef, int> dictionary = factionCounts;
					dictionary[factionDef]--;
					SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				}
			}
			else
			{
				widgetRow.ButtonRect(" - ", null);
			}
			Text.Anchor = TextAnchor.MiddleCenter;
			widgetRow.Label(factionCounts[factionDef].ToString(), 18f, null, -1f);
			Text.Anchor = TextAnchor.UpperLeft;
			bool flag2 = factionCounts[factionDef] >= factionDef.maxConfigurableAtWorldCreation;
			bool flag3 = (from f in factionCounts
			where !f.Key.hidden
			select f).Sum((KeyValuePair<FactionDef, int> f) => f.Value) >= 12;
			if (flag2 || (flag3 && !factionDef.hidden))
			{
				Rect rect2 = widgetRow.ButtonRect(" + ", null);
				if (Mouse.IsOver(rect2))
				{
					if (flag2)
					{
						TooltipHandler.TipRegion(rect2, "MaxFactionsForType".Translate(factionDef.maxConfigurableAtWorldCreation, factionDef.label));
					}
					else if (!factionDef.hidden)
					{
						TooltipHandler.TipRegion(rect2, "TotalFactionsAllowed".Translate(12));
					}
				}
			}
			else if (widgetRow.ButtonText(" + ", null, true, true, true, null) && TutorSystem.AllowAction("ConfiguringWorldFactions"))
			{
				Dictionary<FactionDef, int> dictionary = factionCounts;
				dictionary[factionDef]++;
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			}
			widgetRow.Gap(4f);
			GUI.color = factionDef.DefaultColor;
			Rect rect3 = widgetRow.Icon(factionDef.FactionIcon, null);
			GUI.color = Color.white;
			widgetRow.Gap(4f);
			Text.Anchor = TextAnchor.MiddleCenter;
			GUI.color = (flag ? Color.gray : Color.white);
			Rect rect4 = widgetRow.Label(factionDef.LabelCap, -1f, null, -1f);
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect5 = new Rect(rect3.x, rect3.y, rect.width - rect3.x - 4f, rect4.height);
			if (Mouse.IsOver(rect5))
			{
				string text = factionDef.LabelCap.Colorize(ColoredText.TipSectionTitleColor);
				if (!factionDef.description.NullOrEmpty())
				{
					text = text + "\n\n" + factionDef.description;
				}
				TooltipHandler.TipRegion(rect5, text);
				Widgets.DrawHighlight(rect5);
			}
			GUI.EndGroup();
		}

		// Token: 0x04005AD9 RID: 23257
		private static Vector2 scrollPosition;

		// Token: 0x04005ADA RID: 23258
		private static List<FactionDef> factionList = new List<FactionDef>();

		// Token: 0x04005ADB RID: 23259
		private const int MaxVisibleFactions = 12;

		// Token: 0x04005ADC RID: 23260
		private const int MaxVisibleFactionsRecommended = 11;

		// Token: 0x04005ADD RID: 23261
		private const string PlusSymbol = " + ";

		// Token: 0x04005ADE RID: 23262
		private const string MinusSymbol = " - ";

		// Token: 0x04005ADF RID: 23263
		private const float FactionNumberWidth = 18f;

		// Token: 0x04005AE0 RID: 23264
		private const float ResetButtonWidth = 200f;

		// Token: 0x04005AE1 RID: 23265
		private static Color BackgroundColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 15);
	}
}
