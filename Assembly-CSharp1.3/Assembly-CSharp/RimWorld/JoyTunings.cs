using System;

namespace RimWorld
{
	// Token: 0x02000E4C RID: 3660
	public static class JoyTunings
	{
		// Token: 0x04003219 RID: 12825
		public const float BaseJoyGainPerHour = 0.36f;

		// Token: 0x0400321A RID: 12826
		public const float ThreshLow = 0.15f;

		// Token: 0x0400321B RID: 12827
		public const float ThreshSatisfied = 0.3f;

		// Token: 0x0400321C RID: 12828
		public const float ThreshHigh = 0.7f;

		// Token: 0x0400321D RID: 12829
		public const float ThreshVeryHigh = 0.85f;

		// Token: 0x0400321E RID: 12830
		public const float BaseFallPerInterval = 0.0015f;

		// Token: 0x0400321F RID: 12831
		public const float FallRateFactorWhenLow = 0.7f;

		// Token: 0x04003220 RID: 12832
		public const float FallRateFactorWhenVeryLow = 0.4f;

		// Token: 0x04003221 RID: 12833
		public const float ToleranceGainPerJoy = 0.65f;

		// Token: 0x04003222 RID: 12834
		public const float BoredStartToleranceThreshold = 0.5f;

		// Token: 0x04003223 RID: 12835
		public const float BoredEndToleranceThreshold = 0.3f;
	}
}
