using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001EA2 RID: 7842
	public class SymbolResolver_Interior_Brewery : SymbolResolver
	{
		// Token: 0x1700196D RID: 6509
		// (get) Token: 0x0600A886 RID: 43142 RVA: 0x0006EF47 File Offset: 0x0006D147
		private float SpawnPassiveCoolerIfTemperatureAbove
		{
			get
			{
				return ThingDefOf.FermentingBarrel.GetCompProperties<CompProperties_TemperatureRuinable>().maxSafeTemperature;
			}
		}

		// Token: 0x0600A887 RID: 43143 RVA: 0x00311C88 File Offset: 0x0030FE88
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

		// Token: 0x0400724D RID: 29261
		private const float SpawnHeaterIfTemperatureBelow = 7f;
	}
}
