using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F39 RID: 3897
	public class IngestibleProperties
	{
		// Token: 0x17000D0E RID: 3342
		// (get) Token: 0x060055AA RID: 21930 RVA: 0x0003B851 File Offset: 0x00039A51
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

		// Token: 0x17000D0F RID: 3343
		// (get) Token: 0x060055AB RID: 21931 RVA: 0x0003B867 File Offset: 0x00039A67
		public bool HumanEdible
		{
			get
			{
				return (FoodTypeFlags.OmnivoreHuman & this.foodType) > FoodTypeFlags.None;
			}
		}

		// Token: 0x17000D10 RID: 3344
		// (get) Token: 0x060055AC RID: 21932 RVA: 0x0003B878 File Offset: 0x00039A78
		public bool IsMeal
		{
			get
			{
				return this.preferability >= FoodPreferability.MealAwful && this.preferability <= FoodPreferability.MealLavish;
			}
		}

		// Token: 0x17000D11 RID: 3345
		// (get) Token: 0x060055AD RID: 21933 RVA: 0x0003B892 File Offset: 0x00039A92
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

		// Token: 0x060055AE RID: 21934 RVA: 0x0003B8BE File Offset: 0x00039ABE
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

		// Token: 0x060055AF RID: 21935 RVA: 0x001C8C14 File Offset: 0x001C6E14
		public RoyalTitleDef MaxSatisfiedTitle()
		{
			return (from t in DefDatabase<FactionDef>.AllDefsListForReading.SelectMany((FactionDef f) => f.RoyalTitlesAwardableInSeniorityOrderForReading)
			where t.foodRequirement.Defined && t.foodRequirement.Acceptable(this.parent)
			orderby t.seniority descending
			select t).FirstOrDefault<RoyalTitleDef>();
		}

		// Token: 0x060055B0 RID: 21936 RVA: 0x0003B8CE File Offset: 0x00039ACE
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

		// Token: 0x0400370F RID: 14095
		[Unsaved(false)]
		public ThingDef parent;

		// Token: 0x04003710 RID: 14096
		public int maxNumToIngestAtOnce = 20;

		// Token: 0x04003711 RID: 14097
		public List<IngestionOutcomeDoer> outcomeDoers;

		// Token: 0x04003712 RID: 14098
		public int baseIngestTicks = 500;

		// Token: 0x04003713 RID: 14099
		public float chairSearchRadius = 32f;

		// Token: 0x04003714 RID: 14100
		public bool useEatingSpeedStat = true;

		// Token: 0x04003715 RID: 14101
		public ThoughtDef tasteThought;

		// Token: 0x04003716 RID: 14102
		public ThoughtDef specialThoughtDirect;

		// Token: 0x04003717 RID: 14103
		public ThoughtDef specialThoughtAsIngredient;

		// Token: 0x04003718 RID: 14104
		public EffecterDef ingestEffect;

		// Token: 0x04003719 RID: 14105
		public EffecterDef ingestEffectEat;

		// Token: 0x0400371A RID: 14106
		public SoundDef ingestSound;

		// Token: 0x0400371B RID: 14107
		[MustTranslate]
		public string ingestCommandString;

		// Token: 0x0400371C RID: 14108
		[MustTranslate]
		public string ingestReportString;

		// Token: 0x0400371D RID: 14109
		[MustTranslate]
		public string ingestReportStringEat;

		// Token: 0x0400371E RID: 14110
		public HoldOffsetSet ingestHoldOffsetStanding;

		// Token: 0x0400371F RID: 14111
		public bool ingestHoldUsesTable = true;

		// Token: 0x04003720 RID: 14112
		public FoodTypeFlags foodType;

		// Token: 0x04003721 RID: 14113
		public float joy;

		// Token: 0x04003722 RID: 14114
		public JoyKindDef joyKind;

		// Token: 0x04003723 RID: 14115
		public ThingDef sourceDef;

		// Token: 0x04003724 RID: 14116
		public FoodPreferability preferability;

		// Token: 0x04003725 RID: 14117
		public bool nurseable;

		// Token: 0x04003726 RID: 14118
		public float optimalityOffsetHumanlikes;

		// Token: 0x04003727 RID: 14119
		public float optimalityOffsetFeedingAnimals;

		// Token: 0x04003728 RID: 14120
		public DrugCategory drugCategory;

		// Token: 0x04003729 RID: 14121
		public bool canAutoSelectAsFoodForCaravan = true;

		// Token: 0x0400372A RID: 14122
		[Unsaved(false)]
		private float cachedNutrition = -1f;
	}
}
