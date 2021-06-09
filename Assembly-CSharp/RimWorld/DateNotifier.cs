using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001015 RID: 4117
	public class DateNotifier : IExposable
	{
		// Token: 0x060059D3 RID: 22995 RVA: 0x0003E5CD File Offset: 0x0003C7CD
		public void ExposeData()
		{
			Scribe_Values.Look<Season>(ref this.lastSeason, "lastSeason", Season.Undefined, false);
		}

		// Token: 0x060059D4 RID: 22996 RVA: 0x001D3608 File Offset: 0x001D1808
		public void DateNotifierTick()
		{
			Map map = this.FindPlayerHomeWithMinTimezone();
			float latitude = (map != null) ? Find.WorldGrid.LongLatOf(map.Tile).y : 0f;
			float longitude = (map != null) ? Find.WorldGrid.LongLatOf(map.Tile).x : 0f;
			Season season = GenDate.Season((long)Find.TickManager.TicksAbs, latitude, longitude);
			if (season != this.lastSeason && (this.lastSeason == Season.Undefined || season != this.lastSeason.GetPreviousSeason()))
			{
				if (this.lastSeason != Season.Undefined && this.AnyPlayerHomeSeasonsAreMeaningful())
				{
					if (GenDate.YearsPassed == 0 && season == Season.Summer && this.AnyPlayerHomeAvgTempIsLowInWinter())
					{
						Find.LetterStack.ReceiveLetter("LetterLabelFirstSummerWarning".Translate(), "FirstSummerWarning".Translate(), LetterDefOf.NeutralEvent, null);
					}
					else if (GenDate.DaysPassed > 5)
					{
						Messages.Message("MessageSeasonBegun".Translate(season.Label()).CapitalizeFirst(), MessageTypeDefOf.NeutralEvent, true);
					}
				}
				this.lastSeason = season;
			}
		}

		// Token: 0x060059D5 RID: 22997 RVA: 0x001D3728 File Offset: 0x001D1928
		private Map FindPlayerHomeWithMinTimezone()
		{
			List<Map> maps = Find.Maps;
			Map map = null;
			int num = -1;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					int num2 = GenDate.TimeZoneAt(Find.WorldGrid.LongLatOf(maps[i].Tile).x);
					if (map == null || num2 < num)
					{
						map = maps[i];
						num = num2;
					}
				}
			}
			return map;
		}

		// Token: 0x060059D6 RID: 22998 RVA: 0x001D3794 File Offset: 0x001D1994
		private bool AnyPlayerHomeSeasonsAreMeaningful()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome && maps[i].mapTemperature.LocalSeasonsAreMeaningful())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060059D7 RID: 22999 RVA: 0x001D37DC File Offset: 0x001D19DC
		private bool AnyPlayerHomeAvgTempIsLowInWinter()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome && GenTemperature.AverageTemperatureAtTileForTwelfth(maps[i].Tile, Season.Winter.GetMiddleTwelfth(Find.WorldGrid.LongLatOf(maps[i].Tile).y)) < 8f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04003C86 RID: 15494
		private Season lastSeason;
	}
}
