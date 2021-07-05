using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A29 RID: 2601
	public class IngestibleProperties
	{
		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x06003F22 RID: 16162 RVA: 0x001584C6 File Offset: 0x001566C6
		public JoyKindDef JoyKind
		{
			get
			{
				if (this.joyKind == null)
				{
					return JoyKindDefOf.Gluttonous;
				}
				return this.joyKind;
			}
		}

		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x06003F23 RID: 16163 RVA: 0x001584DC File Offset: 0x001566DC
		public bool HumanEdible
		{
			get
			{
				return (FoodTypeFlags.OmnivoreHuman & this.foodType) > FoodTypeFlags.None;
			}
		}

		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x06003F24 RID: 16164 RVA: 0x001584ED File Offset: 0x001566ED
		public bool IsMeal
		{
			get
			{
				return this.preferability >= FoodPreferability.MealAwful && this.preferability <= FoodPreferability.MealLavish;
			}
		}

		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x06003F25 RID: 16165 RVA: 0x00158507 File Offset: 0x00156707
		public float CachedNutrition
		{
			get
			{
				if (this.cachedNutrition == -1f)
				{
					this.cachedNutrition = this.parent.GetStatValueAbstract(StatDefOf.Nutrition, null);
				}
				return this.cachedNutrition;
			}
		}

		// Token: 0x06003F26 RID: 16166 RVA: 0x00158533 File Offset: 0x00156733
		public IEnumerable<string> ConfigErrors()
		{
			if (this.preferability == FoodPreferability.Undefined)
			{
				yield return "undefined preferability";
			}
			if (this.foodType == FoodTypeFlags.None)
			{
				yield return "no foodType";
			}
			if (this.parent.GetStatValueAbstract(StatDefOf.Nutrition, null) == 0f && this.preferability != FoodPreferability.NeverForNutrition)
			{
				yield return string.Concat(new object[]
				{
					"Nutrition == 0 but preferability is ",
					this.preferability,
					" instead of ",
					FoodPreferability.NeverForNutrition
				});
			}
			if (!this.parent.IsCorpse && this.preferability > FoodPreferability.DesperateOnlyForHumanlikes && !this.parent.socialPropernessMatters && this.parent.EverHaulable)
			{
				yield return "ingestible preferability > DesperateOnlyForHumanlikes but socialPropernessMatters=false. This will cause bugs wherein wardens will look in prison cells for food to give to prisoners and so will repeatedly pick up and drop food inside the cell.";
			}
			if (this.joy > 0f && this.joyKind == null)
			{
				yield return "joy > 0 with no joy kind";
			}
			if (this.joy == 0f && this.joyKind != null)
			{
				yield return "joy is 0 but joyKind is " + this.joyKind;
			}
			yield break;
		}

		// Token: 0x06003F27 RID: 16167 RVA: 0x00158544 File Offset: 0x00156744
		public RoyalTitleDef MaxSatisfiedTitle()
		{
			return (from t in DefDatabase<FactionDef>.AllDefsListForReading.SelectMany((FactionDef f) => f.RoyalTitlesAwardableInSeniorityOrderForReading)
			where t.foodRequirement.Defined && t.foodRequirement.Acceptable(this.parent)
			orderby t.seniority descending
			select t).FirstOrDefault<RoyalTitleDef>();
		}

		// Token: 0x06003F28 RID: 16168 RVA: 0x001585B4 File Offset: 0x001567B4
		internal IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.joy > 0f)
			{
				StatCategoryDef category = (this.drugCategory != DrugCategory.None) ? StatCategoryDefOf.Drug : StatCategoryDefOf.Basics;
				yield return new StatDrawEntry(category, "Joy".Translate(), this.joy.ToStringPercent("F0") + " (" + this.JoyKind.label + ")", "Stat_Thing_Ingestible_Joy_Desc".Translate(), 4751, null, null, false);
			}
			if (this.HumanEdible)
			{
				RoyalTitleDef royalTitleDef = this.MaxSatisfiedTitle();
				if (royalTitleDef != null)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Stat_Thing_Ingestible_MaxSatisfiedTitle".Translate(), royalTitleDef.GetLabelCapForBothGenders(), "Stat_Thing_Ingestible_MaxSatisfiedTitle_Desc".Translate(), 4752, null, null, false);
				}
			}
			if (this.drugCategory != DrugCategory.None)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Drug, "DrugCategory".Translate().CapitalizeFirst(), this.drugCategory.GetLabel().CapitalizeFirst(), "Stat_Thing_Drug_Category_Desc".Translate(), 2485, null, null, false);
			}
			if (this.outcomeDoers != null)
			{
				int num;
				for (int i = 0; i < this.outcomeDoers.Count; i = num + 1)
				{
					foreach (StatDrawEntry statDrawEntry in this.outcomeDoers[i].SpecialDisplayStats(this.parent))
					{
						yield return statDrawEntry;
					}
					IEnumerator<StatDrawEntry> enumerator = null;
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x04002272 RID: 8818
		[Unsaved(false)]
		public ThingDef parent;

		// Token: 0x04002273 RID: 8819
		public int maxNumToIngestAtOnce = 20;

		// Token: 0x04002274 RID: 8820
		public List<IngestionOutcomeDoer> outcomeDoers;

		// Token: 0x04002275 RID: 8821
		public int baseIngestTicks = 500;

		// Token: 0x04002276 RID: 8822
		public float chairSearchRadius = 32f;

		// Token: 0x04002277 RID: 8823
		public bool useEatingSpeedStat = true;

		// Token: 0x04002278 RID: 8824
		public ThoughtDef tasteThought;

		// Token: 0x04002279 RID: 8825
		public ThoughtDef specialThoughtDirect;

		// Token: 0x0400227A RID: 8826
		public ThoughtDef specialThoughtAsIngredient;

		// Token: 0x0400227B RID: 8827
		public HistoryEventDef ateEvent;

		// Token: 0x0400227C RID: 8828
		public EffecterDef ingestEffect;

		// Token: 0x0400227D RID: 8829
		public EffecterDef ingestEffectEat;

		// Token: 0x0400227E RID: 8830
		public SoundDef ingestSound;

		// Token: 0x0400227F RID: 8831
		[MustTranslate]
		public string ingestCommandString;

		// Token: 0x04002280 RID: 8832
		[MustTranslate]
		public string ingestReportString;

		// Token: 0x04002281 RID: 8833
		[MustTranslate]
		public string ingestReportStringEat;

		// Token: 0x04002282 RID: 8834
		public HoldOffsetSet ingestHoldOffsetStanding;

		// Token: 0x04002283 RID: 8835
		public bool ingestHoldUsesTable = true;

		// Token: 0x04002284 RID: 8836
		public bool tableDesired = true;

		// Token: 0x04002285 RID: 8837
		public FoodTypeFlags foodType;

		// Token: 0x04002286 RID: 8838
		public float joy;

		// Token: 0x04002287 RID: 8839
		public JoyKindDef joyKind;

		// Token: 0x04002288 RID: 8840
		public ThingDef sourceDef;

		// Token: 0x04002289 RID: 8841
		public FoodPreferability preferability;

		// Token: 0x0400228A RID: 8842
		public bool nurseable;

		// Token: 0x0400228B RID: 8843
		public float optimalityOffsetHumanlikes;

		// Token: 0x0400228C RID: 8844
		public float optimalityOffsetFeedingAnimals;

		// Token: 0x0400228D RID: 8845
		public DrugCategory drugCategory;

		// Token: 0x0400228E RID: 8846
		public bool canAutoSelectAsFoodForCaravan = true;

		// Token: 0x0400228F RID: 8847
		[Unsaved(false)]
		private float cachedNutrition = -1f;
	}
}
