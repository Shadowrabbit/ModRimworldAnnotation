using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000745 RID: 1861
	public class Listing_Standard : Listing
	{
		// Token: 0x06002EC1 RID: 11969 RVA: 0x00024A79 File Offset: 0x00022C79
		public Listing_Standard(GameFont font)
		{
			this.font = font;
		}

		// Token: 0x06002EC2 RID: 11970 RVA: 0x00024A88 File Offset: 0x00022C88
		public Listing_Standard()
		{
			this.font = GameFont.Small;
		}

		// Token: 0x06002EC3 RID: 11971 RVA: 0x00024A97 File Offset: 0x00022C97
		public override void Begin(Rect rect)
		{
			base.Begin(rect);
			Text.Font = this.font;
		}

		// Token: 0x06002EC4 RID: 11972 RVA: 0x00024AAB File Offset: 0x00022CAB
		public void BeginScrollView(Rect rect, ref Vector2 scrollPosition, ref Rect viewRect)
		{
			Widgets.BeginScrollView(rect, ref scrollPosition, viewRect, true);
			rect.height = 100000f;
			rect.width -= 20f;
			this.Begin(rect.AtZero());
		}

		// Token: 0x06002EC5 RID: 11973 RVA: 0x00138D68 File Offset: 0x00136F68
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

		// Token: 0x06002EC6 RID: 11974 RVA: 0x00024AE6 File Offset: 0x00022CE6
		public void EndScrollView(ref Rect viewRect)
		{
			viewRect = new Rect(0f, 0f, this.listingRect.width, this.curY);
			Widgets.EndScrollView();
			this.End();
		}

		// Token: 0x06002EC7 RID: 11975 RVA: 0x00024B19 File Offset: 0x00022D19
		public Rect Label(TaggedString label, float maxHeight = -1f, string tooltip = null)
		{
			return this.Label(label.Resolve(), maxHeight, tooltip);
		}

		// Token: 0x06002EC8 RID: 11976 RVA: 0x00138DD4 File Offset: 0x00136FD4
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

		// Token: 0x06002EC9 RID: 11977 RVA: 0x00138E64 File Offset: 0x00137064
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

		// Token: 0x06002ECA RID: 11978 RVA: 0x00138EE8 File Offset: 0x001370E8
		[Obsolete]
		public bool RadioButton(string label, bool active, float tabIn = 0f, string tooltip = null)
		{
			return this.RadioButton_NewTemp(label, active, tabIn, tooltip, null);
		}

		// Token: 0x06002ECB RID: 11979 RVA: 0x00138F0C File Offset: 0x0013710C
		public bool RadioButton_NewTemp(string label, bool active, float tabIn = 0f, string tooltip = null, float? tooltipDelay = null)
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

		// Token: 0x06002ECC RID: 11980 RVA: 0x00138F8C File Offset: 0x0013718C
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

		// Token: 0x06002ECD RID: 11981 RVA: 0x00138FE4 File Offset: 0x001371E4
		public bool CheckboxLabeledSelectable(string label, ref bool selected, ref bool checkOn)
		{
			float lineHeight = Text.LineHeight;
			bool result = Widgets.CheckboxLabeledSelectable(base.GetRect(lineHeight), label, ref selected, ref checkOn);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06002ECE RID: 11982 RVA: 0x00139014 File Offset: 0x00137214
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

		// Token: 0x06002ECF RID: 11983 RVA: 0x00024B2A File Offset: 0x00022D2A
		public bool ButtonTextLabeled(string label, string buttonLabel)
		{
			Rect rect = base.GetRect(30f);
			Widgets.Label(rect.LeftHalf(), label);
			bool result = Widgets.ButtonText(rect.RightHalf(), buttonLabel, true, true, true);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06002ED0 RID: 11984 RVA: 0x00024B5D File Offset: 0x00022D5D
		public bool ButtonImage(Texture2D tex, float width, float height)
		{
			base.NewColumnIfNeeded(height);
			bool result = Widgets.ButtonImage(new Rect(this.curX, this.curY, width, height), tex, true);
			base.Gap(height + this.verticalSpacing);
			return result;
		}

		// Token: 0x06002ED1 RID: 11985 RVA: 0x00024B8E File Offset: 0x00022D8E
		public void None()
		{
			GUI.color = Color.gray;
			Text.Anchor = TextAnchor.UpperCenter;
			this.Label("NoneBrackets".Translate(), -1f, null);
			GenUI.ResetLabelAlign();
			GUI.color = Color.white;
		}

		// Token: 0x06002ED2 RID: 11986 RVA: 0x00139050 File Offset: 0x00137250
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

		// Token: 0x06002ED3 RID: 11987 RVA: 0x00024BC6 File Offset: 0x00022DC6
		public string TextEntryLabeled(string label, string text, int lineCount = 1)
		{
			string result = Widgets.TextEntryLabeled(base.GetRect(Text.LineHeight * (float)lineCount), label, text);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06002ED4 RID: 11988 RVA: 0x00024BE9 File Offset: 0x00022DE9
		public void TextFieldNumeric<T>(ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			Widgets.TextFieldNumeric<T>(base.GetRect(Text.LineHeight), ref val, ref buffer, min, max);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06002ED5 RID: 11989 RVA: 0x00024C0C File Offset: 0x00022E0C
		public void TextFieldNumericLabeled<T>(string label, ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			Widgets.TextFieldNumericLabeled<T>(base.GetRect(Text.LineHeight), label, ref val, ref buffer, min, max);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06002ED6 RID: 11990 RVA: 0x00024C31 File Offset: 0x00022E31
		public void IntRange(ref IntRange range, int min, int max)
		{
			Widgets.IntRange(base.GetRect(28f), (int)base.CurHeight, ref range, min, max, null, 0);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06002ED7 RID: 11991 RVA: 0x00139090 File Offset: 0x00137290
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

		// Token: 0x06002ED8 RID: 11992 RVA: 0x001390D4 File Offset: 0x001372D4
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

		// Token: 0x06002ED9 RID: 11993 RVA: 0x00024C5B File Offset: 0x00022E5B
		public void IntSetter(ref int val, int target, string label, float width = 42f)
		{
			if (Widgets.ButtonText(base.GetRect(24f), label, true, true, true))
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				val = target;
			}
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06002EDA RID: 11994 RVA: 0x00024C8D File Offset: 0x00022E8D
		public void IntEntry(ref int val, ref string editBuffer, int multiplier = 1)
		{
			Widgets.IntEntry(base.GetRect(24f), ref val, ref editBuffer, multiplier);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06002EDB RID: 11995 RVA: 0x00024CAE File Offset: 0x00022EAE
		[Obsolete]
		public Listing_Standard BeginSection(float height)
		{
			return this.BeginSection_NewTemp(height, 4f, 4f);
		}

		// Token: 0x06002EDC RID: 11996 RVA: 0x00139194 File Offset: 0x00137394
		public Listing_Standard BeginSection_NewTemp(float height, float sectionBorder = 4f, float bottomBorder = 4f)
		{
			Rect rect = base.GetRect(height + sectionBorder + bottomBorder);
			Widgets.DrawMenuSection(rect);
			Listing_Standard listing_Standard = new Listing_Standard();
			Rect rect2 = new Rect(rect.x + sectionBorder, rect.y + sectionBorder, rect.width - sectionBorder * 2f, rect.height - (sectionBorder + bottomBorder));
			listing_Standard.Begin(rect2);
			return listing_Standard;
		}

		// Token: 0x06002EDD RID: 11997 RVA: 0x00024CC1 File Offset: 0x00022EC1
		public void EndSection(Listing_Standard listing)
		{
			listing.End();
		}

		// Token: 0x06002EDE RID: 11998 RVA: 0x001391F4 File Offset: 0x001373F4
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

		// Token: 0x06002EDF RID: 11999 RVA: 0x00139268 File Offset: 0x00137468
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

		// Token: 0x06002EE0 RID: 12000 RVA: 0x00139314 File Offset: 0x00137514
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
				Widgets.DrawBox(rect, 1);
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

		// Token: 0x06002EE1 RID: 12001 RVA: 0x00024CC9 File Offset: 0x00022EC9
		[Obsolete("Only used for mod compatibility")]
		public void LabelCheckboxDebug(string label, ref bool checkOn)
		{
			this.LabelCheckboxDebug_NewTmp(label, ref checkOn, false);
		}

		// Token: 0x06002EE2 RID: 12002 RVA: 0x001393E0 File Offset: 0x001375E0
		public void LabelCheckboxDebug_NewTmp(string label, ref bool checkOn, bool highlight)
		{
			Text.Font = GameFont.Tiny;
			base.NewColumnIfNeeded(22f);
			Rect rect = new Rect(this.curX, this.curY, base.ColumnWidth, 22f);
			Widgets.CheckboxLabeled(rect, label, ref checkOn, false, null, null, false);
			if (highlight)
			{
				GUI.color = Color.yellow;
				Widgets.DrawBox(rect, 2);
				GUI.color = Color.white;
			}
			base.Gap(22f + this.verticalSpacing);
		}

		// Token: 0x06002EE3 RID: 12003 RVA: 0x00024CD4 File Offset: 0x00022ED4
		[Obsolete("Only used for mod compatibility")]
		public bool ButtonDebug(string label)
		{
			return this.ButtonDebug_NewTmp(label, false);
		}

		// Token: 0x06002EE4 RID: 12004 RVA: 0x00139458 File Offset: 0x00137658
		public bool ButtonDebug_NewTmp(string label, bool highlight)
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
				Widgets.DrawBox(rect, 2);
				GUI.color = Color.white;
			}
			base.Gap(22f + this.verticalSpacing);
			return result;
		}

		// Token: 0x04001FD6 RID: 8150
		private GameFont font;

		// Token: 0x04001FD7 RID: 8151
		private List<Pair<Vector2, Vector2>> labelScrollbarPositions;

		// Token: 0x04001FD8 RID: 8152
		private List<Vector2> labelScrollbarPositionsSetThisFrame;

		// Token: 0x04001FD9 RID: 8153
		private const float DefSelectionLineHeight = 21f;
	}
}
