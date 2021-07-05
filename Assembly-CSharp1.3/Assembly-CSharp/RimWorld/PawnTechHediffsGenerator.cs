using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DC0 RID: 3520
	public static class PawnTechHediffsGenerator
	{
		// Token: 0x06005177 RID: 20855 RVA: 0x001B6C38 File Offset: 0x001B4E38
		public static void GenerateTechHediffsFor(Pawn pawn)
		{
			float partsMoney = pawn.kindDef.techHediffsMoney.RandomInRange;
			int num = pawn.kindDef.techHediffsMaxAmount;
			if (pawn.kindDef.techHediffsRequired != null)
			{
				foreach (ThingDef thingDef in pawn.kindDef.techHediffsRequired)
				{
					partsMoney -= thingDef.BaseMarketValue;
					num--;
					PawnTechHediffsGenerator.InstallPart(pawn, thingDef);
				}
			}
			if (pawn.kindDef.techHediffsTags == null || pawn.kindDef.techHediffsChance <= 0f)
			{
				return;
			}
			PawnTechHediffsGenerator.tmpGeneratedTechHediffsList.Clear();
			Func<ThingDef, bool> <>9__0;
			for (int i = 0; i < num; i++)
			{
				if (Rand.Value > pawn.kindDef.techHediffsChance)
				{
					return;
				}
				IEnumerable<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefs;
				Func<ThingDef, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((ThingDef x) => x.isTechHediff && !PawnTechHediffsGenerator.tmpGeneratedTechHediffsList.Contains(x) && x.BaseMarketValue <= partsMoney && x.techHediffsTags != null && pawn.kindDef.techHediffsTags.Any((string tag) => x.techHediffsTags.Contains(tag)) && (!pawn.WorkTagIsDisabled(WorkTags.Violent) || !x.violentTechHediff) && (pawn.kindDef.techHediffsDisallowTags == null || !pawn.kindDef.techHediffsDisallowTags.Any((string tag) => x.techHediffsTags.Contains(tag)))));
				}
				IEnumerable<ThingDef> source = allDefs.Where(predicate);
				if (source.Any<ThingDef>())
				{
					ThingDef thingDef2 = source.RandomElementByWeight((ThingDef w) => w.BaseMarketValue);
					partsMoney -= thingDef2.BaseMarketValue;
					PawnTechHediffsGenerator.InstallPart(pawn, thingDef2);
					PawnTechHediffsGenerator.tmpGeneratedTechHediffsList.Add(thingDef2);
				}
			}
		}

		// Token: 0x06005178 RID: 20856 RVA: 0x001B6DEC File Offset: 0x001B4FEC
		private static void InstallPart(Pawn pawn, ThingDef partDef)
		{
			IEnumerable<RecipeDef> source = from x in DefDatabase<RecipeDef>.AllDefs
			where x.IsIngredient(partDef) && pawn.def.AllRecipes.Contains(x)
			select x;
			if (source.Any<RecipeDef>())
			{
				RecipeDef recipeDef = source.RandomElement<RecipeDef>();
				if (recipeDef.Worker.GetPartsToApplyOn(pawn, recipeDef).Any<BodyPartRecord>())
				{
					recipeDef.Worker.ApplyOnPawn(pawn, recipeDef.Worker.GetPartsToApplyOn(pawn, recipeDef).RandomElement<BodyPartRecord>(), null, PawnTechHediffsGenerator.emptyIngredientsList, null);
				}
			}
		}

		// Token: 0x04003047 RID: 12359
		private static List<Thing> emptyIngredientsList = new List<Thing>();

		// Token: 0x04003048 RID: 12360
		private static List<ThingDef> tmpGeneratedTechHediffsList = new List<ThingDef>();
	}
}
