using System;

namespace Verse
{
	// Token: 0x02000297 RID: 663
	public class HediffComp_GetsPermanent : HediffComp
	{
		// Token: 0x17000397 RID: 919
		// (get) Token: 0x0600126C RID: 4716 RVA: 0x0006A3BB File Offset: 0x000685BB
		public HediffCompProperties_GetsPermanent Props
		{
			get
			{
				return (HediffCompProperties_GetsPermanent)this.props;
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x0600126D RID: 4717 RVA: 0x0006A3C8 File Offset: 0x000685C8
		// (set) Token: 0x0600126E RID: 4718 RVA: 0x0006A3D0 File Offset: 0x000685D0
		public bool IsPermanent
		{
			get
			{
				return this.isPermanentInt;
			}
			set
			{
				if (value == this.isPermanentInt)
				{
					return;
				}
				this.isPermanentInt = value;
				if (this.isPermanentInt)
				{
					this.permanentDamageThreshold = 9999f;
					this.SetPainCategory(HealthTuning.InjuryPainCategories.RandomElementByWeight((HealthTuning.PainCategoryWeighted cat) => cat.weight).category);
				}
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x0600126F RID: 4719 RVA: 0x0006A435 File Offset: 0x00068635
		public PainCategory PainCategory
		{
			get
			{
				return this.painCategory;
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06001270 RID: 4720 RVA: 0x0006A43D File Offset: 0x0006863D
		public float PainFactor
		{
			get
			{
				return (float)this.painCategory;
			}
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x0006A446 File Offset: 0x00068646
		public void SetPainCategory(PainCategory category)
		{
			this.painCategory = category;
			if (base.Pawn != null)
			{
				base.Pawn.health.Notify_HediffChanged(this.parent);
			}
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x0006A470 File Offset: 0x00068670
		public override void CompExposeData()
		{
			Scribe_Values.Look<bool>(ref this.isPermanentInt, "isPermanent", false, false);
			Scribe_Values.Look<float>(ref this.permanentDamageThreshold, "permanentDamageThreshold", 9999f, false);
			Scribe_Values.Look<PainCategory>(ref this.painCategory, "painCategory", PainCategory.Painless, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x0006A4C0 File Offset: 0x000686C0
		public void PreFinalizeInjury()
		{
			if (base.Pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(this.parent.Part))
			{
				return;
			}
			float num = 0.02f * this.parent.Part.def.permanentInjuryChanceFactor * this.Props.becomePermanentChanceFactor;
			if (!this.parent.Part.def.delicate)
			{
				num *= HealthTuning.BecomePermanentChanceFactorBySeverityCurve.Evaluate(this.parent.Severity);
			}
			if (Rand.Chance(num))
			{
				if (this.parent.Part.def.delicate)
				{
					this.IsPermanent = true;
					return;
				}
				this.permanentDamageThreshold = Rand.Range(1f, this.parent.Severity / 2f);
			}
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x0006A590 File Offset: 0x00068790
		public override void CompPostInjuryHeal(float amount)
		{
			if (this.permanentDamageThreshold >= 9999f || this.IsPermanent)
			{
				return;
			}
			if (this.parent.Severity <= this.permanentDamageThreshold && this.parent.Severity >= this.permanentDamageThreshold - amount)
			{
				this.parent.Severity = this.permanentDamageThreshold;
				this.IsPermanent = true;
				base.Pawn.health.Notify_HediffChanged(this.parent);
			}
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x0006A60C File Offset: 0x0006880C
		public override string CompDebugString()
		{
			return string.Concat(new object[]
			{
				"isPermanent: ",
				this.isPermanentInt.ToString(),
				"\npermanentDamageThreshold: ",
				this.permanentDamageThreshold,
				"\npainCategory: ",
				this.painCategory
			});
		}

		// Token: 0x04000DF5 RID: 3573
		public float permanentDamageThreshold = 9999f;

		// Token: 0x04000DF6 RID: 3574
		public bool isPermanentInt;

		// Token: 0x04000DF7 RID: 3575
		private PainCategory painCategory;

		// Token: 0x04000DF8 RID: 3576
		private const float NonActivePermanentDamageThresholdValue = 9999f;
	}
}
