using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001602 RID: 5634
	public class SymbolResolver_Interior_Storage : SymbolResolver
	{
		// Token: 0x060083F8 RID: 33784 RVA: 0x002F3D58 File Offset: 0x002F1F58
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

		// Token: 0x04005252 RID: 21074
		private const float SpawnPassiveCoolerIfTemperatureAbove = 15f;
	}
}
