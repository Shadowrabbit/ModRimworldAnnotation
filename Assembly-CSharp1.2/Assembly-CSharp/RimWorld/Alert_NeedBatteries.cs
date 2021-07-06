using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001963 RID: 6499
	public class Alert_NeedBatteries : Alert
	{
		// Token: 0x06008FD1 RID: 36817 RVA: 0x0006067B File Offset: 0x0005E87B
		public Alert_NeedBatteries()
		{
			this.defaultLabel = "NeedBatteries".Translate();
			this.defaultExplanation = "NeedBatteriesDesc".Translate();
		}

		// Token: 0x06008FD2 RID: 36818 RVA: 0x002967A4 File Offset: 0x002949A4
		public override AlertReport GetReport()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (this.NeedBatteries(maps[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008FD3 RID: 36819 RVA: 0x002967E4 File Offset: 0x002949E4
		private bool NeedBatteries(Map map)
		{
			if (!map.IsPlayerHome)
			{
				return false;
			}
			return !map.listerBuildings.ColonistsHaveBuilding((Thing building) => building is Building_Battery) && (map.listerBuildings.ColonistsHaveBuilding(ThingDefOf.SolarGenerator) || map.listerBuildings.ColonistsHaveBuilding(ThingDefOf.WindTurbine)) && !map.listerBuildings.ColonistsHaveBuilding(ThingDefOf.GeothermalGenerator) && !map.listerBuildings.ColonistsHaveBuilding(ThingDefOf.WatermillGenerator);
		}
	}
}
