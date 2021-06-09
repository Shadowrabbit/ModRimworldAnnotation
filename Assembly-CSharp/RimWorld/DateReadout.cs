using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AD7 RID: 6871
	public static class DateReadout
	{
		// Token: 0x170017C5 RID: 6085
		// (get) Token: 0x06009755 RID: 38741 RVA: 0x00064E19 File Offset: 0x00063019
		public static float Height
		{
			get
			{
				return (float)(48 + (DateReadout.SeasonLabelVisible ? 26 : 0));
			}
		}

		// Token: 0x170017C6 RID: 6086
		// (get) Token: 0x06009756 RID: 38742 RVA: 0x00064E2B File Offset: 0x0006302B
		private static bool SeasonLabelVisible
		{
			get
			{
				return !WorldRendererUtility.WorldRenderedNow && Find.CurrentMap != null;
			}
		}

		// Token: 0x06009757 RID: 38743 RVA: 0x00064E3E File Offset: 0x0006303E
		static DateReadout()
		{
			DateReadout.Reset();
		}

		// Token: 0x06009758 RID: 38744 RVA: 0x002C6A14 File Offset: 0x002C4C14
		public static void Reset()
		{
			DateReadout.dateString = null;
			DateReadout.dateStringDay = -1;
			DateReadout.dateStringSeason = Season.Undefined;
			DateReadout.dateStringQuadrum = Quadrum.Undefined;
			DateReadout.dateStringYear = -1;
			DateReadout.fastHourStrings.Clear();
			for (int i = 0; i < 24; i++)
			{
				DateReadout.fastHourStrings.Add(i + "LetterHour".Translate());
			}
			DateReadout.seasonsCached.Clear();
			int length = Enum.GetValues(typeof(Season)).Length;
			for (int j = 0; j < length; j++)
			{
				Season season = (Season)j;
				DateReadout.seasonsCached.Add((season == Season.Undefined) ? "" : season.LabelCap());
			}
		}

		// Token: 0x06009759 RID: 38745 RVA: 0x002C6AC4 File Offset: 0x002C4CC4
		public static void DateOnGUI(Rect dateRect)
		{
			Vector2 vector;
			if (WorldRendererUtility.WorldRenderedNow && Find.WorldSelector.selectedTile >= 0)
			{
				vector = Find.WorldGrid.LongLatOf(Find.WorldSelector.selectedTile);
			}
			else if (WorldRendererUtility.WorldRenderedNow && Find.WorldSelector.NumSelectedObjects > 0)
			{
				vector = Find.WorldGrid.LongLatOf(Find.WorldSelector.FirstSelectedObject.Tile);
			}
			else
			{
				if (Find.CurrentMap == null)
				{
					return;
				}
				vector = Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile);
			}
			int index = GenDate.HourInteger((long)Find.TickManager.TicksAbs, vector.x);
			int num = GenDate.DayOfTwelfth((long)Find.TickManager.TicksAbs, vector.x);
			Season season = GenDate.Season((long)Find.TickManager.TicksAbs, vector);
			Quadrum quadrum = GenDate.Quadrum((long)Find.TickManager.TicksAbs, vector.x);
			int num2 = GenDate.Year((long)Find.TickManager.TicksAbs, vector.x);
			string text = DateReadout.SeasonLabelVisible ? DateReadout.seasonsCached[(int)season] : "";
			if (num != DateReadout.dateStringDay || season != DateReadout.dateStringSeason || quadrum != DateReadout.dateStringQuadrum || num2 != DateReadout.dateStringYear)
			{
				DateReadout.dateString = GenDate.DateReadoutStringAt((long)Find.TickManager.TicksAbs, vector);
				DateReadout.dateStringDay = num;
				DateReadout.dateStringSeason = season;
				DateReadout.dateStringQuadrum = quadrum;
				DateReadout.dateStringYear = num2;
			}
			Text.Font = GameFont.Small;
			float num3 = Mathf.Max(Mathf.Max(Text.CalcSize(DateReadout.fastHourStrings[index]).x, Text.CalcSize(DateReadout.dateString).x), Text.CalcSize(text).x) + 7f;
			dateRect.xMin = dateRect.xMax - num3;
			if (Mouse.IsOver(dateRect))
			{
				Widgets.DrawHighlight(dateRect);
			}
			GUI.BeginGroup(dateRect);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperRight;
			Rect rect = dateRect.AtZero();
			rect.xMax -= 7f;
			Widgets.Label(rect, DateReadout.fastHourStrings[index]);
			rect.yMin += 26f;
			Widgets.Label(rect, DateReadout.dateString);
			rect.yMin += 26f;
			if (!text.NullOrEmpty())
			{
				Widgets.Label(rect, text);
			}
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.EndGroup();
			if (Mouse.IsOver(dateRect))
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < 4; i++)
				{
					Quadrum quadrum2 = (Quadrum)i;
					stringBuilder.AppendLine(quadrum2.Label() + " - " + quadrum2.GetSeason(vector.y).LabelCap());
				}
				TaggedString taggedString = "DateReadoutTip".Translate(GenDate.DaysPassed, 15, season.LabelCap(), 15, GenDate.Quadrum((long)GenTicks.TicksAbs, vector.x).Label(), stringBuilder.ToString());
				TooltipHandler.TipRegion(dateRect, new TipSignal(taggedString, 86423));
			}
		}

		// Token: 0x040060A8 RID: 24744
		private static string dateString;

		// Token: 0x040060A9 RID: 24745
		private static int dateStringDay = -1;

		// Token: 0x040060AA RID: 24746
		private static Season dateStringSeason = Season.Undefined;

		// Token: 0x040060AB RID: 24747
		private static Quadrum dateStringQuadrum = Quadrum.Undefined;

		// Token: 0x040060AC RID: 24748
		private static int dateStringYear = -1;

		// Token: 0x040060AD RID: 24749
		private static readonly List<string> fastHourStrings = new List<string>();

		// Token: 0x040060AE RID: 24750
		private static readonly List<string> seasonsCached = new List<string>();

		// Token: 0x040060AF RID: 24751
		private const float DateRightPadding = 7f;
	}
}
