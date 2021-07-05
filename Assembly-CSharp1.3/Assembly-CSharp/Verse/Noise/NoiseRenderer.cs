using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000509 RID: 1289
	public static class NoiseRenderer
	{
		// Token: 0x0600270E RID: 9998 RVA: 0x000F1653 File Offset: 0x000EF853
		public static Texture2D NoiseRendered(ModuleBase noise)
		{
			return NoiseRenderer.NoiseRendered(new CellRect(0, 0, NoiseRenderer.renderSize.x, NoiseRenderer.renderSize.z), noise);
		}

		// Token: 0x0600270F RID: 9999 RVA: 0x000F1678 File Offset: 0x000EF878
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

		// Token: 0x06002710 RID: 10000 RVA: 0x000F1704 File Offset: 0x000EF904
		private static Color ColorForValue(float val)
		{
			val = val * 0.5f + 0.5f;
			return ColorsFromSpectrum.Get(NoiseRenderer.spectrum, val);
		}

		// Token: 0x0400184C RID: 6220
		public static IntVec2 renderSize = new IntVec2(200, 200);

		// Token: 0x0400184D RID: 6221
		private static Color[] spectrum = new Color[]
		{
			Color.black,
			Color.blue,
			Color.green,
			Color.white
		};
	}
}
