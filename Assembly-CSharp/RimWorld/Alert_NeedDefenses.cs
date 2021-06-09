using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001943 RID: 6467
	public class Alert_NeedDefenses : Alert
	{
		// Token: 0x06008F50 RID: 36688 RVA: 0x0005FF59 File Offset: 0x0005E159
		public Alert_NeedDefenses()
		{
			this.defaultLabel = "NeedDefenses".Translate();
			this.defaultExplanation = "NeedDefensesDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06008F51 RID: 36689 RVA: 0x00293F70 File Offset: 0x00292170
		public override AlertReport GetReport()
		{
			if (GenDate.DaysPassed < 2 || GenDate.DaysPassed > 5)
			{
				return false;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (this.NeedDefenses(maps[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008F52 RID: 36690 RVA: 0x00293FC8 File Offset: 0x002921C8
		private bool NeedDefenses(Map map)
		{
			if (!map.IsPlayerHome)
			{
				return false;
			}
			if (!map.mapPawns.AnyColonistSpawned && !map.listerBuildings.allBuildingsColonist.Any<Building>())
			{
				return false;
			}
			List<Building> allBuildingsColonist = map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				Building building = allBuildingsColonist[i];
				if ((building.def.building != null && (building.def.building.IsTurret || building.def.building.isTrap)) || building.def == ThingDefOf.Sandbags || building.def == ThingDefOf.Barricade)
				{
					return false;
				}
			}
			return true;
		}
	}
}
