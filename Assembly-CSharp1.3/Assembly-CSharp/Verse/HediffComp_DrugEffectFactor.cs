using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000290 RID: 656
	public class HediffComp_DrugEffectFactor : HediffComp
	{
		// Token: 0x17000392 RID: 914
		// (get) Token: 0x0600125D RID: 4701 RVA: 0x0006A19A File Offset: 0x0006839A
		public HediffCompProperties_DrugEffectFactor Props
		{
			get
			{
				return (HediffCompProperties_DrugEffectFactor)this.props;
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x0600125E RID: 4702 RVA: 0x0006A1A7 File Offset: 0x000683A7
		private float CurrentFactor
		{
			get
			{
				return HediffComp_DrugEffectFactor.EffectFactorSeverityCurve.Evaluate(this.parent.Severity);
			}
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x0600125F RID: 4703 RVA: 0x0006A1C0 File Offset: 0x000683C0
		public override string CompTipStringExtra
		{
			get
			{
				return "DrugEffectMultiplier".Translate(this.Props.chemical.label, this.CurrentFactor.ToStringPercent()).CapitalizeFirst();
			}
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x0006A209 File Offset: 0x00068409
		public override void CompModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
			if (this.Props.chemical == chem)
			{
				effect *= this.CurrentFactor;
			}
		}

		// Token: 0x04000DE9 RID: 3561
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
