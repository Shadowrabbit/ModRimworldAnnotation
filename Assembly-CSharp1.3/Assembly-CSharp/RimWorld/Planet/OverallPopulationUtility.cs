using System;

namespace RimWorld.Planet
{
	// Token: 0x02001745 RID: 5957
	public static class OverallPopulationUtility
	{
		// Token: 0x17001667 RID: 5735
		// (get) Token: 0x060089AE RID: 35246 RVA: 0x00316C88 File Offset: 0x00314E88
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

		// Token: 0x060089AF RID: 35247 RVA: 0x00316CB0 File Offset: 0x00314EB0
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

		// Token: 0x04005741 RID: 22337
		private static int cachedEnumValuesCount = -1;

		// Token: 0x04005742 RID: 22338
		private const float ScaleFactor_AlmostNone = 0.1f;

		// Token: 0x04005743 RID: 22339
		private const float ScaleFactor_Little = 0.4f;

		// Token: 0x04005744 RID: 22340
		private const float ScaleFactor_LittleBitLess = 0.7f;

		// Token: 0x04005745 RID: 22341
		private const float ScaleFactor_Normal = 1f;

		// Token: 0x04005746 RID: 22342
		private const float ScaleFactor_LittleBitMore = 1.5f;

		// Token: 0x04005747 RID: 22343
		private const float ScaleFactor_High = 2f;

		// Token: 0x04005748 RID: 22344
		private const float ScaleFactor_VeryHigh = 2.75f;
	}
}
