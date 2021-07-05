using System;

namespace Verse
{
	// Token: 0x020002A2 RID: 674
	public static class HediffGrowthModeUtility
	{
		// Token: 0x06001288 RID: 4744 RVA: 0x0006AAEC File Offset: 0x00068CEC
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
