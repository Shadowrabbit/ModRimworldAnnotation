﻿using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200031D RID: 797
	public class WeatherWorker
	{
		// Token: 0x06001448 RID: 5192 RVA: 0x000CE2DC File Offset: 0x000CC4DC
		public WeatherWorker(WeatherDef def)
		{
			this.def = def;
			foreach (Type genericParam in def.overlayClasses)
			{
				SkyOverlay item = (SkyOverlay)GenGeneric.InvokeStaticGenericMethod(typeof(WeatherPartPool), genericParam, "GetInstanceOf");
				this.overlays.Add(item);
			}
			this.skyTargets[0] = new WeatherWorker.SkyThreshold(def.skyColorsNightMid, 0f);
			this.skyTargets[1] = new WeatherWorker.SkyThreshold(def.skyColorsNightEdge, 0.1f);
			this.skyTargets[2] = new WeatherWorker.SkyThreshold(def.skyColorsDusk, 0.6f);
			this.skyTargets[3] = new WeatherWorker.SkyThreshold(def.skyColorsDay, 1f);
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x000CE3E4 File Offset: 0x000CC5E4
		public void DrawWeather(Map map)
		{
			for (int i = 0; i < this.overlays.Count; i++)
			{
				this.overlays[i].DrawOverlay(map);
			}
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x000CE41C File Offset: 0x000CC61C
		public void WeatherTick(Map map, float lerpFactor)
		{
			for (int i = 0; i < this.overlays.Count; i++)
			{
				this.overlays[i].TickOverlay(map);
			}
			for (int j = 0; j < this.def.eventMakers.Count; j++)
			{
				this.def.eventMakers[j].WeatherEventMakerTick(map, lerpFactor);
			}
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x000CE484 File Offset: 0x000CC684
		public SkyTarget CurSkyTarget(Map map)
		{
			float num = GenCelestial.CurCelestialSunGlow(map);
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < this.skyTargets.Length; i++)
			{
				num3 = i;
				if (num + 0.001f < this.skyTargets[i].celGlowThreshold)
				{
					break;
				}
				num2 = i;
			}
			WeatherWorker.SkyThreshold skyThreshold = this.skyTargets[num2];
			WeatherWorker.SkyThreshold skyThreshold2 = this.skyTargets[num3];
			float num4 = skyThreshold2.celGlowThreshold - skyThreshold.celGlowThreshold;
			float t;
			if (num4 == 0f)
			{
				t = 1f;
			}
			else
			{
				t = (num - skyThreshold.celGlowThreshold) / num4;
			}
			SkyTarget result = default(SkyTarget);
			result.glow = num;
			result.colors = SkyColorSet.Lerp(skyThreshold.colors, skyThreshold2.colors, t);
			if (GenCelestial.IsDaytime(num))
			{
				result.lightsourceShineIntensity = 1f;
				result.lightsourceShineSize = 1f;
			}
			else
			{
				result.lightsourceShineIntensity = 0.7f;
				result.lightsourceShineSize = 0.5f;
			}
			return result;
		}

		// Token: 0x04000FEB RID: 4075
		private WeatherDef def;

		// Token: 0x04000FEC RID: 4076
		public List<SkyOverlay> overlays = new List<SkyOverlay>();

		// Token: 0x04000FED RID: 4077
		private WeatherWorker.SkyThreshold[] skyTargets = new WeatherWorker.SkyThreshold[4];

		// Token: 0x0200031E RID: 798
		private struct SkyThreshold
		{
			// Token: 0x0600144C RID: 5196 RVA: 0x00014999 File Offset: 0x00012B99
			public SkyThreshold(SkyColorSet colors, float celGlowThreshold)
			{
				this.colors = colors;
				this.celGlowThreshold = celGlowThreshold;
			}

			// Token: 0x04000FEE RID: 4078
			public SkyColorSet colors;

			// Token: 0x04000FEF RID: 4079
			public float celGlowThreshold;
		}
	}
}
