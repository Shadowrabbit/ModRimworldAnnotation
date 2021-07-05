using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DBE RID: 3518
	public static class PawnInventoryGenerator
	{
		// Token: 0x06005169 RID: 20841 RVA: 0x001B64D0 File Offset: 0x001B46D0
		public static void GenerateInventoryFor(Pawn p, PawnGenerationRequest request)
		{
			p.inventory.DestroyAll(DestroyMode.Vanish);
			for (int i = 0; i < p.kindDef.fixedInventory.Count; i++)
			{
				ThingDefCountClass thingDefCountClass = p.kindDef.fixedInventory[i];
				Thing thing = ThingMaker.MakeThing(thingDefCountClass.thingDef, null);
				thing.stackCount = thingDefCountClass.count;
				p.inventory.innerContainer.TryAdd(thing, true);
			}
			if (p.kindDef.inventoryOptions != null)
			{
				foreach (Thing item in p.kindDef.inventoryOptions.GenerateThings())
				{
					p.inventory.innerContainer.TryAdd(item, true);
				}
			}
			if (request.AllowFood)
			{
				PawnInventoryGenerator.GiveRandomFood(p);
			}
			PawnInventoryGenerator.GiveDrugsIfAddicted(p);
			PawnInventoryGenerator.GiveCombatEnhancingDrugs(p);
		}

		// Token: 0x0600516A RID: 20842 RVA: 0x001B65C4 File Offset: 0x001B47C4
		public static void GiveRandomFood(Pawn p)
		{
			if (p.kindDef.invNutrition > 0.001f)
			{
				ThingDef def;
				if (p.kindDef.invFoodDef != null)
				{
					def = p.kindDef.invFoodDef;
				}
				else
				{
					float value = Rand.Value;
					if (value < 0.5f)
					{
						def = ThingDefOf.MealSimple;
					}
					else if ((double)value < 0.75)
					{
						def = ThingDefOf.MealFine;
					}
					else
					{
						def = ThingDefOf.MealSurvivalPack;
					}
				}
				Thing thing = ThingMaker.MakeThing(def, null);
				thing.stackCount = GenMath.RoundRandom(p.kindDef.invNutrition / thing.GetStatValue(StatDefOf.Nutrition, true));
				p.inventory.TryAddItemNotForSale(thing);
			}
		}

		// Token: 0x0600516B RID: 20843 RVA: 0x001B666C File Offset: 0x001B486C
		private static void GiveDrugsIfAddicted(Pawn p)
		{
			if (!p.RaceProps.Humanlike)
			{
				return;
			}
			using (IEnumerator<Hediff_Addiction> enumerator = p.health.hediffSet.GetHediffs<Hediff_Addiction>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Hediff_Addiction addiction = enumerator.Current;
					ThingDef def;
					if (DefDatabase<ThingDef>.AllDefsListForReading.Where(delegate(ThingDef x)
					{
						if (x.category != ThingCategory.Item)
						{
							return false;
						}
						if (p.Faction != null && x.techLevel > p.Faction.def.techLevel)
						{
							return false;
						}
						CompProperties_Drug compProperties = x.GetCompProperties<CompProperties_Drug>();
						return compProperties != null && compProperties.chemical != null && compProperties.chemical.addictionHediff == addiction.def;
					}).TryRandomElement(out def))
					{
						int stackCount = Rand.RangeInclusive(2, 5);
						Thing thing = ThingMaker.MakeThing(def, null);
						thing.stackCount = stackCount;
						p.inventory.TryAddItemNotForSale(thing);
					}
				}
			}
		}

		// Token: 0x0600516C RID: 20844 RVA: 0x001B6744 File Offset: 0x001B4944
		private static void GiveCombatEnhancingDrugs(Pawn pawn)
		{
			if (Rand.Value >= pawn.kindDef.combatEnhancingDrugsChance)
			{
				return;
			}
			if (pawn.IsTeetotaler())
			{
				return;
			}
			for (int i = 0; i < pawn.inventory.innerContainer.Count; i++)
			{
				CompDrug compDrug = pawn.inventory.innerContainer[i].TryGetComp<CompDrug>();
				if (compDrug != null && compDrug.Props.isCombatEnhancingDrug)
				{
					return;
				}
			}
			int randomInRange = pawn.kindDef.combatEnhancingDrugsCount.RandomInRange;
			if (randomInRange <= 0)
			{
				return;
			}
			IEnumerable<ThingDef> source = DefDatabase<ThingDef>.AllDefsListForReading.Where(delegate(ThingDef x)
			{
				if (x.category != ThingCategory.Item)
				{
					return false;
				}
				if (pawn.Faction != null && x.techLevel > pawn.Faction.def.techLevel)
				{
					return false;
				}
				CompProperties_Drug compProperties = x.GetCompProperties<CompProperties_Drug>();
				return compProperties != null && compProperties.isCombatEnhancingDrug;
			});
			int num = 0;
			ThingDef def;
			while (num < randomInRange && source.TryRandomElement(out def))
			{
				pawn.inventory.innerContainer.TryAdd(ThingMaker.MakeThing(def, null), true);
				num++;
			}
		}
	}
}
