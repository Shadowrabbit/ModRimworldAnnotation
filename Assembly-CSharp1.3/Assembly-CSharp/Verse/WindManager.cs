using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse.Noise;

namespace Verse
{
	// Token: 0x02000227 RID: 551
	public class WindManager
	{
		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000FA4 RID: 4004 RVA: 0x000591E8 File Offset: 0x000573E8
		public float WindSpeed
		{
			get
			{
				return this.cachedWindSpeed;
			}
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x000591F0 File Offset: 0x000573F0
		public WindManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x00059200 File Offset: 0x00057400
		public void WindManagerTick()
		{
			this.cachedWindSpeed = this.BaseWindSpeedAt(Find.TickManager.TicksAbs) * this.map.weatherManager.CurWindSpeedFactor;
			float curWindSpeedOffset = this.map.weatherManager.CurWindSpeedOffset;
			if (curWindSpeedOffset > 0f)
			{
				FloatRange floatRange = WindManager.WindSpeedRange * this.map.weatherManager.CurWindSpeedFactor;
				float num = (this.cachedWindSpeed - floatRange.min) / (floatRange.max - floatRange.min) * (floatRange.max - curWindSpeedOffset);
				this.cachedWindSpeed = curWindSpeedOffset + num;
			}
			List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.WindSource);
			for (int i = 0; i < list.Count; i++)
			{
				CompWindSource compWindSource = list[i].TryGetComp<CompWindSource>();
				this.cachedWindSpeed = Mathf.Max(this.cachedWindSpeed, compWindSource.wind);
			}
			if (Prefs.PlantWindSway)
			{
				this.plantSwayHead += Mathf.Min(this.WindSpeed, 1f);
			}
			else
			{
				this.plantSwayHead = 0f;
			}
			if (Find.CurrentMap == this.map)
			{
				for (int j = 0; j < WindManager.plantMaterials.Count; j++)
				{
					WindManager.plantMaterials[j].SetFloat(ShaderPropertyIDs.SwayHead, this.plantSwayHead);
				}
			}
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x00059358 File Offset: 0x00057558
		public static void Notify_PlantMaterialCreated(Material newMat)
		{
			WindManager.plantMaterials.Add(newMat);
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x00059368 File Offset: 0x00057568
		private float BaseWindSpeedAt(int ticksAbs)
		{
			if (this.windNoise == null)
			{
				int seed = Gen.HashCombineInt(this.map.Tile, 122049541) ^ Find.World.info.Seed;
				this.windNoise = new Perlin(3.9999998989515007E-05, 2.0, 0.5, 4, seed, QualityMode.Medium);
				this.windNoise = new ScaleBias(1.5, 0.5, this.windNoise);
				this.windNoise = new Clamp((double)WindManager.WindSpeedRange.min, (double)WindManager.WindSpeedRange.max, this.windNoise);
			}
			return (float)this.windNoise.GetValue((double)ticksAbs, 0.0, 0.0);
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x00059439 File Offset: 0x00057639
		public string DebugString()
		{
			return string.Concat(new object[]
			{
				"WindSpeed: ",
				this.WindSpeed,
				"\nplantSwayHead: ",
				this.plantSwayHead
			});
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x00059474 File Offset: 0x00057674
		public void LogWindSpeeds()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Upcoming wind speeds:");
			for (int i = 0; i < 72; i++)
			{
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"Hour ",
					i,
					" - ",
					this.BaseWindSpeedAt(Find.TickManager.TicksAbs + 2500 * i).ToString("F2")
				}));
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04000C50 RID: 3152
		private static readonly FloatRange WindSpeedRange = new FloatRange(0.04f, 2f);

		// Token: 0x04000C51 RID: 3153
		private Map map;

		// Token: 0x04000C52 RID: 3154
		private static List<Material> plantMaterials = new List<Material>();

		// Token: 0x04000C53 RID: 3155
		private float cachedWindSpeed;

		// Token: 0x04000C54 RID: 3156
		private ModuleBase windNoise;

		// Token: 0x04000C55 RID: 3157
		private float plantSwayHead;
	}
}
