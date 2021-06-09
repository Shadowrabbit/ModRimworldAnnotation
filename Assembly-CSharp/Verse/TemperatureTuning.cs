using System;

namespace Verse
{
	// Token: 0x02000311 RID: 785
	public static class TemperatureTuning
	{
		// Token: 0x04000FBA RID: 4026
		public const float MinimumTemperature = -273.15f;

		// Token: 0x04000FBB RID: 4027
		public const float MaximumTemperature = 1000f;

		// Token: 0x04000FBC RID: 4028
		public const float DefaultTemperature = 21f;

		// Token: 0x04000FBD RID: 4029
		public const float DeepUndergroundTemperature = 15f;

		// Token: 0x04000FBE RID: 4030
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

		// Token: 0x04000FBF RID: 4031
		public const float DailyTempVariationAmplitude = 7f;

		// Token: 0x04000FC0 RID: 4032
		public const float DailySunEffect = 14f;

		// Token: 0x04000FC1 RID: 4033
		public const float FoodRefrigerationTemp = 10f;

		// Token: 0x04000FC2 RID: 4034
		public const float FoodFreezingTemp = 0f;

		// Token: 0x04000FC3 RID: 4035
		public const int RoomTempEqualizeInterval = 120;

		// Token: 0x04000FC4 RID: 4036
		public const int Door_TempEqualizeIntervalOpen = 34;

		// Token: 0x04000FC5 RID: 4037
		public const int Door_TempEqualizeIntervalClosed = 375;

		// Token: 0x04000FC6 RID: 4038
		public const float Door_TempEqualizeRate = 1f;

		// Token: 0x04000FC7 RID: 4039
		public const float Vent_TempEqualizeRate = 14f;

		// Token: 0x04000FC8 RID: 4040
		public const float InventoryTemperature = 14f;

		// Token: 0x04000FC9 RID: 4041
		public const float DropPodTemperature = 14f;

		// Token: 0x04000FCA RID: 4042
		public const float TradeShipTemperature = 14f;
	}
}
