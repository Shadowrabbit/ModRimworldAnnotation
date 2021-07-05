using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F57 RID: 3927
	public abstract class RitualOutcomeComp_QualitySingleOffset : RitualOutcomeComp_Quality
	{
		// Token: 0x17001020 RID: 4128
		// (get) Token: 0x06005D2F RID: 23855 RVA: 0x001FF9CC File Offset: 0x001FDBCC
		protected virtual string LabelForDesc
		{
			get
			{
				return this.label.CapitalizeFirst();
			}
		}

		// Token: 0x06005D30 RID: 23856 RVA: 0x001FF9DC File Offset: 0x001FDBDC
		public override string GetDesc(LordJob_Ritual ritual = null, RitualOutcomeComp_Data data = null)
		{
			string str = (this.qualityOffset < 0f) ? "" : "+";
			return this.LabelForDesc + ": " + "OutcomeBonusDesc_QualitySingleOffset".Translate(str + this.qualityOffset.ToStringPercent()) + ".";
		}

		// Token: 0x06005D31 RID: 23857 RVA: 0x001FFA47 File Offset: 0x001FDC47
		public override string GetBonusDescShort()
		{
			return "OutcomeBonusDesc_QualitySingleOffset".Translate("+" + this.qualityOffset.ToStringPercent()) + ".";
		}

		// Token: 0x06005D32 RID: 23858 RVA: 0x0001F15E File Offset: 0x0001D35E
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			return 1f;
		}

		// Token: 0x06005D33 RID: 23859 RVA: 0x001FFA7C File Offset: 0x001FDC7C
		public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			return this.qualityOffset;
		}

		// Token: 0x06005D34 RID: 23860 RVA: 0x001FFA84 File Offset: 0x001FDC84
		protected override string ExpectedOffsetDesc(bool positive, float quality = -1f)
		{
			quality = ((quality == -1f) ? this.qualityOffset : quality);
			return positive ? quality.ToStringWithSign("0.#%") : "QualityOutOf".Translate("+0", quality.ToStringWithSign("0.#%"));
		}
	}
}
