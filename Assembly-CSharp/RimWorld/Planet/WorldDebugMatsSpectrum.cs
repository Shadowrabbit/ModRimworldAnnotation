using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002044 RID: 8260
	[StaticConstructorOnStartup]
	public static class WorldDebugMatsSpectrum
	{
		// Token: 0x0600AF16 RID: 44822 RVA: 0x0032E7C8 File Offset: 0x0032C9C8
		static WorldDebugMatsSpectrum()
		{
			for (int i = 0; i < 100; i++)
			{
				WorldDebugMatsSpectrum.spectrumMats[i] = MatsFromSpectrum.Get(WorldDebugMatsSpectrum.DebugSpectrum, (float)i / 100f, ShaderDatabase.WorldOverlayTransparent);
				WorldDebugMatsSpectrum.spectrumMats[i].renderQueue = WorldMaterials.DebugTileRenderQueue;
			}
		}

		// Token: 0x0600AF17 RID: 44823 RVA: 0x00071FFD File Offset: 0x000701FD
		public static Material Mat(int ind)
		{
			ind = Mathf.Clamp(ind, 0, 99);
			return WorldDebugMatsSpectrum.spectrumMats[ind];
		}

		// Token: 0x0400785D RID: 30813
		private static readonly Material[] spectrumMats = new Material[100];

		// Token: 0x0400785E RID: 30814
		public const int MaterialCount = 100;

		// Token: 0x0400785F RID: 30815
		private const float Opacity = 0.25f;

		// Token: 0x04007860 RID: 30816
		private static readonly Color[] DebugSpectrum = DebugMatsSpectrum.DebugSpectrum;
	}
}
