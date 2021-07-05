using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x0200021C RID: 540
	public class MapTemperature
	{
		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000F6E RID: 3950 RVA: 0x00057A06 File Offset: 0x00055C06
		public float OutdoorTemp
		{
			get
			{
				return Find.World.tileTemperatures.GetOutdoorTemp(this.map.Tile);
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000F6F RID: 3951 RVA: 0x00057A22 File Offset: 0x00055C22
		public float SeasonalTemp
		{
			get
			{
				return Find.World.tileTemperatures.GetSeasonalTemp(this.map.Tile);
			}
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x00057A3E File Offset: 0x00055C3E
		public MapTemperature(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x00057A50 File Offset: 0x00055C50
		public void MapTemperatureTick()
		{
			if (Find.TickManager.TicksGame % 120 == 7 || DebugSettings.fastEcology)
			{
				List<Room> allRooms = this.map.regionGrid.allRooms;
				for (int i = 0; i < allRooms.Count; i++)
				{
					allRooms[i].TempTracker.EqualizeTemperature();
				}
			}
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x00057AA7 File Offset: 0x00055CA7
		public bool SeasonAcceptableFor(ThingDef animalRace)
		{
			return Find.World.tileTemperatures.SeasonAcceptableFor(this.map.Tile, animalRace);
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x00057AC4 File Offset: 0x00055CC4
		public bool OutdoorTemperatureAcceptableFor(ThingDef animalRace)
		{
			return Find.World.tileTemperatures.OutdoorTemperatureAcceptableFor(this.map.Tile, animalRace);
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x00057AE1 File Offset: 0x00055CE1
		public bool SeasonAndOutdoorTemperatureAcceptableFor(ThingDef animalRace)
		{
			return Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(this.map.Tile, animalRace);
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x00057B00 File Offset: 0x00055D00
		public bool LocalSeasonsAreMeaningful()
		{
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < 12; i++)
			{
				float num = Find.World.tileTemperatures.AverageTemperatureForTwelfth(this.map.Tile, (Twelfth)i);
				if (num > 0f)
				{
					flag2 = true;
				}
				if (num < 0f)
				{
					flag = true;
				}
			}
			return flag2 && flag;
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x00057B50 File Offset: 0x00055D50
		public void DebugLogTemps()
		{
			StringBuilder stringBuilder = new StringBuilder();
			float num = (Find.CurrentMap != null) ? Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile).y : 0f;
			stringBuilder.AppendLine("Latitude " + num);
			stringBuilder.AppendLine("-----Temperature for each hour this day------");
			stringBuilder.AppendLine("Hour    Temp    SunEffect");
			int num2 = Find.TickManager.TicksAbs - Find.TickManager.TicksAbs % 60000;
			for (int i = 0; i < 24; i++)
			{
				int absTick = num2 + i * 2500;
				stringBuilder.Append(i.ToString().PadRight(5));
				stringBuilder.Append(Find.World.tileTemperatures.OutdoorTemperatureAt(this.map.Tile, absTick).ToString("F2").PadRight(8));
				stringBuilder.Append(GenTemperature.OffsetFromSunCycle(absTick, this.map.Tile).ToString("F2"));
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("-----Temperature for each twelfth this year------");
			for (int j = 0; j < 12; j++)
			{
				Twelfth twelfth = (Twelfth)j;
				float num3 = Find.World.tileTemperatures.AverageTemperatureForTwelfth(this.map.Tile, twelfth);
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					twelfth.GetQuadrum(),
					"/",
					SeasonUtility.GetReportedSeason(twelfth.GetMiddleYearPct(), num),
					" - ",
					twelfth.ToString(),
					" ",
					num3.ToString("F2")
				}));
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("-----Temperature for each day this year------");
			stringBuilder.AppendLine("Tile avg: " + this.map.TileInfo.temperature + "°C");
			stringBuilder.AppendLine("Seasonal shift: " + GenTemperature.SeasonalShiftAmplitudeAt(this.map.Tile));
			stringBuilder.AppendLine("Equatorial distance: " + Find.WorldGrid.DistanceFromEquatorNormalized(this.map.Tile));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Day  Lo   Hi   OffsetFromSeason RandomDailyVariation");
			for (int k = 0; k < 60; k++)
			{
				int absTick2 = (int)((float)(k * 60000) + 15000f);
				int absTick3 = (int)((float)(k * 60000) + 45000f);
				stringBuilder.Append(k.ToString().PadRight(8));
				stringBuilder.Append(Find.World.tileTemperatures.OutdoorTemperatureAt(this.map.Tile, absTick2).ToString("F2").PadRight(11));
				stringBuilder.Append(Find.World.tileTemperatures.OutdoorTemperatureAt(this.map.Tile, absTick3).ToString("F2").PadRight(11));
				stringBuilder.Append(GenTemperature.OffsetFromSeasonCycle(absTick3, this.map.Tile).ToString("F2").PadRight(11));
				stringBuilder.Append(Find.World.tileTemperatures.OffsetFromDailyRandomVariation(this.map.Tile, absTick3).ToString("F2"));
				stringBuilder.AppendLine();
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04000C23 RID: 3107
		private Map map;
	}
}
