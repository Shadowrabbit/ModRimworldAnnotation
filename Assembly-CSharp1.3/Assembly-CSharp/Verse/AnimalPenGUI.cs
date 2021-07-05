using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000176 RID: 374
	public static class AnimalPenGUI
	{
		// Token: 0x06000A86 RID: 2694 RVA: 0x00039970 File Offset: 0x00037B70
		public static void DoAllowedAreaMessage(Rect rect, Pawn pawn)
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Tiny;
			CompAnimalPenMarker currentPenOf = AnimalPenUtility.GetCurrentPenOf(pawn, false);
			TaggedString taggedString;
			TaggedString str;
			if (currentPenOf != null)
			{
				taggedString = "InPen".Translate() + ": " + currentPenOf.label;
				str = taggedString;
			}
			else
			{
				GUI.color = Color.gray;
				taggedString = "(" + "Unpenned".Translate() + ")";
				str = "UnpennedTooltip".Translate();
			}
			Widgets.Label(rect, taggedString);
			TooltipHandler.TipRegion(rect, str);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x00039A18 File Offset: 0x00037C18
		public static void DrawPlacingMouseAttachments(IntVec3 mouseCell, Map map, PenFoodCalculator calc)
		{
			AnimalPenGUI.<>c__DisplayClass3_0 CS$<>8__locals1;
			CS$<>8__locals1.calc = calc;
			CS$<>8__locals1.sb = new StringBuilder();
			Vector2 location = Find.WorldGrid.LongLatOf(map.Tile);
			CS$<>8__locals1.summerQuadrum = CS$<>8__locals1.calc.GetSummerOrBestQuadrum();
			CS$<>8__locals1.summerLabel = CS$<>8__locals1.summerQuadrum.GetSeason(location);
			CS$<>8__locals1.sb.AppendLine(CS$<>8__locals1.calc.PenSizeDescription());
			CS$<>8__locals1.sb.AppendLine();
			AnimalPenGUI.<DrawPlacingMouseAttachments>g__AppendCapacityOf|3_0(ThingDefOf.Cow, ref CS$<>8__locals1);
			AnimalPenGUI.<DrawPlacingMouseAttachments>g__AppendCapacityOf|3_0(ThingDefOf.Goat, ref CS$<>8__locals1);
			AnimalPenGUI.<DrawPlacingMouseAttachments>g__AppendCapacityOf|3_0(ThingDefOf.Chicken, ref CS$<>8__locals1);
			Widgets.MouseAttachedLabel(CS$<>8__locals1.sb.ToString().TrimEnd(Array.Empty<char>()), 8f, 35f);
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x00039ADC File Offset: 0x00037CDC
		[CompilerGenerated]
		internal static void <DrawPlacingMouseAttachments>g__AppendCapacityOf|3_0(ThingDef animalDef, ref AnimalPenGUI.<>c__DisplayClass3_0 A_1)
		{
			A_1.sb.Append("PenCapacityDesc".Translate(animalDef.Named("ANIMALDEF")).CapitalizeFirst());
			A_1.sb.Append(" (").Append(A_1.summerLabel.Label()).Append("): ");
			if (A_1.calc.Unenclosed)
			{
				A_1.sb.Append("?");
			}
			else
			{
				A_1.sb.Append(A_1.calc.CapacityOf(A_1.summerQuadrum, animalDef).ToString("F1"));
			}
			A_1.sb.AppendLine();
		}

		// Token: 0x02001942 RID: 6466
		public class PenPainter : AnimalPenEnclosureCalculator
		{
			// Token: 0x060097C6 RID: 38854 RVA: 0x0035D720 File Offset: 0x0035B920
			protected override void VisitDirectlyConnectedRegion(Region r)
			{
				this.directEdgeCells.AddRange(r.Cells);
			}

			// Token: 0x060097C7 RID: 38855 RVA: 0x0035D733 File Offset: 0x0035B933
			protected override void VisitIndirectlyDirectlyConnectedRegion(Region r)
			{
				this.indirectEdgeCells.AddRange(r.Cells);
			}

			// Token: 0x060097C8 RID: 38856 RVA: 0x0035D746 File Offset: 0x0035B946
			protected override void VisitPassableDoorway(Region r)
			{
				this.openDoorEdgeCells.AddRange(r.Cells);
			}

			// Token: 0x060097C9 RID: 38857 RVA: 0x0035D75C File Offset: 0x0035B95C
			public void Paint(IntVec3 position, Map map)
			{
				this.directEdgeCells.Clear();
				this.indirectEdgeCells.Clear();
				this.openDoorEdgeCells.Clear();
				if (base.VisitPen(position, map))
				{
					GenDraw.DrawFieldEdges(this.directEdgeCells, Color.green, null);
					GenDraw.DrawFieldEdges(this.indirectEdgeCells, Color.white, null);
					GenDraw.DrawFieldEdges(this.openDoorEdgeCells, Color.white, null);
					return;
				}
				GenDraw.DrawFieldEdges(this.openDoorEdgeCells, Color.red, null);
			}

			// Token: 0x040060FD RID: 24829
			private readonly List<IntVec3> directEdgeCells = new List<IntVec3>();

			// Token: 0x040060FE RID: 24830
			private readonly List<IntVec3> indirectEdgeCells = new List<IntVec3>();

			// Token: 0x040060FF RID: 24831
			private readonly List<IntVec3> openDoorEdgeCells = new List<IntVec3>();
		}

		// Token: 0x02001943 RID: 6467
		public class PenBlueprintPainter
		{
			// Token: 0x060097CB RID: 38859 RVA: 0x0035D824 File Offset: 0x0035BA24
			public void Paint(IntVec3 position, Map map)
			{
				this.filler.VisitPen(position, map);
				if (this.filler.isEnclosed)
				{
					GenDraw.DrawFieldEdges(this.filler.cellsFound, Color.white, null);
				}
			}

			// Token: 0x04006100 RID: 24832
			private AnimalPenBlueprintEnclosureCalculator filler = new AnimalPenBlueprintEnclosureCalculator();
		}
	}
}
