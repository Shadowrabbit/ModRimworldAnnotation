using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001EA6 RID: 7846
	public class SymbolResolver_Interior_Storage : SymbolResolver
	{
		// Token: 0x0600A88F RID: 43151 RVA: 0x00311EE8 File Offset: 0x003100E8
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			BaseGen.symbolStack.Push("stockpile", rp, null);
			if (map.mapTemperature.OutdoorTemp > 15f)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingDef = ThingDefOf.PassiveCooler;
				BaseGen.symbolStack.Push("edgeThing", resolveParams, null);
			}
		}

		// Token: 0x0400724F RID: 29263
		private const float SpawnPassiveCoolerIfTemperatureAbove = 15f;
	}
}
