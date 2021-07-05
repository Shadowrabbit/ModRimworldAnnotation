using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200133D RID: 4925
	public sealed class WeatherManager : IExposable
	{
		// Token: 0x17001075 RID: 4213
		// (get) Token: 0x06006ADD RID: 27357 RVA: 0x0020FE18 File Offset: 0x0020E018
		public float TransitionLerpFactor
		{
			get
			{
				float num = (float)this.curWeatherAge / 4000f;
				if (num > 1f)
				{
					num = 1f;
				}
				return num;
			}
		}

		// Token: 0x17001076 RID: 4214
		// (get) Token: 0x06006ADE RID: 27358 RVA: 0x00048B56 File Offset: 0x00046D56
		public float RainRate
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.rainRate, this.curWeather.rainRate, this.TransitionLerpFactor);
			}
		}

		// Token: 0x17001077 RID: 4215
		// (get) Token: 0x06006ADF RID: 27359 RVA: 0x00048B79 File Offset: 0x00046D79
		public float SnowRate
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.snowRate, this.curWeather.snowRate, this.TransitionLerpFactor);
			}
		}

		// Token: 0x17001078 RID: 4216
		// (get) Token: 0x06006AE0 RID: 27360 RVA: 0x00048B9C File Offset: 0x00046D9C
		public float CurWindSpeedFactor
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.windSpeedFactor, this.curWeather.windSpeedFactor, this.TransitionLerpFactor);
			}
		}

		// Token: 0x17001079 RID: 4217
		// (get) Token: 0x06006AE1 RID: 27361 RVA: 0x00048BBF File Offset: 0x00046DBF
		public float CurWindSpeedOffset
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.windSpeedOffset, this.curWeather.windSpeedOffset, this.TransitionLerpFactor);
			}
		}

		// Token: 0x1700107A RID: 4218
		// (get) Token: 0x06006AE2 RID: 27362 RVA: 0x00048BE2 File Offset: 0x00046DE2
		public float CurMoveSpeedMultiplier
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.moveSpeedMultiplier, this.curWeather.moveSpeedMultiplier, this.TransitionLerpFactor);
			}
		}

		// Token: 0x1700107B RID: 4219
		// (get) Token: 0x06006AE3 RID: 27363 RVA: 0x00048C05 File Offset: 0x00046E05
		public float CurWeatherAccuracyMultiplier
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.accuracyMultiplier, this.curWeather.accuracyMultiplier, this.TransitionLerpFactor);
			}
		}

		// Token: 0x1700107C RID: 4220
		// (get) Token: 0x06006AE4 RID: 27364 RVA: 0x0020FE44 File Offset: 0x0020E044
		public WeatherDef CurWeatherPerceived
		{
			get
			{
				if (this.curWeather == null)
				{
					return this.lastWeather;
				}
				if (this.lastWeather == null)
				{
					return this.curWeather;
				}
				float num;
				if (this.curWeather.perceivePriority > this.lastWeather.perceivePriority)
				{
					num = 0.18f;
				}
				else if (this.lastWeather.perceivePriority > this.curWeather.perceivePriority)
				{
					num = 0.82f;
				}
				else
				{
					num = 0.5f;
				}
				if (this.TransitionLerpFactor >= num)
				{
					return this.curWeather;
				}
				return this.lastWeather;
			}
		}

		// Token: 0x1700107D RID: 4221
		// (get) Token: 0x06006AE5 RID: 27365 RVA: 0x00048C28 File Offset: 0x00046E28
		public WeatherDef CurWeatherLerped
		{
			get
			{
				if (this.curWeather == null)
				{
					return this.lastWeather;
				}
				if (this.lastWeather == null)
				{
					return this.curWeather;
				}
				if (this.TransitionLerpFactor >= 0.5f)
				{
					return this.curWeather;
				}
				return this.lastWeather;
			}
		}

		// Token: 0x06006AE6 RID: 27366 RVA: 0x0020FED4 File Offset: 0x0020E0D4
		public WeatherManager(Map map)
		{
			this.map = map;
			this.growthSeasonMemory = new TemperatureMemory(map);
		}

		// Token: 0x06006AE7 RID: 27367 RVA: 0x0020FF28 File Offset: 0x0020E128
		public void ExposeData()
		{
			Scribe_Defs.Look<WeatherDef>(ref this.curWeather, "curWeather");
			Scribe_Defs.Look<WeatherDef>(ref this.lastWeather, "lastWeather");
			Scribe_Values.Look<int>(ref this.curWeatherAge, "curWeatherAge", 0, true);
			Scribe_Deep.Look<TemperatureMemory>(ref this.growthSeasonMemory, "growthSeasonMemory", new object[]
			{
				this.map
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.ambienceSustainers.Clear();
			}
		}

		// Token: 0x06006AE8 RID: 27368 RVA: 0x00048C62 File Offset: 0x00046E62
		public void TransitionTo(WeatherDef newWeather)
		{
			this.lastWeather = this.curWeather;
			this.curWeather = newWeather;
			this.curWeatherAge = 0;
		}

		// Token: 0x06006AE9 RID: 27369 RVA: 0x0020FF9C File Offset: 0x0020E19C
		public void DoWeatherGUI(Rect rect)
		{
			WeatherDef curWeatherPerceived = this.CurWeatherPerceived;
			Text.Anchor = TextAnchor.MiddleRight;
			Rect rect2 = new Rect(rect);
			rect2.width -= 15f;
			Text.Font = GameFont.Small;
			Widgets.Label(rect2, curWeatherPerceived.LabelCap);
			if (!curWeatherPerceived.description.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, curWeatherPerceived.description);
			}
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06006AEA RID: 27370 RVA: 0x00210008 File Offset: 0x0020E208
		public void WeatherManagerTick()
		{
			this.eventHandler.WeatherEventHandlerTick();
			this.curWeatherAge++;
			this.curWeather.Worker.WeatherTick(this.map, this.TransitionLerpFactor);
			this.lastWeather.Worker.WeatherTick(this.map, 1f - this.TransitionLerpFactor);
			this.growthSeasonMemory.GrowthSeasonMemoryTick();
			for (int i = 0; i < this.curWeather.ambientSounds.Count; i++)
			{
				bool flag = false;
				for (int j = this.ambienceSustainers.Count - 1; j >= 0; j--)
				{
					if (this.ambienceSustainers[j].def == this.curWeather.ambientSounds[i])
					{
						flag = true;
						break;
					}
				}
				if (!flag && this.VolumeOfAmbientSound(this.curWeather.ambientSounds[i]) > 0.0001f)
				{
					SoundInfo info = SoundInfo.OnCamera(MaintenanceType.None);
					Sustainer sustainer = this.curWeather.ambientSounds[i].TrySpawnSustainer(info);
					if (sustainer != null)
					{
						this.ambienceSustainers.Add(sustainer);
					}
				}
			}
		}

		// Token: 0x06006AEB RID: 27371 RVA: 0x00048C7E File Offset: 0x00046E7E
		public void WeatherManagerUpdate()
		{
			this.SetAmbienceSustainersVolume();
		}

		// Token: 0x06006AEC RID: 27372 RVA: 0x0021012C File Offset: 0x0020E32C
		public void EndAllSustainers()
		{
			for (int i = 0; i < this.ambienceSustainers.Count; i++)
			{
				this.ambienceSustainers[i].End();
			}
			this.ambienceSustainers.Clear();
		}

		// Token: 0x06006AED RID: 27373 RVA: 0x0021016C File Offset: 0x0020E36C
		private void SetAmbienceSustainersVolume()
		{
			for (int i = this.ambienceSustainers.Count - 1; i >= 0; i--)
			{
				float num = this.VolumeOfAmbientSound(this.ambienceSustainers[i].def);
				if (num > 0.0001f)
				{
					this.ambienceSustainers[i].externalParams["LerpFactor"] = num;
				}
				else
				{
					this.ambienceSustainers[i].End();
					this.ambienceSustainers.RemoveAt(i);
				}
			}
		}

		// Token: 0x06006AEE RID: 27374 RVA: 0x002101EC File Offset: 0x0020E3EC
		private float VolumeOfAmbientSound(SoundDef soundDef)
		{
			if (this.map != Find.CurrentMap)
			{
				return 0f;
			}
			for (int i = 0; i < Find.WindowStack.Count; i++)
			{
				if (Find.WindowStack[i].silenceAmbientSound)
				{
					return 0f;
				}
			}
			float num = 0f;
			for (int j = 0; j < this.lastWeather.ambientSounds.Count; j++)
			{
				if (this.lastWeather.ambientSounds[j] == soundDef)
				{
					num += 1f - this.TransitionLerpFactor;
				}
			}
			for (int k = 0; k < this.curWeather.ambientSounds.Count; k++)
			{
				if (this.curWeather.ambientSounds[k] == soundDef)
				{
					num += this.TransitionLerpFactor;
				}
			}
			return num;
		}

		// Token: 0x06006AEF RID: 27375 RVA: 0x00048C86 File Offset: 0x00046E86
		public void DrawAllWeather()
		{
			this.eventHandler.WeatherEventsDraw();
			this.lastWeather.Worker.DrawWeather(this.map);
			this.curWeather.Worker.DrawWeather(this.map);
		}

		// Token: 0x0400471E RID: 18206
		public Map map;

		// Token: 0x0400471F RID: 18207
		public WeatherEventHandler eventHandler = new WeatherEventHandler();

		// Token: 0x04004720 RID: 18208
		public WeatherDef curWeather = WeatherDefOf.Clear;

		// Token: 0x04004721 RID: 18209
		public WeatherDef lastWeather = WeatherDefOf.Clear;

		// Token: 0x04004722 RID: 18210
		public int curWeatherAge;

		// Token: 0x04004723 RID: 18211
		private List<Sustainer> ambienceSustainers = new List<Sustainer>();

		// Token: 0x04004724 RID: 18212
		public TemperatureMemory growthSeasonMemory;

		// Token: 0x04004725 RID: 18213
		public const float TransitionTicks = 4000f;
	}
}
