using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020013C1 RID: 5057
	public class Recipe_AdministerIngestible : Recipe_Surgery
	{
		// Token: 0x06006DAB RID: 28075 RVA: 0x002197E0 File Offset: 0x002179E0
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			float num = ingredients[0].Ingested(pawn, (pawn.needs != null && pawn.needs.food != null) ? pawn.needs.food.NutritionWanted : 0f);
			if (!pawn.Dead)
			{
				pawn.needs.food.CurLevel += num;
			}
			if (pawn.needs.mood != null)
			{
				if (pawn.IsTeetotaler() && ingredients[0].def.IsNonMedicalDrug)
				{
					pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.ForcedMeToTakeDrugs, billDoer);
					return;
				}
				if (pawn.IsProsthophobe() && ingredients[0].def == ThingDefOf.Luciferium)
				{
					pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.ForcedMeToTakeLuciferium, billDoer);
				}
			}
		}

		// Token: 0x06006DAC RID: 28076 RVA: 0x00006A05 File Offset: 0x00004C05
		public override void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
		}

		// Token: 0x06006DAD RID: 28077 RVA: 0x002198D0 File Offset: 0x00217AD0
		public override bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			if (pawn.Faction == billDoerFaction)
			{
				return false;
			}
			ThingDef thingDef = this.recipe.ingredients[0].filter.AllowedThingDefs.First<ThingDef>();
			if (thingDef.IsNonMedicalDrug)
			{
				foreach (CompProperties compProperties in thingDef.comps)
				{
					CompProperties_Drug compProperties_Drug = compProperties as CompProperties_Drug;
					if (compProperties_Drug != null && compProperties_Drug.chemical != null && compProperties_Drug.chemical.addictionHediff != null && pawn.health.hediffSet.HasHediff(compProperties_Drug.chemical.addictionHediff, false))
					{
						return false;
					}
				}
			}
			return thingDef.IsNonMedicalDrug;
		}

		// Token: 0x06006DAE RID: 28078 RVA: 0x00219998 File Offset: 0x00217B98
		public override string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			if (pawn.IsTeetotaler() && this.recipe.ingredients[0].filter.BestThingRequest.singleDef.IsNonMedicalDrug)
			{
				return base.GetLabelWhenUsedOn(pawn, part) + " (" + "TeetotalerUnhappy".Translate() + ")";
			}
			if (pawn.IsProsthophobe() && this.recipe.ingredients[0].filter.BestThingRequest.singleDef == ThingDefOf.Luciferium)
			{
				return base.GetLabelWhenUsedOn(pawn, part) + " (" + "ProsthophobeUnhappy".Translate() + ")";
			}
			return base.GetLabelWhenUsedOn(pawn, part);
		}
	}
}
