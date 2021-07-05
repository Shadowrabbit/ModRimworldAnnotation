using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001682 RID: 5762
	public class QuestNode_GetPlantPlayerCanHarvest : QuestNode
	{
		// Token: 0x06008617 RID: 34327 RVA: 0x00301BC3 File Offset: 0x002FFDC3
		protected override bool TestRunInt(Slate slate)
		{
			return this.DoWork(slate);
		}

		// Token: 0x06008618 RID: 34328 RVA: 0x00301BCC File Offset: 0x002FFDCC
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x06008619 RID: 34329 RVA: 0x00301BDC File Offset: 0x002FFDDC
		private bool DoWork(Slate slate)
		{
			Map map = slate.Get<Map>("map", null, false);
			if (map == null)
			{
				return false;
			}
			float x = slate.Get<float>("points", 0f, false);
			float seasonalTemp = Find.World.tileTemperatures.GetSeasonalTemp(map.Tile);
			int ticksAbs = GenTicks.TicksAbs;
			for (int i = 0; i < 15; i++)
			{
				int absTick = ticksAbs + 60000 * i;
				float num = seasonalTemp + Find.World.tileTemperatures.OffsetFromDailyRandomVariation(map.Tile, absTick);
				if (num <= 5f || num >= 53f)
				{
					return false;
				}
			}
			ThingDef thingDef;
			if (!(from def in DefDatabase<ThingDef>.AllDefs
			where def.category == ThingCategory.Plant && !def.plant.cavePlant && def.plant.Sowable && def.plant.harvestedThingDef != null && def.plant.growDays <= (float)this.maxPlantGrowDays.GetValue(slate) && Command_SetPlantToGrow.IsPlantAvailable(def, map)
			select def).TryRandomElement(out thingDef))
			{
				return false;
			}
			SimpleCurve value = this.pointsToRequiredWorkCurve.GetValue(slate);
			float randomInRange = (this.workRandomFactorRange.GetValue(slate) ?? FloatRange.One).RandomInRange;
			float num2 = value.Evaluate(x) * randomInRange;
			float num3 = (thingDef.plant.sowWork + thingDef.plant.harvestWork) / thingDef.plant.harvestYield;
			int num4 = GenMath.RoundRandom(num2 / num3);
			num4 = Mathf.Max(num4, 1);
			slate.Set<ThingDef>(this.storeHarvestItemDefAs.GetValue(slate), thingDef.plant.harvestedThingDef, false);
			slate.Set<int>(this.storeHarvestItemCountAs.GetValue(slate), num4, false);
			if (this.storeGrowDaysAs.GetValue(slate) != null)
			{
				slate.Set<float>(this.storeGrowDaysAs.GetValue(slate), thingDef.plant.growDays, false);
			}
			return true;
		}

		// Token: 0x040053EB RID: 21483
		[NoTranslate]
		public SlateRef<string> storeHarvestItemDefAs;

		// Token: 0x040053EC RID: 21484
		[NoTranslate]
		public SlateRef<string> storeHarvestItemCountAs;

		// Token: 0x040053ED RID: 21485
		[NoTranslate]
		public SlateRef<string> storeGrowDaysAs;

		// Token: 0x040053EE RID: 21486
		public SlateRef<int> maxPlantGrowDays;

		// Token: 0x040053EF RID: 21487
		public SlateRef<SimpleCurve> pointsToRequiredWorkCurve;

		// Token: 0x040053F0 RID: 21488
		public SlateRef<FloatRange?> workRandomFactorRange;

		// Token: 0x040053F1 RID: 21489
		private const float TemperatureBuffer = 5f;

		// Token: 0x040053F2 RID: 21490
		private const int TemperatureCheckDays = 15;
	}
}
