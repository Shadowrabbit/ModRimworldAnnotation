using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D95 RID: 3477
	public class Recipe_AdministerIngestible : Recipe_Surgery
	{
		// Token: 0x060050B0 RID: 20656 RVA: 0x001AFFE4 File Offset: 0x001AE1E4
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
					pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.ForcedMeToTakeDrugs, billDoer, null);
				}
				else if (pawn.IsProsthophobe() && ingredients[0].def == ThingDefOf.Luciferium)
				{
					pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.ForcedMeToTakeLuciferium, billDoer, null);
				}
			}
			if (billDoer != null)
			{
				if (ingredients[0].def.IsDrug)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AdministeredDrug, billDoer.Named(HistoryEventArgsNames.Doer)), true);
				}
				if (ingredients[0].def.IsDrug && ingredients[0].def.ingestible.drugCategory == DrugCategory.Hard)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AdministeredHardDrug, billDoer.Named(HistoryEventArgsNames.Doer)), true);
				}
				if (ingredients[0].def.IsNonMedicalDrug)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AdministeredRecreationalDrug, billDoer.Named(HistoryEventArgsNames.Doer)), true);
				}
			}
		}

		// Token: 0x060050B1 RID: 20657 RVA: 0x0000313F File Offset: 0x0000133F
		public override void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
		}

		// Token: 0x060050B2 RID: 20658 RVA: 0x001B0194 File Offset: 0x001AE394
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

		// Token: 0x060050B3 RID: 20659 RVA: 0x001B025C File Offset: 0x001AE45C
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
