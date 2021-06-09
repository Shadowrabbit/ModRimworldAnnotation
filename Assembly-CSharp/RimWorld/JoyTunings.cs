using System;

namespace RimWorld
{
	// Token: 0x020014E9 RID: 5353
	public static class JoyTunings
	{
		// Token: 0x04004C14 RID: 19476
		public const float BaseJoyGainPerHour = 0.36f;

		// Token: 0x04004C15 RID: 19477
		public const float ThreshLow = 0.15f;

		// Token: 0x04004C16 RID: 19478
		public const float ThreshSatisfied = 0.3f;

		// Token: 0x04004C17 RID: 19479
		public const float ThreshHigh = 0.7f;

		// Token: 0x04004C18 RID: 19480
		public const float ThreshVeryHigh = 0.85f;

		// Token: 0x04004C19 RID: 19481
		public const float BaseFallPerInterval = 0.0015f;

		// Token: 0x04004C1A RID: 19482
		public const float FallRateFactorWhenLow = 0.7f;

		// Token: 0x04004C1B RID: 19483
		public const float FallRateFactorWhenVeryLow = 0.4f;

		// Token: 0x04004C1C RID: 19484
		public const float ToleranceGainPerJoy = 0.65f;

		// Token: 0x04004C1D RID: 19485
		public const float BoredStartToleranceThreshold = 0.5f;

		// Token: 0x04004C1E RID: 19486
		public const float BoredEndToleranceThreshold = 0.3f;
	}
}
