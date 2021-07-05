using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001396 RID: 5014
	public class PawnColumnWorker_Label : PawnColumnWorker
	{
		// Token: 0x060079F3 RID: 31219 RVA: 0x002B0CE0 File Offset: 0x002AEEE0
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Rect rect2 = new Rect(rect.x, rect.y, rect.width, Mathf.Min(rect.height, 30f));
			if (pawn.health.summaryHealth.SummaryHealthPercent < 0.99f)
			{
				Rect rect3 = new Rect(rect2);
				rect3.xMin -= 4f;
				rect3.yMin += 4f;
				rect3.yMax -= 6f;
				Widgets.FillableBar(rect3, pawn.health.summaryHealth.SummaryHealthPercent, GenMapUI.OverlayHealthTex, BaseContent.ClearTex, false);
			}
			if (Mouse.IsOver(rect2))
			{
				GUI.DrawTexture(rect2, TexUI.HighlightTex);
			}
			string text;
			if (!pawn.RaceProps.Humanlike && pawn.Name != null && !pawn.Name.Numerical)
			{
				text = pawn.Name.ToStringShort.CapitalizeFirst() + ", " + pawn.KindLabel;
			}
			else
			{
				text = pawn.LabelShortCap;
			}
			Rect rect4 = rect2;
			rect4.xMin += 3f;
			if (rect4.width != PawnColumnWorker_Label.labelCacheForWidth)
			{
				PawnColumnWorker_Label.labelCacheForWidth = rect4.width;
				PawnColumnWorker_Label.labelCache.Clear();
			}
			text = text.Truncate(rect4.width, PawnColumnWorker_Label.labelCache);
			if (pawn.IsSlave)
			{
				text = text.Colorize(PawnNameColorUtility.PawnNameColorOf(pawn));
			}
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Widgets.Label(rect4, text);
			Text.WordWrap = true;
			Text.Anchor = TextAnchor.UpperLeft;
			if (Widgets.ButtonInvisible(rect2, true))
			{
				CameraJumper.TryJumpAndSelect(pawn);
				if (Current.ProgramState == ProgramState.Playing && Event.current.button == 0)
				{
					Find.MainTabsRoot.EscapeCurrentTab(false);
				}
				return;
			}
			if (Mouse.IsOver(rect2))
			{
				TipSignal tooltip = pawn.GetTooltip();
				tooltip.text = "ClickToJumpTo".Translate() + "\n\n" + tooltip.text;
				TooltipHandler.TipRegion(rect2, tooltip);
			}
		}

		// Token: 0x060079F4 RID: 31220 RVA: 0x002B0EF1 File Offset: 0x002AF0F1
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 80);
		}

		// Token: 0x060079F5 RID: 31221 RVA: 0x002B0F01 File Offset: 0x002AF101
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(165, this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x04004390 RID: 17296
		private const int LeftMargin = 3;

		// Token: 0x04004391 RID: 17297
		private static Dictionary<string, string> labelCache = new Dictionary<string, string>();

		// Token: 0x04004392 RID: 17298
		private static float labelCacheForWidth = -1f;
	}
}
