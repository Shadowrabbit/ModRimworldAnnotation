using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EE7 RID: 3815
	public static class RecipeDefGenerator
	{
		// Token: 0x06005465 RID: 21605 RVA: 0x0003A982 File Offset: 0x00038B82
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

		// Token: 0x06005466 RID: 21606 RVA: 0x0003A98B File Offset: 0x00038B8B
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

		// Token: 0x06005467 RID: 21607 RVA: 0x001C3C30 File Offset: 0x001C1E30
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
			if (def.MadeFromStuff)
			{
				IngredientCount ingredientCount = new IngredientCount();
				ingredientCount.SetBaseCount((float)(def.costStuffCount * adjustedCount));
				ingredientCount.filter.SetAllowAllWhoCanMake(def);
				recipeDef.ingredients.Add(ingredientCount);
				recipeDef.fixedIngredientFilter.SetAllowAllWhoCanMake(def);
				recipeDef.productHasIngredientStuff = true;
			}
			recipeDef.useIngredientsForColor = recipeMaker.useIngredientsForColor;
			if (def.costList != null)
			{
				foreach (ThingDefCountClass thingDefCountClass in def.costList)
				{
					IngredientCount ingredientCount2 = new IngredientCount();
					ingredientCount2.SetBaseCount((float)(thingDefCountClass.count * adjustedCount));
					ingredientCount2.filter.SetAllow(thingDefCountClass.thingDef, true);
					recipeDef.ingredients.Add(ingredientCount2);
				}
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
			recipeDef.researchPrerequisites = recipeMaker.researchPrerequisites;
			recipeDef.factionPrerequisiteTags = recipeMaker.factionPrerequisiteTags;
			string[] items = recipeDef.products.Select(delegate(ThingDefCountClass p)
			{
				if (p.count != 1)
				{
					return p.Label;
				}
				return Find.ActiveLanguageWorker.WithIndefiniteArticle(p.thingDef.label, false, false);
			}).ToArray<string>();
			recipeDef.description = "RecipeMakeDescription".Translate(items.ToCommaList(true));
			recipeDef.descriptionHyperlinks = (from p in recipeDef.products
			select new DefHyperlink(p.thingDef)).ToList<DefHyperlink>();
			if (adjustedCount != 1 && recipeDef.workAmount < 0f)
			{
				recipeDef.workAmount = recipeDef.WorkAmountTotal(null) * (float)adjustedCount;
			}
			return recipeDef;
		}

		// Token: 0x06005468 RID: 21608 RVA: 0x0003A994 File Offset: 0x00038B94
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
