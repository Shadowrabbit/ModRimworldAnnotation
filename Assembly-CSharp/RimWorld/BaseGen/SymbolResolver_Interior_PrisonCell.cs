using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02001EA5 RID: 7845
	public class SymbolResolver_Interior_PrisonCell : SymbolResolver
	{
		// Token: 0x0600A88D RID: 43149 RVA: 0x00311E58 File Offset: 0x00310058
		public override void Resolve(ResolveParams rp)
		{
			ThingSetMakerParams value = default(ThingSetMakerParams);
			value.techLevel = new TechLevel?((rp.faction != null) ? rp.faction.def.techLevel : TechLevel.Spacer);
			ResolveParams resolveParams = rp;
			resolveParams.thingSetMakerDef = ThingSetMakerDefOf.MapGen_PrisonCellStockpile;
			resolveParams.thingSetMakerParams = new ThingSetMakerParams?(value);
			resolveParams.innerStockpileSize = new int?(3);
			BaseGen.symbolStack.Push("innerStockpile", resolveParams, null);
			InteriorSymbolResolverUtility.PushBedroomHeatersCoolersAndLightSourcesSymbols(rp, false);
			BaseGen.symbolStack.Push("prisonerBed", rp, null);
		}

		// Token: 0x0400724E RID: 29262
		private const int FoodStockpileSize = 3;
	}
}
