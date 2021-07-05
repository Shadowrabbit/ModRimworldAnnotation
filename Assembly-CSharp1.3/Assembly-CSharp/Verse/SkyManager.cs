using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000225 RID: 549
	public class SkyManager
	{
		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000F95 RID: 3989 RVA: 0x0005888E File Offset: 0x00056A8E
		public float CurSkyGlow
		{
			get
			{
				return this.curSkyGlowInt;
			}
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x00058896 File Offset: 0x00056A96
		public SkyManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x000588BC File Offset: 0x00056ABC
		public void SkyManagerUpdate()
		{
			SkyTarget skyTarget = this.CurrentSkyTarget();
			this.curSkyGlowInt = skyTarget.glow;
			if (this.map == Find.CurrentMap)
			{
				MatBases.LightOverlay.color = skyTarget.colors.sky;
				Find.CameraColor.saturation = skyTarget.colors.saturation;
				Color color = skyTarget.colors.sky;
				color.a = 1f;
				color *= SkyManager.FogOfWarBaseColor;
				MatBases.FogOfWar.color = color;
				Color color2 = skyTarget.colors.shadow;
				Vector3? overridenShadowVector = this.GetOverridenShadowVector();
				if (overridenShadowVector != null)
				{
					this.SetSunShadowVector(overridenShadowVector.Value);
				}
				else
				{
					this.SetSunShadowVector(GenCelestial.GetLightSourceInfo(this.map, GenCelestial.LightType.Shadow).vector);
					color2 = Color.Lerp(Color.white, color2, GenCelestial.CurShadowStrength(this.map));
				}
				GenCelestial.LightInfo lightSourceInfo = GenCelestial.GetLightSourceInfo(this.map, GenCelestial.LightType.LightingSun);
				GenCelestial.LightInfo lightSourceInfo2 = GenCelestial.GetLightSourceInfo(this.map, GenCelestial.LightType.LightingMoon);
				Shader.SetGlobalVector(ShaderPropertyIDs.WaterCastVectSun, new Vector4(lightSourceInfo.vector.x, 0f, lightSourceInfo.vector.y, lightSourceInfo.intensity));
				Shader.SetGlobalVector(ShaderPropertyIDs.WaterCastVectMoon, new Vector4(lightSourceInfo2.vector.x, 0f, lightSourceInfo2.vector.y, lightSourceInfo2.intensity));
				Shader.SetGlobalFloat("_LightsourceShineSizeReduction", 20f * (1f / skyTarget.lightsourceShineSize));
				Shader.SetGlobalFloat("_LightsourceShineIntensity", skyTarget.lightsourceShineIntensity);
				MatBases.SunShadow.color = color2;
				this.UpdateOverlays(skyTarget);
			}
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x00058A63 File Offset: 0x00056C63
		public void ForceSetCurSkyGlow(float curSkyGlow)
		{
			this.curSkyGlowInt = curSkyGlow;
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x00058A6C File Offset: 0x00056C6C
		private void UpdateOverlays(SkyTarget curSky)
		{
			this.tempOverlays.Clear();
			List<SkyOverlay> overlays = this.map.weatherManager.curWeather.Worker.overlays;
			for (int i = 0; i < overlays.Count; i++)
			{
				this.AddTempOverlay(new Pair<SkyOverlay, float>(overlays[i], this.map.weatherManager.TransitionLerpFactor));
			}
			List<SkyOverlay> overlays2 = this.map.weatherManager.lastWeather.Worker.overlays;
			for (int j = 0; j < overlays2.Count; j++)
			{
				this.AddTempOverlay(new Pair<SkyOverlay, float>(overlays2[j], 1f - this.map.weatherManager.TransitionLerpFactor));
			}
			for (int k = 0; k < this.map.gameConditionManager.ActiveConditions.Count; k++)
			{
				GameCondition gameCondition = this.map.gameConditionManager.ActiveConditions[k];
				List<SkyOverlay> list = gameCondition.SkyOverlays(this.map);
				if (list != null)
				{
					for (int l = 0; l < list.Count; l++)
					{
						this.AddTempOverlay(new Pair<SkyOverlay, float>(list[l], gameCondition.SkyTargetLerpFactor(this.map)));
					}
				}
			}
			for (int m = 0; m < this.tempOverlays.Count; m++)
			{
				Color overlay = curSky.colors.overlay;
				overlay.a = this.tempOverlays[m].Second;
				this.tempOverlays[m].First.OverlayColor = overlay;
			}
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x00058C14 File Offset: 0x00056E14
		private void AddTempOverlay(Pair<SkyOverlay, float> pair)
		{
			for (int i = 0; i < this.tempOverlays.Count; i++)
			{
				if (this.tempOverlays[i].First == pair.First)
				{
					this.tempOverlays[i] = new Pair<SkyOverlay, float>(this.tempOverlays[i].First, Mathf.Clamp01(this.tempOverlays[i].Second + pair.Second));
					return;
				}
			}
			this.tempOverlays.Add(pair);
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x00058CA7 File Offset: 0x00056EA7
		private void SetSunShadowVector(Vector2 vec)
		{
			Shader.SetGlobalVector(ShaderPropertyIDs.MapSunLightDirection, new Vector4(vec.x, 0f, vec.y, GenCelestial.CurShadowStrength(this.map)));
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x00058CD4 File Offset: 0x00056ED4
		private SkyTarget CurrentSkyTarget()
		{
			SkyTarget b = this.map.weatherManager.curWeather.Worker.CurSkyTarget(this.map);
			SkyTarget skyTarget = SkyTarget.Lerp(this.map.weatherManager.lastWeather.Worker.CurSkyTarget(this.map), b, this.map.weatherManager.TransitionLerpFactor);
			this.map.gameConditionManager.GetAllGameConditionsAffectingMap(this.map, this.tempAllGameConditionsAffectingMap);
			for (int i = 0; i < this.tempAllGameConditionsAffectingMap.Count; i++)
			{
				SkyTarget? skyTarget2 = this.tempAllGameConditionsAffectingMap[i].SkyTarget(this.map);
				if (skyTarget2 != null)
				{
					skyTarget = SkyTarget.LerpDarken(skyTarget, skyTarget2.Value, this.tempAllGameConditionsAffectingMap[i].SkyTargetLerpFactor(this.map));
				}
			}
			this.tempAllGameConditionsAffectingMap.Clear();
			List<WeatherEvent> liveEventsListForReading = this.map.weatherManager.eventHandler.LiveEventsListForReading;
			for (int j = 0; j < liveEventsListForReading.Count; j++)
			{
				if (liveEventsListForReading[j].CurrentlyAffectsSky)
				{
					skyTarget = SkyTarget.Lerp(skyTarget, liveEventsListForReading[j].SkyTarget, liveEventsListForReading[j].SkyTargetLerpFactor);
				}
			}
			List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.AffectsSky);
			for (int k = 0; k < list.Count; k++)
			{
				CompAffectsSky compAffectsSky = list[k].TryGetComp<CompAffectsSky>();
				if (compAffectsSky.LerpFactor > 0f)
				{
					if (compAffectsSky.Props.lerpDarken)
					{
						skyTarget = SkyTarget.LerpDarken(skyTarget, compAffectsSky.SkyTarget, compAffectsSky.LerpFactor);
					}
					else
					{
						skyTarget = SkyTarget.Lerp(skyTarget, compAffectsSky.SkyTarget, compAffectsSky.LerpFactor);
					}
				}
			}
			return skyTarget;
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x00058EA0 File Offset: 0x000570A0
		private Vector3? GetOverridenShadowVector()
		{
			List<WeatherEvent> liveEventsListForReading = this.map.weatherManager.eventHandler.LiveEventsListForReading;
			int i = 0;
			while (i < liveEventsListForReading.Count)
			{
				Vector2? overrideShadowVector = liveEventsListForReading[i].OverrideShadowVector;
				if (overrideShadowVector != null)
				{
					Vector2? vector = overrideShadowVector;
					if (vector == null)
					{
						return null;
					}
					return new Vector3?(vector.GetValueOrDefault());
				}
				else
				{
					i++;
				}
			}
			List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.AffectsSky);
			int j = 0;
			while (j < list.Count)
			{
				Vector2? overrideShadowVector2 = list[j].TryGetComp<CompAffectsSky>().OverrideShadowVector;
				if (overrideShadowVector2 != null)
				{
					Vector2? vector = overrideShadowVector2;
					if (vector == null)
					{
						return null;
					}
					return new Vector3?(vector.GetValueOrDefault());
				}
				else
				{
					j++;
				}
			}
			return null;
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x00058F8C File Offset: 0x0005718C
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("SkyManager: ");
			stringBuilder.AppendLine("CurCelestialSunGlow: " + GenCelestial.CurCelestialSunGlow(Find.CurrentMap));
			stringBuilder.AppendLine("CurSkyGlow: " + this.CurSkyGlow.ToStringPercent());
			stringBuilder.AppendLine("CurrentSkyTarget: " + this.CurrentSkyTarget().ToString());
			return stringBuilder.ToString();
		}

		// Token: 0x04000C45 RID: 3141
		private Map map;

		// Token: 0x04000C46 RID: 3142
		private float curSkyGlowInt;

		// Token: 0x04000C47 RID: 3143
		private List<Pair<SkyOverlay, float>> tempOverlays = new List<Pair<SkyOverlay, float>>();

		// Token: 0x04000C48 RID: 3144
		private static readonly Color FogOfWarBaseColor = new Color32(77, 69, 66, byte.MaxValue);

		// Token: 0x04000C49 RID: 3145
		public const float NightMaxCelGlow = 0.1f;

		// Token: 0x04000C4A RID: 3146
		public const float DuskMaxCelGlow = 0.6f;

		// Token: 0x04000C4B RID: 3147
		private List<GameCondition> tempAllGameConditionsAffectingMap = new List<GameCondition>();
	}
}
