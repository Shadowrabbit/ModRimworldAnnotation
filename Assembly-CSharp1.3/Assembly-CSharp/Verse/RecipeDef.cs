using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x020000F3 RID: 243
	public class RecipeDef : Def
	{
		// Token: 0x17000127 RID: 295
		// (get) Token: 0x0600067B RID: 1659 RVA: 0x0001F9AE File Offset: 0x0001DBAE
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

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600067C RID: 1660 RVA: 0x0001F9E0 File Offset: 0x0001DBE0
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

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x0600067D RID: 1661 RVA: 0x0001FA12 File Offset: 0x0001DC12
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

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600067E RID: 1662 RVA: 0x0001FA38 File Offset: 0x0001DC38
		public bool AvailableNow
		{
			get
			{
				if (this.researchPrerequisite != null && !this.researchPrerequisite.IsFinished)
				{
					return false;
				}
				if (this.memePrerequisitesAny != null)
				{
					bool flag = false;
					foreach (MemeDef meme in this.memePrerequisitesAny)
					{
						if (Faction.OfPlayer.ideos.HasAnyIdeoWithMeme(meme))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
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
						RecipeDef.<>c__DisplayClass67_0 CS$<>8__locals1;
						CS$<>8__locals1.unlockedByIdeo = false;
						this.<get_AvailableNow>g__Check|67_2(ref CS$<>8__locals1);
						if (!CS$<>8__locals1.unlockedByIdeo)
						{
							return false;
						}
					}
				}
				return !this.fromIdeoBuildingPreceptOnly || (ModsConfig.IdeologyActive && IdeoUtility.PlayerHasPreceptForBuilding(this.ProducedThingDef));
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x0001FB64 File Offset: 0x0001DD64
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

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000680 RID: 1664 RVA: 0x0001FC1C File Offset: 0x0001DE1C
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

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000681 RID: 1665 RVA: 0x0001FC2C File Offset: 0x0001DE2C
		public bool UsesUnfinishedThing
		{
			get
			{
				return this.unfinishedThingDef != null;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000682 RID: 1666 RVA: 0x0001FC38 File Offset: 0x0001DE38
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

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000683 RID: 1667 RVA: 0x0001FCB8 File Offset: 0x0001DEB8
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

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000684 RID: 1668 RVA: 0x0001FCED File Offset: 0x0001DEED
		public ThingDef UIIconThing
		{
			get
			{
				return this.uiIconThing ?? this.ProducedThingDef;
			}
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x0001FCFF File Offset: 0x0001DEFF
		public bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			return this.Worker.AvailableOnNow(thing, part);
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0001FD0E File Offset: 0x0001DF0E
		public float WorkAmountTotal(ThingDef stuffDef)
		{
			if (this.workAmount >= 0f)
			{
				return this.workAmount;
			}
			return this.products[0].thingDef.GetStatValueAbstract(StatDefOf.WorkToMake, stuffDef);
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0001FD40 File Offset: 0x0001DF40
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

		// Token: 0x06000688 RID: 1672 RVA: 0x0001FD60 File Offset: 0x0001DF60
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

		// Token: 0x06000689 RID: 1673 RVA: 0x0001FDC0 File Offset: 0x0001DFC0
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

		// Token: 0x0600068A RID: 1674 RVA: 0x0001FDD0 File Offset: 0x0001DFD0
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

		// Token: 0x0600068B RID: 1675 RVA: 0x0001FF14 File Offset: 0x0001E114
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

		// Token: 0x0600068C RID: 1676 RVA: 0x0001FF8C File Offset: 0x0001E18C
		public bool PawnSatisfiesSkillRequirements(Pawn pawn)
		{
			return this.FirstSkillRequirementPawnDoesntSatisfy(pawn) == null;
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0001FF98 File Offset: 0x0001E198
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

		// Token: 0x0600068E RID: 1678 RVA: 0x0001FFE8 File Offset: 0x0001E1E8
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

		// Token: 0x0600068F RID: 1679 RVA: 0x00020114 File Offset: 0x0001E314
		private IEnumerable<Dialog_InfoCard.Hyperlink> GetIngredientsHyperlinks()
		{
			return Dialog_InfoCard.DefsToHyperlinks(from i in this.ingredients
			where i.IsFixedIngredient
			select i.FixedIngredient into i
			where i != null
			select i);
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00020198 File Offset: 0x0001E398
		private IEnumerable<Dialog_InfoCard.Hyperlink> GetProductsHyperlinks()
		{
			return Dialog_InfoCard.DefsToHyperlinks(from i in this.products
			select i.thingDef);
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x000201C9 File Offset: 0x0001E3C9
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			if (this.workSkill != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Skill".Translate(), this.workSkill.LabelCap, "Stat_Recipe_Skill_Desc".Translate(), 4404, null, null, false);
			}
			if (this.ingredients != null && this.ingredients.Count > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Ingredients".Translate(), (from ic in this.ingredients
				select ic.Summary).ToCommaList(false, false), "Stat_Recipe_Ingredients_Desc".Translate(), 4405, null, this.GetIngredientsHyperlinks(), false);
			}
			if (this.skillRequirements != null && this.skillRequirements.Count > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "SkillRequirements".Translate(), (from sr in this.skillRequirements
				select sr.Summary).ToCommaList(false, false), "Stat_Recipe_SkillRequirements_Desc".Translate(), 4403, null, null, false);
			}
			if (this.products != null && this.products.Count > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Products".Translate(), (from pr in this.products
				select pr.Summary).ToCommaList(false, false), "Stat_Recipe_Products_Desc".Translate(), 4405, null, this.GetProductsHyperlinks(), false);
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

		// Token: 0x06000693 RID: 1683 RVA: 0x000202B4 File Offset: 0x0001E4B4
		[CompilerGenerated]
		private void <get_AvailableNow>g__Check|67_2(ref RecipeDef.<>c__DisplayClass67_0 A_1)
		{
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				foreach (Precept_Role precept_Role in ideo.RolesListForReading)
				{
					foreach (PreceptApparelRequirement preceptApparelRequirement in precept_Role.apparelRequirements)
					{
						ThingDef thingDef = preceptApparelRequirement.requirement.AllRequiredApparel(Gender.None).First<ThingDef>();
						using (List<ThingDefCountClass>.Enumerator enumerator4 = this.products.GetEnumerator())
						{
							while (enumerator4.MoveNext())
							{
								if (enumerator4.Current.thingDef == thingDef)
								{
									A_1.unlockedByIdeo = true;
									return;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x040005AE RID: 1454
		public Type workerClass = typeof(RecipeWorker);

		// Token: 0x040005AF RID: 1455
		public Type workerCounterClass = typeof(RecipeWorkerCounter);

		// Token: 0x040005B0 RID: 1456
		[MustTranslate]
		public string jobString = "Doing an unknown recipe.";

		// Token: 0x040005B1 RID: 1457
		public WorkTypeDef requiredGiverWorkType;

		// Token: 0x040005B2 RID: 1458
		public float workAmount = -1f;

		// Token: 0x040005B3 RID: 1459
		public StatDef workSpeedStat;

		// Token: 0x040005B4 RID: 1460
		public StatDef efficiencyStat;

		// Token: 0x040005B5 RID: 1461
		public StatDef workTableEfficiencyStat;

		// Token: 0x040005B6 RID: 1462
		public StatDef workTableSpeedStat;

		// Token: 0x040005B7 RID: 1463
		public List<IngredientCount> ingredients = new List<IngredientCount>();

		// Token: 0x040005B8 RID: 1464
		public ThingFilter fixedIngredientFilter = new ThingFilter();

		// Token: 0x040005B9 RID: 1465
		public ThingFilter defaultIngredientFilter;

		// Token: 0x040005BA RID: 1466
		public bool allowMixingIngredients;

		// Token: 0x040005BB RID: 1467
		public bool ignoreIngredientCountTakeEntireStacks;

		// Token: 0x040005BC RID: 1468
		private Type ingredientValueGetterClass = typeof(IngredientValueGetter_Volume);

		// Token: 0x040005BD RID: 1469
		public List<SpecialThingFilterDef> forceHiddenSpecialFilters;

		// Token: 0x040005BE RID: 1470
		public bool autoStripCorpses = true;

		// Token: 0x040005BF RID: 1471
		public bool interruptIfIngredientIsRotting;

		// Token: 0x040005C0 RID: 1472
		public List<ThingDefCountClass> products = new List<ThingDefCountClass>();

		// Token: 0x040005C1 RID: 1473
		public List<SpecialProductType> specialProducts;

		// Token: 0x040005C2 RID: 1474
		public bool productHasIngredientStuff;

		// Token: 0x040005C3 RID: 1475
		public bool useIngredientsForColor = true;

		// Token: 0x040005C4 RID: 1476
		public int targetCountAdjustment = 1;

		// Token: 0x040005C5 RID: 1477
		public ThingDef unfinishedThingDef;

		// Token: 0x040005C6 RID: 1478
		public List<SkillRequirement> skillRequirements;

		// Token: 0x040005C7 RID: 1479
		public SkillDef workSkill;

		// Token: 0x040005C8 RID: 1480
		public float workSkillLearnFactor = 1f;

		// Token: 0x040005C9 RID: 1481
		public EffecterDef effectWorking;

		// Token: 0x040005CA RID: 1482
		public SoundDef soundWorking;

		// Token: 0x040005CB RID: 1483
		private ThingDef uiIconThing;

		// Token: 0x040005CC RID: 1484
		public List<ThingDef> recipeUsers;

		// Token: 0x040005CD RID: 1485
		public List<BodyPartDef> appliedOnFixedBodyParts = new List<BodyPartDef>();

		// Token: 0x040005CE RID: 1486
		public List<BodyPartGroupDef> appliedOnFixedBodyPartGroups = new List<BodyPartGroupDef>();

		// Token: 0x040005CF RID: 1487
		public HediffDef addsHediff;

		// Token: 0x040005D0 RID: 1488
		public HediffDef removesHediff;

		// Token: 0x040005D1 RID: 1489
		public HediffDef changesHediffLevel;

		// Token: 0x040005D2 RID: 1490
		public List<string> incompatibleWithHediffTags;

		// Token: 0x040005D3 RID: 1491
		public int hediffLevelOffset;

		// Token: 0x040005D4 RID: 1492
		public bool hideBodyPartNames;

		// Token: 0x040005D5 RID: 1493
		public bool isViolation;

		// Token: 0x040005D6 RID: 1494
		[MustTranslate]
		public string successfullyRemovedHediffMessage;

		// Token: 0x040005D7 RID: 1495
		public float surgerySuccessChanceFactor = 1f;

		// Token: 0x040005D8 RID: 1496
		public float deathOnFailedSurgeryChance;

		// Token: 0x040005D9 RID: 1497
		public bool targetsBodyPart = true;

		// Token: 0x040005DA RID: 1498
		public bool anesthetize = true;

		// Token: 0x040005DB RID: 1499
		public int minPartCount = -1;

		// Token: 0x040005DC RID: 1500
		public bool surgeryIgnoreEnvironment;

		// Token: 0x040005DD RID: 1501
		public ResearchProjectDef researchPrerequisite;

		// Token: 0x040005DE RID: 1502
		public List<MemeDef> memePrerequisitesAny;

		// Token: 0x040005DF RID: 1503
		public List<ResearchProjectDef> researchPrerequisites;

		// Token: 0x040005E0 RID: 1504
		[NoTranslate]
		public List<string> factionPrerequisiteTags;

		// Token: 0x040005E1 RID: 1505
		public bool fromIdeoBuildingPreceptOnly;

		// Token: 0x040005E2 RID: 1506
		public ConceptDef conceptLearned;

		// Token: 0x040005E3 RID: 1507
		public bool dontShowIfAnyIngredientMissing;

		// Token: 0x040005E4 RID: 1508
		[Unsaved(false)]
		private RecipeWorker workerInt;

		// Token: 0x040005E5 RID: 1509
		[Unsaved(false)]
		private RecipeWorkerCounter workerCounterInt;

		// Token: 0x040005E6 RID: 1510
		[Unsaved(false)]
		private IngredientValueGetter ingredientValueGetterInt;

		// Token: 0x040005E7 RID: 1511
		[Unsaved(false)]
		private List<ThingDef> premultipliedSmallIngredients;

		// Token: 0x040005E8 RID: 1512
		[Unsaved(false)]
		public bool regenerateOnDifficultyChange;

		// Token: 0x040005E9 RID: 1513
		[Unsaved(false)]
		public int adjustedCount = 1;

		// Token: 0x040005EA RID: 1514
		private bool? isSurgeryCached;
	}
}
