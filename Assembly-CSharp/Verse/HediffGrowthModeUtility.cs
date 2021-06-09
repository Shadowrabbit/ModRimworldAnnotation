using System;

namespace Verse
{
	// Token: 0x020003DE RID: 990
	public static class HediffGrowthModeUtility
	{
		// Token: 0x06001850 RID: 6224 RVA: 0x000DEF1C File Offset: 0x000DD11C
		public static string GetLabel(this HediffGrowthMode m)
		{
			switch (m)
			{
			case HediffGrowthMode.Growing:
				return "HediffGrowthMode_Growing".Translate();
			case HediffGrowthMode.Stable:
				return "HediffGrowthMode_Stable".Translate();
			case HediffGrowthMode.Remission:
				return "HediffGrowthMode_Remission".Translate();
			default:
				throw new ArgumentException();
			}
		}
	}
}
