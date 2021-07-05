using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020012DA RID: 4826
	public class Dialog_AutoSlaughter : Window
	{
		// Token: 0x17001434 RID: 5172
		// (get) Token: 0x06007352 RID: 29522 RVA: 0x00268688 File Offset: 0x00266888
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1050f, 600f);
			}
		}

		// Token: 0x06007353 RID: 29523 RVA: 0x0026869C File Offset: 0x0026689C
		public Dialog_AutoSlaughter(Map map)
		{
			this.map = map;
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06007354 RID: 29524 RVA: 0x00268708 File Offset: 0x00266908
		public override void PostOpen()
		{
			base.PostOpen();
			this.animalCounts.Clear();
			foreach (AutoSlaughterConfig autoSlaughterConfig in this.map.autoSlaughterManager.configs)
			{
				Dialog_AutoSlaughter.AnimalCountRecord value = default(Dialog_AutoSlaughter.AnimalCountRecord);
				this.CountPlayerAnimals(this.map, autoSlaughterConfig.animal, out value.male, out value.female, out value.total, out value.unslaughterable);
				this.animalCounts.Add(autoSlaughterConfig.animal, value);
			}
			this.configsOrdered = (from c in this.map.autoSlaughterManager.configs
			orderby this.animalCounts[c.animal].total descending, this.animalCounts[c.animal].unslaughterable descending, c.animal.label
			select c).ToList<AutoSlaughterConfig>();
		}

		// Token: 0x06007355 RID: 29525 RVA: 0x00268818 File Offset: 0x00266A18
		public override void DoWindowContents(Rect inRect)
		{
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = inRect.width - 16f - 4f;
			Rect outRect = new Rect(inRect);
			outRect.yMax -= Window.CloseButSize.y;
			outRect.yMin += 8f;
			this.viewRect = new Rect(0f, 0f, outRect.width - 16f, 30f * (float)(this.configsOrdered.Count + 1));
			Widgets.BeginScrollView(outRect, ref this.scrollPos, this.viewRect, true);
			listing_Standard.Begin(this.viewRect);
			this.DoAnimalHeader(this.map, listing_Standard.GetRect(24f), listing_Standard.GetRect(24f));
			listing_Standard.Gap(6f);
			List<AutoSlaughterConfig> configs = this.map.autoSlaughterManager.configs;
			int num = 0;
			foreach (AutoSlaughterConfig config in this.configsOrdered)
			{
				Rect rect = listing_Standard.GetRect(24f);
				this.DoAnimalRow(this.map, rect, config, num);
				listing_Standard.Gap(6f);
				num++;
			}
			listing_Standard.End();
			Widgets.EndScrollView();
			Rect rect2 = new Rect(inRect.x + inRect.width / 2f + Window.CloseButSize.x / 2f + 20f, inRect.y + inRect.height - 40f, 395f, 50f);
			Color color = GUI.color;
			GameFont font = Text.Font;
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleCenter;
			GUI.color = Color.gray;
			Text.Font = GameFont.Tiny;
			Widgets.Label(rect2, "AutoSlaugtherTip".Translate());
			Text.Font = font;
			Text.Anchor = anchor;
			GUI.color = color;
		}

		// Token: 0x06007356 RID: 29526 RVA: 0x00268A24 File Offset: 0x00266C24
		private void CountPlayerAnimals(Map map, ThingDef animal, out int currentMales, out int currentFemales, out int currentTotal, out int unslaughterableTotal)
		{
			currentMales = (currentFemales = (currentTotal = (unslaughterableTotal = 0)));
			foreach (Pawn pawn in map.mapPawns.SpawnedColonyAnimals)
			{
				if (pawn.def == animal)
				{
					if (!AutoSlaughterManager.CanEverAutoSlaughter(pawn))
					{
						unslaughterableTotal++;
					}
					else
					{
						if (pawn.gender == Gender.Male)
						{
							currentMales++;
						}
						if (pawn.gender == Gender.Female)
						{
							currentFemales++;
						}
						currentTotal++;
					}
				}
			}
		}

		// Token: 0x06007357 RID: 29527 RVA: 0x00268AD0 File Offset: 0x00266CD0
		private float CalculateLabelWidth(Rect rect)
		{
			float num = 64f;
			float num2 = 184f;
			float num3 = 62f;
			return rect.width - 24f - 4f - 4f - 4f - num - num2 - num - num2 - num - num2 - num3 - 24f;
		}

		// Token: 0x06007358 RID: 29528 RVA: 0x00268B24 File Offset: 0x00266D24
		private void DoMaxColumn(WidgetRow row, ref int val, ref string buffer, int current)
		{
			int num = val;
			if (val == -1)
			{
				float num2 = 68f;
				float width = (184f - num2) / 2f;
				row.Gap(width);
				if (row.ButtonIconWithBG(TexButton.Infinity, 48f, "AutoSlaughterTooltipSetLimit".Translate(), true))
				{
					SoundDefOf.Click.PlayOneShotOnCamera(null);
					val = current;
				}
				row.Gap(width);
			}
			else
			{
				row.CellGap = 0f;
				if (row.ButtonText("-10", null, true, true, true, new float?(12f)))
				{
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					val -= 10;
					buffer = null;
				}
				if (row.ButtonText("-1", null, true, true, true, new float?(12f)))
				{
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					val--;
					buffer = null;
				}
				row.TextFieldNumeric<int>(ref val, ref buffer, 40f);
				if (row.ButtonText("+1", null, true, true, true, new float?(12f)))
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					val++;
					buffer = null;
				}
				if (row.ButtonText("+10", null, true, true, true, new float?(12f)))
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					val += 10;
					buffer = null;
				}
				val = Mathf.Max(0, val);
				if (row.ButtonIconWithBG(TexButton.Infinity, 12f, null, true))
				{
					SoundDefOf.Click.PlayOneShotOnCamera(null);
					val = -1;
					buffer = null;
				}
				row.CellGap = 4f;
			}
			if (num != val)
			{
				this.map.autoSlaughterManager.Notify_ConfigChanged();
			}
		}

		// Token: 0x06007359 RID: 29529 RVA: 0x00268CB8 File Offset: 0x00266EB8
		private void DoAnimalHeader(Map map, Rect rect1, Rect rect2)
		{
			float width = this.CalculateLabelWidth(rect1);
			GUI.BeginGroup(new Rect(rect1.x, rect1.y, rect1.width, rect1.height + rect2.height + 1f));
			int num = 0;
			foreach (Rect rect3 in this.tmpGroupRects)
			{
				if (num % 2 == 1)
				{
					Widgets.DrawLightHighlight(rect3);
					Widgets.DrawLightHighlight(rect3);
				}
				else
				{
					Widgets.DrawLightHighlight(rect3);
				}
				GUI.color = Color.gray;
				if (num > 0)
				{
					Widgets.DrawLineVertical(rect3.xMin, 0f, rect1.height + rect2.height + 1f);
				}
				if (num < this.tmpGroupRects.Count - 1)
				{
					Widgets.DrawLineVertical(rect3.xMax, 0f, rect1.height + rect2.height + 1f);
				}
				GUI.color = Color.white;
				num++;
			}
			foreach (Rect rect4 in this.tmpMouseoverHighlightRects)
			{
				Widgets.DrawHighlightIfMouseover(rect4);
			}
			GUI.EndGroup();
			this.tmpMouseoverHighlightRects.Clear();
			this.tmpGroupRects.Clear();
			GUI.BeginGroup(rect1);
			WidgetRow widgetRow = new WidgetRow(0f, 0f, UIDirection.RightThenUp, 99999f, 4f);
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleCenter;
			widgetRow.Label("", 24f, null, -1f);
			float finalX = widgetRow.FinalX;
			widgetRow.Label("", width, "AutoSlaugtherHeaderTooltipLabel".Translate(), -1f);
			this.tmpMouseoverHighlightRects.Add(new Rect(finalX, rect1.height, widgetRow.FinalX - finalX, rect2.height));
			this.tmpGroupRects.Add(new Rect(finalX, rect1.height, widgetRow.FinalX - finalX, rect2.height));
			finalX = widgetRow.FinalX;
			widgetRow.Label("", 60f, "AutoSlaugtherHeaderTooltipCurrentTotal".Translate(), -1f);
			this.tmpMouseoverHighlightRects.Add(new Rect(finalX, rect1.height, widgetRow.FinalX - finalX, rect2.height));
			float finalX2 = widgetRow.FinalX;
			widgetRow.Label("", 180f, "AutoSlaugtherHeaderTooltipMaxTotal".Translate(), -1f);
			this.tmpMouseoverHighlightRects.Add(new Rect(finalX2, rect1.height, widgetRow.FinalX - finalX2, rect2.height));
			Dialog_AutoSlaughter.<DoAnimalHeader>g__DrawEquidistantLabel|22_0("AutoSlaugtherHeaderColTotal".Translate(), "AutoSlaugtherHeaderColCurrent".Translate(), "AutoSlaugtherHeaderColMax".Translate(), finalX, finalX2, widgetRow.FinalX);
			this.tmpGroupRects.Add(new Rect(finalX, rect1.height, widgetRow.FinalX - finalX, rect2.height));
			finalX = widgetRow.FinalX;
			widgetRow.Label("", 60f, "AutoSlaugtherHeaderTooltipCurrentMales".Translate(), -1f);
			this.tmpMouseoverHighlightRects.Add(new Rect(finalX, rect1.height, widgetRow.FinalX - finalX, rect2.height));
			finalX2 = widgetRow.FinalX;
			widgetRow.Label("", 180f, "AutoSlaugtherHeaderTooltipMaxMales".Translate(), -1f);
			this.tmpMouseoverHighlightRects.Add(new Rect(finalX2, rect1.height, widgetRow.FinalX - finalX2, rect2.height));
			Dialog_AutoSlaughter.<DoAnimalHeader>g__DrawEquidistantLabel|22_0(Gender.Male.GetLabel(false).CapitalizeFirst(), "AutoSlaugtherHeaderColCurrent".Translate(), "AutoSlaugtherHeaderColMax".Translate(), finalX, finalX2, widgetRow.FinalX);
			this.tmpGroupRects.Add(new Rect(finalX, rect1.height, widgetRow.FinalX - finalX, rect2.height));
			finalX = widgetRow.FinalX;
			widgetRow.Label("", 60f, "AutoSlaugtherHeaderTooltipCurrentFemales".Translate(), -1f);
			this.tmpMouseoverHighlightRects.Add(new Rect(finalX, rect1.height, widgetRow.FinalX - finalX, rect2.height));
			finalX2 = widgetRow.FinalX;
			widgetRow.Label("", 180f, "AutoSlaugtherHeaderTooltipMaxFemales".Translate(), -1f);
			this.tmpMouseoverHighlightRects.Add(new Rect(finalX2, rect1.height, widgetRow.FinalX - finalX2, rect2.height));
			Dialog_AutoSlaughter.<DoAnimalHeader>g__DrawEquidistantLabel|22_0(Gender.Female.GetLabel(false).CapitalizeFirst(), "AutoSlaugtherHeaderColCurrent".Translate(), "AutoSlaugtherHeaderColMax".Translate(), finalX, finalX2, widgetRow.FinalX);
			this.tmpGroupRects.Add(new Rect(finalX, rect1.height, widgetRow.FinalX - finalX, rect2.height));
			finalX = widgetRow.FinalX;
			widgetRow.Label("", 60f, "AutoSlaugtherHeaderTooltipOther".Translate(), -1f);
			this.tmpMouseoverHighlightRects.Add(new Rect(finalX, rect1.height, widgetRow.FinalX - finalX, rect2.height));
			Dialog_AutoSlaughter.<DoAnimalHeader>g__DrawSingleLabel|22_1("AutoSlaugtherHeaderColOther".Translate().CapitalizeFirst(), finalX, widgetRow.FinalX);
			this.tmpGroupRects.Add(new Rect(finalX, rect1.height, widgetRow.FinalX - finalX, rect2.height));
			Text.Anchor = anchor;
			GUI.EndGroup();
			GUI.BeginGroup(rect2);
			WidgetRow widgetRow2 = new WidgetRow(0f, 0f, UIDirection.RightThenUp, 99999f, 4f);
			TextAnchor anchor2 = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleCenter;
			widgetRow2.Label("", 24f, null, -1f);
			widgetRow2.Label("AutoSlaugtherHeaderColLabel".Translate(), width, "AutoSlaugtherHeaderTooltipLabel".Translate(), -1f);
			widgetRow2.Label("AutoSlaugtherHeaderColCurrent".Translate(), 60f, "AutoSlaugtherHeaderTooltipCurrentTotal".Translate(), -1f);
			widgetRow2.Label("AutoSlaugtherHeaderColMax".Translate(), 180f, "AutoSlaugtherHeaderTooltipMaxTotal".Translate(), -1f);
			widgetRow2.Label("AutoSlaugtherHeaderColCurrent".Translate(), 60f, "AutoSlaugtherHeaderTooltipCurrentMales".Translate(), -1f);
			widgetRow2.Label("AutoSlaugtherHeaderColMax".Translate(), 180f, "AutoSlaugtherHeaderTooltipMaxMales".Translate(), -1f);
			widgetRow2.Label("AutoSlaugtherHeaderColCurrent".Translate(), 60f, "AutoSlaugtherHeaderTooltipCurrentFemales".Translate(), -1f);
			widgetRow2.Label("AutoSlaugtherHeaderColMax".Translate(), 180f, "AutoSlaugtherHeaderTooltipMaxFemales".Translate(), -1f);
			widgetRow2.Label("AutoSlaugtherHeaderColCurrent".Translate(), 60f, "AutoSlaugtherHeaderTooltipOther".Translate(), -1f);
			Text.Anchor = anchor2;
			GUI.EndGroup();
			GUI.color = Color.gray;
			Widgets.DrawLineHorizontal(rect2.x, rect2.y + rect2.height + 1f, rect2.width);
			GUI.color = Color.white;
		}

		// Token: 0x0600735A RID: 29530 RVA: 0x002694F0 File Offset: 0x002676F0
		private void DoAnimalRow(Map map, Rect rect, AutoSlaughterConfig config, int index)
		{
			if (index % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			Color color = GUI.color;
			Dialog_AutoSlaughter.AnimalCountRecord animalCountRecord = this.animalCounts[config.animal];
			float width = this.CalculateLabelWidth(rect);
			GUI.BeginGroup(rect);
			Dialog_AutoSlaughter.<>c__DisplayClass23_0 CS$<>8__locals1;
			CS$<>8__locals1.row = new WidgetRow(0f, 0f, UIDirection.RightThenUp, 99999f, 4f);
			CS$<>8__locals1.row.DefIcon(config.animal, null);
			CS$<>8__locals1.row.Gap(4f);
			GUI.color = ((animalCountRecord.total == 0 && animalCountRecord.unslaughterable == 0) ? Color.gray : color);
			CS$<>8__locals1.row.Label(config.animal.LabelCap, width, null, -1f);
			GUI.color = color;
			Dialog_AutoSlaughter.<DoAnimalRow>g__DrawCurrentCol|23_0(animalCountRecord.total, new int?(config.maxTotal), ref CS$<>8__locals1);
			this.DoMaxColumn(CS$<>8__locals1.row, ref config.maxTotal, ref config.uiMaxTotalBuffer, animalCountRecord.total);
			Dialog_AutoSlaughter.<DoAnimalRow>g__DrawCurrentCol|23_0(animalCountRecord.male, new int?(config.maxMales), ref CS$<>8__locals1);
			this.DoMaxColumn(CS$<>8__locals1.row, ref config.maxMales, ref config.uiMaxMalesBuffer, animalCountRecord.male);
			Dialog_AutoSlaughter.<DoAnimalRow>g__DrawCurrentCol|23_0(animalCountRecord.female, new int?(config.maxFemales), ref CS$<>8__locals1);
			this.DoMaxColumn(CS$<>8__locals1.row, ref config.maxFemales, ref config.uiMaxFemalesBuffer, animalCountRecord.female);
			Dialog_AutoSlaughter.<DoAnimalRow>g__DrawCurrentCol|23_0(animalCountRecord.unslaughterable, null, ref CS$<>8__locals1);
			GUI.EndGroup();
		}

		// Token: 0x0600735D RID: 29533 RVA: 0x002696A8 File Offset: 0x002678A8
		[CompilerGenerated]
		internal static void <DoAnimalHeader>g__DrawEquidistantLabel|22_0(string text, string cell1Text, string cell2Text, float cell1StartX, float cell2StartX, float finalX)
		{
			Vector2 vector = Text.CalcSize(text);
			float x = Text.CalcSize(cell1Text).x;
			float x2 = Text.CalcSize(cell2Text).x;
			float num = cell2StartX - cell1StartX - 4f;
			float num2 = finalX - cell2StartX - 4f;
			float num3 = cell1StartX + num / 2f + 2f + x / 2f;
			float num4 = cell2StartX + num2 / 2f + 2f - x2 / 2f;
			float num5 = (num3 + num4) / 2f;
			Widgets.Label(new Rect(num5 - vector.x / 2f, 0f, vector.x, 24f), text);
		}

		// Token: 0x0600735E RID: 29534 RVA: 0x00269754 File Offset: 0x00267954
		[CompilerGenerated]
		internal static void <DoAnimalHeader>g__DrawSingleLabel|22_1(string text, float startX, float finalX)
		{
			Vector2 vector = Text.CalcSize(text);
			float num = finalX - startX;
			Widgets.Label(new Rect(startX + (num - vector.x) / 2f, 0f, vector.x, 24f), text);
		}

		// Token: 0x0600735F RID: 29535 RVA: 0x00269798 File Offset: 0x00267998
		[CompilerGenerated]
		internal static void <DoAnimalRow>g__DrawCurrentCol|23_0(int val, int? limit, ref Dialog_AutoSlaughter.<>c__DisplayClass23_0 A_2)
		{
			Color? color = null;
			if (val == 0)
			{
				color = new Color?(Color.gray);
			}
			else if (limit != null)
			{
				int? num = limit;
				int num2 = -1;
				if (!(num.GetValueOrDefault() == num2 & num != null))
				{
					int num3 = val;
					num = limit;
					if (num3 > num.GetValueOrDefault() & num != null)
					{
						color = new Color?(ColorLibrary.RedReadable);
					}
				}
			}
			Color color2 = GUI.color;
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleCenter;
			GUI.color = (color ?? Color.white);
			A_2.row.Label(val.ToString(), 60f, null, -1f);
			Text.Anchor = anchor;
			GUI.color = color2;
		}

		// Token: 0x04003EEC RID: 16108
		private Map map;

		// Token: 0x04003EED RID: 16109
		private Vector2 scrollPos;

		// Token: 0x04003EEE RID: 16110
		private Rect viewRect;

		// Token: 0x04003EEF RID: 16111
		private Dictionary<ThingDef, Dialog_AutoSlaughter.AnimalCountRecord> animalCounts = new Dictionary<ThingDef, Dialog_AutoSlaughter.AnimalCountRecord>();

		// Token: 0x04003EF0 RID: 16112
		private List<AutoSlaughterConfig> configsOrdered = new List<AutoSlaughterConfig>();

		// Token: 0x04003EF1 RID: 16113
		private List<Rect> tmpMouseoverHighlightRects = new List<Rect>();

		// Token: 0x04003EF2 RID: 16114
		private List<Rect> tmpGroupRects = new List<Rect>();

		// Token: 0x04003EF3 RID: 16115
		private const float ColumnWidthCurrent = 60f;

		// Token: 0x04003EF4 RID: 16116
		private const float ColumnWidthMaxNoLabelSpacing = 180f;

		// Token: 0x04003EF5 RID: 16117
		private const float ColumnWidthMax = 184f;

		// Token: 0x04003EF6 RID: 16118
		private const float SizeControlButton = 12f;

		// Token: 0x04003EF7 RID: 16119
		private const float SizeControlInfinityButton = 48f;

		// Token: 0x04003EF8 RID: 16120
		private const float SizeControlTextArea = 40f;

		// Token: 0x02002627 RID: 9767
		private struct AnimalCountRecord
		{
			// Token: 0x0600D564 RID: 54628 RVA: 0x00406C93 File Offset: 0x00404E93
			public AnimalCountRecord(int total, int male, int female, int unslaughterable)
			{
				this.total = total;
				this.male = male;
				this.female = female;
				this.unslaughterable = unslaughterable;
			}

			// Token: 0x04009166 RID: 37222
			public int total;

			// Token: 0x04009167 RID: 37223
			public int male;

			// Token: 0x04009168 RID: 37224
			public int female;

			// Token: 0x04009169 RID: 37225
			public int unslaughterable;
		}
	}
}
