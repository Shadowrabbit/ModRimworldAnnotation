using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AA9 RID: 2729
	public class PreceptDef : Def
	{
		// Token: 0x17000B5E RID: 2910
		// (get) Token: 0x060040D2 RID: 16594 RVA: 0x0015E04C File Offset: 0x0015C24C
		public Texture2D Icon
		{
			get
			{
				if (this.icon == null)
				{
					if (this.iconPath != null)
					{
						this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
					}
					else if (!this.issue.iconPath.NullOrEmpty())
					{
						this.icon = this.issue.Icon;
					}
				}
				return this.icon;
			}
		}

		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x060040D3 RID: 16595 RVA: 0x0015E0AC File Offset: 0x0015C2AC
		public PreceptWorker Worker
		{
			get
			{
				if (this.worker == null)
				{
					this.worker = (PreceptWorker)Activator.CreateInstance(this.workerClass);
					this.worker.def = this;
				}
				return this.worker;
			}
		}

		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x060040D4 RID: 16596 RVA: 0x0015E0E0 File Offset: 0x0015C2E0
		public List<TraitRequirement> TraitsAffecting
		{
			get
			{
				if (this.traitsAffectingCached == null)
				{
					this.traitsAffectingCached = new List<TraitRequirement>();
					for (int i = 0; i < this.comps.Count; i++)
					{
						this.traitsAffectingCached.AddRange(this.comps[i].TraitsAffecting);
					}
					this.traitsAffectingCached.RemoveDuplicates((TraitRequirement a, TraitRequirement b) => a.def == b.def && ((a.degree != null) ? a.degree.Value : 0) == ((b.degree != null) ? b.degree.Value : 0));
				}
				return this.traitsAffectingCached;
			}
		}

		// Token: 0x17000B61 RID: 2913
		// (get) Token: 0x060040D5 RID: 16597 RVA: 0x0015E164 File Offset: 0x0015C364
		public List<string> RequiredMemeLabels
		{
			get
			{
				if (this.requiredMemeLabels == null && !this.requiredMemes.NullOrEmpty<MemeDef>())
				{
					this.requiredMemeLabels = new List<string>();
					foreach (MemeDef memeDef in this.requiredMemes)
					{
						this.requiredMemeLabels.Add(memeDef.label);
					}
				}
				return this.requiredMemeLabels;
			}
		}

		// Token: 0x060040D6 RID: 16598 RVA: 0x0015E1E8 File Offset: 0x0015C3E8
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.preceptClass == typeof(Precept_Ritual) && this.ritualPatternBase == null)
			{
				yield return "Ritual doesn't define a ritualPatternBase, was it meant to be abstract?";
			}
			if (typeof(Precept_ThingStyle).IsAssignableFrom(this.preceptClass))
			{
				foreach (PreceptThingChance preceptThingChance in this.Worker.ThingDefs)
				{
					ThingDef def = preceptThingChance.def;
					if (!def.CanBeStyled())
					{
						yield return string.Concat(new string[]
						{
							"ThingDef ",
							def.defName,
							" is on available things list of ",
							this.defName,
							" precept, but missing CompStyleable thing comp!"
						});
					}
				}
				IEnumerator<PreceptThingChance> enumerator2 = null;
			}
			foreach (PreceptComp preceptComp in this.comps)
			{
				foreach (string text2 in preceptComp.ConfigErrors(this))
				{
					yield return text2;
				}
				enumerator = null;
			}
			List<PreceptComp>.Enumerator enumerator3 = default(List<PreceptComp>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x040025BE RID: 9662
		public Type preceptClass = typeof(Precept);

		// Token: 0x040025BF RID: 9663
		public IssueDef issue;

		// Token: 0x040025C0 RID: 9664
		public List<PreceptComp> comps = new List<PreceptComp>();

		// Token: 0x040025C1 RID: 9665
		public List<AbilityStatModifiers> abilityStatFactors;

		// Token: 0x040025C2 RID: 9666
		public List<StatModifier> statOffsets;

		// Token: 0x040025C3 RID: 9667
		public List<StatModifier> statFactors;

		// Token: 0x040025C4 RID: 9668
		public float selectionWeight = 1f;

		// Token: 0x040025C5 RID: 9669
		public List<WorkTypeDef> opposedWorkTypes = new List<WorkTypeDef>();

		// Token: 0x040025C6 RID: 9670
		public PreceptImpact impact;

		// Token: 0x040025C7 RID: 9671
		public List<MemeDef> associatedMemes = new List<MemeDef>();

		// Token: 0x040025C8 RID: 9672
		public List<MemeDef> conflictingMemes = new List<MemeDef>();

		// Token: 0x040025C9 RID: 9673
		public List<MemeDef> requiredMemes = new List<MemeDef>();

		// Token: 0x040025CA RID: 9674
		public bool visible = true;

		// Token: 0x040025CB RID: 9675
		public bool listedForRoles = true;

		// Token: 0x040025CC RID: 9676
		public PreceptDef takeNameFrom;

		// Token: 0x040025CD RID: 9677
		public PreceptDef alsoAdds;

		// Token: 0x040025CE RID: 9678
		public int maxCount = 1;

		// Token: 0x040025CF RID: 9679
		[NoTranslate]
		public List<string> exclusionTags = new List<string>();

		// Token: 0x040025D0 RID: 9680
		public bool allowDuplicates;

		// Token: 0x040025D1 RID: 9681
		public bool ignoreLimitsInEditMode;

		// Token: 0x040025D2 RID: 9682
		public bool canUseAlreadyUsedThingDef;

		// Token: 0x040025D3 RID: 9683
		public bool classic;

		// Token: 0x040025D4 RID: 9684
		public float defaultSelectionWeight;

		// Token: 0x040025D5 RID: 9685
		public bool allowedForNPCFactions = true;

		// Token: 0x040025D6 RID: 9686
		public bool countsTowardsPreceptLimit = true;

		// Token: 0x040025D7 RID: 9687
		public bool canGenerateAsSpecialPrecept = true;

		// Token: 0x040025D8 RID: 9688
		public RulePackDef nameMaker;

		// Token: 0x040025D9 RID: 9689
		public int nameMaxLength = 16;

		// Token: 0x040025DA RID: 9690
		public SimpleCurve preceptInstanceCountCurve;

		// Token: 0x040025DB RID: 9691
		public RitualPatternDef ritualPatternBase;

		// Token: 0x040025DC RID: 9692
		public bool receivesExpectationsQualityOffset;

		// Token: 0x040025DD RID: 9693
		public bool usesIdeoVisualEffects = true;

		// Token: 0x040025DE RID: 9694
		public List<PreceptThingChanceClass> buildingDefChances;

		// Token: 0x040025DF RID: 9695
		public List<ExpectationDef> buildingMinExpectations;

		// Token: 0x040025E0 RID: 9696
		public List<RoomRequirement> buildingRoomRequirements;

		// Token: 0x040025E1 RID: 9697
		public List<RoomRequirement> buildingRoomRequirementsFixed;

		// Token: 0x040025E2 RID: 9698
		public SimpleCurve roomRequirementCountCurve;

		// Token: 0x040025E3 RID: 9699
		public bool leaderRole;

		// Token: 0x040025E4 RID: 9700
		public int activationBelieverCount = -1;

		// Token: 0x040025E5 RID: 9701
		public int deactivationBelieverCount = -1;

		// Token: 0x040025E6 RID: 9702
		public List<RoleRequirement> roleRequirements;

		// Token: 0x040025E7 RID: 9703
		public WorkTags roleDisabledWorkTags;

		// Token: 0x040025E8 RID: 9704
		public WorkTags roleRequiredWorkTags;

		// Token: 0x040025E9 RID: 9705
		public WorkTags roleRequiredWorkTagAny;

		// Token: 0x040025EA RID: 9706
		public List<PreceptApparelRequirement> roleApparelRequirements;

		// Token: 0x040025EB RID: 9707
		public SimpleCurve roleApparelRequirementCountCurve;

		// Token: 0x040025EC RID: 9708
		public List<AbilityDef> grantedAbilities;

		// Token: 0x040025ED RID: 9709
		public List<RoleEffect> roleEffects;

		// Token: 0x040025EE RID: 9710
		public List<string> roleTags;

		// Token: 0x040025EF RID: 9711
		public string iconPath;

		// Token: 0x040025F0 RID: 9712
		public float restrictToSupremeGenderChance;

		// Token: 0x040025F1 RID: 9713
		public float certaintyLossFactor = 1f;

		// Token: 0x040025F2 RID: 9714
		public float convertPowerFactor = 1f;

		// Token: 0x040025F3 RID: 9715
		public int expectationsOffset;

		// Token: 0x040025F4 RID: 9716
		public bool createsRoleEmptyThought = true;

		// Token: 0x040025F5 RID: 9717
		public bool disallowLoggingCamps;

		// Token: 0x040025F6 RID: 9718
		public bool disallowMiningCamps;

		// Token: 0x040025F7 RID: 9719
		public bool disallowHuntingCamps;

		// Token: 0x040025F8 RID: 9720
		public bool disallowFarmingCamps;

		// Token: 0x040025F9 RID: 9721
		public bool approvesOfSlavery;

		// Token: 0x040025FA RID: 9722
		public bool prefersNudity;

		// Token: 0x040025FB RID: 9723
		public Gender genderPrefersNudity;

		// Token: 0x040025FC RID: 9724
		public bool useChoicesFromBuildingDefs;

		// Token: 0x040025FD RID: 9725
		public int displayOrderInImpact;

		// Token: 0x040025FE RID: 9726
		public int displayOrderInIssue;

		// Token: 0x040025FF RID: 9727
		public bool proselytizes;

		// Token: 0x04002600 RID: 9728
		public int requiredScars;

		// Token: 0x04002601 RID: 9729
		public bool approvesOfCharity;

		// Token: 0x04002602 RID: 9730
		public float blindPawnChance = -1f;

		// Token: 0x04002603 RID: 9731
		public bool approvesOfBlindness;

		// Token: 0x04002604 RID: 9732
		public bool canRemoveInUI = true;

		// Token: 0x04002605 RID: 9733
		public bool prefersDarkness;

		// Token: 0x04002606 RID: 9734
		public bool disableCrampedRoomThoughts;

		// Token: 0x04002607 RID: 9735
		public float blindPsychicSensitivityOffset;

		// Token: 0x04002608 RID: 9736
		public DrugPolicyDef defaultDrugPolicyOverride;

		// Token: 0x04002609 RID: 9737
		public bool warnPlayerOnDesignateChopTree;

		// Token: 0x0400260A RID: 9738
		public bool warnPlayerOnDesignateMine;

		// Token: 0x0400260B RID: 9739
		public bool willingToConstructOtherIdeoBuildables;

		// Token: 0x0400260C RID: 9740
		public float biosculpterPodCycleSpeedFactor = 1f;

		// Token: 0x0400260D RID: 9741
		public bool prefersSlabBed;

		// Token: 0x0400260E RID: 9742
		public bool useRepeatPenalty = true;

		// Token: 0x0400260F RID: 9743
		[MustTranslate]
		public string tipLabelOverride;

		// Token: 0x04002610 RID: 9744
		public bool capitalizeAsTitle = true;

		// Token: 0x04002611 RID: 9745
		public bool ignoreNameUniqueness;

		// Token: 0x04002612 RID: 9746
		[MustTranslate]
		public string extraTextPawnDeathLetter;

		// Token: 0x04002613 RID: 9747
		public PreceptDef apparelPreceptSwapDef;

		// Token: 0x04002614 RID: 9748
		public Type workerClass = typeof(PreceptWorker);

		// Token: 0x04002615 RID: 9749
		private PreceptWorker worker;

		// Token: 0x04002616 RID: 9750
		[Unsaved(false)]
		private List<TraitRequirement> traitsAffectingCached;

		// Token: 0x04002617 RID: 9751
		[Unsaved(false)]
		private List<string> requiredMemeLabels;

		// Token: 0x04002618 RID: 9752
		private Texture2D icon;
	}
}
