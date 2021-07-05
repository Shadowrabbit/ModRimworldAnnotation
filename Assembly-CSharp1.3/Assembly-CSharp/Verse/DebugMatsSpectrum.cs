using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200018C RID: 396
	[StaticConstructorOnStartup]
	public static class DebugMatsSpectrum
	{
		// Token: 0x06000B2F RID: 2863 RVA: 0x0003CB90 File Offset: 0x0003AD90
		static DebugMatsSpectrum()
		{
			for (int i = 0; i < 100; i++)
			{
				DebugMatsSpectrum.spectrumMatsTranparent[i] = MatsFromSpectrum.Get(DebugMatsSpectrum.DebugSpectrumWithOpacity(0.25f), (float)i / 100f);
				DebugMatsSpectrum.spectrumMatsOpaque[i] = MatsFromSpectrum.Get(DebugMatsSpectrum.DebugSpectrumWithOpacity(1f), (float)i / 100f);
			}
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x0003CC90 File Offset: 0x0003AE90
		private static Color[] DebugSpectrumWithOpacity(float opacity)
		{
			Color[] array = new Color[DebugMatsSpectrum.DebugSpectrum.Length];
			for (int i = 0; i < DebugMatsSpectrum.DebugSpectrum.Length; i++)
			{
				array[i] = new Color(DebugMatsSpectrum.DebugSpectrum[i].r, DebugMatsSpectrum.DebugSpectrum[i].g, DebugMatsSpectrum.DebugSpectrum[i].b, opacity);
			}
			return array;
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x0003CCFA File Offset: 0x0003AEFA
		public static Material Mat(int ind, bool transparent)
		{
			if (ind >= 100)
			{
				ind = 99;
			}
			if (ind < 0)
			{
				ind = 0;
			}
			if (!transparent)
			{
				return DebugMatsSpectrum.spectrumMatsOpaque[ind];
			}
			return DebugMatsSpectrum.spectrumMatsTranparent[ind];
		}

		// Token: 0x0400094D RID: 2381
		private static readonly Material[] spectrumMatsTranparent = new Material[100];

		// Token: 0x0400094E RID: 2382
		private static readonly Material[] spectrumMatsOpaque = new Material[100];

		// Token: 0x0400094F RID: 2383
		public const int MaterialCount = 100;

		// Token: 0x04000950 RID: 2384
		public static Color[] DebugSpectrum = new Color[]
		{
			new Color(0.75f, 0f, 0f),
			new Color(0.5f, 0.3f, 0f),
			new Color(0f, 1f, 0f),
			new Color(0f, 0f, 1f),
			new Color(0.7f, 0f, 1f)
		};
	}
}
