using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D03 RID: 3331
	public class WeatherDecider : IExposable
	{
		// Token: 0x17000D6C RID: 3436
		// (get) Token: 0x06004DD5 RID: 19925 RVA: 0x001A1DB4 File Offset: 0x0019FFB4
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

		// Token: 0x06004DD6 RID: 19926 RVA: 0x001A1E34 File Offset: 0x001A0034
		public WeatherDecider(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004DD7 RID: 19927 RVA: 0x001A1E4E File Offset: 0x001A004E
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.curWeatherDuration, "curWeatherDuration", 0, true);
			Scribe_Values.Look<int>(ref this.ticksWhenRainAllowedAgain, "ticksWhenRainAllowedAgain", 0, false);
		}

		// Token: 0x06004DD8 RID: 19928 RVA: 0x001A1E74 File Offset: 0x001A0074
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

		// Token: 0x06004DD9 RID: 19929 RVA: 0x001A1F14 File Offset: 0x001A0114
		public void StartNextWeather()
		{
			WeatherDef weatherDef = this.ChooseNextWeather();
			this.map.weatherManager.TransitionTo(weatherDef);
			this.curWeatherDuration = weatherDef.durationRange.RandomInRange;
		}

		// Token: 0x06004DDA RID: 19930 RVA: 0x001A1F4C File Offset: 0x001A014C
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

		// Token: 0x06004DDB RID: 19931 RVA: 0x001A2000 File Offset: 0x001A0200
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
				Log.Warning("All weather commonalities were zero. Defaulting to " + WeatherDefOf.Clear.defName + ".");
				return WeatherDefOf.Clear;
			}
			return result;
		}

		// Token: 0x06004DDC RID: 19932 RVA: 0x001A2065 File Offset: 0x001A0265
		public void DisableRainFor(int ticks)
		{
			this.ticksWhenRainAllowedAgain = Find.TickManager.TicksGame + ticks;
		}

		// Token: 0x06004DDD RID: 19933 RVA: 0x001A207C File Offset: 0x001A027C
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

		// Token: 0x06004DDE RID: 19934 RVA: 0x001A2214 File Offset: 0x001A0414
		public void LogWeatherChances()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (WeatherDef weatherDef in from w in DefDatabase<WeatherDef>.AllDefs
			orderby this.CurrentWeatherCommonality(w) descending
			select w)
			{
				stringBuilder.AppendLine(weatherDef.label + " - " + this.CurrentWeatherCommonality(weatherDef).ToString());
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04002EFF RID: 12031
		private Map map;

		// Token: 0x04002F00 RID: 12032
		private int curWeatherDuration = 10000;

		// Token: 0x04002F01 RID: 12033
		private int ticksWhenRainAllowedAgain;

		// Token: 0x04002F02 RID: 12034
		private const int FirstWeatherDuration = 10000;

		// Token: 0x04002F03 RID: 12035
		private const float ChanceFactorRainOnFire = 15f;

		// Token: 0x04002F04 RID: 12036
		private static List<GameCondition> allConditionsTmp = new List<GameCondition>();
	}
}
