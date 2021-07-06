using System;

namespace RimWorld.Planet
{
	// Token: 0x02002032 RID: 8242
	public static class OverallPopulationUtility
	{
		// Token: 0x170019C7 RID: 6599
		// (get) Token: 0x0600AED7 RID: 44759 RVA: 0x00071CC1 File Offset: 0x0006FEC1
		public static int EnumValuesCount
		{
			get
			{
				if (OverallPopulationUtility.cachedEnumValuesCount < 0)
				{
					OverallPopulationUtility.cachedEnumValuesCount = Enum.GetNames(typeof(OverallPopulation)).Length;
				}
				return OverallPopulationUtility.cachedEnumValuesCount;
			}
		}

		// Token: 0x0600AED8 RID: 44760 RVA: 0x0032CD7C File Offset: 0x0032AF7C
		public static float GetScaleFactor(this OverallPopulation population)
		{
			switch (population)
			{
			case OverallPopulation.AlmostNone:
				return 0.1f;
			case OverallPopulation.Little:
				return 0.4f;
			case OverallPopulation.LittleBitLess:
				return 0.7f;
			case OverallPopulation.Normal:
				return 1f;
			case OverallPopulation.LittleBitMore:
				return 1.5f;
			case OverallPopulation.High:
				return 2f;
			case OverallPopulation.VeryHigh:
				return 2.75f;
			default:
				return 1f;
			}
		}

		// Token: 0x040077EE RID: 30702
		private static int cachedEnumValuesCount = -1;

		// Token: 0x040077EF RID: 30703
		private const float ScaleFactor_AlmostNone = 0.1f;

		// Token: 0x040077F0 RID: 30704
		private const float ScaleFactor_Little = 0.4f;

		// Token: 0x040077F1 RID: 30705
		private const float ScaleFactor_LittleBitLess = 0.7f;

		// Token: 0x040077F2 RID: 30706
		private const float ScaleFactor_Normal = 1f;

		// Token: 0x040077F3 RID: 30707
		private const float ScaleFactor_LittleBitMore = 1.5f;

		// Token: 0x040077F4 RID: 30708
		private const float ScaleFactor_High = 2f;

		// Token: 0x040077F5 RID: 30709
		private const float ScaleFactor_VeryHigh = 2.75f;
	}
}
