using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ADF RID: 2783
	public class TraitDegreeData
	{
		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x06004192 RID: 16786 RVA: 0x0015FD4B File Offset: 0x0015DF4B
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

		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x06004193 RID: 16787 RVA: 0x0015FD6C File Offset: 0x0015DF6C
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

		// Token: 0x06004194 RID: 16788 RVA: 0x0015FD9E File Offset: 0x0015DF9E
		public string GetLabelFor(Pawn pawn)
		{
			return this.GetLabelFor((pawn != null) ? pawn.gender : Gender.None);
		}

		// Token: 0x06004195 RID: 16789 RVA: 0x0015FDB2 File Offset: 0x0015DFB2
		public string GetLabelCapFor(Pawn pawn)
		{
			return this.GetLabelCapFor((pawn != null) ? pawn.gender : Gender.None);
		}

		// Token: 0x06004196 RID: 16790 RVA: 0x0015FDC8 File Offset: 0x0015DFC8
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

		// Token: 0x06004197 RID: 16791 RVA: 0x0015FE1C File Offset: 0x0015E01C
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

		// Token: 0x06004198 RID: 16792 RVA: 0x0015FEA4 File Offset: 0x0015E0A4
		public List<IssueDef> GetAffectedIssues(TraitDef def)
		{
			if (this.affectedIssuesCached == null)
			{
				this.affectedIssuesCached = new List<IssueDef>();
				List<PreceptDef> allDefsListForReading = DefDatabase<PreceptDef>.AllDefsListForReading;
				Predicate<TraitRequirement> <>9__0;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (!this.affectedIssuesCached.Contains(allDefsListForReading[i].issue))
					{
						List<TraitRequirement> traitsAffecting = allDefsListForReading[i].TraitsAffecting;
						Predicate<TraitRequirement> predicate;
						if ((predicate = <>9__0) == null)
						{
							predicate = (<>9__0 = ((TraitRequirement x) => x.def == def && ((x.degree != null) ? x.degree.Value : 0) == this.degree));
						}
						if (traitsAffecting.Any(predicate))
						{
							this.affectedIssuesCached.Add(allDefsListForReading[i].issue);
						}
					}
				}
			}
			return this.affectedIssuesCached;
		}

		// Token: 0x06004199 RID: 16793 RVA: 0x0015FF5A File Offset: 0x0015E15A
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x040027A1 RID: 10145
		[MustTranslate]
		public string label;

		// Token: 0x040027A2 RID: 10146
		[MustTranslate]
		public string labelMale;

		// Token: 0x040027A3 RID: 10147
		[MustTranslate]
		public string labelFemale;

		// Token: 0x040027A4 RID: 10148
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabel;

		// Token: 0x040027A5 RID: 10149
		[MustTranslate]
		public string description;

		// Token: 0x040027A6 RID: 10150
		public int degree;

		// Token: 0x040027A7 RID: 10151
		public float commonality = 1f;

		// Token: 0x040027A8 RID: 10152
		public List<StatModifier> statOffsets;

		// Token: 0x040027A9 RID: 10153
		public List<StatModifier> statFactors;

		// Token: 0x040027AA RID: 10154
		public ThinkTreeDef thinkTree;

		// Token: 0x040027AB RID: 10155
		public MentalStateDef randomMentalState;

		// Token: 0x040027AC RID: 10156
		public SimpleCurve randomMentalStateMtbDaysMoodCurve;

		// Token: 0x040027AD RID: 10157
		public MentalStateDef forcedMentalState;

		// Token: 0x040027AE RID: 10158
		public float forcedMentalStateMtbDays = -1f;

		// Token: 0x040027AF RID: 10159
		public List<MentalStateDef> disallowedMentalStates;

		// Token: 0x040027B0 RID: 10160
		public List<ThoughtDef> disallowedThoughts;

		// Token: 0x040027B1 RID: 10161
		public List<TraitIngestionThoughtsOverride> disallowedThoughtsFromIngestion;

		// Token: 0x040027B2 RID: 10162
		public List<TraitIngestionThoughtsOverride> extraThoughtsFromIngestion;

		// Token: 0x040027B3 RID: 10163
		public List<InspirationDef> disallowedInspirations;

		// Token: 0x040027B4 RID: 10164
		public List<InspirationDef> mentalBreakInspirationGainSet;

		// Token: 0x040027B5 RID: 10165
		public string mentalBreakInspirationGainReasonText;

		// Token: 0x040027B6 RID: 10166
		public List<MeditationFocusDef> allowedMeditationFocusTypes;

		// Token: 0x040027B7 RID: 10167
		public List<MeditationFocusDef> disallowedMeditationFocusTypes;

		// Token: 0x040027B8 RID: 10168
		public float mentalBreakInspirationGainChance;

		// Token: 0x040027B9 RID: 10169
		public List<MentalBreakDef> theOnlyAllowedMentalBreaks;

		// Token: 0x040027BA RID: 10170
		public Dictionary<SkillDef, int> skillGains = new Dictionary<SkillDef, int>();

		// Token: 0x040027BB RID: 10171
		public float socialFightChanceFactor = 1f;

		// Token: 0x040027BC RID: 10172
		public float marketValueFactorOffset;

		// Token: 0x040027BD RID: 10173
		public float randomDiseaseMtbDays;

		// Token: 0x040027BE RID: 10174
		public float hungerRateFactor = 1f;

		// Token: 0x040027BF RID: 10175
		public Type mentalStateGiverClass = typeof(TraitMentalStateGiver);

		// Token: 0x040027C0 RID: 10176
		public List<AbilityDef> abilities;

		// Token: 0x040027C1 RID: 10177
		public List<NeedDef> needs;

		// Token: 0x040027C2 RID: 10178
		public List<IngestibleModifiers> ingestibleModifiers;

		// Token: 0x040027C3 RID: 10179
		[Unsaved(false)]
		private TraitMentalStateGiver mentalStateGiverInt;

		// Token: 0x040027C4 RID: 10180
		[Unsaved(false)]
		private string cachedLabelCap;

		// Token: 0x040027C5 RID: 10181
		[Unsaved(false)]
		private string cachedLabelMaleCap;

		// Token: 0x040027C6 RID: 10182
		[Unsaved(false)]
		private string cachedLabelFemaleCap;

		// Token: 0x040027C7 RID: 10183
		[Unsaved(false)]
		private List<IssueDef> affectedIssuesCached;
	}
}
