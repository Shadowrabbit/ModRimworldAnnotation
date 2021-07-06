using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001002 RID: 4098
	public class TraitDegreeData
	{
		// Token: 0x17000DD7 RID: 3543
		// (get) Token: 0x06005968 RID: 22888 RVA: 0x0003E18D File Offset: 0x0003C38D
		public string LabelCap
		{
			get
			{
				if (this.cachedLabelCap == null)
				{
					this.cachedLabelCap = this.label.CapitalizeFirst();
				}
				return this.cachedLabelCap;
			}
		}

		// Token: 0x17000DD8 RID: 3544
		// (get) Token: 0x06005969 RID: 22889 RVA: 0x0003E1AE File Offset: 0x0003C3AE
		public TraitMentalStateGiver MentalStateGiver
		{
			get
			{
				if (this.mentalStateGiverInt == null)
				{
					this.mentalStateGiverInt = (TraitMentalStateGiver)Activator.CreateInstance(this.mentalStateGiverClass);
					this.mentalStateGiverInt.traitDegreeData = this;
				}
				return this.mentalStateGiverInt;
			}
		}

		// Token: 0x0600596A RID: 22890 RVA: 0x0003E1E0 File Offset: 0x0003C3E0
		public string GetLabelFor(Pawn pawn)
		{
			return this.GetLabelFor((pawn != null) ? pawn.gender : Gender.None);
		}

		// Token: 0x0600596B RID: 22891 RVA: 0x0003E1F4 File Offset: 0x0003C3F4
		public string GetLabelCapFor(Pawn pawn)
		{
			return this.GetLabelCapFor((pawn != null) ? pawn.gender : Gender.None);
		}

		// Token: 0x0600596C RID: 22892 RVA: 0x001D23E4 File Offset: 0x001D05E4
		public string GetLabelFor(Gender gender)
		{
			if (gender != Gender.Male)
			{
				if (gender != Gender.Female)
				{
					return this.label;
				}
				if (!this.labelFemale.NullOrEmpty())
				{
					return this.labelFemale;
				}
				return this.label;
			}
			else
			{
				if (!this.labelMale.NullOrEmpty())
				{
					return this.labelMale;
				}
				return this.label;
			}
		}

		// Token: 0x0600596D RID: 22893 RVA: 0x001D2438 File Offset: 0x001D0638
		public string GetLabelCapFor(Gender gender)
		{
			if (gender != Gender.Male)
			{
				if (gender != Gender.Female)
				{
					return this.LabelCap;
				}
				if (this.labelFemale.NullOrEmpty())
				{
					return this.LabelCap;
				}
				if (this.cachedLabelFemaleCap == null)
				{
					this.cachedLabelFemaleCap = this.labelFemale.CapitalizeFirst();
				}
				return this.cachedLabelFemaleCap;
			}
			else
			{
				if (this.labelMale.NullOrEmpty())
				{
					return this.LabelCap;
				}
				if (this.cachedLabelMaleCap == null)
				{
					this.cachedLabelMaleCap = this.labelMale.CapitalizeFirst();
				}
				return this.cachedLabelMaleCap;
			}
		}

		// Token: 0x0600596E RID: 22894 RVA: 0x0003E208 File Offset: 0x0003C408
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x04003BF8 RID: 15352
		[MustTranslate]
		public string label;

		// Token: 0x04003BF9 RID: 15353
		[MustTranslate]
		public string labelMale;

		// Token: 0x04003BFA RID: 15354
		[MustTranslate]
		public string labelFemale;

		// Token: 0x04003BFB RID: 15355
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabel;

		// Token: 0x04003BFC RID: 15356
		[MustTranslate]
		public string description;

		// Token: 0x04003BFD RID: 15357
		public int degree;

		// Token: 0x04003BFE RID: 15358
		public float commonality = 1f;

		// Token: 0x04003BFF RID: 15359
		public List<StatModifier> statOffsets;

		// Token: 0x04003C00 RID: 15360
		public List<StatModifier> statFactors;

		// Token: 0x04003C01 RID: 15361
		public ThinkTreeDef thinkTree;

		// Token: 0x04003C02 RID: 15362
		public MentalStateDef randomMentalState;

		// Token: 0x04003C03 RID: 15363
		public SimpleCurve randomMentalStateMtbDaysMoodCurve;

		// Token: 0x04003C04 RID: 15364
		public List<MentalStateDef> disallowedMentalStates;

		// Token: 0x04003C05 RID: 15365
		public List<InspirationDef> disallowedInspirations;

		// Token: 0x04003C06 RID: 15366
		public List<InspirationDef> mentalBreakInspirationGainSet;

		// Token: 0x04003C07 RID: 15367
		public string mentalBreakInspirationGainReasonText;

		// Token: 0x04003C08 RID: 15368
		public List<MeditationFocusDef> allowedMeditationFocusTypes;

		// Token: 0x04003C09 RID: 15369
		public List<MeditationFocusDef> disallowedMeditationFocusTypes;

		// Token: 0x04003C0A RID: 15370
		public float mentalBreakInspirationGainChance;

		// Token: 0x04003C0B RID: 15371
		public List<MentalBreakDef> theOnlyAllowedMentalBreaks;

		// Token: 0x04003C0C RID: 15372
		public Dictionary<SkillDef, int> skillGains = new Dictionary<SkillDef, int>();

		// Token: 0x04003C0D RID: 15373
		public float socialFightChanceFactor = 1f;

		// Token: 0x04003C0E RID: 15374
		public float marketValueFactorOffset;

		// Token: 0x04003C0F RID: 15375
		public float randomDiseaseMtbDays;

		// Token: 0x04003C10 RID: 15376
		public float hungerRateFactor = 1f;

		// Token: 0x04003C11 RID: 15377
		public Type mentalStateGiverClass = typeof(TraitMentalStateGiver);

		// Token: 0x04003C12 RID: 15378
		[Unsaved(false)]
		private TraitMentalStateGiver mentalStateGiverInt;

		// Token: 0x04003C13 RID: 15379
		[Unsaved(false)]
		private string cachedLabelCap;

		// Token: 0x04003C14 RID: 15380
		[Unsaved(false)]
		private string cachedLabelMaleCap;

		// Token: 0x04003C15 RID: 15381
		[Unsaved(false)]
		private string cachedLabelFemaleCap;
	}
}
