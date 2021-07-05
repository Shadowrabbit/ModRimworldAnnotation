using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000413 RID: 1043
	public class Listing_Standard : Listing
	{
		// Token: 0x06001F35 RID: 7989 RVA: 0x000C2951 File Offset: 0x000C0B51
		public Listing_Standard(GameFont font)
		{
			this.font = font;
		}

		// Token: 0x06001F36 RID: 7990 RVA: 0x000C2960 File Offset: 0x000C0B60
		public Listing_Standard()
		{
			this.font = GameFont.Small;
		}

		// Token: 0x06001F37 RID: 7991 RVA: 0x000C296F File Offset: 0x000C0B6F
		public override void Begin(Rect rect)
		{
			base.Begin(rect);
			Text.Font = this.font;
		}

		// Token: 0x06001F38 RID: 7992 RVA: 0x000C2984 File Offset: 0x000C0B84
		public override void End()
		{
			base.End();
			if (this.labelScrollbarPositions != null)
			{
				for (int i = this.labelScrollbarPositions.Count - 1; i >= 0; i--)
				{
					if (!this.labelScrollbarPositionsSetThisFrame.Contains(this.labelScrollbarPositions[i].First))
					{
						this.labelScrollbarPositions.RemoveAt(i);
					}
				}
				this.labelScrollbarPositionsSetThisFrame.Clear();
			}
		}

		// Token: 0x06001F39 RID: 7993 RVA: 0x000C29EF File Offset: 0x000C0BEF
		public Rect Label(TaggedString label, float maxHeight = -1f, string tooltip = null)
		{
			return this.Label(label.Resolve(), maxHeight, tooltip);
		}

		// Token: 0x06001F3A RID: 7994 RVA: 0x000C2A00 File Offset: 0x000C0C00
		public Rect Label(string label, float maxHeight = -1f, string tooltip = null)
		{
			float num = Text.CalcHeight(label, base.ColumnWidth);
			bool flag = false;
			if (maxHeight >= 0f && num > maxHeight)
			{
				num = maxHeight;
				flag = true;
			}
			Rect rect = base.GetRect(num);
			if (flag)
			{
				Vector2 labelScrollbarPosition = this.GetLabelScrollbarPosition(this.curX, this.curY);
				Widgets.LabelScrollable(rect, label, ref labelScrollbarPosition, false, true, false);
				this.SetLabelScrollbarPosition(this.curX, this.curY, labelScrollbarPosition);
			}
			else
			{
				Widgets.Label(rect, label);
			}
			if (tooltip != null)
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			base.Gap(this.verticalSpacing);
			return rect;
		}

		// Token: 0x06001F3B RID: 7995 RVA: 0x000C2A90 File Offset: 0x000C0C90
		public void LabelDouble(string leftLabel, string rightLabel, string tip = null)
		{
			float num = base.ColumnWidth / 2f;
			float width = base.ColumnWidth - num;
			float a = Text.CalcHeight(leftLabel, num);
			float b = Text.CalcHeight(rightLabel, width);
			float height = Mathf.Max(a, b);
			Rect rect = base.GetRect(height);
			if (!tip.NullOrEmpty())
			{
				Widgets.DrawHighlightIfMouseover(rect);
				TooltipHandler.TipRegion(rect, tip);
			}
			Widgets.Label(rect.LeftHalf(), leftLabel);
			Widgets.Label(rect.RightHalf(), rightLabel);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06001F3C RID: 7996 RVA: 0x000C2B14 File Offset: 0x000C0D14
		public bool RadioButton(string label, bool active, float tabIn = 0f, string tooltip = null, float? tooltipDelay = null)
		{
			float lineHeight = Text.LineHeight;
			Rect rect = base.GetRect(lineHeight);
			rect.xMin += tabIn;
			if (!tooltip.NullOrEmpty())
			{
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
				}
				TipSignal tip = (tooltipDelay != null) ? new TipSignal(tooltip, tooltipDelay.Value) : new TipSignal(tooltip);
				TooltipHandler.TipRegion(rect, tip);
			}
			bool result = Widgets.RadioButtonLabeled(rect, label, active);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06001F3D RID: 7997 RVA: 0x000C2B94 File Offset: 0x000C0D94
		public void CheckboxLabeled(string label, ref bool checkOn, string tooltip = null)
		{
			float lineHeight = Text.LineHeight;
			Rect rect = base.GetRect(lineHeight);
			if (!tooltip.NullOrEmpty())
			{
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
				}
				TooltipHandler.TipRegion(rect, tooltip);
			}
			Widgets.CheckboxLabeled(rect, label, ref checkOn, false, null, null, false);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06001F3E RID: 7998 RVA: 0x000C2BEC File Offset: 0x000C0DEC
		public bool CheckboxLabeledSelectable(string label, ref bool selected, ref bool checkOn)
		{
			float lineHeight = Text.LineHeight;
			bool result = Widgets.CheckboxLabeledSelectable(base.GetRect(lineHeight), label, ref selected, ref checkOn, null, 1f);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06001F3F RID: 7999 RVA: 0x000C2C20 File Offset: 0x000C0E20
		public bool ButtonText(string label, string highlightTag = null)
		{
			Rect rect = base.GetRect(30f);
			bool result = Widgets.ButtonText(rect, label, true, true, true);
			if (highlightTag != null)
			{
				UIHighlighter.HighlightOpportunity(rect, highlightTag);
			}
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06001F40 RID: 8000 RVA: 0x000C2C59 File Offset: 0x000C0E59
		public bool ButtonTextLabeled(string label, string buttonLabel)
		{
			Rect rect = base.GetRect(30f);
			Widgets.Label(rect.LeftHalf(), label);
			bool result = Widgets.ButtonText(rect.RightHalf(), buttonLabel, true, true, true);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06001F41 RID: 8001 RVA: 0x000C2C8C File Offset: 0x000C0E8C
		public bool ButtonImage(Texture2D tex, float width, float height)
		{
			base.NewColumnIfNeeded(height);
			bool result = Widgets.ButtonImage(new Rect(this.curX, this.curY, width, height), tex, true);
			base.Gap(height + this.verticalSpacing);
			return result;
		}

		// Token: 0x06001F42 RID: 8002 RVA: 0x000C2CBD File Offset: 0x000C0EBD
		public void None()
		{
			GUI.color = Color.gray;
			Text.Anchor = TextAnchor.UpperCenter;
			this.Label("NoneBrackets".Translate(), -1f, null);
			GenUI.ResetLabelAlign();
			GUI.color = Color.white;
		}

		// Token: 0x06001F43 RID: 8003 RVA: 0x000C2CF8 File Offset: 0x000C0EF8
		public string TextEntry(string text, int lineCount = 1)
		{
			Rect rect = base.GetRect(Text.LineHeight * (float)lineCount);
			string result;
			if (lineCount == 1)
			{
				result = Widgets.TextField(rect, text);
			}
			else
			{
				result = Widgets.TextArea(rect, text, false);
			}
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06001F44 RID: 8004 RVA: 0x000C2D38 File Offset: 0x000C0F38
		public string TextEntryLabeled(string label, string text, int lineCount = 1)
		{
			string result = Widgets.TextEntryLabeled(base.GetRect(Text.LineHeight * (float)lineCount), label, text);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06001F45 RID: 8005 RVA: 0x000C2D5B File Offset: 0x000C0F5B
		public void TextFieldNumeric<T>(ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			Widgets.TextFieldNumeric<T>(base.GetRect(Text.LineHeight), ref val, ref buffer, min, max);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06001F46 RID: 8006 RVA: 0x000C2D7E File Offset: 0x000C0F7E
		public void TextFieldNumericLabeled<T>(string label, ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			Widgets.TextFieldNumericLabeled<T>(base.GetRect(Text.LineHeight), label, ref val, ref buffer, min, max);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06001F47 RID: 8007 RVA: 0x000C2DA3 File Offset: 0x000C0FA3
		public void IntRange(ref IntRange range, int min, int max)
		{
			Widgets.IntRange(base.GetRect(28f), (int)base.CurHeight, ref range, min, max, null, 0);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06001F48 RID: 8008 RVA: 0x000C2DD0 File Offset: 0x000C0FD0
		public float Slider(float val, float min, float max)
		{
			float num = Widgets.HorizontalSlider(base.GetRect(22f), val, min, max, false, null, null, null, -1f);
			if (num != val)
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			}
			base.Gap(this.verticalSpacing);
			return num;
		}

		// Token: 0x06001F49 RID: 8009 RVA: 0x000C2E14 File Offset: 0x000C1014
		public void IntAdjuster(ref int val, int countChange, int min = 0)
		{
			Rect rect = base.GetRect(24f);
			rect.width = 42f;
			if (Widgets.ButtonText(rect, "-" + countChange, true, true, true))
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				val -= countChange * GenUI.CurrentAdjustmentMultiplier();
				if (val < min)
				{
					val = min;
				}
			}
			rect.x += rect.width + 2f;
			if (Widgets.ButtonText(rect, "+" + countChange, true, true, true))
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				val += countChange * GenUI.CurrentAdjustmentMultiplier();
				if (val < min)
				{
					val = min;
				}
			}
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06001F4A RID: 8010 RVA: 0x000C2ED4 File Offset: 0x000C10D4
		public void IntSetter(ref int val, int target, string label, float width = 42f)
		{
			if (Widgets.ButtonText(base.GetRect(24f), label, true, true, true))
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				val = target;
			}
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06001F4B RID: 8011 RVA: 0x000C2F06 File Offset: 0x000C1106
		public void IntEntry(ref int val, ref string editBuffer, int multiplier = 1)
		{
			Widgets.IntEntry(base.GetRect(24f), ref val, ref editBuffer, multiplier);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06001F4C RID: 8012 RVA: 0x000C2F28 File Offset: 0x000C1128
		public Listing_Standard BeginSection(float height, float sectionBorder = 4f, float bottomBorder = 4f)
		{
			Rect rect = base.GetRect(height + sectionBorder + bottomBorder);
			Widgets.DrawMenuSection(rect);
			Listing_Standard listing_Standard = new Listing_Standard();
			Rect rect2 = new Rect(rect.x + sectionBorder, rect.y + sectionBorder, rect.width - sectionBorder * 2f, rect.height - (sectionBorder + bottomBorder));
			listing_Standard.Begin(rect2);
			return listing_Standard;
		}

		// Token: 0x06001F4D RID: 8013 RVA: 0x000C2F86 File Offset: 0x000C1186
		public void EndSection(Listing_Standard listing)
		{
			listing.End();
		}

		// Token: 0x06001F4E RID: 8014 RVA: 0x000C2F90 File Offset: 0x000C1190
		private Vector2 GetLabelScrollbarPosition(float x, float y)
		{
			if (this.labelScrollbarPositions == null)
			{
				return Vector2.zero;
			}
			for (int i = 0; i < this.labelScrollbarPositions.Count; i++)
			{
				Vector2 first = this.labelScrollbarPositions[i].First;
				if (first.x == x && first.y == y)
				{
					return this.labelScrollbarPositions[i].Second;
				}
			}
			return Vector2.zero;
		}

		// Token: 0x06001F4F RID: 8015 RVA: 0x000C3004 File Offset: 0x000C1204
		private void SetLabelScrollbarPosition(float x, float y, Vector2 scrollbarPosition)
		{
			if (this.labelScrollbarPositions == null)
			{
				this.labelScrollbarPositions = new List<Pair<Vector2, Vector2>>();
				this.labelScrollbarPositionsSetThisFrame = new List<Vector2>();
			}
			this.labelScrollbarPositionsSetThisFrame.Add(new Vector2(x, y));
			for (int i = 0; i < this.labelScrollbarPositions.Count; i++)
			{
				Vector2 first = this.labelScrollbarPositions[i].First;
				if (first.x == x && first.y == y)
				{
					this.labelScrollbarPositions[i] = new Pair<Vector2, Vector2>(new Vector2(x, y), scrollbarPosition);
					return;
				}
			}
			this.labelScrollbarPositions.Add(new Pair<Vector2, Vector2>(new Vector2(x, y), scrollbarPosition));
		}

		// Token: 0x06001F50 RID: 8016 RVA: 0x000C30B0 File Offset: 0x000C12B0
		public bool SelectableDef(string name, bool selected, Action deleteCallback)
		{
			Text.Font = GameFont.Tiny;
			float width = this.listingRect.width - 21f;
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect = new Rect(this.curX, this.curY, width, 21f);
			if (selected)
			{
				Widgets.DrawHighlight(rect);
			}
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawBox(rect, 1, null);
			}
			Text.WordWrap = false;
			Widgets.Label(rect, name);
			Text.WordWrap = true;
			if (deleteCallback != null && Widgets.ButtonImage(new Rect(rect.xMax, rect.y, 21f, 21f), TexButton.DeleteX, Color.white, GenUI.SubtleMouseoverColor, true))
			{
				deleteCallback();
			}
			Text.Anchor = TextAnchor.UpperLeft;
			this.curY += 21f;
			return Widgets.ButtonInvisible(rect, true);
		}

		// Token: 0x06001F51 RID: 8017 RVA: 0x000C317C File Offset: 0x000C137C
		public void LabelCheckboxDebug(string label, ref bool checkOn, bool highlight)
		{
			Text.Font = GameFont.Tiny;
			base.NewColumnIfNeeded(22f);
			Rect rect = new Rect(this.curX, this.curY, base.ColumnWidth, 22f);
			Widgets.CheckboxLabeled(rect, label, ref checkOn, false, null, null, false);
			if (highlight)
			{
				GUI.color = Color.yellow;
				Widgets.DrawBox(rect, 2, null);
				GUI.color = Color.white;
			}
			base.Gap(22f + this.verticalSpacing);
		}

		// Token: 0x06001F52 RID: 8018 RVA: 0x000C31F8 File Offset: 0x000C13F8
		public bool ButtonDebug(string label, bool highlight)
		{
			Text.Font = GameFont.Tiny;
			base.NewColumnIfNeeded(22f);
			bool wordWrap = Text.WordWrap;
			Text.WordWrap = false;
			Rect rect = new Rect(this.curX, this.curY, base.ColumnWidth, 22f);
			bool result = Widgets.ButtonText(rect, label, true, true, true);
			Text.WordWrap = wordWrap;
			if (highlight)
			{
				GUI.color = Color.yellow;
				Widgets.DrawBox(rect, 2, null);
				GUI.color = Color.white;
			}
			base.Gap(22f + this.verticalSpacing);
			return result;
		}

		// Token: 0x04001305 RID: 4869
		private GameFont font;

		// Token: 0x04001306 RID: 4870
		private List<Pair<Vector2, Vector2>> labelScrollbarPositions;

		// Token: 0x04001307 RID: 4871
		private List<Vector2> labelScrollbarPositionsSetThisFrame;

		// Token: 0x04001308 RID: 4872
		private const float DefSelectionLineHeight = 21f;
	}
}
