using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001757 RID: 5975
	[StaticConstructorOnStartup]
	public static class WorldDebugMatsSpectrum
	{
		// Token: 0x060089ED RID: 35309 RVA: 0x00318A10 File Offset: 0x00316C10
		static WorldDebugMatsSpectrum()
		{
			for (int i = 0; i < 100; i++)
			{
				WorldDebugMatsSpectrum.spectrumMats[i] = MatsFromSpectrum.Get(WorldDebugMatsSpectrum.DebugSpectrum, (float)i / 100f, ShaderDatabase.WorldOverlayTransparent);
				WorldDebugMatsSpectrum.spectrumMats[i].renderQueue = WorldMaterials.DebugTileRenderQueue;
			}
		}

		// Token: 0x060089EE RID: 35310 RVA: 0x00318A6F File Offset: 0x00316C6F
		public static Material Mat(int ind)
		{
			ind = Mathf.Clamp(ind, 0, 99);
			return WorldDebugMatsSpectrum.spectrumMats[ind];
		}

		// Token: 0x040057B0 RID: 22448
		private static readonly Material[] spectrumMats = new Material[100];

		// Token: 0x040057B1 RID: 22449
		public const int MaterialCount = 100;

		// Token: 0x040057B2 RID: 22450
		private const float Opacity = 0.25f;

		// Token: 0x040057B3 RID: 22451
		private static readonly Color[] DebugSpectrum = DebugMatsSpectrum.DebugSpectrum;
	}
}
