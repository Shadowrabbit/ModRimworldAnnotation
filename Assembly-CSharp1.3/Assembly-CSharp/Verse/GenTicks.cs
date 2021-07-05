using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004BE RID: 1214
	public static class GenTicks
	{
		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x0600251A RID: 9498 RVA: 0x000E7180 File Offset: 0x000E5380
		public static int TicksAbs
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing && Find.GameInitData != null && Find.GameInitData.gameToLoad.NullOrEmpty())
				{
					return GenTicks.ConfiguredTicksAbsAtGameStart;
				}
				if (Current.Game != null && Find.TickManager != null)
				{
					return Find.TickManager.TicksAbs;
				}
				return 0;
			}
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x0600251B RID: 9499 RVA: 0x000E71CD File Offset: 0x000E53CD
		public static int TicksGame
		{
			get
			{
				if (Current.Game != null && Find.TickManager != null)
				{
					return Find.TickManager.TicksGame;
				}
				return 0;
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x0600251C RID: 9500 RVA: 0x000E71EC File Offset: 0x000E53EC
		public static int ConfiguredTicksAbsAtGameStart
		{
			get
			{
				GameInitData gameInitData = Find.GameInitData;
				ConfiguredTicksAbsAtGameStartCache ticksAbsCache = Find.World.ticksAbsCache;
				int result;
				if (ticksAbsCache.TryGetCachedValue(gameInitData, out result))
				{
					return result;
				}
				Vector2 vector;
				if (gameInitData.startingTile >= 0)
				{
					vector = Find.WorldGrid.LongLatOf(gameInitData.startingTile);
				}
				else
				{
					vector = Vector2.zero;
				}
				Twelfth twelfth;
				if (gameInitData.startingSeason != Season.Undefined)
				{
					twelfth = gameInitData.startingSeason.GetFirstTwelfth(vector.y);
				}
				else if (gameInitData.startingTile >= 0)
				{
					twelfth = TwelfthUtility.FindStartingWarmTwelfth(gameInitData.startingTile);
				}
				else
				{
					twelfth = Season.Summer.GetFirstTwelfth(0f);
				}
				int num = (24 - GenDate.TimeZoneAt(vector.x)) % 24;
				int num2 = 300000 * (int)twelfth + 2500 * (6 + num);
				ticksAbsCache.Cache(num2, gameInitData);
				return num2;
			}
		}

		// Token: 0x0600251D RID: 9501 RVA: 0x000E72AE File Offset: 0x000E54AE
		public static float TicksToSeconds(this int numTicks)
		{
			return (float)numTicks / 60f;
		}

		// Token: 0x0600251E RID: 9502 RVA: 0x000E72B8 File Offset: 0x000E54B8
		public static int SecondsToTicks(this float numSeconds)
		{
			return Mathf.RoundToInt(60f * numSeconds);
		}

		// Token: 0x0600251F RID: 9503 RVA: 0x000E72C8 File Offset: 0x000E54C8
		public static string ToStringSecondsFromTicks(this int numTicks)
		{
			return numTicks.TicksToSeconds().ToString("F1") + " " + "SecondsLower".Translate();
		}

		// Token: 0x06002520 RID: 9504 RVA: 0x000E7308 File Offset: 0x000E5508
		public static string ToStringTicksFromSeconds(this float numSeconds)
		{
			return numSeconds.SecondsToTicks().ToString();
		}

		// Token: 0x04001720 RID: 5920
		public const int TicksPerRealSecond = 60;

		// Token: 0x04001721 RID: 5921
		public const int TickRareInterval = 250;

		// Token: 0x04001722 RID: 5922
		public const int TickLongInterval = 2000;
	}
}
