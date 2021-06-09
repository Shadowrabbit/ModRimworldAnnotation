using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003D3 RID: 979
	public class HediffComp_DrugEffectFactor : HediffComp
	{
		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06001833 RID: 6195 RVA: 0x0001702D File Offset: 0x0001522D
		public HediffCompProperties_DrugEffectFactor Props
		{
			get
			{
				return (HediffCompProperties_DrugEffectFactor)this.props;
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06001834 RID: 6196 RVA: 0x0001703A File Offset: 0x0001523A
		private float CurrentFactor
		{
			get
			{
				return HediffComp_DrugEffectFactor.EffectFactorSeverityCurve.Evaluate(this.parent.Severity);
			}
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06001835 RID: 6197 RVA: 0x000DEB2C File Offset: 0x000DCD2C
		public override string CompTipStringExtra
		{
			get
			{
				return "DrugEffectMultiplier".Translate(this.Props.chemical.label, this.CurrentFactor.ToStringPercent()).CapitalizeFirst();
			}
		}

		// Token: 0x06001836 RID: 6198 RVA: 0x00017051 File Offset: 0x00015251
		public override void CompModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
			if (this.Props.chemical == chem)
			{
				effect *= this.CurrentFactor;
			}
		}

		// Token: 0x0400125A RID: 4698
		private static readonly SimpleCurve EffectFactorSeverityCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(1f, 0.25f),
				true
			}
		};
	}
}
