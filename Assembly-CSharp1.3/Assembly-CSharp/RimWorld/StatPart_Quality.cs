using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014DE RID: 5342
	public class StatPart_Quality : StatPart
	{
		// Token: 0x06007F54 RID: 32596 RVA: 0x002D0154 File Offset: 0x002CE354
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

		// Token: 0x06007F55 RID: 32597 RVA: 0x002D01A4 File Offset: 0x002CE3A4
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

		// Token: 0x06007F56 RID: 32598 RVA: 0x002D0290 File Offset: 0x002CE490
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

		// Token: 0x06007F57 RID: 32599 RVA: 0x002D02F8 File Offset: 0x002CE4F8
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

		// Token: 0x04004F78 RID: 20344
		private bool applyToNegativeValues;

		// Token: 0x04004F79 RID: 20345
		private float factorAwful = 1f;

		// Token: 0x04004F7A RID: 20346
		private float factorPoor = 1f;

		// Token: 0x04004F7B RID: 20347
		private float factorNormal = 1f;

		// Token: 0x04004F7C RID: 20348
		private float factorGood = 1f;

		// Token: 0x04004F7D RID: 20349
		private float factorExcellent = 1f;

		// Token: 0x04004F7E RID: 20350
		private float factorMasterwork = 1f;

		// Token: 0x04004F7F RID: 20351
		private float factorLegendary = 1f;

		// Token: 0x04004F80 RID: 20352
		private float maxGainAwful = 9999999f;

		// Token: 0x04004F81 RID: 20353
		private float maxGainPoor = 9999999f;

		// Token: 0x04004F82 RID: 20354
		private float maxGainNormal = 9999999f;

		// Token: 0x04004F83 RID: 20355
		private float maxGainGood = 9999999f;

		// Token: 0x04004F84 RID: 20356
		private float maxGainExcellent = 9999999f;

		// Token: 0x04004F85 RID: 20357
		private float maxGainMasterwork = 9999999f;

		// Token: 0x04004F86 RID: 20358
		private float maxGainLegendary = 9999999f;
	}
}
