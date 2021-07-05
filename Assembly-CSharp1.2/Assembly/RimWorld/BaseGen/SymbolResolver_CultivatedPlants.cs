using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E45 RID: 7749
	public class SymbolResolver_CultivatedPlants : SymbolResolver
	{
		// Token: 0x0600A761 RID: 42849 RVA: 0x0006E8D1 File Offset: 0x0006CAD1
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && (rp.cultivatedPlantDef != null || SymbolResolver_CultivatedPlants.DeterminePlantDef(rp.rect) != null);
		}

		// Token: 0x0600A762 RID: 42850 RVA: 0x0030B38C File Offset: 0x0030958C
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			ThingDef thingDef = rp.cultivatedPlantDef ?? SymbolResolver_CultivatedPlants.DeterminePlantDef(rp.rect);
			if (thingDef == null)
			{
				return;
			}
			float growth = Rand.Range(0.2f, 1f);
			int age = thingDef.plant.LimitedLifespan ? Rand.Range(0, Mathf.Max(thingDef.plant.LifespanTicks - 2500, 0)) : 0;
			foreach (IntVec3 intVec in rp.rect)
			{
				if (map.fertilityGrid.FertilityAt(intVec) >= thingDef.plant.fertilityMin && this.TryDestroyBlockingThingsAt(intVec))
				{
					Plant plant = (Plant)GenSpawn.Spawn(thingDef, intVec, map, WipeMode.Vanish);
					plant.Growth = growth;
					if (plant.def.plant.LimitedLifespan)
					{
						plant.Age = age;
					}
				}
			}
		}

		// Token: 0x0600A763 RID: 42851 RVA: 0x0030B498 File Offset: 0x00309698
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

		// Token: 0x0600A764 RID: 42852 RVA: 0x0030B57C File Offset: 0x0030977C
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

		// Token: 0x040071B9 RID: 29113
		private const float MinPlantGrowth = 0.2f;

		// Token: 0x040071BA RID: 29114
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
