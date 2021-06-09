using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200087E RID: 2174
	public static class ColorsFromSpectrum
	{
		// Token: 0x060035FC RID: 13820 RVA: 0x0015B1D8 File Offset: 0x001593D8
		public static Color Get(IList<Color> spectrum, float val)
		{
			if (spectrum.Count == 0)
			{
				Log.Warning("Color spectrum empty.", false);
				return Color.white;
			}
			if (spectrum.Count == 1)
			{
				return spectrum[0];
			}
			val = Mathf.Clamp01(val);
			float num = 1f / (float)(spectrum.Count - 1);
			int num2 = (int)(val / num);
			if (num2 == spectrum.Count - 1)
			{
				return spectrum[spectrum.Count - 1];
			}
			float t = (val - (float)num2 * num) / num;
			return Color.Lerp(spectrum[num2], spectrum[num2 + 1], t);
		}
	}
}
