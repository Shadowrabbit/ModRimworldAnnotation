using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001271 RID: 4721
	public class Alert_NeedMealSource : Alert
	{
		// Token: 0x060070FA RID: 28922 RVA: 0x0025A4EC File Offset: 0x002586EC
		public Alert_NeedMealSource()
		{
			this.defaultLabel = "NeedMealSource".Translate();
			this.defaultExplanation = "NeedMealSourceDesc".Translate();
		}

		// Token: 0x060070FB RID: 28923 RVA: 0x0025A520 File Offset: 0x00258720
		public override AlertReport GetReport()
		{
			if (GenDate.DaysPassed < 2)
			{
				return false;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (this.NeedMealSource(maps[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060070FC RID: 28924 RVA: 0x0025A570 File Offset: 0x00258770
		private bool NeedMealSource(Map map)
		{
			if (!map.IsPlayerHome)
			{
				return false;
			}
			if (!map.mapPawns.AnyColonistSpawned)
			{
				return false;
			}
			List<Building> allBuildingsColonist = map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				if (allBuildingsColonist[i].def.building.isMealSource)
				{
					return false;
				}
			}
			return true;
		}
	}
}
