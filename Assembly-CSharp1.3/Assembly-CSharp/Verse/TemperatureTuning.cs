using System;

namespace Verse
{
	// Token: 0x02000220 RID: 544
	public static class TemperatureTuning
	{
		// Token: 0x04000C2E RID: 3118
		public const float MinimumTemperature = -273.15f;

		// Token: 0x04000C2F RID: 3119
		public const float MaximumTemperature = 1000f;

		// Token: 0x04000C30 RID: 3120
		public const float DefaultTemperature = 21f;

		// Token: 0x04000C31 RID: 3121
		public const float DeepUndergroundTemperature = 15f;

		// Token: 0x04000C32 RID: 3122
		public static readonly SimpleCurve SeasonalTempVariationCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 3f),
				true
			},
			{
				new CurvePoint(0.1f, 4f),
				true
			},
			{
				new CurvePoint(1f, 28f),
				true
			}
		};

		// Token: 0x04000C33 RID: 3123
		public const float DailyTempVariationAmplitude = 7f;

		// Token: 0x04000C34 RID: 3124
		public const float DailySunEffect = 14f;

		// Token: 0x04000C35 RID: 3125
		public const float FoodRefrigerationTemp = 10f;

		// Token: 0x04000C36 RID: 3126
		public const float FoodFreezingTemp = 0f;

		// Token: 0x04000C37 RID: 3127
		public const int RoomTempEqualizeInterval = 120;

		// Token: 0x04000C38 RID: 3128
		public const int Door_TempEqualizeIntervalOpen = 34;

		// Token: 0x04000C39 RID: 3129
		public const int Door_TempEqualizeIntervalClosed = 375;

		// Token: 0x04000C3A RID: 3130
		public const float Door_TempEqualizeRate = 1f;

		// Token: 0x04000C3B RID: 3131
		public const float Vent_TempEqualizeRate = 14f;

		// Token: 0x04000C3C RID: 3132
		public const float InventoryTemperature = 14f;

		// Token: 0x04000C3D RID: 3133
		public const float DropPodTemperature = 14f;

		// Token: 0x04000C3E RID: 3134
		public const float TradeShipTemperature = 14f;

		// Token: 0x04000C3F RID: 3135
		public const float TransporterTemperature = 14f;
	}
}
