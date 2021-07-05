using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010E6 RID: 4326
	public class ThingSetMaker_RandomGeneralGoods : ThingSetMaker
	{
		// Token: 0x06006779 RID: 26489 RVA: 0x0022F910 File Offset: 0x0022DB10
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			IntRange intRange = parms.countRange ?? new IntRange(10, 20);
			TechLevel techLevel = parms.techLevel ?? TechLevel.Undefined;
			int num = Mathf.Max(intRange.RandomInRange, 1);
			for (int i = 0; i < num; i++)
			{
				outThings.Add(this.GenerateSingle(techLevel));
			}
		}

		// Token: 0x0600677A RID: 26490 RVA: 0x0022F988 File Offset: 0x0022DB88
		private Thing GenerateSingle(TechLevel techLevel)
		{
			Thing thing = null;
			int num = 0;
			while (thing == null && num < 50)
			{
				IEnumerable<Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>> goodsWeights = ThingSetMaker_RandomGeneralGoods.GoodsWeights;
				Func<Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>, float> weightSelector = (Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float> x) => x.Second;
				switch (goodsWeights.RandomElementByWeight(weightSelector).First)
				{
				case ThingSetMaker_RandomGeneralGoods.GoodsType.Meals:
					thing = this.RandomMeals(techLevel);
					break;
				case ThingSetMaker_RandomGeneralGoods.GoodsType.RawFood:
					thing = this.RandomRawFood(techLevel);
					break;
				case ThingSetMaker_RandomGeneralGoods.GoodsType.Medicine:
					thing = this.RandomMedicine(techLevel);
					break;
				case ThingSetMaker_RandomGeneralGoods.GoodsType.Drugs:
					thing = this.RandomDrugs(techLevel);
					break;
				case ThingSetMaker_RandomGeneralGoods.GoodsType.Resources:
					thing = this.RandomResources(techLevel);
					break;
				default:
					throw new Exception();
				}
				num++;
			}
			return thing;
		}

		// Token: 0x0600677B RID: 26491 RVA: 0x0022FA38 File Offset: 0x0022DC38
		private Thing RandomMeals(TechLevel techLevel)
		{
			ThingDef thingDef;
			if (techLevel.IsNeolithicOrWorse())
			{
				thingDef = ThingDefOf.Pemmican;
			}
			else
			{
				float value = Rand.Value;
				if (value < 0.5f)
				{
					thingDef = ThingDefOf.MealSimple;
				}
				else if ((double)value < 0.75)
				{
					thingDef = ThingDefOf.MealFine;
				}
				else
				{
					thingDef = ThingDefOf.MealSurvivalPack;
				}
			}
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			int num = Mathf.Min(thingDef.stackLimit, 10);
			thing.stackCount = Rand.RangeInclusive(num / 2, num);
			return thing;
		}

		// Token: 0x0600677C RID: 26492 RVA: 0x0022FAAC File Offset: 0x0022DCAC
		private Thing RandomRawFood(TechLevel techLevel)
		{
			ThingDef thingDef;
			if (!this.PossibleRawFood(techLevel).TryRandomElement(out thingDef))
			{
				return null;
			}
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			int max = Mathf.Min(thingDef.stackLimit, 75);
			thing.stackCount = Rand.RangeInclusive(1, max);
			return thing;
		}

		// Token: 0x0600677D RID: 26493 RVA: 0x0022FAF0 File Offset: 0x0022DCF0
		private IEnumerable<ThingDef> PossibleRawFood(TechLevel techLevel)
		{
			return from x in ThingSetMakerUtility.allGeneratableItems
			where x.IsNutritionGivingIngestible && !x.IsCorpse && x.ingestible.HumanEdible && !x.HasComp(typeof(CompHatcher)) && x.techLevel <= techLevel && x.ingestible.preferability < FoodPreferability.MealAwful
			select x;
		}

		// Token: 0x0600677E RID: 26494 RVA: 0x0022FB20 File Offset: 0x0022DD20
		private Thing RandomMedicine(TechLevel techLevel)
		{
			ThingDef thingDef;
			if (Rand.Value < 0.75f && techLevel >= ThingDefOf.MedicineHerbal.techLevel)
			{
				thingDef = (from x in ThingSetMakerUtility.allGeneratableItems
				where x.IsMedicine && x.techLevel <= techLevel
				select x).MaxBy((ThingDef x) => x.GetStatValueAbstract(StatDefOf.MedicalPotency, null));
			}
			else if (!(from x in ThingSetMakerUtility.allGeneratableItems
			where x.IsMedicine
			select x).TryRandomElement(out thingDef))
			{
				return null;
			}
			if (techLevel.IsNeolithicOrWorse())
			{
				thingDef = ThingDefOf.MedicineHerbal;
			}
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			int max = Mathf.Min(thingDef.stackLimit, 20);
			thing.stackCount = Rand.RangeInclusive(1, max);
			return thing;
		}

		// Token: 0x0600677F RID: 26495 RVA: 0x0022FC08 File Offset: 0x0022DE08
		private Thing RandomDrugs(TechLevel techLevel)
		{
			ThingDef thingDef;
			if (!(from x in ThingSetMakerUtility.allGeneratableItems
			where x.IsDrug && x.techLevel <= techLevel
			select x).TryRandomElement(out thingDef))
			{
				return null;
			}
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			int max = Mathf.Min(thingDef.stackLimit, 25);
			thing.stackCount = Rand.RangeInclusive(1, max);
			return thing;
		}

		// Token: 0x06006780 RID: 26496 RVA: 0x0022FC68 File Offset: 0x0022DE68
		private Thing RandomResources(TechLevel techLevel)
		{
			ThingDef thingDef = BaseGenUtility.RandomCheapWallStuff(techLevel, false);
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			int num = Mathf.Min(thingDef.stackLimit, 75);
			thing.stackCount = Rand.RangeInclusive(num / 2, num);
			return thing;
		}

		// Token: 0x06006781 RID: 26497 RVA: 0x0022FCA1 File Offset: 0x0022DEA1
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			TechLevel techLevel = parms.techLevel ?? TechLevel.Undefined;
			if (techLevel.IsNeolithicOrWorse())
			{
				yield return ThingDefOf.Pemmican;
			}
			else
			{
				yield return ThingDefOf.MealSimple;
				yield return ThingDefOf.MealFine;
				yield return ThingDefOf.MealSurvivalPack;
			}
			foreach (ThingDef thingDef in this.PossibleRawFood(techLevel))
			{
				yield return thingDef;
			}
			IEnumerator<ThingDef> enumerator = null;
			foreach (ThingDef thingDef2 in from x in ThingSetMakerUtility.allGeneratableItems
			where x.IsMedicine
			select x)
			{
				yield return thingDef2;
			}
			enumerator = null;
			IEnumerable<ThingDef> allGeneratableItems = ThingSetMakerUtility.allGeneratableItems;
			Func<ThingDef, bool> <>9__1;
			Func<ThingDef, bool> predicate;
			if ((predicate = <>9__1) == null)
			{
				predicate = (<>9__1 = ((ThingDef x) => x.IsDrug && x.techLevel <= techLevel));
			}
			foreach (ThingDef thingDef3 in allGeneratableItems.Where(predicate))
			{
				yield return thingDef3;
			}
			enumerator = null;
			if (techLevel.IsNeolithicOrWorse())
			{
				yield return ThingDefOf.WoodLog;
			}
			else
			{
				foreach (ThingDef thingDef4 in from d in DefDatabase<ThingDef>.AllDefsListForReading
				where BaseGenUtility.IsCheapWallStuff(d)
				select d)
				{
					yield return thingDef4;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x04003A5B RID: 14939
		private static Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>[] GoodsWeights = new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>[]
		{
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.Meals, 1f),
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.RawFood, 0.75f),
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.Medicine, 0.234f),
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.Drugs, 0.234f),
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.Resources, 0.234f)
		};

		// Token: 0x02002500 RID: 9472
		private enum GoodsType
		{
			// Token: 0x04008CFD RID: 36093
			None,
			// Token: 0x04008CFE RID: 36094
			Meals,
			// Token: 0x04008CFF RID: 36095
			RawFood,
			// Token: 0x04008D00 RID: 36096
			Medicine,
			// Token: 0x04008D01 RID: 36097
			Drugs,
			// Token: 0x04008D02 RID: 36098
			Resources
		}
	}
}
