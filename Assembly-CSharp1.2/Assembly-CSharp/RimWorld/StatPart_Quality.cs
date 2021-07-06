using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D45 RID: 7493
	public class StatPart_Quality : StatPart
	{
		// Token: 0x0600A2CC RID: 41676 RVA: 0x002F5E74 File Offset: 0x002F4074
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (val <= 0f && !this.applyToNegativeValues)
			{
				return;
			}
			float num = val * this.QualityMultiplier(req.QualityCategory) - val;
			num = Mathf.Min(num, this.MaxGain(req.QualityCategory));
			val += num;
		}

		// Token: 0x0600A2CD RID: 41677 RVA: 0x002F5EC4 File Offset: 0x002F40C4
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && !this.applyToNegativeValues && req.Thing.GetStatValue(this.parentStat, true) <= 0f)
			{
				return null;
			}
			QualityCategory qc;
			if (req.HasThing && req.Thing.TryGetQuality(out qc))
			{
				string text = "StatsReport_QualityMultiplier".Translate() + ": x" + this.QualityMultiplier(qc).ToStringPercent();
				float num = this.MaxGain(qc);
				if (num < 999999f)
				{
					text += "\n    (" + "StatsReport_MaxGain".Translate() + ": " + num.ToStringByStyle(this.parentStat.ToStringStyleUnfinalized, this.parentStat.toStringNumberSense) + ")";
				}
				return text;
			}
			return null;
		}

		// Token: 0x0600A2CE RID: 41678 RVA: 0x002F5FB0 File Offset: 0x002F41B0
		private float QualityMultiplier(QualityCategory qc)
		{
			switch (qc)
			{
			case QualityCategory.Awful:
				return this.factorAwful;
			case QualityCategory.Poor:
				return this.factorPoor;
			case QualityCategory.Normal:
				return this.factorNormal;
			case QualityCategory.Good:
				return this.factorGood;
			case QualityCategory.Excellent:
				return this.factorExcellent;
			case QualityCategory.Masterwork:
				return this.factorMasterwork;
			case QualityCategory.Legendary:
				return this.factorLegendary;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x0600A2CF RID: 41679 RVA: 0x002F6018 File Offset: 0x002F4218
		private float MaxGain(QualityCategory qc)
		{
			switch (qc)
			{
			case QualityCategory.Awful:
				return this.maxGainAwful;
			case QualityCategory.Poor:
				return this.maxGainPoor;
			case QualityCategory.Normal:
				return this.maxGainNormal;
			case QualityCategory.Good:
				return this.maxGainGood;
			case QualityCategory.Excellent:
				return this.maxGainExcellent;
			case QualityCategory.Masterwork:
				return this.maxGainMasterwork;
			case QualityCategory.Legendary:
				return this.maxGainLegendary;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x04006E87 RID: 28295
		private bool applyToNegativeValues;

		// Token: 0x04006E88 RID: 28296
		private float factorAwful = 1f;

		// Token: 0x04006E89 RID: 28297
		private float factorPoor = 1f;

		// Token: 0x04006E8A RID: 28298
		private float factorNormal = 1f;

		// Token: 0x04006E8B RID: 28299
		private float factorGood = 1f;

		// Token: 0x04006E8C RID: 28300
		private float factorExcellent = 1f;

		// Token: 0x04006E8D RID: 28301
		private float factorMasterwork = 1f;

		// Token: 0x04006E8E RID: 28302
		private float factorLegendary = 1f;

		// Token: 0x04006E8F RID: 28303
		private float maxGainAwful = 9999999f;

		// Token: 0x04006E90 RID: 28304
		private float maxGainPoor = 9999999f;

		// Token: 0x04006E91 RID: 28305
		private float maxGainNormal = 9999999f;

		// Token: 0x04006E92 RID: 28306
		private float maxGainGood = 9999999f;

		// Token: 0x04006E93 RID: 28307
		private float maxGainExcellent = 9999999f;

		// Token: 0x04006E94 RID: 28308
		private float maxGainMasterwork = 9999999f;

		// Token: 0x04006E95 RID: 28309
		private float maxGainLegendary = 9999999f;
	}
}
