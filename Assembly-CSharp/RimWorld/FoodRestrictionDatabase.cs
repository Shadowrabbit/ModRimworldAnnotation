using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001018 RID: 4120
	public class FoodRestrictionDatabase : IExposable
	{
		// Token: 0x17000DEA RID: 3562
		// (get) Token: 0x060059E3 RID: 23011 RVA: 0x0003E650 File Offset: 0x0003C850
		public List<FoodRestriction> AllFoodRestrictions
		{
			get
			{
				return this.foodRestrictions;
			}
		}

		// Token: 0x060059E4 RID: 23012 RVA: 0x0003E658 File Offset: 0x0003C858
		public FoodRestrictionDatabase()
		{
			this.GenerateStartingFoodRestrictions();
		}

		// Token: 0x060059E5 RID: 23013 RVA: 0x0003E671 File Offset: 0x0003C871
		public void ExposeData()
		{
			Scribe_Collections.Look<FoodRestriction>(ref this.foodRestrictions, "foodRestrictions", LookMode.Deep, Array.Empty<object>());
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x060059E6 RID: 23014 RVA: 0x0003E68F File Offset: 0x0003C88F
		public FoodRestriction DefaultFoodRestriction()
		{
			if (this.foodRestrictions.Count == 0)
			{
				this.MakeNewFoodRestriction();
			}
			return this.foodRestrictions[0];
		}

		// Token: 0x060059E7 RID: 23015 RVA: 0x001D3AE4 File Offset: 0x001D1CE4
		public AcceptanceReport TryDelete(FoodRestriction foodRestriction)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive)
			{
				if (pawn.foodRestriction != null && pawn.foodRestriction.CurrentFoodRestriction == foodRestriction)
				{
					return new AcceptanceReport("FoodRestrictionInUse".Translate(pawn));
				}
			}
			foreach (Pawn pawn2 in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
			{
				if (pawn2.foodRestriction != null && pawn2.foodRestriction.CurrentFoodRestriction == foodRestriction)
				{
					pawn2.foodRestriction.CurrentFoodRestriction = null;
				}
			}
			this.foodRestrictions.Remove(foodRestriction);
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x060059E8 RID: 23016 RVA: 0x001D3BD4 File Offset: 0x001D1DD4
		public FoodRestriction MakeNewFoodRestriction()
		{
			int num;
			if (!this.foodRestrictions.Any<FoodRestriction>())
			{
				num = 1;
			}
			else
			{
				num = this.foodRestrictions.Max((FoodRestriction o) => o.id) + 1;
			}
			int id = num;
			FoodRestriction foodRestriction = new FoodRestriction(id, "FoodRestriction".Translate() + " " + id.ToString());
			foodRestriction.filter.SetAllow(ThingCategoryDefOf.Foods, true, null, null);
			foodRestriction.filter.SetAllow(ThingCategoryDefOf.CorpsesHumanlike, true, null, null);
			foodRestriction.filter.SetAllow(ThingCategoryDefOf.CorpsesAnimal, true, null, null);
			this.foodRestrictions.Add(foodRestriction);
			return foodRestriction;
		}

		// Token: 0x060059E9 RID: 23017 RVA: 0x001D3C90 File Offset: 0x001D1E90
		private void GenerateStartingFoodRestrictions()
		{
			this.MakeNewFoodRestriction().label = "FoodRestrictionLavish".Translate();
			FoodRestriction foodRestriction = this.MakeNewFoodRestriction();
			foodRestriction.label = "FoodRestrictionFine".Translate();
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.ingestible != null && thingDef.ingestible.preferability >= FoodPreferability.MealLavish && thingDef != ThingDefOf.InsectJelly)
				{
					foodRestriction.filter.SetAllow(thingDef, false);
				}
			}
			FoodRestriction foodRestriction2 = this.MakeNewFoodRestriction();
			foodRestriction2.label = "FoodRestrictionSimple".Translate();
			foreach (ThingDef thingDef2 in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef2.ingestible != null && thingDef2.ingestible.preferability >= FoodPreferability.MealFine && thingDef2 != ThingDefOf.InsectJelly)
				{
					foodRestriction2.filter.SetAllow(thingDef2, false);
				}
			}
			foodRestriction2.filter.SetAllow(ThingDefOf.MealSurvivalPack, false);
			FoodRestriction foodRestriction3 = this.MakeNewFoodRestriction();
			foodRestriction3.label = "FoodRestrictionPaste".Translate();
			foreach (ThingDef thingDef3 in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef3.ingestible != null && thingDef3.ingestible.preferability >= FoodPreferability.MealSimple && thingDef3 != ThingDefOf.MealNutrientPaste && thingDef3 != ThingDefOf.InsectJelly && thingDef3 != ThingDefOf.Pemmican)
				{
					foodRestriction3.filter.SetAllow(thingDef3, false);
				}
			}
			FoodRestriction foodRestriction4 = this.MakeNewFoodRestriction();
			foodRestriction4.label = "FoodRestrictionRaw".Translate();
			foreach (ThingDef thingDef4 in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef4.ingestible != null && thingDef4.ingestible.preferability >= FoodPreferability.MealAwful)
				{
					foodRestriction4.filter.SetAllow(thingDef4, false);
				}
			}
			foodRestriction4.filter.SetAllow(ThingDefOf.Chocolate, false);
			FoodRestriction foodRestriction5 = this.MakeNewFoodRestriction();
			foodRestriction5.label = "FoodRestrictionNothing".Translate();
			foodRestriction5.filter.SetDisallowAll(null, null);
		}

		// Token: 0x04003C8A RID: 15498
		private List<FoodRestriction> foodRestrictions = new List<FoodRestriction>();
	}
}
