using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A62 RID: 6754
	public class StatDrawEntry
	{
		// Token: 0x1700177A RID: 6010
		// (get) Token: 0x060094E7 RID: 38119 RVA: 0x0006373C File Offset: 0x0006193C
		public bool ShouldDisplay
		{
			get
			{
				return this.stat == null || !Mathf.Approximately(this.value, this.stat.hideAtValue);
			}
		}

		// Token: 0x1700177B RID: 6011
		// (get) Token: 0x060094E8 RID: 38120 RVA: 0x00063761 File Offset: 0x00061961
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

		// Token: 0x1700177C RID: 6012
		// (get) Token: 0x060094E9 RID: 38121 RVA: 0x002B3EB4 File Offset: 0x002B20B4
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

		// Token: 0x1700177D RID: 6013
		// (get) Token: 0x060094EA RID: 38122 RVA: 0x00063787 File Offset: 0x00061987
		public int DisplayPriorityWithinCategory
		{
			get
			{
				return this.displayOrderWithinCategory;
			}
		}

		// Token: 0x060094EB RID: 38123 RVA: 0x002B3F18 File Offset: 0x002B2118
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

		// Token: 0x060094EC RID: 38124 RVA: 0x002B3F9C File Offset: 0x002B219C
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

		// Token: 0x060094ED RID: 38125 RVA: 0x002B4008 File Offset: 0x002B2208
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

		// Token: 0x060094EE RID: 38126 RVA: 0x0006378F File Offset: 0x0006198F
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

		// Token: 0x060094EF RID: 38127 RVA: 0x002B405C File Offset: 0x002B225C
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

		// Token: 0x060094F0 RID: 38128 RVA: 0x002B40C4 File Offset: 0x002B22C4
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

		// Token: 0x060094F1 RID: 38129 RVA: 0x000637BB File Offset: 0x000619BB
		public StatDrawEntry SetReportText(string reportText)
		{
			this.overrideReportText = reportText;
			return this;
		}

		// Token: 0x060094F2 RID: 38130 RVA: 0x002B4140 File Offset: 0x002B2340
		public float Draw(float x, float y, float width, bool selected, Action clickedCallback, Action mousedOverCallback, Vector2 scrollPosition, Rect scrollOutRect)
		{
			float num = width * 0.45f;
			Rect rect = new Rect(8f, y, width, Text.CalcHeight(this.ValueString, num));
			if (y - scrollPosition.y + rect.height >= 0f && y - scrollPosition.y <= scrollOutRect.height)
			{
				if (selected)
				{
					Widgets.DrawHighlightSelected(rect);
				}
				else if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
				}
				Rect rect2 = rect;
				rect2.width -= num;
				Widgets.Label(rect2, this.LabelCap);
				Rect rect3 = rect;
				rect3.x = rect2.xMax;
				rect3.width = num;
				Widgets.Label(rect3, this.ValueString);
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

		// Token: 0x060094F3 RID: 38131 RVA: 0x000637C5 File Offset: 0x000619C5
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

		// Token: 0x04005EB4 RID: 24244
		public StatCategoryDef category;

		// Token: 0x04005EB5 RID: 24245
		private int displayOrderWithinCategory;

		// Token: 0x04005EB6 RID: 24246
		public StatDef stat;

		// Token: 0x04005EB7 RID: 24247
		private float value;

		// Token: 0x04005EB8 RID: 24248
		public StatRequest optionalReq;

		// Token: 0x04005EB9 RID: 24249
		public bool hasOptionalReq;

		// Token: 0x04005EBA RID: 24250
		public bool forceUnfinalizedMode;

		// Token: 0x04005EBB RID: 24251
		private IEnumerable<Dialog_InfoCard.Hyperlink> hyperlinks;

		// Token: 0x04005EBC RID: 24252
		private string labelInt;

		// Token: 0x04005EBD RID: 24253
		private string valueStringInt;

		// Token: 0x04005EBE RID: 24254
		private string overrideReportText;

		// Token: 0x04005EBF RID: 24255
		private string overrideReportTitle;

		// Token: 0x04005EC0 RID: 24256
		private string explanationText;

		// Token: 0x04005EC1 RID: 24257
		private ToStringNumberSense numberSense;
	}
}
