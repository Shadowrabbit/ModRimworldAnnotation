using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020001B0 RID: 432
	public class WeatherDef : Def
	{
		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000AFB RID: 2811 RVA: 0x0000E8F3 File Offset: 0x0000CAF3
		public WeatherWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = new WeatherWorker(this);
				}
				return this.workerInt;
			}
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0000E90F File Offset: 0x0000CB0F
		public override void PostLoad()
		{
			base.PostLoad();
			this.workerInt = new WeatherWorker(this);
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0000E923 File Offset: 0x0000CB23
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.skyColorsDay.saturation == 0f || this.skyColorsDusk.saturation == 0f || this.skyColorsNightMid.saturation == 0f || this.skyColorsNightEdge.saturation == 0f)
			{
				yield return "a sky color has saturation of 0";
			}
			yield break;
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x0000E933 File Offset: 0x0000CB33
		public static WeatherDef Named(string defName)
		{
			return DefDatabase<WeatherDef>.GetNamed(defName, true);
		}

		// Token: 0x040009C0 RID: 2496
		public IntRange durationRange = new IntRange(16000, 160000);

		// Token: 0x040009C1 RID: 2497
		public bool repeatable;

		// Token: 0x040009C2 RID: 2498
		public bool isBad;

		// Token: 0x040009C3 RID: 2499
		public Favorability favorability = Favorability.Neutral;

		// Token: 0x040009C4 RID: 2500
		public FloatRange temperatureRange = new FloatRange(-999f, 999f);

		// Token: 0x040009C5 RID: 2501
		public SimpleCurve commonalityRainfallFactor;

		// Token: 0x040009C6 RID: 2502
		public float rainRate;

		// Token: 0x040009C7 RID: 2503
		public float snowRate;

		// Token: 0x040009C8 RID: 2504
		public float windSpeedFactor = 1f;

		// Token: 0x040009C9 RID: 2505
		public float windSpeedOffset;

		// Token: 0x040009CA RID: 2506
		public float moveSpeedMultiplier = 1f;

		// Token: 0x040009CB RID: 2507
		public float accuracyMultiplier = 1f;

		// Token: 0x040009CC RID: 2508
		public float perceivePriority;

		// Token: 0x040009CD RID: 2509
		public ThoughtDef exposedThought;

		// Token: 0x040009CE RID: 2510
		public List<SoundDef> ambientSounds = new List<SoundDef>();

		// Token: 0x040009CF RID: 2511
		public List<WeatherEventMaker> eventMakers = new List<WeatherEventMaker>();

		// Token: 0x040009D0 RID: 2512
		public List<Type> overlayClasses = new List<Type>();

		// Token: 0x040009D1 RID: 2513
		public SkyColorSet skyColorsNightMid;

		// Token: 0x040009D2 RID: 2514
		public SkyColorSet skyColorsNightEdge;

		// Token: 0x040009D3 RID: 2515
		public SkyColorSet skyColorsDay;

		// Token: 0x040009D4 RID: 2516
		public SkyColorSet skyColorsDusk;

		// Token: 0x040009D5 RID: 2517
		[Unsaved(false)]
		private WeatherWorker workerInt;
	}
}
