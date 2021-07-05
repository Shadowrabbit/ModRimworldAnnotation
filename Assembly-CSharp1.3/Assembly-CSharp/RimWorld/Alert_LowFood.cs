using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001259 RID: 4697
	public class Alert_LowFood : Alert
	{
		// Token: 0x06007091 RID: 28817 RVA: 0x00257D9C File Offset: 0x00255F9C
		public Alert_LowFood()
		{
			this.defaultLabel = "LowFood".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06007092 RID: 28818 RVA: 0x00257DC0 File Offset: 0x00255FC0
		public override TaggedString GetExplanation()
		{
			Map map = this.MapWithLowFood();
			if (map == null)
			{
				return "";
			}
			float totalHumanEdibleNutrition = map.resourceCounter.TotalHumanEdibleNutrition;
			int num = map.mapPawns.FreeColonistsSpawnedCount + map.mapPawns.PrisonersOfColonyCount;
			int num2 = Mathf.FloorToInt(totalHumanEdibleNutrition / (float)num);
			return "LowFoodDesc".Translate(totalHumanEdibleNutrition.ToString("F0"), num.ToStringCached(), num2.ToStringCached());
		}

		// Token: 0x06007093 RID: 28819 RVA: 0x00257E41 File Offset: 0x00256041
		public override AlertReport GetReport()
		{
			if (Find.TickManager.TicksGame < 150000)
			{
				return false;
			}
			return this.MapWithLowFood() != null;
		}

		// Token: 0x06007094 RID: 28820 RVA: 0x00257E6C File Offset: 0x0025606C
		private Map MapWithLowFood()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map.IsPlayerHome && map.mapPawns.AnyColonistSpawned)
				{
					int freeColonistsSpawnedCount = map.mapPawns.FreeColonistsSpawnedCount;
					if (map.resourceCounter.TotalHumanEdibleNutrition < 4f * (float)freeColonistsSpawnedCount)
					{
						return map;
					}
				}
			}
			return null;
		}

		// Token: 0x04003E13 RID: 15891
		private const float NutritionThresholdPerColonist = 4f;
	}
}
