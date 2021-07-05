using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001416 RID: 5142
	public static class PawnInventoryGenerator
	{
		// Token: 0x06006F1B RID: 28443 RVA: 0x00220CB8 File Offset: 0x0021EEB8
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

		// Token: 0x06006F1C RID: 28444 RVA: 0x00220DAC File Offset: 0x0021EFAC
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

		// Token: 0x06006F1D RID: 28445 RVA: 0x00220E54 File Offset: 0x0021F054
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

		// Token: 0x06006F1E RID: 28446 RVA: 0x00220F2C File Offset: 0x0021F12C
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
