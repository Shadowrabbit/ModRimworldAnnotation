using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F45 RID: 8005
	public class QuestNode_GetPlantPlayerCanHarvest : QuestNode
	{
		// Token: 0x0600AAF1 RID: 43761 RVA: 0x0006FE63 File Offset: 0x0006E063
		protected override bool TestRunInt(Slate slate)
		{
			return this.DoWork(slate);
		}

		// Token: 0x0600AAF2 RID: 43762 RVA: 0x0006FE6C File Offset: 0x0006E06C
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0600AAF3 RID: 43763 RVA: 0x0031DB90 File Offset: 0x0031BD90
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

		// Token: 0x0400744A RID: 29770
		[NoTranslate]
		public SlateRef<string> storeHarvestItemDefAs;

		// Token: 0x0400744B RID: 29771
		[NoTranslate]
		public SlateRef<string> storeHarvestItemCountAs;

		// Token: 0x0400744C RID: 29772
		[NoTranslate]
		public SlateRef<string> storeGrowDaysAs;

		// Token: 0x0400744D RID: 29773
		public SlateRef<int> maxPlantGrowDays;

		// Token: 0x0400744E RID: 29774
		public SlateRef<SimpleCurve> pointsToRequiredWorkCurve;

		// Token: 0x0400744F RID: 29775
		public SlateRef<FloatRange?> workRandomFactorRange;

		// Token: 0x04007450 RID: 29776
		private const float TemperatureBuffer = 5f;

		// Token: 0x04007451 RID: 29777
		private const int TemperatureCheckDays = 15;
	}
}
