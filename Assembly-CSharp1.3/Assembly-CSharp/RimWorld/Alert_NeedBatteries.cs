using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001274 RID: 4724
	public class Alert_NeedBatteries : Alert
	{
		// Token: 0x06007106 RID: 28934 RVA: 0x0025A8E8 File Offset: 0x00258AE8
		public Alert_NeedBatteries()
		{
			this.defaultLabel = "NeedBatteries".Translate();
			this.defaultExplanation = "NeedBatteriesDesc".Translate();
		}

		// Token: 0x06007107 RID: 28935 RVA: 0x0025A91C File Offset: 0x00258B1C
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

		// Token: 0x06007108 RID: 28936 RVA: 0x0025A95C File Offset: 0x00258B5C
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
