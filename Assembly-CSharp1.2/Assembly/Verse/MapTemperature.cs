using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x0200030C RID: 780
	public class MapTemperature
	{
		// Token: 0x170003AA RID: 938
		// (get) Token: 0x060013F2 RID: 5106 RVA: 0x00014507 File Offset: 0x00012707
		public float OutdoorTemp
		{
			get
			{
				return Find.World.tileTemperatures.GetOutdoorTemp(this.map.Tile);
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x060013F3 RID: 5107 RVA: 0x00014523 File Offset: 0x00012723
		public float SeasonalTemp
		{
			get
			{
				return Find.World.tileTemperatures.GetSeasonalTemp(this.map.Tile);
			}
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x0001453F File Offset: 0x0001273F
		public MapTemperature(Map map)
		{
			this.map = map;
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x000CC874 File Offset: 0x000CAA74
		public void MapTemperatureTick()
		{
			if (Find.TickManager.TicksGame % 120 == 7 || DebugSettings.fastEcology)
			{
				this.fastProcessedRoomGroups.Clear();
				List<Room> allRooms = this.map.regionGrid.allRooms;
				for (int i = 0; i < allRooms.Count; i++)
				{
					RoomGroup group = allRooms[i].Group;
					if (!this.fastProcessedRoomGroups.Contains(group))
					{
						group.TempTracker.EqualizeTemperature();
						this.fastProcessedRoomGroups.Add(group);
					}
				}
				this.fastProcessedRoomGroups.Clear();
			}
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x00014559 File Offset: 0x00012759
		public bool SeasonAcceptableFor(ThingDef animalRace)
		{
			return Find.World.tileTemperatures.SeasonAcceptableFor(this.map.Tile, animalRace);
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x00014576 File Offset: 0x00012776
		public bool OutdoorTemperatureAcceptableFor(ThingDef animalRace)
		{
			return Find.World.tileTemperatures.OutdoorTemperatureAcceptableFor(this.map.Tile, animalRace);
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x00014593 File Offset: 0x00012793
		public bool SeasonAndOutdoorTemperatureAcceptableFor(ThingDef animalRace)
		{
			return Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(this.map.Tile, animalRace);
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x000CC904 File Offset: 0x000CAB04
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

		// Token: 0x060013FA RID: 5114 RVA: 0x000CC954 File Offset: 0x000CAB54
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
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x04000FAC RID: 4012
		private Map map;

		// Token: 0x04000FAD RID: 4013
		private HashSet<RoomGroup> fastProcessedRoomGroups = new HashSet<RoomGroup>();
	}
}
