using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E89 RID: 7817
	public class SymbolResolver_IndoorLighting : SymbolResolver
	{
		// Token: 0x0600A836 RID: 43062 RVA: 0x0030FDB4 File Offset: 0x0030DFB4
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			ThingDef thingDef;
			if (rp.faction == null || rp.faction.def.techLevel >= TechLevel.Industrial)
			{
				thingDef = ThingDefOf.StandingLamp;
			}
			else if (map.mapTemperature.OutdoorTemp > 18f)
			{
				thingDef = null;
			}
			else
			{
				thingDef = ThingDefOf.TorchLamp;
			}
			if (thingDef != null)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingDef = thingDef;
				BaseGen.symbolStack.Push("edgeThing", resolveParams, null);
			}
		}

		// Token: 0x04007220 RID: 29216
		private const float NeverSpawnTorchesIfTemperatureAbove = 18f;
	}
}
