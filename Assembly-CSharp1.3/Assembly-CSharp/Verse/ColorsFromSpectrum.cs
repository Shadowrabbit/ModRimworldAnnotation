using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004D3 RID: 1235
	public static class ColorsFromSpectrum
	{
		// Token: 0x0600256A RID: 9578 RVA: 0x000E95D4 File Offset: 0x000E77D4
		public static Color Get(IList<Color> spectrum, float val)
		{
			if (spectrum.Count == 0)
			{
				Log.Warning("Color spectrum empty.");
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
