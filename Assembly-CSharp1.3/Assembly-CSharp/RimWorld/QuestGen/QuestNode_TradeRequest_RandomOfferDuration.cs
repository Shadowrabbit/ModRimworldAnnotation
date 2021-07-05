using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200170F RID: 5903
	public class QuestNode_TradeRequest_RandomOfferDuration : QuestNode
	{
		// Token: 0x06008854 RID: 34900 RVA: 0x0030FE44 File Offset: 0x0030E044
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

		// Token: 0x06008855 RID: 34901 RVA: 0x0030FEAC File Offset: 0x0030E0AC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = slate.Get<Map>("map", null, false);
			int var;
			slate.Set<int>(this.storeAs.GetValue(slate), QuestNode_TradeRequest_RandomOfferDuration.RandomOfferDurationTicks(map.Tile, this.settlement.GetValue(slate).Tile, out var), false);
			slate.Set<int>(this.storeEstimatedTravelTimeAs.GetValue(slate), var, false);
		}

		// Token: 0x06008856 RID: 34902 RVA: 0x0030FF14 File Offset: 0x0030E114
		protected override bool TestRunInt(Slate slate)
		{
			Map map = slate.Get<Map>("map", null, false);
			int var;
			slate.Set<int>(this.storeAs.GetValue(slate), QuestNode_TradeRequest_RandomOfferDuration.RandomOfferDurationTicks(map.Tile, this.settlement.GetValue(slate).Tile, out var), false);
			slate.Set<int>(this.storeEstimatedTravelTimeAs.GetValue(slate), var, false);
			return true;
		}

		// Token: 0x0400563B RID: 22075
		private const float MinNonTravelTimeFractionOfTravelTime = 0.35f;

		// Token: 0x0400563C RID: 22076
		private const float MinNonTravelTimeDays = 6f;

		// Token: 0x0400563D RID: 22077
		public SlateRef<Settlement> settlement;

		// Token: 0x0400563E RID: 22078
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400563F RID: 22079
		[NoTranslate]
		public SlateRef<string> storeEstimatedTravelTimeAs;
	}
}
