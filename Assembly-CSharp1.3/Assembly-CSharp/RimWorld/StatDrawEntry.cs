using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001310 RID: 4880
	public class StatDrawEntry
	{
		// Token: 0x17001486 RID: 5254
		// (get) Token: 0x0600756C RID: 30060 RVA: 0x00287644 File Offset: 0x00285844
		public bool ShouldDisplay
		{
			get
			{
				return this.stat == null || !Mathf.Approximately(this.value, this.stat.hideAtValue);
			}
		}

		// Token: 0x17001487 RID: 5255
		// (get) Token: 0x0600756D RID: 30061 RVA: 0x00287669 File Offset: 0x00285869
		public string LabelCap
		{
			get
			{
				if (this.labelInt != null)
				{
					return this.labelInt.CapitalizeFirst();
				}
				return this.stat.LabelCap;
			}
		}

		// Token: 0x17001488 RID: 5256
		// (get) Token: 0x0600756E RID: 30062 RVA: 0x00287690 File Offset: 0x00285890
		public string ValueString
		{
			get
			{
				if (this.numberSense == ToStringNumberSense.Factor)
				{
					return this.value.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Absolute);
				}
				if (this.valueStringInt == null)
				{
					return this.stat.Worker.GetStatDrawEntryLabel(this.stat, this.value, this.numberSense, this.optionalReq, !this.forceUnfinalizedMode);
				}
				return this.valueStringInt;
			}
		}

		// Token: 0x17001489 RID: 5257
		// (get) Token: 0x0600756F RID: 30063 RVA: 0x002876F4 File Offset: 0x002858F4
		public int DisplayPriorityWithinCategory
		{
			get
			{
				return this.displayOrderWithinCategory;
			}
		}

		// Token: 0x06007570 RID: 30064 RVA: 0x002876FC File Offset: 0x002858FC
		public StatDrawEntry(StatCategoryDef category, StatDef stat, float value, StatRequest optionalReq, ToStringNumberSense numberSense = ToStringNumberSense.Undefined, int? overrideDisplayPriorityWithinCategory = null, bool forceUnfinalizedMode = false)
		{
			this.category = category;
			this.stat = stat;
			this.labelInt = null;
			this.value = value;
			this.valueStringInt = null;
			this.displayOrderWithinCategory = ((overrideDisplayPriorityWithinCategory != null) ? overrideDisplayPriorityWithinCategory.Value : stat.displayPriorityInCategory);
			this.optionalReq = optionalReq;
			this.forceUnfinalizedMode = forceUnfinalizedMode;
			this.hasOptionalReq = true;
			if (numberSense == ToStringNumberSense.Undefined)
			{
				this.numberSense = stat.toStringNumberSense;
				return;
			}
			this.numberSense = numberSense;
		}

		// Token: 0x06007571 RID: 30065 RVA: 0x00287780 File Offset: 0x00285980
		public StatDrawEntry(StatCategoryDef category, string label, string valueString, string reportText, int displayPriorityWithinCategory, string overrideReportTitle = null, IEnumerable<Dialog_InfoCard.Hyperlink> hyperlinks = null, bool forceUnfinalizedMode = false)
		{
			this.category = category;
			this.stat = null;
			this.labelInt = label;
			this.value = 0f;
			this.valueStringInt = valueString;
			this.displayOrderWithinCategory = displayPriorityWithinCategory;
			this.numberSense = ToStringNumberSense.Absolute;
			this.overrideReportText = reportText;
			this.overrideReportTitle = overrideReportTitle;
			this.hyperlinks = hyperlinks;
			this.forceUnfinalizedMode = forceUnfinalizedMode;
		}

		// Token: 0x06007572 RID: 30066 RVA: 0x002877EC File Offset: 0x002859EC
		public StatDrawEntry(StatCategoryDef category, StatDef stat)
		{
			this.category = category;
			this.stat = stat;
			this.labelInt = null;
			this.value = 0f;
			this.valueStringInt = "-";
			this.displayOrderWithinCategory = stat.displayPriorityInCategory;
			this.numberSense = ToStringNumberSense.Undefined;
		}

		// Token: 0x06007573 RID: 30067 RVA: 0x0028783D File Offset: 0x00285A3D
		public IEnumerable<Dialog_InfoCard.Hyperlink> GetHyperlinks(StatRequest req)
		{
			if (this.hyperlinks != null)
			{
				return this.hyperlinks;
			}
			if (this.stat != null)
			{
				return this.stat.Worker.GetInfoCardHyperlinks(req);
			}
			return null;
		}

		// Token: 0x06007574 RID: 30068 RVA: 0x0028786C File Offset: 0x00285A6C
		public string GetExplanationText(StatRequest optionalReq)
		{
			if (this.explanationText == null)
			{
				this.WriteExplanationTextInt();
			}
			string result;
			if (optionalReq.Empty || this.stat == null)
			{
				result = this.explanationText;
			}
			else
			{
				result = string.Format("{0}\n\n{1}", this.explanationText, this.stat.Worker.GetExplanationFull(optionalReq, this.numberSense, this.value));
			}
			return result;
		}

		// Token: 0x06007575 RID: 30069 RVA: 0x002878D4 File Offset: 0x00285AD4
		private void WriteExplanationTextInt()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!this.overrideReportTitle.NullOrEmpty())
			{
				stringBuilder.AppendLine(this.overrideReportTitle);
			}
			if (!this.overrideReportText.NullOrEmpty())
			{
				stringBuilder.AppendLine(this.overrideReportText);
			}
			else if (this.stat != null)
			{
				stringBuilder.AppendLine(this.stat.description);
			}
			stringBuilder.AppendLine();
			this.explanationText = stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06007576 RID: 30070 RVA: 0x0028794F File Offset: 0x00285B4F
		public StatDrawEntry SetReportText(string reportText)
		{
			this.overrideReportText = reportText;
			return this;
		}

		// Token: 0x06007577 RID: 30071 RVA: 0x0028795C File Offset: 0x00285B5C
		public float Draw(float x, float y, float width, bool selected, bool highlightLabel, bool lowlightLabel, Action clickedCallback, Action mousedOverCallback, Vector2 scrollPosition, Rect scrollOutRect)
		{
			float num = width * 0.45f;
			Rect rect = new Rect(8f, y, width, Text.CalcHeight(this.ValueString, num));
			if (y - scrollPosition.y + rect.height >= 0f && y - scrollPosition.y <= scrollOutRect.height)
			{
				GUI.color = Color.white;
				if (selected)
				{
					Widgets.DrawHighlightSelected(rect);
				}
				else if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
				}
				if (highlightLabel)
				{
					QuickSearchWidget.DrawTextHighlight(rect, 4f);
				}
				if (lowlightLabel)
				{
					GUI.color = Color.grey;
				}
				Rect rect2 = rect;
				rect2.width -= num;
				Widgets.Label(rect2, this.LabelCap);
				Rect rect3 = rect;
				rect3.x = rect2.xMax;
				rect3.width = num;
				Widgets.Label(rect3, this.ValueString);
				GUI.color = Color.white;
				if (this.stat != null && Mouse.IsOver(rect))
				{
					StatDef localStat = this.stat;
					TooltipHandler.TipRegion(rect, new TipSignal(() => localStat.LabelCap + ": " + localStat.description, this.stat.GetHashCode()));
				}
				if (Widgets.ButtonInvisible(rect, true))
				{
					clickedCallback();
				}
				if (Mouse.IsOver(rect))
				{
					mousedOverCallback();
				}
			}
			return rect.height;
		}

		// Token: 0x06007578 RID: 30072 RVA: 0x00287AB6 File Offset: 0x00285CB6
		public bool Same(StatDrawEntry other)
		{
			return other != null && (this == other || (this.stat == other.stat && this.labelInt == other.labelInt));
		}

		// Token: 0x06007579 RID: 30073 RVA: 0x00287AE4 File Offset: 0x00285CE4
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.LabelCap,
				": ",
				this.ValueString,
				")"
			});
		}

		// Token: 0x04004102 RID: 16642
		public StatCategoryDef category;

		// Token: 0x04004103 RID: 16643
		private int displayOrderWithinCategory;

		// Token: 0x04004104 RID: 16644
		public StatDef stat;

		// Token: 0x04004105 RID: 16645
		private float value;

		// Token: 0x04004106 RID: 16646
		public StatRequest optionalReq;

		// Token: 0x04004107 RID: 16647
		public bool hasOptionalReq;

		// Token: 0x04004108 RID: 16648
		public bool forceUnfinalizedMode;

		// Token: 0x04004109 RID: 16649
		private IEnumerable<Dialog_InfoCard.Hyperlink> hyperlinks;

		// Token: 0x0400410A RID: 16650
		private string labelInt;

		// Token: 0x0400410B RID: 16651
		private string valueStringInt;

		// Token: 0x0400410C RID: 16652
		private string overrideReportText;

		// Token: 0x0400410D RID: 16653
		private string overrideReportTitle;

		// Token: 0x0400410E RID: 16654
		private string explanationText;

		// Token: 0x0400410F RID: 16655
		private ToStringNumberSense numberSense;
	}
}
