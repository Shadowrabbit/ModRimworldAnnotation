using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse.Noise;

namespace Verse
{
	// Token: 0x02000318 RID: 792
	public class WindManager
	{
		// Token: 0x170003AD RID: 941
		// (get) Token: 0x0600142A RID: 5162 RVA: 0x0001481E File Offset: 0x00012A1E
		public float WindSpeed
		{
			get
			{
				return this.cachedWindSpeed;
			}
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x00014826 File Offset: 0x00012A26
		public WindManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x000CDD7C File Offset: 0x000CBF7C
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

		// Token: 0x0600142D RID: 5165 RVA: 0x00014835 File Offset: 0x00012A35
		public static void Notify_PlantMaterialCreated(Material newMat)
		{
			WindManager.plantMaterials.Add(newMat);
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x000CDED4 File Offset: 0x000CC0D4
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

		// Token: 0x0600142F RID: 5167 RVA: 0x00014842 File Offset: 0x00012A42
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

		// Token: 0x06001430 RID: 5168 RVA: 0x000CDFA8 File Offset: 0x000CC1A8
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
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x04000FDB RID: 4059
		private static readonly FloatRange WindSpeedRange = new FloatRange(0.04f, 2f);

		// Token: 0x04000FDC RID: 4060
		private Map map;

		// Token: 0x04000FDD RID: 4061
		private static List<Material> plantMaterials = new List<Material>();

		// Token: 0x04000FDE RID: 4062
		private float cachedWindSpeed;

		// Token: 0x04000FDF RID: 4063
		private ModuleBase windNoise;

		// Token: 0x04000FE0 RID: 4064
		private float plantSwayHead;
	}
}
