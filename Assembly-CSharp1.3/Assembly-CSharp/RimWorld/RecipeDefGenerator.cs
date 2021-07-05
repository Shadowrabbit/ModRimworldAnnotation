using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020009E6 RID: 2534
	public static class RecipeDefGenerator
	{
		// Token: 0x06003E7C RID: 15996 RVA: 0x0015588A File Offset: 0x00153A8A
		public static IEnumerable<RecipeDef> ImpliedRecipeDefs()
		{
			foreach (RecipeDef recipeDef in RecipeDefGenerator.DefsFromRecipeMakers().Concat(RecipeDefGenerator.DrugAdministerDefs()))
			{
				yield return recipeDef;
			}
			IEnumerator<RecipeDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06003E7D RID: 15997 RVA: 0x00155893 File Offset: 0x00153A93
		private static IEnumerable<RecipeDef> DefsFromRecipeMakers()
		{
			foreach (ThingDef def in from d in DefDatabase<ThingDef>.AllDefs
			where d.recipeMaker != null
			select d)
			{
				yield return RecipeDefGenerator.CreateRecipeDefFromMaker(def, 1);
				if (def.recipeMaker.bulkRecipeCount > 0)
				{
					yield return RecipeDefGenerator.CreateRecipeDefFromMaker(def, def.recipeMaker.bulkRecipeCount);
				}
				def = null;
			}
			IEnumerator<ThingDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06003E7E RID: 15998 RVA: 0x0015589C File Offset: 0x00153A9C
		private static RecipeDef CreateRecipeDefFromMaker(ThingDef def, int adjustedCount = 1)
		{
			RecipeMakerProperties recipeMaker = def.recipeMaker;
			RecipeDef recipeDef = new RecipeDef();
			recipeDef.defName = "Make_" + def.defName;
			if (adjustedCount != 1)
			{
				RecipeDef recipeDef2 = recipeDef;
				recipeDef2.defName += "Bulk";
			}
			string text = def.label;
			if (adjustedCount != 1)
			{
				text = text + " x" + adjustedCount;
			}
			recipeDef.label = "RecipeMake".Translate(text);
			recipeDef.jobString = "RecipeMakeJobString".Translate(text);
			recipeDef.modContentPack = def.modContentPack;
			recipeDef.workAmount = (float)(recipeMaker.workAmount * adjustedCount);
			recipeDef.workSpeedStat = recipeMaker.workSpeedStat;
			recipeDef.efficiencyStat = recipeMaker.efficiencyStat;
			RecipeDefGenerator.SetIngredients(recipeDef, def, adjustedCount);
			recipeDef.useIngredientsForColor = recipeMaker.useIngredientsForColor;
			if (def.costListForDifficulty != null)
			{
				recipeDef.regenerateOnDifficultyChange = true;
			}
			recipeDef.defaultIngredientFilter = recipeMaker.defaultIngredientFilter;
			recipeDef.products.Add(new ThingDefCountClass(def, recipeMaker.productCount * adjustedCount));
			recipeDef.targetCountAdjustment = recipeMaker.targetCountAdjustment * adjustedCount;
			recipeDef.skillRequirements = recipeMaker.skillRequirements.ListFullCopyOrNull<SkillRequirement>();
			recipeDef.workSkill = recipeMaker.workSkill;
			recipeDef.workSkillLearnFactor = recipeMaker.workSkillLearnPerTick;
			recipeDef.requiredGiverWorkType = recipeMaker.requiredGiverWorkType;
			recipeDef.unfinishedThingDef = recipeMaker.unfinishedThingDef;
			recipeDef.recipeUsers = recipeMaker.recipeUsers.ListFullCopyOrNull<ThingDef>();
			recipeDef.effectWorking = recipeMaker.effectWorking;
			recipeDef.soundWorking = recipeMaker.soundWorking;
			recipeDef.researchPrerequisite = recipeMaker.researchPrerequisite;
			recipeDef.memePrerequisitesAny = recipeMaker.memePrerequisitesAny;
			recipeDef.researchPrerequisites = recipeMaker.researchPrerequisites;
			recipeDef.factionPrerequisiteTags = recipeMaker.factionPrerequisiteTags;
			recipeDef.fromIdeoBuildingPreceptOnly = recipeMaker.fromIdeoBuildingPreceptOnly;
			string[] items = recipeDef.products.Select(delegate(ThingDefCountClass p)
			{
				if (p.count != 1)
				{
					return p.Label;
				}
				return Find.ActiveLanguageWorker.WithIndefiniteArticle(p.thingDef.label, false, false);
			}).ToArray<string>();
			recipeDef.description = "RecipeMakeDescription".Translate(items.ToCommaList(true, false));
			recipeDef.descriptionHyperlinks = (from p in recipeDef.products
			select new DefHyperlink(p.thingDef)).ToList<DefHyperlink>();
			if (adjustedCount != 1 && recipeDef.workAmount < 0f)
			{
				recipeDef.workAmount = recipeDef.WorkAmountTotal(null) * (float)adjustedCount;
			}
			return recipeDef;
		}

		// Token: 0x06003E7F RID: 15999 RVA: 0x00155B14 File Offset: 0x00153D14
		public static void SetIngredients(RecipeDef r, ThingDef def, int adjustedCount = 1)
		{
			r.ingredients.Clear();
			r.adjustedCount = adjustedCount;
			if (def.MadeFromStuff)
			{
				IngredientCount ingredientCount = new IngredientCount();
				ingredientCount.SetBaseCount((float)(def.CostStuffCount * adjustedCount));
				ingredientCount.filter.SetAllowAllWhoCanMake(def);
				r.ingredients.Add(ingredientCount);
				r.fixedIngredientFilter.SetAllowAllWhoCanMake(def);
				r.productHasIngredientStuff = true;
			}
			if (def.CostList != null)
			{
				foreach (ThingDefCountClass thingDefCountClass in def.CostList)
				{
					IngredientCount ingredientCount2 = new IngredientCount();
					ingredientCount2.SetBaseCount((float)(thingDefCountClass.count * adjustedCount));
					ingredientCount2.filter.SetAllow(thingDefCountClass.thingDef, true);
					r.ingredients.Add(ingredientCount2);
				}
			}
		}

		// Token: 0x06003E80 RID: 16000 RVA: 0x00155BF8 File Offset: 0x00153DF8
		public static void ResetRecipeIngredientsForDifficulty()
		{
			foreach (RecipeDef recipeDef in from x in DefDatabase<RecipeDef>.AllDefs
			where x.regenerateOnDifficultyChange
			select x)
			{
				RecipeDefGenerator.SetIngredients(recipeDef, recipeDef.products[0].thingDef, recipeDef.adjustedCount);
			}
		}

		// Token: 0x06003E81 RID: 16001 RVA: 0x00155C80 File Offset: 0x00153E80
		private static IEnumerable<RecipeDef> DrugAdministerDefs()
		{
			foreach (ThingDef thingDef in from d in DefDatabase<ThingDef>.AllDefs
			where d.IsDrug
			select d)
			{
				RecipeDef recipeDef = new RecipeDef();
				recipeDef.defName = "Administer_" + thingDef.defName;
				recipeDef.label = "RecipeAdminister".Translate(thingDef.label);
				recipeDef.jobString = "RecipeAdministerJobString".Translate(thingDef.label);
				recipeDef.workerClass = typeof(Recipe_AdministerIngestible);
				recipeDef.targetsBodyPart = false;
				recipeDef.anesthetize = false;
				recipeDef.surgerySuccessChanceFactor = 99999f;
				recipeDef.modContentPack = thingDef.modContentPack;
				recipeDef.workAmount = (float)thingDef.ingestible.baseIngestTicks;
				IngredientCount ingredientCount = new IngredientCount();
				ingredientCount.SetBaseCount(1f);
				ingredientCount.filter.SetAllow(thingDef, true);
				recipeDef.ingredients.Add(ingredientCount);
				recipeDef.fixedIngredientFilter.SetAllow(thingDef, true);
				recipeDef.recipeUsers = new List<ThingDef>();
				foreach (ThingDef item in DefDatabase<ThingDef>.AllDefs.Where((ThingDef d) => d.category == ThingCategory.Pawn && d.race.IsFlesh))
				{
					recipeDef.recipeUsers.Add(item);
				}
				yield return recipeDef;
			}
			IEnumerator<ThingDef> enumerator = null;
			yield break;
			yield break;
		}
	}
}
