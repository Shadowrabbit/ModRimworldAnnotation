using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x020008C8 RID: 2248
	public static class NoiseRenderer
	{
		// Token: 0x060037E6 RID: 14310 RVA: 0x0002B33C File Offset: 0x0002953C
		public static Texture2D NoiseRendered(ModuleBase noise)
		{
			return NoiseRenderer.NoiseRendered(new CellRect(0, 0, NoiseRenderer.renderSize.x, NoiseRenderer.renderSize.z), noise);
		}

		// Token: 0x060037E7 RID: 14311 RVA: 0x00162044 File Offset: 0x00160244
		public static Texture2D NoiseRendered(CellRect rect, ModuleBase noise)
		{
			Texture2D texture2D = new Texture2D(rect.Width, rect.Height);
			texture2D.name = "NoiseRender";
			foreach (IntVec2 intVec in rect.Cells2D)
			{
				texture2D.SetPixel(intVec.x, intVec.z, NoiseRenderer.ColorForValue(noise.GetValue(intVec)));
			}
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x060037E8 RID: 14312 RVA: 0x0002B35F File Offset: 0x0002955F
		private static Color ColorForValue(float val)
		{
			val = val * 0.5f + 0.5f;
			return ColorsFromSpectrum.Get(NoiseRenderer.spectrum, val);
		}

		// Token: 0x040026C3 RID: 9923
		public static IntVec2 renderSize = new IntVec2(200, 200);

		// Token: 0x040026C4 RID: 9924
		private static Color[] spectrum = new Color[]
		{
			Color.black,
			Color.blue,
			Color.green,
			Color.white
		};
	}
}
