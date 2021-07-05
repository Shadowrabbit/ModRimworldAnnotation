using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020015FE RID: 5630
	public class SymbolResolver_Interior_PrisonCell : SymbolResolver
	{
		// Token: 0x060083EC RID: 33772 RVA: 0x002F38D8 File Offset: 0x002F1AD8
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

		// Token: 0x04005250 RID: 21072
		private const int FoodStockpileSize = 3;
	}
}
