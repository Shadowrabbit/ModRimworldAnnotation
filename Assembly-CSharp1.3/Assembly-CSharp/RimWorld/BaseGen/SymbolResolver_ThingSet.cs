using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015CB RID: 5579
	public class SymbolResolver_ThingSet : SymbolResolver
	{
		// Token: 0x06008355 RID: 33621 RVA: 0x002ECC2C File Offset: 0x002EAE2C
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			ThingSetMakerDef thingSetMakerDef = rp.thingSetMakerDef ?? ThingSetMakerDefOf.MapGen_DefaultStockpile;
			ThingSetMakerParams parms;
			if (rp.thingSetMakerParams != null)
			{
				parms = rp.thingSetMakerParams.Value;
			}
			else
			{
				int num = rp.rect.Cells.Count((IntVec3 x) => x.Standable(map) && x.GetFirstItem(map) == null);
				parms = default(ThingSetMakerParams);
				parms.countRange = new IntRange?(new IntRange(num, num));
				parms.techLevel = new TechLevel?((rp.faction != null) ? rp.faction.def.techLevel : TechLevel.Undefined);
			}
			parms.makingFaction = rp.faction;
			List<Thing> list = thingSetMakerDef.root.Generate(parms);
			for (int i = 0; i < list.Count; i++)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingToSpawn = list[i];
				BaseGen.symbolStack.Push("thing", resolveParams, null);
			}
		}
	}
}
