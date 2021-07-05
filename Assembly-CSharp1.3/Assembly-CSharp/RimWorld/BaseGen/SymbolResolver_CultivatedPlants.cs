using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015B2 RID: 5554
	public class SymbolResolver_CultivatedPlants : SymbolResolver
	{
		// Token: 0x060082F2 RID: 33522 RVA: 0x002E925C File Offset: 0x002E745C
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && (rp.cultivatedPlantDef != null || SymbolResolver_CultivatedPlants.DeterminePlantDef(rp.rect) != null);
		}

		// Token: 0x060082F3 RID: 33523 RVA: 0x002E9284 File Offset: 0x002E7484
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			ThingDef thingDef = rp.cultivatedPlantDef ?? SymbolResolver_CultivatedPlants.DeterminePlantDef(rp.rect);
			if (thingDef == null)
			{
				return;
			}
			float growth = rp.fixedCulativedPlantGrowth ?? Rand.Range(0.2f, 1f);
			int age = thingDef.plant.LimitedLifespan ? Rand.Range(0, Mathf.Max(thingDef.plant.LifespanTicks - 2500, 0)) : 0;
			List<Thing> list = new List<Thing>();
			using (CellRect.Enumerator enumerator = rp.rect.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IntVec3 c = enumerator.Current;
					if (c.InBounds(map) && !list.Any((Thing p) => p.def.plant.blockAdjacentSow && c.IsAdjacentToCardinalOrInside(p.OccupiedRect())) && map.fertilityGrid.FertilityAt(c) >= thingDef.plant.fertilityMin && this.TryDestroyBlockingThingsAt(c))
					{
						Plant plant = (Plant)GenSpawn.Spawn(thingDef, c, map, WipeMode.Vanish);
						list.Add(plant);
						plant.Growth = growth;
						if (plant.def.plant.LimitedLifespan)
						{
							plant.Age = age;
						}
					}
				}
			}
		}

		// Token: 0x060082F4 RID: 33524 RVA: 0x002E9404 File Offset: 0x002E7604
		public static ThingDef DeterminePlantDef(CellRect rect)
		{
			Map map = BaseGen.globalSettings.map;
			if (map.mapTemperature.OutdoorTemp < 0f || map.mapTemperature.OutdoorTemp > 58f)
			{
				return null;
			}
			float minFertility = float.MaxValue;
			bool flag = false;
			foreach (IntVec3 loc in rect)
			{
				float num = map.fertilityGrid.FertilityAt(loc);
				if (num > 0f)
				{
					flag = true;
					minFertility = Mathf.Min(minFertility, num);
				}
			}
			if (!flag)
			{
				return null;
			}
			ThingDef result;
			if ((from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.category == ThingCategory.Plant && x.plant.Sowable && !x.plant.IsTree && !x.plant.cavePlant && x.plant.fertilityMin <= minFertility && x.plant.Harvestable
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x060082F5 RID: 33525 RVA: 0x002E94E8 File Offset: 0x002E76E8
		private bool TryDestroyBlockingThingsAt(IntVec3 c)
		{
			Map map = BaseGen.globalSettings.map;
			SymbolResolver_CultivatedPlants.tmpThings.Clear();
			SymbolResolver_CultivatedPlants.tmpThings.AddRange(c.GetThingList(map));
			for (int i = 0; i < SymbolResolver_CultivatedPlants.tmpThings.Count; i++)
			{
				if (!(SymbolResolver_CultivatedPlants.tmpThings[i] is Pawn) && !SymbolResolver_CultivatedPlants.tmpThings[i].def.destroyable)
				{
					SymbolResolver_CultivatedPlants.tmpThings.Clear();
					return false;
				}
			}
			for (int j = 0; j < SymbolResolver_CultivatedPlants.tmpThings.Count; j++)
			{
				if (!(SymbolResolver_CultivatedPlants.tmpThings[j] is Pawn))
				{
					SymbolResolver_CultivatedPlants.tmpThings[j].Destroy(DestroyMode.Vanish);
				}
			}
			SymbolResolver_CultivatedPlants.tmpThings.Clear();
			return true;
		}

		// Token: 0x040051EE RID: 20974
		private const float MinPlantGrowth = 0.2f;

		// Token: 0x040051EF RID: 20975
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
