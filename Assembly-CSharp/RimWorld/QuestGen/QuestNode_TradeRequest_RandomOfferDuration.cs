using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FE3 RID: 8163
	public class QuestNode_TradeRequest_RandomOfferDuration : QuestNode
	{
		// Token: 0x0600AD27 RID: 44327 RVA: 0x00326ADC File Offset: 0x00324CDC
		public static int RandomOfferDurationTicks(int tileIdFrom, int tileIdTo, out int travelTicks)
		{
			int randomInRange = SiteTuning.QuestSiteTimeoutDaysRange.RandomInRange;
			travelTicks = CaravanArrivalTimeEstimator.EstimatedTicksToArrive(tileIdFrom, tileIdTo, null);
			float num = (float)travelTicks / 60000f;
			int num2 = Mathf.CeilToInt(Mathf.Max(num + 6f, num * 1.35f));
			if (num2 > SiteTuning.QuestSiteTimeoutDaysRange.max)
			{
				return -1;
			}
			int num3 = Mathf.Max(randomInRange, num2);
			return 60000 * num3;
		}

		// Token: 0x0600AD28 RID: 44328 RVA: 0x00326B44 File Offset: 0x00324D44
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = slate.Get<Map>("map", null, false);
			int var;
			slate.Set<int>(this.storeAs.GetValue(slate), QuestNode_TradeRequest_RandomOfferDuration.RandomOfferDurationTicks(map.Tile, this.settlement.GetValue(slate).Tile, out var), false);
			slate.Set<int>(this.storeEstimatedTravelTimeAs.GetValue(slate), var, false);
		}

		// Token: 0x0600AD29 RID: 44329 RVA: 0x00326BAC File Offset: 0x00324DAC
		protected override bool TestRunInt(Slate slate)
		{
			Map map = slate.Get<Map>("map", null, false);
			int var;
			slate.Set<int>(this.storeAs.GetValue(slate), QuestNode_TradeRequest_RandomOfferDuration.RandomOfferDurationTicks(map.Tile, this.settlement.GetValue(slate).Tile, out var), false);
			slate.Set<int>(this.storeEstimatedTravelTimeAs.GetValue(slate), var, false);
			return true;
		}

		// Token: 0x040076A8 RID: 30376
		private const float MinNonTravelTimeFractionOfTravelTime = 0.35f;

		// Token: 0x040076A9 RID: 30377
		private const float MinNonTravelTimeDays = 6f;

		// Token: 0x040076AA RID: 30378
		public SlateRef<Settlement> settlement;

		// Token: 0x040076AB RID: 30379
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040076AC RID: 30380
		[NoTranslate]
		public SlateRef<string> storeEstimatedTravelTimeAs;
	}
}
