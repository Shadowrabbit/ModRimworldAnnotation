using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000D04 RID: 3332
	public class WeatherEvent_LightningFlash : WeatherEvent
	{
		// Token: 0x17000D6D RID: 3437
		// (get) Token: 0x06004DE2 RID: 19938 RVA: 0x001A22B9 File Offset: 0x001A04B9
		public override bool Expired
		{
			get
			{
				return this.age > this.duration;
			}
		}

		// Token: 0x17000D6E RID: 3438
		// (get) Token: 0x06004DE3 RID: 19939 RVA: 0x001A22C9 File Offset: 0x001A04C9
		public override SkyTarget SkyTarget
		{
			get
			{
				return new SkyTarget(1f, WeatherEvent_LightningFlash.LightningFlashColors, 1f, 1f);
			}
		}

		// Token: 0x17000D6F RID: 3439
		// (get) Token: 0x06004DE4 RID: 19940 RVA: 0x001A22E4 File Offset: 0x001A04E4
		public override Vector2? OverrideShadowVector
		{
			get
			{
				return new Vector2?(this.shadowVector);
			}
		}

		// Token: 0x17000D70 RID: 3440
		// (get) Token: 0x06004DE5 RID: 19941 RVA: 0x001A22F1 File Offset: 0x001A04F1
		public override float SkyTargetLerpFactor
		{
			get
			{
				return this.LightningBrightness;
			}
		}

		// Token: 0x17000D71 RID: 3441
		// (get) Token: 0x06004DE6 RID: 19942 RVA: 0x001A22F9 File Offset: 0x001A04F9
		protected float LightningBrightness
		{
			get
			{
				if (this.age <= 3)
				{
					return (float)this.age / 3f;
				}
				return 1f - (float)this.age / (float)this.duration;
			}
		}

		// Token: 0x06004DE7 RID: 19943 RVA: 0x001A2328 File Offset: 0x001A0528
		public WeatherEvent_LightningFlash(Map map) : base(map)
		{
			this.duration = Rand.Range(15, 60);
			this.shadowVector = new Vector2(Rand.Range(-5f, 5f), Rand.Range(-5f, 0f));
		}

		// Token: 0x06004DE8 RID: 19944 RVA: 0x001A2374 File Offset: 0x001A0574
		public override void FireEvent()
		{
			SoundDefOf.Thunder_OffMap.PlayOneShotOnCamera(this.map);
		}

		// Token: 0x06004DE9 RID: 19945 RVA: 0x001A2386 File Offset: 0x001A0586
		public override void WeatherEventTick()
		{
			this.age++;
		}

		// Token: 0x04002F05 RID: 12037
		private int duration;

		// Token: 0x04002F06 RID: 12038
		private Vector2 shadowVector;

		// Token: 0x04002F07 RID: 12039
		private int age;

		// Token: 0x04002F08 RID: 12040
		private const int FlashFadeInTicks = 3;

		// Token: 0x04002F09 RID: 12041
		private const int MinFlashDuration = 15;

		// Token: 0x04002F0A RID: 12042
		private const int MaxFlashDuration = 60;

		// Token: 0x04002F0B RID: 12043
		private const float FlashShadowDistance = 5f;

		// Token: 0x04002F0C RID: 12044
		private static readonly SkyColorSet LightningFlashColors = new SkyColorSet(new Color(0.9f, 0.95f, 1f), new Color(0.78431374f, 0.8235294f, 0.84705883f), new Color(0.9f, 0.95f, 1f), 1.15f);
	}
}
