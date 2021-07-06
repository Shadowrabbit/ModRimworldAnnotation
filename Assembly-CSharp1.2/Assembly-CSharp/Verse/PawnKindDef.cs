using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000157 RID: 343
	public class PawnKindDef : Def
	{
		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060008B9 RID: 2233 RVA: 0x0000CE73 File Offset: 0x0000B073
		public RaceProperties RaceProps
		{
			get
			{
				return this.race.race;
			}
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x00097048 File Offset: 0x00095248
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.lifeStages.Count; i++)
			{
				this.lifeStages[i].ResolveReferences();
			}
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0000CE80 File Offset: 0x0000B080
		public string GetLabelPlural(int count = -1)
		{
			if (!this.labelPlural.NullOrEmpty())
			{
				return this.labelPlural;
			}
			return Find.ActiveLanguageWorker.Pluralize(this.label, count);
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x00097084 File Offset: 0x00095284
		public override void PostLoad()
		{
			if (this.backstoryCategories != null && this.backstoryCategories.Count > 0)
			{
				if (this.backstoryFilters == null)
				{
					this.backstoryFilters = new List<BackstoryCategoryFilter>();
				}
				this.backstoryFilters.Add(new BackstoryCategoryFilter
				{
					categories = this.backstoryCategories
				});
			}
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x000970D8 File Offset: 0x000952D8
		public float GetAnimalPointsToHuntOrSlaughter()
		{
			return this.combatPower * 5f * (1f + this.RaceProps.manhunterOnDamageChance * 0.5f) * (1f + this.RaceProps.manhunterOnTameFailChance * 0.2f) * (1f + this.RaceProps.wildness) + this.race.BaseMarketValue;
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x0000CEA7 File Offset: 0x0000B0A7
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.backstoryFilters != null && this.backstoryFiltersOverride != null)
			{
				yield return "both backstoryCategories and backstoryCategoriesOverride are defined";
			}
			if (this.race == null)
			{
				yield return "no race";
			}
			if (this.baseRecruitDifficulty > 1.0001f)
			{
				yield return this.defName + " recruitDifficulty is greater than 1. 1 means impossible to recruit.";
			}
			if (this.combatPower < 0f)
			{
				yield return this.defName + " has no combatPower.";
			}
			if (this.weaponMoney != FloatRange.Zero)
			{
				float num = 999999f;
				int num2;
				int i;
				for (i = 0; i < this.weaponTags.Count; i = num2 + 1)
				{
					IEnumerable<ThingDef> source = from d in DefDatabase<ThingDef>.AllDefs
					where d.weaponTags != null && d.weaponTags.Contains(this.weaponTags[i])
					select d;
					if (source.Any<ThingDef>())
					{
						num = Mathf.Min(num, source.Min((ThingDef d) => PawnWeaponGenerator.CheapestNonDerpPriceFor(d)));
					}
					num2 = i;
				}
				if (num < 999999f && num > this.weaponMoney.min)
				{
					yield return string.Concat(new object[]
					{
						"Cheapest weapon with one of my weaponTags costs ",
						num,
						" but weaponMoney min is ",
						this.weaponMoney.min,
						", so could end up weaponless."
					});
				}
			}
			if (!this.RaceProps.Humanlike && this.lifeStages.Count != this.RaceProps.lifeStageAges.Count)
			{
				yield return string.Concat(new object[]
				{
					"PawnKindDef defines ",
					this.lifeStages.Count,
					" lifeStages while race def defines ",
					this.RaceProps.lifeStageAges.Count
				});
			}
			if (this.apparelRequired != null)
			{
				int num2;
				for (int k = 0; k < this.apparelRequired.Count; k = num2 + 1)
				{
					for (int j = k + 1; j < this.apparelRequired.Count; j = num2 + 1)
					{
						if (!ApparelUtility.CanWearTogether(this.apparelRequired[k], this.apparelRequired[j], this.race.race.body))
						{
							yield return string.Concat(new object[]
							{
								"required apparel can't be worn together (",
								this.apparelRequired[k],
								", ",
								this.apparelRequired[j],
								")"
							});
						}
						num2 = j;
					}
					num2 = k;
				}
			}
			if (this.alternateGraphics != null)
			{
				using (List<AlternateGraphic>.Enumerator enumerator2 = this.alternateGraphics.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.Weight < 0f)
						{
							yield return "alternate graphic has negative weight.";
						}
					}
				}
				List<AlternateGraphic>.Enumerator enumerator2 = default(List<AlternateGraphic>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x0000CEB7 File Offset: 0x0000B0B7
		public static PawnKindDef Named(string defName)
		{
			return DefDatabase<PawnKindDef>.GetNamed(defName, true);
		}

		// Token: 0x04000718 RID: 1816
		public ThingDef race;

		// Token: 0x04000719 RID: 1817
		public FactionDef defaultFactionType;

		// Token: 0x0400071A RID: 1818
		[NoTranslate]
		public List<BackstoryCategoryFilter> backstoryFilters;

		// Token: 0x0400071B RID: 1819
		[NoTranslate]
		public List<BackstoryCategoryFilter> backstoryFiltersOverride;

		// Token: 0x0400071C RID: 1820
		[NoTranslate]
		public List<string> backstoryCategories;

		// Token: 0x0400071D RID: 1821
		[MustTranslate]
		public string labelPlural;

		// Token: 0x0400071E RID: 1822
		public List<PawnKindLifeStage> lifeStages = new List<PawnKindLifeStage>();

		// Token: 0x0400071F RID: 1823
		public List<AlternateGraphic> alternateGraphics;

		// Token: 0x04000720 RID: 1824
		public List<TraitDef> disallowedTraits;

		// Token: 0x04000721 RID: 1825
		public float alternateGraphicChance;

		// Token: 0x04000722 RID: 1826
		public float backstoryCryptosleepCommonality;

		// Token: 0x04000723 RID: 1827
		public int minGenerationAge;

		// Token: 0x04000724 RID: 1828
		public int maxGenerationAge = 999999;

		// Token: 0x04000725 RID: 1829
		public bool factionLeader;

		// Token: 0x04000726 RID: 1830
		public bool destroyGearOnDrop;

		// Token: 0x04000727 RID: 1831
		public float defendPointRadius = -1f;

		// Token: 0x04000728 RID: 1832
		public bool factionHostileOnKill;

		// Token: 0x04000729 RID: 1833
		public bool factionHostileOnDeath;

		// Token: 0x0400072A RID: 1834
		public float royalTitleChance;

		// Token: 0x0400072B RID: 1835
		public RoyalTitleDef titleRequired;

		// Token: 0x0400072C RID: 1836
		public RoyalTitleDef minTitleRequired;

		// Token: 0x0400072D RID: 1837
		public List<RoyalTitleDef> titleSelectOne;

		// Token: 0x0400072E RID: 1838
		public bool allowRoyalRoomRequirements = true;

		// Token: 0x0400072F RID: 1839
		public bool allowRoyalApparelRequirements = true;

		// Token: 0x04000730 RID: 1840
		public bool isFighter = true;

		// Token: 0x04000731 RID: 1841
		public float combatPower = -1f;

		// Token: 0x04000732 RID: 1842
		public bool canArriveManhunter = true;

		// Token: 0x04000733 RID: 1843
		public bool canBeSapper;

		// Token: 0x04000734 RID: 1844
		public float baseRecruitDifficulty = 0.5f;

		// Token: 0x04000735 RID: 1845
		public bool aiAvoidCover;

		// Token: 0x04000736 RID: 1846
		public FloatRange fleeHealthThresholdRange = new FloatRange(-0.4f, 0.4f);

		// Token: 0x04000737 RID: 1847
		public QualityCategory itemQuality = QualityCategory.Normal;

		// Token: 0x04000738 RID: 1848
		public bool forceNormalGearQuality;

		// Token: 0x04000739 RID: 1849
		public FloatRange gearHealthRange = FloatRange.One;

		// Token: 0x0400073A RID: 1850
		public FloatRange weaponMoney = FloatRange.Zero;

		// Token: 0x0400073B RID: 1851
		[NoTranslate]
		public List<string> weaponTags;

		// Token: 0x0400073C RID: 1852
		public FloatRange apparelMoney = FloatRange.Zero;

		// Token: 0x0400073D RID: 1853
		public List<ThingDef> apparelRequired;

		// Token: 0x0400073E RID: 1854
		[NoTranslate]
		public List<string> apparelTags;

		// Token: 0x0400073F RID: 1855
		[NoTranslate]
		public List<string> apparelDisallowTags;

		// Token: 0x04000740 RID: 1856
		[NoTranslate]
		public List<string> hairTags;

		// Token: 0x04000741 RID: 1857
		public float apparelAllowHeadgearChance = 1f;

		// Token: 0x04000742 RID: 1858
		public bool apparelIgnoreSeasons;

		// Token: 0x04000743 RID: 1859
		public Color apparelColor = Color.white;

		// Token: 0x04000744 RID: 1860
		public List<SpecificApparelRequirement> specificApparelRequirements;

		// Token: 0x04000745 RID: 1861
		public List<ThingDef> techHediffsRequired;

		// Token: 0x04000746 RID: 1862
		public FloatRange techHediffsMoney = FloatRange.Zero;

		// Token: 0x04000747 RID: 1863
		[NoTranslate]
		public List<string> techHediffsTags;

		// Token: 0x04000748 RID: 1864
		[NoTranslate]
		public List<string> techHediffsDisallowTags;

		// Token: 0x04000749 RID: 1865
		public float techHediffsChance;

		// Token: 0x0400074A RID: 1866
		public int techHediffsMaxAmount = 1;

		// Token: 0x0400074B RID: 1867
		public float biocodeWeaponChance;

		// Token: 0x0400074C RID: 1868
		public List<ThingDefCountClass> fixedInventory = new List<ThingDefCountClass>();

		// Token: 0x0400074D RID: 1869
		public PawnInventoryOption inventoryOptions;

		// Token: 0x0400074E RID: 1870
		public float invNutrition;

		// Token: 0x0400074F RID: 1871
		public ThingDef invFoodDef;

		// Token: 0x04000750 RID: 1872
		public float chemicalAddictionChance;

		// Token: 0x04000751 RID: 1873
		public float combatEnhancingDrugsChance;

		// Token: 0x04000752 RID: 1874
		public IntRange combatEnhancingDrugsCount = IntRange.zero;

		// Token: 0x04000753 RID: 1875
		public bool trader;

		// Token: 0x04000754 RID: 1876
		public List<SkillRange> skills;

		// Token: 0x04000755 RID: 1877
		public WorkTags requiredWorkTags;

		// Token: 0x04000756 RID: 1878
		[MustTranslate]
		public string labelMale;

		// Token: 0x04000757 RID: 1879
		[MustTranslate]
		public string labelMalePlural;

		// Token: 0x04000758 RID: 1880
		[MustTranslate]
		public string labelFemale;

		// Token: 0x04000759 RID: 1881
		[MustTranslate]
		public string labelFemalePlural;

		// Token: 0x0400075A RID: 1882
		public IntRange wildGroupSize = IntRange.one;

		// Token: 0x0400075B RID: 1883
		public float ecoSystemWeight = 1f;

		// Token: 0x0400075C RID: 1884
		private const int MaxWeaponMoney = 999999;
	}
}
