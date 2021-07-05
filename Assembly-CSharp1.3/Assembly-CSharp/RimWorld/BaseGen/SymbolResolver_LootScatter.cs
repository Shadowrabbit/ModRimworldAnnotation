using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015C0 RID: 5568
	public class SymbolResolver_LootScatter : SymbolResolver
	{
		// Token: 0x0600832B RID: 33579 RVA: 0x002EB684 File Offset: 0x002E9884
		public override void Resolve(ResolveParams rp)
		{
			SymbolResolver_LootScatter.<>c__DisplayClass1_0 CS$<>8__locals1 = new SymbolResolver_LootScatter.<>c__DisplayClass1_0();
			if (rp.lootMarketValue != null && rp.lootMarketValue.Value <= 0f)
			{
				return;
			}
			CS$<>8__locals1.map = BaseGen.globalSettings.map;
			IList<Thing> list = rp.lootConcereteContents;
			if (list == null)
			{
				ThingSetMakerParams parms;
				if (rp.thingSetMakerParams != null)
				{
					parms = rp.thingSetMakerParams.Value;
				}
				else
				{
					parms = default(ThingSetMakerParams);
					parms.countRange = new IntRange?(SymbolResolver_LootScatter.DefaultLootCountRange);
					parms.techLevel = new TechLevel?((rp.faction != null) ? rp.faction.def.techLevel : TechLevel.Undefined);
				}
				parms.makingFaction = rp.faction;
				parms.totalMarketValueRange = new FloatRange?(new FloatRange(rp.lootMarketValue.Value, rp.lootMarketValue.Value));
				list = rp.thingSetMakerDef.root.Generate(parms);
			}
			List<IntVec3> list2 = (from c in rp.rect.Cells
			where base.<Resolve>g__CanPlace|0(c)
			select c).ToList<IntVec3>();
			while (list2.Count > 0 && list.Count > 0)
			{
				int index = Rand.Range(0, list2.Count);
				IntVec3 loc = list2[index];
				list2.RemoveAt(index);
				index = Rand.Range(0, list.Count);
				Thing newThing = list[index];
				list.RemoveAt(index);
				GenSpawn.Spawn(newThing, loc, CS$<>8__locals1.map, WipeMode.Vanish);
			}
			if (list.Count > 0)
			{
				Log.Warning("Could not scatter loot things in rooms: " + string.Join(", ", from t in list
				select t.Label));
				foreach (Thing newThing2 in list)
				{
					for (int i = 1000; i > 0; i--)
					{
						IntVec3 intVec = CellFinder.RandomCell(CS$<>8__locals1.map);
						if (CS$<>8__locals1.<Resolve>g__CanPlace|0(intVec))
						{
							GenSpawn.Spawn(newThing2, intVec, CS$<>8__locals1.map, WipeMode.Vanish);
						}
					}
				}
			}
		}

		// Token: 0x040051FC RID: 20988
		private static readonly IntRange DefaultLootCountRange = new IntRange(3, 10);
	}
}
