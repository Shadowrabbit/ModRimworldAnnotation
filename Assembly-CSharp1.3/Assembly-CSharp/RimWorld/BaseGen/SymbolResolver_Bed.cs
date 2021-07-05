using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015B0 RID: 5552
	public class SymbolResolver_Bed : SymbolResolver
	{
		// Token: 0x060082ED RID: 33517 RVA: 0x002E9000 File Offset: 0x002E7200
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
