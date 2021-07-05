using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015FB RID: 5627
	public class SymbolResolver_Interior_Brewery : SymbolResolver
	{
		// Token: 0x17001606 RID: 5638
		// (get) Token: 0x060083E5 RID: 33765 RVA: 0x002F36F7 File Offset: 0x002F18F7
		private float SpawnPassiveCoolerIfTemperatureAbove
		{
			get
			{
				return ThingDefOf.FermentingBarrel.GetCompProperties<CompProperties_TemperatureRuinable>().maxSafeTemperature;
			}
		}

		// Token: 0x060083E6 RID: 33766 RVA: 0x002F3708 File Offset: 0x002F1908
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			if (map.mapTemperature.OutdoorTemp > this.SpawnPassiveCoolerIfTemperatureAbove)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingDef = ThingDefOf.PassiveCooler;
				BaseGen.symbolStack.Push("edgeThing", resolveParams, null);
			}
			if (map.mapTemperature.OutdoorTemp < 7f)
			{
				ThingDef singleThingDef;
				if (rp.faction == null || rp.faction.def.techLevel >= TechLevel.Industrial)
				{
					singleThingDef = ThingDefOf.Heater;
				}
				else
				{
					singleThingDef = ThingDefOf.Campfire;
				}
				ResolveParams resolveParams2 = rp;
				resolveParams2.singleThingDef = singleThingDef;
				BaseGen.symbolStack.Push("edgeThing", resolveParams2, null);
			}
			BaseGen.symbolStack.Push("addWortToFermentingBarrels", rp, null);
			ResolveParams resolveParams3 = rp;
			resolveParams3.singleThingDef = ThingDefOf.FermentingBarrel;
			resolveParams3.thingRot = new Rot4?(Rand.Bool ? Rot4.North : Rot4.East);
			resolveParams3.fillWithThingsPadding = new int?(rp.fillWithThingsPadding ?? 1);
			BaseGen.symbolStack.Push("fillWithThings", resolveParams3, null);
		}

		// Token: 0x0400524F RID: 21071
		private const float SpawnHeaterIfTemperatureBelow = 7f;
	}
}
