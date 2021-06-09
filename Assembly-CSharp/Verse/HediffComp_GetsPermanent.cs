using System;

namespace Verse
{
	// Token: 0x020003DA RID: 986
	public class HediffComp_GetsPermanent : HediffComp
	{
		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06001842 RID: 6210 RVA: 0x0001710E File Offset: 0x0001530E
		public HediffCompProperties_GetsPermanent Props
		{
			get
			{
				return (HediffCompProperties_GetsPermanent)this.props;
			}
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06001843 RID: 6211 RVA: 0x0001711B File Offset: 0x0001531B
		// (set) Token: 0x06001844 RID: 6212 RVA: 0x000DEC6C File Offset: 0x000DCE6C
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
					this.painCategory = HealthTuning.InjuryPainCategories.RandomElementByWeight((HealthTuning.PainCategoryWeighted cat) => cat.weight).category;
					this.permanentDamageThreshold = 9999f;
				}
			}
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06001845 RID: 6213 RVA: 0x00017123 File Offset: 0x00015323
		public PainCategory PainCategory
		{
			get
			{
				return this.painCategory;
			}
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06001846 RID: 6214 RVA: 0x0001712B File Offset: 0x0001532B
		public float PainFactor
		{
			get
			{
				return (float)this.painCategory;
			}
		}

		// Token: 0x06001847 RID: 6215 RVA: 0x000DECD4 File Offset: 0x000DCED4
		public override void CompExposeData()
		{
			Scribe_Values.Look<bool>(ref this.isPermanentInt, "isPermanent", false, false);
			Scribe_Values.Look<float>(ref this.permanentDamageThreshold, "permanentDamageThreshold", 9999f, false);
			Scribe_Values.Look<PainCategory>(ref this.painCategory, "painCategory", PainCategory.Painless, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06001848 RID: 6216 RVA: 0x000DED24 File Offset: 0x000DCF24
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

		// Token: 0x06001849 RID: 6217 RVA: 0x000DEDF4 File Offset: 0x000DCFF4
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

		// Token: 0x0600184A RID: 6218 RVA: 0x000DEE70 File Offset: 0x000DD070
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

		// Token: 0x04001266 RID: 4710
		public float permanentDamageThreshold = 9999f;

		// Token: 0x04001267 RID: 4711
		public bool isPermanentInt;

		// Token: 0x04001268 RID: 4712
		private PainCategory painCategory;

		// Token: 0x04001269 RID: 4713
		private const float NonActivePermanentDamageThresholdValue = 9999f;
	}
}
