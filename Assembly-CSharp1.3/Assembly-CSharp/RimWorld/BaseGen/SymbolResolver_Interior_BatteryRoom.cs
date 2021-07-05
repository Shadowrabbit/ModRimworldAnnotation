using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015FA RID: 5626
	public class SymbolResolver_Interior_BatteryRoom : SymbolResolver
	{
		// Token: 0x060083E3 RID: 33763 RVA: 0x002F3664 File Offset: 0x002F1864
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("indoorLighting", rp, null);
			BaseGen.symbolStack.Push("chargeBatteries", rp, null);
			ResolveParams resolveParams = rp;
			resolveParams.singleThingDef = ThingDefOf.Battery;
			resolveParams.thingRot = new Rot4?(Rand.Bool ? Rot4.North : Rot4.East);
			resolveParams.fillWithThingsPadding = new int?(rp.fillWithThingsPadding ?? 1);
			BaseGen.symbolStack.Push("fillWithThings", resolveParams, null);
		}
	}
}
