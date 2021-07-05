using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200085B RID: 2139
	public static class GenTicks
	{
		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x0600357D RID: 13693 RVA: 0x00158AC8 File Offset: 0x00156CC8
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

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x0600357E RID: 13694 RVA: 0x000298E9 File Offset: 0x00027AE9
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

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x0600357F RID: 13695 RVA: 0x00158B18 File Offset: 0x00156D18
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

		// Token: 0x06003580 RID: 13696 RVA: 0x00029905 File Offset: 0x00027B05
		public static float TicksToSeconds(this int numTicks)
		{
			return (float)numTicks / 60f;
		}

		// Token: 0x06003581 RID: 13697 RVA: 0x0002990F File Offset: 0x00027B0F
		public static int SecondsToTicks(this float numSeconds)
		{
			return Mathf.RoundToInt(60f * numSeconds);
		}

		// Token: 0x06003582 RID: 13698 RVA: 0x00158BDC File Offset: 0x00156DDC
		public static string ToStringSecondsFromTicks(this int numTicks)
		{
			return numTicks.TicksToSeconds().ToString("F1") + " " + "SecondsLower".Translate();
		}

		// Token: 0x06003583 RID: 13699 RVA: 0x00158C1C File Offset: 0x00156E1C
		public static string ToStringTicksFromSeconds(this float numSeconds)
		{
			return numSeconds.SecondsToTicks().ToString();
		}

		// Token: 0x04002535 RID: 9525
		public const int TicksPerRealSecond = 60;

		// Token: 0x04002536 RID: 9526
		public const int TickRareInterval = 250;

		// Token: 0x04002537 RID: 9527
		public const int TickLongInterval = 2000;
	}
}
