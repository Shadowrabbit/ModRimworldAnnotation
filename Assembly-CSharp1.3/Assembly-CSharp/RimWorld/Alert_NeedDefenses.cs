using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001254 RID: 4692
	public class Alert_NeedDefenses : Alert
	{
		// Token: 0x06007080 RID: 28800 RVA: 0x0025762C File Offset: 0x0025582C
		public Alert_NeedDefenses()
		{
			this.defaultLabel = "NeedDefenses".Translate();
			this.defaultExplanation = "NeedDefensesDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06007081 RID: 28801 RVA: 0x00257668 File Offset: 0x00255868
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

		// Token: 0x06007082 RID: 28802 RVA: 0x002576C0 File Offset: 0x002558C0
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
