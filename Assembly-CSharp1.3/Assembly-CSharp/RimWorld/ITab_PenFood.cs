using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001352 RID: 4946
	public class ITab_PenFood : ITab_PenBase
	{
		// Token: 0x060077B8 RID: 30648 RVA: 0x002A2982 File Offset: 0x002A0B82
		public ITab_PenFood()
		{
			this.size = ITab_PenFood.WinSize;
			this.labelKey = "TabPenFood";
		}

		// Token: 0x060077B9 RID: 30649 RVA: 0x002A29AB File Offset: 0x002A0BAB
		public override void OnOpen()
		{
			base.OnOpen();
			this.animalPaneScrollPos = Vector2.zero;
		}

		// Token: 0x060077BA RID: 30650 RVA: 0x002A29C0 File Offset: 0x002A0BC0
		protected override void FillTab()
		{
			CompAnimalPenMarker selectedCompAnimalPenMarker = base.SelectedCompAnimalPenMarker;
			Rect rect = new Rect(0f, 0f, ITab_PenFood.WinSize.x, ITab_PenFood.WinSize.y).ContractedBy(10f);
			if (selectedCompAnimalPenMarker.PenState.Unenclosed)
			{
				Widgets.NoneLabelCenteredVertically(rect, "(" + "PenFoodTab_NotEnclosed".Translate() + ")");
				return;
			}
			PenFoodCalculator penFoodCalculator = selectedCompAnimalPenMarker.PenFoodCalculator;
			GUI.BeginGroup(rect);
			float num = 0f;
			this.DrawTopPane(ref num, rect.width, penFoodCalculator);
			float height = rect.height - num;
			this.DrawAnimalPane(ref num, rect.width, height, penFoodCalculator, selectedCompAnimalPenMarker.parent.Map);
			GUI.EndGroup();
		}

		// Token: 0x060077BB RID: 30651 RVA: 0x002A2A8C File Offset: 0x002A0C8C
		private void DrawTopPane(ref float curY, float width, PenFoodCalculator calc)
		{
			float num = calc.SumNutritionConsumptionPerDay - calc.NutritionPerDayToday;
			bool flag = num > 0f;
			this.DrawStatLine("PenSizeLabel".Translate(), calc.PenSizeDescription(), ref curY, width, null, null);
			this.DrawStatLine("PenFoodTab_NaturalNutritionGrowthRate".Translate(), PenFoodCalculator.NutritionPerDayToString(calc.NutritionPerDayToday, true), ref curY, width, new Func<string>(calc.NaturalGrowthRateTooltip), flag ? new Color?(Color.red) : null);
			this.DrawStatLine("PenFoodTab_TotalNutritionConsumptionRate".Translate(), PenFoodCalculator.NutritionPerDayToString(calc.SumNutritionConsumptionPerDay, true), ref curY, width, new Func<string>(calc.TotalConsumedToolTop), flag ? new Color?(Color.red) : null);
			if (calc.sumStockpiledNutritionAvailableNow > 0f)
			{
				this.DrawStatLine("PenFoodTab_StockpileTotal".Translate(), PenFoodCalculator.NutritionToString(calc.sumStockpiledNutritionAvailableNow, false), ref curY, width, new Func<string>(calc.StockpileToolTip), null);
				if (flag)
				{
					int num2 = Mathf.FloorToInt(calc.sumStockpiledNutritionAvailableNow / num);
					this.DrawStatLine("PenFoodTab_StockpileEmptyDays".Translate(), num2.ToString(), ref curY, width, () => "PenFoodTab_StockpileEmptyDaysDescription".Translate(), new Color?(Color.red));
				}
			}
		}

		// Token: 0x060077BC RID: 30652 RVA: 0x002A2C08 File Offset: 0x002A0E08
		private void DrawStatLine(string label, string value, ref float curY, float width, Func<string> toolipGetter = null, Color? valueColor = null)
		{
			float lineHeight = Text.LineHeight;
			Rect rect = new Rect(8f, curY, width, lineHeight);
			Rect rect2;
			Rect rect3;
			rect.SplitVertically(210f, out rect2, out rect3, 0f);
			Widgets.Label(rect2, label);
			GUI.color = (valueColor ?? Color.white);
			Widgets.Label(rect3, value);
			GUI.color = Color.white;
			if (Mouse.IsOver(rect) && toolipGetter != null)
			{
				Widgets.DrawHighlight(rect);
				TooltipHandler.TipRegion(rect, toolipGetter, Gen.HashCombineInt(10192384, label.GetHashCode()));
			}
			curY += lineHeight;
		}

		// Token: 0x060077BD RID: 30653 RVA: 0x002A2CA8 File Offset: 0x002A0EA8
		private void DrawAnimalPane(ref float curYOuter, float width, float height, PenFoodCalculator calc, Map map)
		{
			ITab_PenFood.<>c__DisplayClass11_0 CS$<>8__locals1 = new ITab_PenFood.<>c__DisplayClass11_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.calc = calc;
			CS$<>8__locals1.map = map;
			float cellWidth = width - 328f;
			CS$<>8__locals1.curY = curYOuter;
			float curY = CS$<>8__locals1.curY;
			float num = Mathf.Max(Text.LineHeight, 27f);
			float num2 = Text.LineHeightOf(GameFont.Small) + 10f;
			float num3 = num;
			Rect rect = new Rect(0f, CS$<>8__locals1.curY, width, height - (CS$<>8__locals1.curY - curY) - num3);
			Rect rect2;
			Rect outRect;
			rect.SplitHorizontally(num2, out rect2, out outRect, 0f);
			float x = rect2.x;
			CS$<>8__locals1.curY = rect2.y;
			CS$<>8__locals1.<DrawAnimalPane>g__DrawIconCell|1(null, ref x, 53f, num2);
			CS$<>8__locals1.<DrawAnimalPane>g__DrawCell|0("PenFoodTab_AnimalType".Translate(), ref x, cellWidth, num2, TextAnchor.LowerLeft, null, null);
			CS$<>8__locals1.<DrawAnimalPane>g__DrawCell|0("PenFoodTab_Count".Translate(), ref x, 100f, num2, TextAnchor.LowerCenter, null, null);
			CS$<>8__locals1.<DrawAnimalPane>g__DrawCell|0("PenFoodTab_NutritionConsumedPerDay_ColumLabel".Translate(), ref x, 120f, num2, TextAnchor.LowerCenter, null, () => "PenFoodTab_NutritionConsumedPerDay_ColumnTooltip".Translate());
			GUI.color = Widgets.SeparatorLineColor;
			Widgets.DrawLineHorizontal(0f, rect2.yMax - 1f, width);
			GUI.color = Color.white;
			this.tmpAnimalInfos.Clear();
			this.tmpAnimalInfos.AddRange(CS$<>8__locals1.calc.ActualAnimalInfos);
			this.tmpAnimalInfos.AddRange(CS$<>8__locals1.calc.ComputeExampleAnimals(base.SelectedCompAnimalPenMarker.ForceDisplayedAnimalDefs));
			Rect viewRect = new Rect(outRect.x, outRect.y, outRect.width - 16f, (float)this.tmpAnimalInfos.Count * num);
			Widgets.BeginScrollView(outRect, ref this.animalPaneScrollPos, viewRect, true);
			CS$<>8__locals1.curY = viewRect.y;
			int num4 = 0;
			using (List<PenFoodCalculator.PenAnimalInfo>.Enumerator enumerator = this.tmpAnimalInfos.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PenFoodCalculator.PenAnimalInfo info = enumerator.Current;
					float x2 = viewRect.x;
					Rect rect3 = new Rect(x2, CS$<>8__locals1.curY, viewRect.width, num);
					if (num4 % 2 == 1)
					{
						Widgets.DrawLightHighlight(rect3);
					}
					CS$<>8__locals1.<DrawAnimalPane>g__DrawIconCell|1(info.animalDef, ref x2, 53f, num);
					CS$<>8__locals1.<DrawAnimalPane>g__DrawCell|0(info.animalDef.LabelCap, ref x2, cellWidth, num, TextAnchor.MiddleLeft, null, null);
					if (!info.example)
					{
						CS$<>8__locals1.<DrawAnimalPane>g__DrawCell|0(info.TotalCount.ToString(), ref x2, 100f, num, TextAnchor.MiddleCenter, null, null);
						CS$<>8__locals1.<DrawAnimalPane>g__DrawCell|0(PenFoodCalculator.NutritionPerDayToString(info.TotalNutritionConsumptionPerDay, false), ref x2, 120f, num, TextAnchor.MiddleCenter, null, null);
					}
					else
					{
						float num5 = SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(info.animalDef);
						int num6 = Mathf.FloorToInt(CS$<>8__locals1.calc.NutritionPerDayToday / num5);
						CS$<>8__locals1.<DrawAnimalPane>g__DrawCell|0("max".Translate() + " " + num6.ToString(), ref x2, 100f, num, TextAnchor.MiddleCenter, new Color?(Color.grey), null);
						CS$<>8__locals1.<DrawAnimalPane>g__DrawCell|0("PenFoodTab_NutritionConsumedEachAnimalLabel".Translate(PenFoodCalculator.NutritionPerDayToString(num5, false).Named("CONSUMEDAMOUNT")), ref x2, 120f, num, TextAnchor.MiddleCenter, new Color?(Color.grey), null);
						CS$<>8__locals1.<DrawAnimalPane>g__DrawExampleAnimalControls|2(info, ref x2, 27f, num);
					}
					if (Mouse.IsOver(rect3))
					{
						Widgets.DrawHighlight(rect3);
						TooltipHandler.TipRegion(rect3, () => info.ToolTip(CS$<>8__locals1.calc), 9477435);
					}
					CS$<>8__locals1.curY += rect3.height;
					num4++;
				}
			}
			Widgets.EndScrollView();
			Rect rect4 = new Rect(rect.x, Mathf.Min(rect.yMax, CS$<>8__locals1.curY), rect.width, num3);
			Widgets.Dropdown<PenFoodCalculator, ThingDef>(rect4.LeftPart(0.35f), CS$<>8__locals1.calc, (PenFoodCalculator calculator) => null, new Func<PenFoodCalculator, IEnumerable<Widgets.DropdownMenuElement<ThingDef>>>(CS$<>8__locals1.<DrawAnimalPane>g__MenuGenerator|5), "PenFoodTab_AddAnimal".Translate(), null, null, null, null, false);
			CS$<>8__locals1.curY = rect4.yMax;
			curYOuter = CS$<>8__locals1.curY;
		}

		// Token: 0x060077BE RID: 30654 RVA: 0x002A31D8 File Offset: 0x002A13D8
		private void RemoveAnimal(PenFoodCalculator calc, PenFoodCalculator.PenAnimalInfo info)
		{
			base.SelectedCompAnimalPenMarker.RemoveForceDisplayedAnimal(info.animalDef);
		}

		// Token: 0x060077BF RID: 30655 RVA: 0x002A31EB File Offset: 0x002A13EB
		private void AddExampleAnimal(PenFoodCalculator calc, ThingDef animal)
		{
			base.SelectedCompAnimalPenMarker.AddForceDisplayedAnimal(animal);
		}

		// Token: 0x04004297 RID: 17047
		private static readonly Vector2 WinSize = new Vector2(500f, 500f);

		// Token: 0x04004298 RID: 17048
		private const int StatLineIndent = 8;

		// Token: 0x04004299 RID: 17049
		private const int StatLabelColumnWidth = 210;

		// Token: 0x0400429A RID: 17050
		private const float AboveTableMargin = 10f;

		// Token: 0x0400429B RID: 17051
		private Vector2 animalPaneScrollPos;

		// Token: 0x0400429C RID: 17052
		private readonly List<PenFoodCalculator.PenAnimalInfo> tmpAnimalInfos = new List<PenFoodCalculator.PenAnimalInfo>();
	}
}
