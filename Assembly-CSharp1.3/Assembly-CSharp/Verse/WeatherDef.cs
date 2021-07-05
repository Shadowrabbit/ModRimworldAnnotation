using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200011E RID: 286
	public class WeatherDef : Def
	{
		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060007AD RID: 1965 RVA: 0x00023B01 File Offset: 0x00021D01
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

		// Token: 0x060007AE RID: 1966 RVA: 0x00023B1D File Offset: 0x00021D1D
		public override void PostLoad()
		{
			base.PostLoad();
			this.workerInt = new WeatherWorker(this);
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x00023B31 File Offset: 0x00021D31
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.skyColorsDay.saturation == 0f || this.skyColorsDusk.saturation == 0f || this.skyColorsNightMid.saturation == 0f || this.skyColorsNightEdge.saturation == 0f)
			{
				yield return "a sky color has saturation of 0";
			}
			yield break;
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x00023B41 File Offset: 0x00021D41
		public static WeatherDef Named(string defName)
		{
			return DefDatabase<WeatherDef>.GetNamed(defName, true);
		}

		// Token: 0x04000749 RID: 1865
		public IntRange durationRange = new IntRange(16000, 160000);

		// Token: 0x0400074A RID: 1866
		public bool repeatable;

		// Token: 0x0400074B RID: 1867
		public bool isBad;

		// Token: 0x0400074C RID: 1868
		public Favorability favorability = Favorability.Neutral;

		// Token: 0x0400074D RID: 1869
		public FloatRange temperatureRange = new FloatRange(-999f, 999f);

		// Token: 0x0400074E RID: 1870
		public SimpleCurve commonalityRainfallFactor;

		// Token: 0x0400074F RID: 1871
		public float rainRate;

		// Token: 0x04000750 RID: 1872
		public float snowRate;

		// Token: 0x04000751 RID: 1873
		public float windSpeedFactor = 1f;

		// Token: 0x04000752 RID: 1874
		public float windSpeedOffset;

		// Token: 0x04000753 RID: 1875
		public float moveSpeedMultiplier = 1f;

		// Token: 0x04000754 RID: 1876
		public float accuracyMultiplier = 1f;

		// Token: 0x04000755 RID: 1877
		public float perceivePriority;

		// Token: 0x04000756 RID: 1878
		public ThoughtDef exposedThought;

		// Token: 0x04000757 RID: 1879
		public List<SoundDef> ambientSounds = new List<SoundDef>();

		// Token: 0x04000758 RID: 1880
		public List<WeatherEventMaker> eventMakers = new List<WeatherEventMaker>();

		// Token: 0x04000759 RID: 1881
		public List<Type> overlayClasses = new List<Type>();

		// Token: 0x0400075A RID: 1882
		public SkyColorSet skyColorsNightMid;

		// Token: 0x0400075B RID: 1883
		public SkyColorSet skyColorsNightEdge;

		// Token: 0x0400075C RID: 1884
		public SkyColorSet skyColorsDay;

		// Token: 0x0400075D RID: 1885
		public SkyColorSet skyColorsDusk;

		// Token: 0x0400075E RID: 1886
		[Unsaved(false)]
		private WeatherWorker workerInt;
	}
}
