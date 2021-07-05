using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AF0 RID: 2800
	public class FoodRestrictionDatabase : IExposable
	{
		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x060041DF RID: 16863 RVA: 0x00160EB8 File Offset: 0x0015F0B8
		public List<FoodRestriction> AllFoodRestrictions
		{
			get
			{
				return this.foodRestrictions;
			}
		}

		// Token: 0x060041E0 RID: 16864 RVA: 0x00160EC0 File Offset: 0x0015F0C0
		public FoodRestrictionDatabase()
		{
			this.GenerateStartingFoodRestrictions();
		}

		// Token: 0x060041E1 RID: 16865 RVA: 0x00160ED9 File Offset: 0x0015F0D9
		public void ExposeData()
		{
			Scribe_Collections.Look<FoodRestriction>(ref this.foodRestrictions, "foodRestrictions", LookMode.Deep, Array.Empty<object>());
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x060041E2 RID: 16866 RVA: 0x00160EF7 File Offset: 0x0015F0F7
		public FoodRestriction DefaultFoodRestriction()
		{
			if (this.foodRestrictions.Count == 0)
			{
				this.MakeNewFoodRestriction();
			}
			return this.foodRestrictions[0];
		}

		// Token: 0x060041E3 RID: 16867 RVA: 0x00160F1C File Offset: 0x0015F11C
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

		// Token: 0x060041E4 RID: 16868 RVA: 0x0016100C File Offset: 0x0015F20C
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
			foreach (ThingDef thingDef in from x in DefDatabase<ThingDef>.AllDefs
			where x.GetStatValueAbstract(StatDefOf.Nutrition, null) > 0f
			select x)
			{
				foodRestriction.filter.SetAllow(thingDef, true);
			}
			this.foodRestrictions.Add(foodRestriction);
			return foodRestriction;
		}

		// Token: 0x060041E5 RID: 16869 RVA: 0x001610F8 File Offset: 0x0015F2F8
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

		// Token: 0x0400282B RID: 10283
		private List<FoodRestriction> foodRestrictions = new List<FoodRestriction>();
	}
}
