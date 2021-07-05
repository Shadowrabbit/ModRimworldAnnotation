using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015E2 RID: 5602
	public class SymbolResolver_IndoorLighting : SymbolResolver
	{
		// Token: 0x06008397 RID: 33687 RVA: 0x002F0288 File Offset: 0x002EE488
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

		// Token: 0x04005229 RID: 21033
		private const float NeverSpawnTorchesIfTemperatureAbove = 18f;
	}
}
