using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000316 RID: 790
	public class SkyManager
	{
		// Token: 0x170003AC RID: 940
		// (get) Token: 0x0600141B RID: 5147 RVA: 0x00014780 File Offset: 0x00012980
		public float CurSkyGlow
		{
			get
			{
				return this.curSkyGlowInt;
			}
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x00014788 File Offset: 0x00012988
		public SkyManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x000CD4C0 File Offset: 0x000CB6C0
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

		// Token: 0x0600141E RID: 5150 RVA: 0x000147AD File Offset: 0x000129AD
		public void ForceSetCurSkyGlow(float curSkyGlow)
		{
			this.curSkyGlowInt = curSkyGlow;
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x000CD668 File Offset: 0x000CB868
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

		// Token: 0x06001420 RID: 5152 RVA: 0x000CD810 File Offset: 0x000CBA10
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

		// Token: 0x06001421 RID: 5153 RVA: 0x000147B6 File Offset: 0x000129B6
		private void SetSunShadowVector(Vector2 vec)
		{
			Shader.SetGlobalVector(ShaderPropertyIDs.MapSunLightDirection, new Vector4(vec.x, 0f, vec.y, GenCelestial.CurShadowStrength(this.map)));
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x000CD8A4 File Offset: 0x000CBAA4
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

		// Token: 0x06001423 RID: 5155 RVA: 0x000CDA70 File Offset: 0x000CBC70
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

		// Token: 0x06001424 RID: 5156 RVA: 0x000CDB5C File Offset: 0x000CBD5C
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("SkyManager: ");
			stringBuilder.AppendLine("CurCelestialSunGlow: " + GenCelestial.CurCelestialSunGlow(Find.CurrentMap));
			stringBuilder.AppendLine("CurSkyGlow: " + this.CurSkyGlow.ToStringPercent());
			stringBuilder.AppendLine("CurrentSkyTarget: " + this.CurrentSkyTarget().ToString());
			return stringBuilder.ToString();
		}

		// Token: 0x04000FD0 RID: 4048
		private Map map;

		// Token: 0x04000FD1 RID: 4049
		private float curSkyGlowInt;

		// Token: 0x04000FD2 RID: 4050
		private List<Pair<SkyOverlay, float>> tempOverlays = new List<Pair<SkyOverlay, float>>();

		// Token: 0x04000FD3 RID: 4051
		private static readonly Color FogOfWarBaseColor = new Color32(77, 69, 66, byte.MaxValue);

		// Token: 0x04000FD4 RID: 4052
		public const float NightMaxCelGlow = 0.1f;

		// Token: 0x04000FD5 RID: 4053
		public const float DuskMaxCelGlow = 0.6f;

		// Token: 0x04000FD6 RID: 4054
		private List<GameCondition> tempAllGameConditionsAffectingMap = new List<GameCondition>();
	}
}
