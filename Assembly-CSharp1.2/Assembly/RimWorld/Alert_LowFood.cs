using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001948 RID: 6472
	public class Alert_LowFood : Alert
	{
		// Token: 0x06008F61 RID: 36705 RVA: 0x00060092 File Offset: 0x0005E292
		public Alert_LowFood()
		{
			this.defaultLabel = "LowFood".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06008F62 RID: 36706 RVA: 0x002945A4 File Offset: 0x002927A4
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

		// Token: 0x06008F63 RID: 36707 RVA: 0x000600B6 File Offset: 0x0005E2B6
		public override AlertReport GetReport()
		{
			if (Find.TickManager.TicksGame < 150000)
			{
				return false;
			}
			return this.MapWithLowFood() != null;
		}

		// Token: 0x06008F64 RID: 36708 RVA: 0x00294628 File Offset: 0x00292828
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

		// Token: 0x04005B60 RID: 23392
		private const float NutritionThresholdPerColonist = 4f;
	}
}
