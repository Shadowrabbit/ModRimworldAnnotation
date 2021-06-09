using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200133B RID: 4923
	public class WeatherEvent_LightningFlash : WeatherEvent
	{
		// Token: 0x17001070 RID: 4208
		// (get) Token: 0x06006ACE RID: 27342 RVA: 0x00048A2F File Offset: 0x00046C2F
		public override bool Expired
		{
			get
			{
				return this.age > this.duration;
			}
		}

		// Token: 0x17001071 RID: 4209
		// (get) Token: 0x06006ACF RID: 27343 RVA: 0x00048A3F File Offset: 0x00046C3F
		public override SkyTarget SkyTarget
		{
			get
			{
				return new SkyTarget(1f, WeatherEvent_LightningFlash.LightningFlashColors, 1f, 1f);
			}
		}

		// Token: 0x17001072 RID: 4210
		// (get) Token: 0x06006AD0 RID: 27344 RVA: 0x00048A5A File Offset: 0x00046C5A
		public override Vector2? OverrideShadowVector
		{
			get
			{
				return new Vector2?(this.shadowVector);
			}
		}

		// Token: 0x17001073 RID: 4211
		// (get) Token: 0x06006AD1 RID: 27345 RVA: 0x00048A67 File Offset: 0x00046C67
		public override float SkyTargetLerpFactor
		{
			get
			{
				return this.LightningBrightness;
			}
		}

		// Token: 0x17001074 RID: 4212
		// (get) Token: 0x06006AD2 RID: 27346 RVA: 0x00048A6F File Offset: 0x00046C6F
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

		// Token: 0x06006AD3 RID: 27347 RVA: 0x0020FC60 File Offset: 0x0020DE60
		public WeatherEvent_LightningFlash(Map map) : base(map)
		{
			this.duration = Rand.Range(15, 60);
			this.shadowVector = new Vector2(Rand.Range(-5f, 5f), Rand.Range(-5f, 0f));
		}

		// Token: 0x06006AD4 RID: 27348 RVA: 0x00048A9D File Offset: 0x00046C9D
		public override void FireEvent()
		{
			SoundDefOf.Thunder_OffMap.PlayOneShotOnCamera(this.map);
		}

		// Token: 0x06006AD5 RID: 27349 RVA: 0x00048AAF File Offset: 0x00046CAF
		public override void WeatherEventTick()
		{
			this.age++;
		}

		// Token: 0x04004713 RID: 18195
		private int duration;

		// Token: 0x04004714 RID: 18196
		private Vector2 shadowVector;

		// Token: 0x04004715 RID: 18197
		private int age;

		// Token: 0x04004716 RID: 18198
		private const int FlashFadeInTicks = 3;

		// Token: 0x04004717 RID: 18199
		private const int MinFlashDuration = 15;

		// Token: 0x04004718 RID: 18200
		private const int MaxFlashDuration = 60;

		// Token: 0x04004719 RID: 18201
		private const float FlashShadowDistance = 5f;

		// Token: 0x0400471A RID: 18202
		private static readonly SkyColorSet LightningFlashColors = new SkyColorSet(new Color(0.9f, 0.95f, 1f), new Color(0.78431374f, 0.8235294f, 0.84705883f), new Color(0.9f, 0.95f, 1f), 1.15f);
	}
}
