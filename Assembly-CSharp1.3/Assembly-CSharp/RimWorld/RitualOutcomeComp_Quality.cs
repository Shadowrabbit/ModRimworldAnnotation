using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F56 RID: 3926
	public abstract class RitualOutcomeComp_Quality : RitualOutcomeComp
	{
		// Token: 0x1700101F RID: 4127
		// (get) Token: 0x06005D27 RID: 23847 RVA: 0x001FF7A4 File Offset: 0x001FD9A4
		protected float MaxValue
		{
			get
			{
				return this.curve.Points[this.curve.PointsCount - 1].x;
			}
		}

		// Token: 0x06005D28 RID: 23848 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool Applies(LordJob_Ritual ritual)
		{
			return true;
		}

		// Token: 0x06005D29 RID: 23849
		public abstract float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data);

		// Token: 0x06005D2A RID: 23850 RVA: 0x001FF7D8 File Offset: 0x001FD9D8
		public override string GetDesc(LordJob_Ritual ritual = null, RitualOutcomeComp_Data data = null)
		{
			if ((this.DataRequired || ritual == null) && data == null)
			{
				return this.label + " (" + "MaxValue".Translate(this.MaxValue) + "): " + "OutcomeBonusDesc_Quality".Translate("+" + this.curve.Points[this.curve.PointsCount - 1].y.ToStringPercent()) + ".";
			}
			return string.Concat(new object[]
			{
				this.Count(ritual, data),
				" / ",
				this.MaxValue,
				" ",
				this.label,
				": +",
				this.QualityOffset(ritual, data).ToStringPercent()
			});
		}

		// Token: 0x06005D2B RID: 23851 RVA: 0x001FF8E0 File Offset: 0x001FDAE0
		public override string GetBonusDescShort()
		{
			return "OutcomeBonusDesc_Quality".Translate("+" + this.curve.Points[this.curve.PointsCount - 1].y.ToStringPercent()) + ".";
		}

		// Token: 0x06005D2C RID: 23852 RVA: 0x001FF93F File Offset: 0x001FDB3F
		public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			if (this.curve == null)
			{
				return 0f;
			}
			return this.curve.Evaluate(this.Count(ritual, data));
		}

		// Token: 0x06005D2D RID: 23853 RVA: 0x001FF964 File Offset: 0x001FDB64
		protected override string ExpectedOffsetDesc(bool positive, float quality = 0f)
		{
			return "QualityOutOf".Translate(quality.ToStringWithSign("0.#%"), this.curve.Points[this.curve.PointsCount - 1].y.ToStringWithSign("0.#%"));
		}

		// Token: 0x040035F7 RID: 13815
		public SimpleCurve curve;
	}
}
