using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017AA RID: 6058
	public static class CaravanNightRestUtility
	{
		// Token: 0x06008C73 RID: 35955 RVA: 0x00326DE9 File Offset: 0x00324FE9
		public static bool RestingNowAt(int tile)
		{
			return CaravanNightRestUtility.WouldBeRestingAt(tile, (long)GenTicks.TicksAbs);
		}

		// Token: 0x06008C74 RID: 35956 RVA: 0x00326DF8 File Offset: 0x00324FF8
		public static bool WouldBeRestingAt(int tile, long ticksAbs)
		{
			float num = GenDate.HourFloat(ticksAbs, Find.WorldGrid.LongLatOf(tile).x);
			return num < 6f || num > 22f;
		}

		// Token: 0x06008C75 RID: 35957 RVA: 0x00326E2E File Offset: 0x0032502E
		public static int LeftRestTicksAt(int tile)
		{
			return CaravanNightRestUtility.LeftRestTicksAt(tile, (long)GenTicks.TicksAbs);
		}

		// Token: 0x06008C76 RID: 35958 RVA: 0x00326E3C File Offset: 0x0032503C
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

		// Token: 0x06008C77 RID: 35959 RVA: 0x00326E9E File Offset: 0x0032509E
		public static int LeftNonRestTicksAt(int tile)
		{
			return CaravanNightRestUtility.LeftNonRestTicksAt(tile, (long)GenTicks.TicksAbs);
		}

		// Token: 0x06008C78 RID: 35960 RVA: 0x00326EAC File Offset: 0x003250AC
		public static int LeftNonRestTicksAt(int tile, long ticksAbs)
		{
			if (CaravanNightRestUtility.WouldBeRestingAt(tile, ticksAbs))
			{
				return 0;
			}
			float num = GenDate.HourFloat(ticksAbs, Find.WorldGrid.LongLatOf(tile).x);
			return Mathf.CeilToInt((22f - num) * 2500f);
		}

		// Token: 0x04005922 RID: 22818
		public const float WakeUpHour = 6f;

		// Token: 0x04005923 RID: 22819
		public const float RestStartHour = 22f;
	}
}
