using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001339 RID: 4921
	public class WeatherDecider : IExposable
	{
		// Token: 0x1700106F RID: 4207
		// (get) Token: 0x06006ABE RID: 27326 RVA: 0x0020F7C4 File Offset: 0x0020D9C4
		public WeatherDef ForcedWeather
		{
			get
			{
				WeatherDecider.allConditionsTmp.Clear();
				this.map.gameConditionManager.GetAllGameConditionsAffectingMap(this.map, WeatherDecider.allConditionsTmp);
				WeatherDef result = null;
				foreach (GameCondition gameCondition in WeatherDecider.allConditionsTmp)
				{
					WeatherDef weatherDef = gameCondition.ForcedWeather();
					if (weatherDef != null)
					{
						result = weatherDef;
					}
				}
				return result;
			}
		}

		// Token: 0x06006ABF RID: 27327 RVA: 0x000489AD File Offset: 0x00046BAD
		public WeatherDecider(Map map)
		{
			this.map = map;
		}

		// Token: 0x06006AC0 RID: 27328 RVA: 0x000489C7 File Offset: 0x00046BC7
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.curWeatherDuration, "curWeatherDuration", 0, true);
			Scribe_Values.Look<int>(ref this.ticksWhenRainAllowedAgain, "ticksWhenRainAllowedAgain", 0, false);
		}

		// Token: 0x06006AC1 RID: 27329 RVA: 0x0020F844 File Offset: 0x0020DA44
		public void WeatherDeciderTick()
		{
			WeatherDef forcedWeather = this.ForcedWeather;
			int num = this.curWeatherDuration;
			if (this.map.fireWatcher.LargeFireDangerPresent || !this.map.weatherManager.curWeather.temperatureRange.Includes(this.map.mapTemperature.OutdoorTemp))
			{
				num = (int)((float)num * 0.25f);
			}
			if (forcedWeather != null && this.map.weatherManager.curWeather != forcedWeather)
			{
				num = 4000;
			}
			if (this.map.weatherManager.curWeatherAge > num)
			{
				this.StartNextWeather();
			}
		}

		// Token: 0x06006AC2 RID: 27330 RVA: 0x0020F8E4 File Offset: 0x0020DAE4
		public void StartNextWeather()
		{
			WeatherDef weatherDef = this.ChooseNextWeather();
			this.map.weatherManager.TransitionTo(weatherDef);
			this.curWeatherDuration = weatherDef.durationRange.RandomInRange;
		}

		// Token: 0x06006AC3 RID: 27331 RVA: 0x0020F91C File Offset: 0x0020DB1C
		public void StartInitialWeather()
		{
			if (Find.GameInitData != null)
			{
				this.map.weatherManager.curWeather = WeatherDefOf.Clear;
				this.curWeatherDuration = 10000;
				this.map.weatherManager.curWeatherAge = 0;
				return;
			}
			this.map.weatherManager.curWeather = null;
			WeatherDef weatherDef = this.ChooseNextWeather();
			WeatherDef lastWeather = this.ChooseNextWeather();
			this.map.weatherManager.curWeather = weatherDef;
			this.map.weatherManager.lastWeather = lastWeather;
			this.curWeatherDuration = weatherDef.durationRange.RandomInRange;
			this.map.weatherManager.curWeatherAge = Rand.Range(0, this.curWeatherDuration);
		}

		// Token: 0x06006AC4 RID: 27332 RVA: 0x0020F9D0 File Offset: 0x0020DBD0
		private WeatherDef ChooseNextWeather()
		{
			if (TutorSystem.TutorialMode)
			{
				return WeatherDefOf.Clear;
			}
			WeatherDef forcedWeather = this.ForcedWeather;
			if (forcedWeather != null)
			{
				return forcedWeather;
			}
			WeatherDef result;
			if (!DefDatabase<WeatherDef>.AllDefs.TryRandomElementByWeight((WeatherDef w) => this.CurrentWeatherCommonality(w), out result))
			{
				Log.Warning("All weather commonalities were zero. Defaulting to " + WeatherDefOf.Clear.defName + ".", false);
				return WeatherDefOf.Clear;
			}
			return result;
		}

		// Token: 0x06006AC5 RID: 27333 RVA: 0x000489ED File Offset: 0x00046BED
		public void DisableRainFor(int ticks)
		{
			this.ticksWhenRainAllowedAgain = Find.TickManager.TicksGame + ticks;
		}

		// Token: 0x06006AC6 RID: 27334 RVA: 0x0020FA38 File Offset: 0x0020DC38
		private float CurrentWeatherCommonality(WeatherDef weather)
		{
			if (this.map.weatherManager.curWeather != null && !this.map.weatherManager.curWeather.repeatable && weather == this.map.weatherManager.curWeather)
			{
				return 0f;
			}
			if (!weather.temperatureRange.Includes(this.map.mapTemperature.OutdoorTemp))
			{
				return 0f;
			}
			if (weather.favorability < Favorability.Neutral && GenDate.DaysPassed < 8)
			{
				return 0f;
			}
			if (weather.rainRate > 0.1f && Find.TickManager.TicksGame < this.ticksWhenRainAllowedAgain)
			{
				return 0f;
			}
			if (weather.rainRate > 0.1f)
			{
				if (this.map.gameConditionManager.ActiveConditions.Any((GameCondition x) => x.def.preventRain))
				{
					return 0f;
				}
			}
			BiomeDef biome = this.map.Biome;
			for (int i = 0; i < biome.baseWeatherCommonalities.Count; i++)
			{
				WeatherCommonalityRecord weatherCommonalityRecord = biome.baseWeatherCommonalities[i];
				if (weatherCommonalityRecord.weather == weather)
				{
					float num = weatherCommonalityRecord.commonality;
					if (this.map.fireWatcher.LargeFireDangerPresent && weather.rainRate > 0.1f)
					{
						num *= 15f;
					}
					if (weatherCommonalityRecord.weather.commonalityRainfallFactor != null)
					{
						num *= weatherCommonalityRecord.weather.commonalityRainfallFactor.Evaluate(this.map.TileInfo.rainfall);
					}
					return num;
				}
			}
			return 0f;
		}

		// Token: 0x06006AC7 RID: 27335 RVA: 0x0020FBD0 File Offset: 0x0020DDD0
		public void LogWeatherChances()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (WeatherDef weatherDef in from w in DefDatabase<WeatherDef>.AllDefs
			orderby this.CurrentWeatherCommonality(w) descending
			select w)
			{
				stringBuilder.AppendLine(weatherDef.label + " - " + this.CurrentWeatherCommonality(weatherDef).ToString());
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x0400470B RID: 18187
		private Map map;

		// Token: 0x0400470C RID: 18188
		private int curWeatherDuration = 10000;

		// Token: 0x0400470D RID: 18189
		private int ticksWhenRainAllowedAgain;

		// Token: 0x0400470E RID: 18190
		private const int FirstWeatherDuration = 10000;

		// Token: 0x0400470F RID: 18191
		private const float ChanceFactorRainOnFire = 15f;

		// Token: 0x04004710 RID: 18192
		private static List<GameCondition> allConditionsTmp = new List<GameCondition>();
	}
}
