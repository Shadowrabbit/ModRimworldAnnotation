using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E43 RID: 7747
	public class SymbolResolver_Bed : SymbolResolver
	{
		// Token: 0x0600A75C RID: 42844 RVA: 0x0030B13C File Offset: 0x0030933C
		public override void Resolve(ResolveParams rp)
		{
			ThingDef singleThingDef = rp.singleThingDef ?? Rand.Element<ThingDef>(ThingDefOf.Bed, ThingDefOf.Bedroll, ThingDefOf.SleepingSpot);
			ResolveParams resolveParams = rp;
			resolveParams.singleThingDef = singleThingDef;
			resolveParams.skipSingleThingIfHasToWipeBuildingOrDoesntFit = new bool?(rp.skipSingleThingIfHasToWipeBuildingOrDoesntFit ?? true);
			BaseGen.symbolStack.Push("thing", resolveParams, null);
		}
	}
}
