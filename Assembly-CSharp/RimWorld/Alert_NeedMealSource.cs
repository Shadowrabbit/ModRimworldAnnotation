using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001960 RID: 6496
	public class Alert_NeedMealSource : Alert
	{
		// Token: 0x06008FC5 RID: 36805 RVA: 0x000605E0 File Offset: 0x0005E7E0
		public Alert_NeedMealSource()
		{
			this.defaultLabel = "NeedMealSource".Translate();
			this.defaultExplanation = "NeedMealSourceDesc".Translate();
		}

		// Token: 0x06008FC6 RID: 36806 RVA: 0x00296458 File Offset: 0x00294658
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

		// Token: 0x06008FC7 RID: 36807 RVA: 0x002964A8 File Offset: 0x002946A8
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
