using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020020FC RID: 8444
	public static class CaravanNightRestUtility
	{
		// Token: 0x0600B359 RID: 45913 RVA: 0x00074841 File Offset: 0x00072A41
		public static bool RestingNowAt(int tile)
		{
			return CaravanNightRestUtility.WouldBeRestingAt(tile, (long)GenTicks.TicksAbs);
		}

		// Token: 0x0600B35A RID: 45914 RVA: 0x0033FC1C File Offset: 0x0033DE1C
		public static bool WouldBeRestingAt(int tile, long ticksAbs)
		{
			float num = GenDate.HourFloat(ticksAbs, Find.WorldGrid.LongLatOf(tile).x);
			return num < 6f || num > 22f;
		}

		// Token: 0x0600B35B RID: 45915 RVA: 0x0007484F File Offset: 0x00072A4F
		public static int LeftRestTicksAt(int tile)
		{
			return CaravanNightRestUtility.LeftRestTicksAt(tile, (long)GenTicks.TicksAbs);
		}

		// Token: 0x0600B35C RID: 45916 RVA: 0x0033FC54 File Offset: 0x0033DE54
		public static int LeftRestTicksAt(int tile, long ticksAbs)
		{
			if (!CaravanNightRestUtility.WouldBeRestingAt(tile, ticksAbs))
			{
				return 0;
			}
			float num = GenDate.HourFloat(ticksAbs, Find.WorldGrid.LongLatOf(tile).x);
			if (num < 6f)
			{
				return Mathf.CeilToInt((6f - num) * 2500f);
			}
			return Mathf.CeilToInt((24f - num + 6f) * 2500f);
		}

		// Token: 0x0600B35D RID: 45917 RVA: 0x0007485D File Offset: 0x00072A5D
		public static int LeftNonRestTicksAt(int tile)
		{
			return CaravanNightRestUtility.LeftNonRestTicksAt(tile, (long)GenTicks.TicksAbs);
		}

		// Token: 0x0600B35E RID: 45918 RVA: 0x0033FCB8 File Offset: 0x0033DEB8
		public static int LeftNonRestTicksAt(int tile, long ticksAbs)
		{
			if (CaravanNightRestUtility.WouldBeRestingAt(tile, ticksAbs))
			{
				return 0;
			}
			float num = GenDate.HourFloat(ticksAbs, Find.WorldGrid.LongLatOf(tile).x);
			return Mathf.CeilToInt((22f - num) * 2500f);
		}

		// Token: 0x04007B41 RID: 31553
		public const float WakeUpHour = 6f;

		// Token: 0x04007B42 RID: 31554
		public const float RestStartHour = 22f;
	}
}
