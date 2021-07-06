using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000167 RID: 359
	public class RecipeDef : Def
	{
		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060008FA RID: 2298 RVA: 0x0000D19D File Offset: 0x0000B39D
		public RecipeWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RecipeWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.recipe = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060008FB RID: 2299 RVA: 0x0000D1CF File Offset: 0x0000B3CF
		public RecipeWorkerCounter WorkerCounter
		{
			get
			{
				if (this.workerCounterInt == null)
				{
					this.workerCounterInt = (RecipeWorkerCounter)Activator.CreateInstance(this.workerCounterClass);
					this.workerCounterInt.recipe = this;
				}
				return this.workerCounterInt;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060008FC RID: 2300 RVA: 0x0000D201 File Offset: 0x0000B401
		public IngredientValueGetter IngredientValueGetter
		{
			get
			{
				if (this.ingredientValueGetterInt == null)
				{
					this.ingredientValueGetterInt = (IngredientValueGetter)Activator.CreateInstance(this.ingredientValueGetterClass);
				}
				return this.ingredientValueGetterInt;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060008FD RID: 2301 RVA: 0x00097C88 File Offset: 0x00095E88
		public bool AvailableNow
		{
			get
			{
				if (this.researchPrerequisite != null && !this.researchPrerequisite.IsFinished)
				{
					return false;
				}
				if (this.researchPrerequisites != null)
				{
					if (this.researchPrerequisites.Any((ResearchProjectDef r) => !r.IsFinished))
					{
						return false;
					}
				}
				if (this.factionPrerequisiteTags != null)
				{
					if (this.factionPrerequisiteTags.Any((string tag) => Faction.OfPlayer.def.recipePrerequisiteTags == null || !Faction.OfPlayer.def.recipePrerequisiteTags.Contains(tag)))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060008FE RID: 2302 RVA: 0x00097D1C File Offset: 0x00095F1C
		public string MinSkillString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = false;
				if (this.skillRequirements != null)
				{
					for (int i = 0; i < this.skillRequirements.Count; i++)
					{
						SkillRequirement skillRequirement = this.skillRequirements[i];
						stringBuilder.AppendLine(string.Concat(new object[]
						{
							"   ",
							skillRequirement.skill.skillLabel.CapitalizeFirst(),
							": ",
							skillRequirement.minLevel
						}));
						flag = true;
					}
				}
				if (!flag)
				{
					stringBuilder.AppendLine("   (" + "NoneLower".Translate() + ")");
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060008FF RID: 2303 RVA: 0x0000D227 File Offset: 0x0000B427
		public IEnumerable<ThingDef> AllRecipeUsers
		{
			get
			{
				int num;
				if (this.recipeUsers != null)
				{
					for (int i = 0; i < this.recipeUsers.Count; i = num + 1)
					{
						yield return this.recipeUsers[i];
						num = i;
					}
				}
				List<ThingDef> thingDefs = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int i = 0; i < thingDefs.Count; i = num + 1)
				{
					if (thingDefs[i].recipes != null && thingDefs[i].recipes.Contains(this))
					{
						yield return thingDefs[i];
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000900 RID: 2304 RVA: 0x0000D237 File Offset: 0x0000B437
		public bool UsesUnfinishedThing
		{
			get
			{
				return this.unfinishedThingDef != null;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000901 RID: 2305 RVA: 0x00097DD4 File Offset: 0x00095FD4
		public bool IsSurgery
		{
			get
			{
				if (this.isSurgeryCached == null)
				{
					this.isSurgeryCached = new bool?(false);
					using (IEnumerator<ThingDef> enumerator = this.AllRecipeUsers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.category == ThingCategory.Pawn)
							{
								this.isSurgeryCached = new bool?(true);
								break;
							}
						}
					}
				}
				return this.isSurgeryCached.Value;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000902 RID: 2306 RVA: 0x0000D242 File Offset: 0x0000B442
		public ThingDef ProducedThingDef
		{
			get
			{
				if (this.specialProducts != null)
				{
					return null;
				}
				if (this.products == null || this.products.Count != 1)
				{
					return null;
				}
				return this.products[0].thingDef;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000903 RID: 2307 RVA: 0x0000D277 File Offset: 0x0000B477
		public ThingDef UIIconThing
		{
			get
			{
				return this.uiIconThing ?? this.ProducedThingDef;
			}
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x0000D289 File Offset: 0x0000B489
		public bool AvailableOnNow(Thing thing)
		{
			return this.Worker.AvailableOnNow(thing);
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x0000D297 File Offset: 0x0000B497
		public float WorkAmountTotal(ThingDef stuffDef)
		{
			if (this.workAmount >= 0f)
			{
				return this.workAmount;
			}
			return this.products[0].thingDef.GetStatValueAbstract(StatDefOf.WorkToMake, stuffDef);
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x0000D2C9 File Offset: 0x0000B4C9
		public IEnumerable<ThingDef> PotentiallyMissingIngredients(Pawn billDoer, Map map)
		{
			int num;
			for (int i = 0; i < this.ingredients.Count; i = num + 1)
			{
				IngredientCount ingredientCount = this.ingredients[i];
				bool flag = false;
				List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.HaulableEver);
				for (int j = 0; j < list.Count; j++)
				{
					Thing thing = list[j];
					if ((billDoer == null || !thing.IsForbidden(billDoer)) && !thing.Position.Fogged(map) && (ingredientCount.IsFixedIngredient || this.fixedIngredientFilter.Allows(thing)) && ingredientCount.filter.Allows(thing))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					if (ingredientCount.IsFixedIngredient)
					{
						yield return ingredientCount.filter.AllowedThingDefs.First<ThingDef>();
					}
					else
					{
						ThingDef thingDef = (from x in ingredientCount.filter.AllowedThingDefs
						orderby x.BaseMarketValue
						select x).FirstOrDefault((ThingDef x) => this.fixedIngredientFilter.Allows(x));
						if (thingDef != null)
						{
							yield return thingDef;
						}
					}
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x00097E54 File Offset: 0x00096054
		public bool IsIngredient(ThingDef th)
		{
			for (int i = 0; i < this.ingredients.Count; i++)
			{
				if (this.ingredients[i].filter.Allows(th) && (this.ingredients[i].IsFixedIngredient || this.fixedIngredientFilter.Allows(th)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x0000D2E7 File Offset: 0x0000B4E7
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.workerClass == null)
			{
				yield return "workerClass is null.";
			}
			yield break;
			yield break;
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x00097EB4 File Offset: 0x000960B4
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			DeepProfiler.Start("Stat refs");
			try
			{
				if (this.workTableSpeedStat == null)
				{
					this.workTableSpeedStat = StatDefOf.WorkTableWorkSpeedFactor;
				}
				if (this.workTableEfficiencyStat == null)
				{
					this.workTableEfficiencyStat = StatDefOf.WorkTableEfficiencyFactor;
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("ingredients reference resolve");
			try
			{
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					this.ingredients[i].ResolveReferences();
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("fixedIngredientFilter.ResolveReferences()");
			try
			{
				if (this.fixedIngredientFilter != null)
				{
					this.fixedIngredientFilter.ResolveReferences();
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("defaultIngredientFilter setup");
			try
			{
				if (this.defaultIngredientFilter == null)
				{
					this.defaultIngredientFilter = new ThingFilter();
					if (this.fixedIngredientFilter != null)
					{
						this.defaultIngredientFilter.CopyAllowancesFrom(this.fixedIngredientFilter);
					}
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("defaultIngredientFilter.ResolveReferences()");
			try
			{
				this.defaultIngredientFilter.ResolveReferences();
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x00097FF8 File Offset: 0x000961F8
		public bool CompatibleWithHediff(HediffDef hediffDef)
		{
			if (this.incompatibleWithHediffTags.NullOrEmpty<string>() || hediffDef.tags.NullOrEmpty<string>())
			{
				return true;
			}
			for (int i = 0; i < this.incompatibleWithHediffTags.Count; i++)
			{
				for (int j = 0; j < hediffDef.tags.Count; j++)
				{
					if (this.incompatibleWithHediffTags[i].Equals(hediffDef.tags[j], StringComparison.InvariantCultureIgnoreCase))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x0000D2F7 File Offset: 0x0000B4F7
		public bool PawnSatisfiesSkillRequirements(Pawn pawn)
		{
			return this.FirstSkillRequirementPawnDoesntSatisfy(pawn) == null;
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x00098070 File Offset: 0x00096270
		public SkillRequirement FirstSkillRequirementPawnDoesntSatisfy(Pawn pawn)
		{
			if (this.skillRequirements == null)
			{
				return null;
			}
			for (int i = 0; i < this.skillRequirements.Count; i++)
			{
				if (!this.skillRequirements[i].PawnSatisfies(pawn))
				{
					return this.skillRequirements[i];
				}
			}
			return null;
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x000980C0 File Offset: 0x000962C0
		public List<ThingDef> GetPremultipliedSmallIngredients()
		{
			if (this.premultipliedSmallIngredients != null)
			{
				return this.premultipliedSmallIngredients;
			}
			this.premultipliedSmallIngredients = (from td in this.ingredients.SelectMany((IngredientCount ingredient) => ingredient.filter.AllowedThingDefs)
			where td.smallVolume
			select td).Distinct<ThingDef>().ToList<ThingDef>();
			bool flag = true;
			while (flag)
			{
				flag = false;
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					if (this.ingredients[i].filter.AllowedThingDefs.Any((ThingDef td) => !this.premultipliedSmallIngredients.Contains(td)))
					{
						foreach (ThingDef item in this.ingredients[i].filter.AllowedThingDefs)
						{
							flag |= this.premultipliedSmallIngredients.Remove(item);
						}
					}
				}
			}
			return this.premultipliedSmallIngredients;
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x000981EC File Offset: 0x000963EC
		private IEnumerable<Dialog_InfoCard.Hyperlink> GetIngredientsHyperlinks()
		{
			return Dialog_InfoCard.DefsToHyperlinks(from i in this.ingredients
			where i.IsFixedIngredient
			select i.FixedIngredient into i
			where i != null
			select i);
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x0000D303 File Offset: 0x0000B503
		private IEnumerable<Dialog_InfoCard.Hyperlink> GetProductsHyperlinks()
		{
			return Dialog_InfoCard.DefsToHyperlinks(from i in this.products
			select i.thingDef);
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x0000D334 File Offset: 0x0000B534
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			if (this.workSkill != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Skill".Translate(), this.workSkill.LabelCap, "Stat_Recipe_Skill_Desc".Translate(), 4404, null, null, false);
			}
			if (this.ingredients != null && this.ingredients.Count > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Ingredients".Translate(), (from ic in this.ingredients
				select ic.Summary).ToCommaList(false), "Stat_Recipe_Ingredients_Desc".Translate(), 4405, null, this.GetIngredientsHyperlinks(), false);
			}
			if (this.skillRequirements != null && this.skillRequirements.Count > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "SkillRequirements".Translate(), (from sr in this.skillRequirements
				select sr.Summary).ToCommaList(false), "Stat_Recipe_SkillRequirements_Desc".Translate(), 4403, null, null, false);
			}
			if (this.products != null && this.products.Count > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Products".Translate(), (from pr in this.products
				select pr.Summary).ToCommaList(false), "Stat_Recipe_Products_Desc".Translate(), 4405, null, this.GetProductsHyperlinks(), false);
				float num = this.WorkAmountTotal(null);
				if (num > 0f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, StatDefOf.WorkToMake.LabelCap, num.ToStringWorkAmount(), StatDefOf.WorkToMake.description, StatDefOf.WorkToMake.displayPriorityInCategory, null, null, false);
				}
			}
			if (this.workSpeedStat != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "WorkSpeedStat".Translate(), this.workSpeedStat.LabelCap, "Stat_Recipe_WorkSpeedStat_Desc".Translate(), 4402, null, null, false);
			}
			if (this.efficiencyStat != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "EfficiencyStat".Translate(), this.efficiencyStat.LabelCap, "Stat_Recipe_EfficiencyStat_Desc".Translate(), 4401, null, null, false);
			}
			if (this.IsSurgery)
			{
				if (this.surgerySuccessChanceFactor >= 99999f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgerySuccessChanceFactor".Translate(), "Stat_Thing_Surgery_SuccessChanceFactor_CantFail".Translate(), "Stat_Thing_Surgery_SuccessChanceFactor_CantFail_Desc".Translate(), 4102, null, null, false);
				}
				else
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgerySuccessChanceFactor".Translate(), this.surgerySuccessChanceFactor.ToStringPercent(), "Stat_Thing_Surgery_SuccessChanceFactor_Desc".Translate(), 4102, null, null, false);
					if (this.deathOnFailedSurgeryChance >= 99999f)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgeryDeathOnFailChance".Translate(), "100%", "Stat_Thing_Surgery_DeathOnFailChance_Desc".Translate(), 4101, null, null, false);
					}
					else
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgeryDeathOnFailChance".Translate(), this.deathOnFailedSurgeryChance.ToStringPercent(), "Stat_Thing_Surgery_DeathOnFailChance_Desc".Translate(), 4101, null, null, false);
					}
				}
			}
			yield break;
		}

		// Token: 0x0400079F RID: 1951
		public Type workerClass = typeof(RecipeWorker);

		// Token: 0x040007A0 RID: 1952
		public Type workerCounterClass = typeof(RecipeWorkerCounter);

		// Token: 0x040007A1 RID: 1953
		[MustTranslate]
		public string jobString = "Doing an unknown recipe.";

		// Token: 0x040007A2 RID: 1954
		public WorkTypeDef requiredGiverWorkType;

		// Token: 0x040007A3 RID: 1955
		public float workAmount = -1f;

		// Token: 0x040007A4 RID: 1956
		public StatDef workSpeedStat;

		// Token: 0x040007A5 RID: 1957
		public StatDef efficiencyStat;

		// Token: 0x040007A6 RID: 1958
		public StatDef workTableEfficiencyStat;

		// Token: 0x040007A7 RID: 1959
		public StatDef workTableSpeedStat;

		// Token: 0x040007A8 RID: 1960
		public List<IngredientCount> ingredients = new List<IngredientCount>();

		// Token: 0x040007A9 RID: 1961
		public ThingFilter fixedIngredientFilter = new ThingFilter();

		// Token: 0x040007AA RID: 1962
		public ThingFilter defaultIngredientFilter;

		// Token: 0x040007AB RID: 1963
		public bool allowMixingIngredients;

		// Token: 0x040007AC RID: 1964
		public bool ignoreIngredientCountTakeEntireStacks;

		// Token: 0x040007AD RID: 1965
		private Type ingredientValueGetterClass = typeof(IngredientValueGetter_Volume);

		// Token: 0x040007AE RID: 1966
		public List<SpecialThingFilterDef> forceHiddenSpecialFilters;

		// Token: 0x040007AF RID: 1967
		public bool autoStripCorpses = true;

		// Token: 0x040007B0 RID: 1968
		public bool interruptIfIngredientIsRotting;

		// Token: 0x040007B1 RID: 1969
		public List<ThingDefCountClass> products = new List<ThingDefCountClass>();

		// Token: 0x040007B2 RID: 1970
		public List<SpecialProductType> specialProducts;

		// Token: 0x040007B3 RID: 1971
		public bool productHasIngredientStuff;

		// Token: 0x040007B4 RID: 1972
		public bool useIngredientsForColor = true;

		// Token: 0x040007B5 RID: 1973
		public int targetCountAdjustment = 1;

		// Token: 0x040007B6 RID: 1974
		public ThingDef unfinishedThingDef;

		// Token: 0x040007B7 RID: 1975
		public List<SkillRequirement> skillRequirements;

		// Token: 0x040007B8 RID: 1976
		public SkillDef workSkill;

		// Token: 0x040007B9 RID: 1977
		public float workSkillLearnFactor = 1f;

		// Token: 0x040007BA RID: 1978
		public EffecterDef effectWorking;

		// Token: 0x040007BB RID: 1979
		public SoundDef soundWorking;

		// Token: 0x040007BC RID: 1980
		private ThingDef uiIconThing;

		// Token: 0x040007BD RID: 1981
		public List<ThingDef> recipeUsers;

		// Token: 0x040007BE RID: 1982
		public List<BodyPartDef> appliedOnFixedBodyParts = new List<BodyPartDef>();

		// Token: 0x040007BF RID: 1983
		public List<BodyPartGroupDef> appliedOnFixedBodyPartGroups = new List<BodyPartGroupDef>();

		// Token: 0x040007C0 RID: 1984
		public HediffDef addsHediff;

		// Token: 0x040007C1 RID: 1985
		public HediffDef removesHediff;

		// Token: 0x040007C2 RID: 1986
		public HediffDef changesHediffLevel;

		// Token: 0x040007C3 RID: 1987
		public List<string> incompatibleWithHediffTags;

		// Token: 0x040007C4 RID: 1988
		public int hediffLevelOffset;

		// Token: 0x040007C5 RID: 1989
		public bool hideBodyPartNames;

		// Token: 0x040007C6 RID: 1990
		public bool isViolation;

		// Token: 0x040007C7 RID: 1991
		[MustTranslate]
		public string successfullyRemovedHediffMessage;

		// Token: 0x040007C8 RID: 1992
		public float surgerySuccessChanceFactor = 1f;

		// Token: 0x040007C9 RID: 1993
		public float deathOnFailedSurgeryChance;

		// Token: 0x040007CA RID: 1994
		public bool targetsBodyPart = true;

		// Token: 0x040007CB RID: 1995
		public bool anesthetize = true;

		// Token: 0x040007CC RID: 1996
		public ResearchProjectDef researchPrerequisite;

		// Token: 0x040007CD RID: 1997
		public List<ResearchProjectDef> researchPrerequisites;

		// Token: 0x040007CE RID: 1998
		[NoTranslate]
		public List<string> factionPrerequisiteTags;

		// Token: 0x040007CF RID: 1999
		public ConceptDef conceptLearned;

		// Token: 0x040007D0 RID: 2000
		public bool dontShowIfAnyIngredientMissing;

		// Token: 0x040007D1 RID: 2001
		[Unsaved(false)]
		private RecipeWorker workerInt;

		// Token: 0x040007D2 RID: 2002
		[Unsaved(false)]
		private RecipeWorkerCounter workerCounterInt;

		// Token: 0x040007D3 RID: 2003
		[Unsaved(false)]
		private IngredientValueGetter ingredientValueGetterInt;

		// Token: 0x040007D4 RID: 2004
		[Unsaved(false)]
		private List<ThingDef> premultipliedSmallIngredients;

		// Token: 0x040007D5 RID: 2005
		private bool? isSurgeryCached;
	}
}
