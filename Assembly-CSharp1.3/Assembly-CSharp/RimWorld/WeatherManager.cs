using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000D06 RID: 3334
	public sealed class WeatherManager : IExposable
	{
		// Token: 0x17000D72 RID: 3442
		// (get) Token: 0x06004DF1 RID: 19953 RVA: 0x001A259C File Offset: 0x001A079C
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

		// Token: 0x17000D73 RID: 3443
		// (get) Token: 0x06004DF2 RID: 19954 RVA: 0x001A25C6 File Offset: 0x001A07C6
		public float RainRate
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.rainRate, this.curWeather.rainRate, this.TransitionLerpFactor);
			}
		}

		// Token: 0x17000D74 RID: 3444
		// (get) Token: 0x06004DF3 RID: 19955 RVA: 0x001A25E9 File Offset: 0x001A07E9
		public float SnowRate
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.snowRate, this.curWeather.snowRate, this.TransitionLerpFactor);
			}
		}

		// Token: 0x17000D75 RID: 3445
		// (get) Token: 0x06004DF4 RID: 19956 RVA: 0x001A260C File Offset: 0x001A080C
		public float CurWindSpeedFactor
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.windSpeedFactor, this.curWeather.windSpeedFactor, this.TransitionLerpFactor);
			}
		}

		// Token: 0x17000D76 RID: 3446
		// (get) Token: 0x06004DF5 RID: 19957 RVA: 0x001A262F File Offset: 0x001A082F
		public float CurWindSpeedOffset
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.windSpeedOffset, this.curWeather.windSpeedOffset, this.TransitionLerpFactor);
			}
		}

		// Token: 0x17000D77 RID: 3447
		// (get) Token: 0x06004DF6 RID: 19958 RVA: 0x001A2652 File Offset: 0x001A0852
		public float CurMoveSpeedMultiplier
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.moveSpeedMultiplier, this.curWeather.moveSpeedMultiplier, this.TransitionLerpFactor);
			}
		}

		// Token: 0x17000D78 RID: 3448
		// (get) Token: 0x06004DF7 RID: 19959 RVA: 0x001A2675 File Offset: 0x001A0875
		public float CurWeatherAccuracyMultiplier
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.accuracyMultiplier, this.curWeather.accuracyMultiplier, this.TransitionLerpFactor);
			}
		}

		// Token: 0x17000D79 RID: 3449
		// (get) Token: 0x06004DF8 RID: 19960 RVA: 0x001A2698 File Offset: 0x001A0898
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

		// Token: 0x17000D7A RID: 3450
		// (get) Token: 0x06004DF9 RID: 19961 RVA: 0x001A2725 File Offset: 0x001A0925
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

		// Token: 0x06004DFA RID: 19962 RVA: 0x001A2760 File Offset: 0x001A0960
		public WeatherManager(Map map)
		{
			this.map = map;
			this.growthSeasonMemory = new TemperatureMemory(map);
		}

		// Token: 0x06004DFB RID: 19963 RVA: 0x001A27B4 File Offset: 0x001A09B4
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

		// Token: 0x06004DFC RID: 19964 RVA: 0x001A2825 File Offset: 0x001A0A25
		public void TransitionTo(WeatherDef newWeather)
		{
			this.lastWeather = this.curWeather;
			this.curWeather = newWeather;
			this.curWeatherAge = 0;
		}

		// Token: 0x06004DFD RID: 19965 RVA: 0x001A2844 File Offset: 0x001A0A44
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

		// Token: 0x06004DFE RID: 19966 RVA: 0x001A28B0 File Offset: 0x001A0AB0
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

		// Token: 0x06004DFF RID: 19967 RVA: 0x001A29D4 File Offset: 0x001A0BD4
		public void WeatherManagerUpdate()
		{
			this.SetAmbienceSustainersVolume();
		}

		// Token: 0x06004E00 RID: 19968 RVA: 0x001A29DC File Offset: 0x001A0BDC
		public void EndAllSustainers()
		{
			for (int i = 0; i < this.ambienceSustainers.Count; i++)
			{
				this.ambienceSustainers[i].End();
			}
			this.ambienceSustainers.Clear();
		}

		// Token: 0x06004E01 RID: 19969 RVA: 0x001A2A1C File Offset: 0x001A0C1C
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

		// Token: 0x06004E02 RID: 19970 RVA: 0x001A2A9C File Offset: 0x001A0C9C
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

		// Token: 0x06004E03 RID: 19971 RVA: 0x001A2B66 File Offset: 0x001A0D66
		public void DrawAllWeather()
		{
			this.eventHandler.WeatherEventsDraw();
			this.lastWeather.Worker.DrawWeather(this.map);
			this.curWeather.Worker.DrawWeather(this.map);
		}

		// Token: 0x04002F10 RID: 12048
		public Map map;

		// Token: 0x04002F11 RID: 12049
		public WeatherEventHandler eventHandler = new WeatherEventHandler();

		// Token: 0x04002F12 RID: 12050
		public WeatherDef curWeather = WeatherDefOf.Clear;

		// Token: 0x04002F13 RID: 12051
		public WeatherDef lastWeather = WeatherDefOf.Clear;

		// Token: 0x04002F14 RID: 12052
		public int curWeatherAge;

		// Token: 0x04002F15 RID: 12053
		private List<Sustainer> ambienceSustainers = new List<Sustainer>();

		// Token: 0x04002F16 RID: 12054
		public TemperatureMemory growthSeasonMemory;

		// Token: 0x04002F17 RID: 12055
		public const float TransitionTicks = 4000f;
	}
}
