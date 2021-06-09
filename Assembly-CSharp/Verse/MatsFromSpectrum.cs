using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200087D RID: 2173
	public static class MatsFromSpectrum
	{
		// Token: 0x060035FA RID: 13818 RVA: 0x00029D8B File Offset: 0x00027F8B
		public static Material Get(Color[] spectrum, float val)
		{
			return MatsFromSpectrum.Get(spectrum, val, ShaderDatabase.MetaOverlay);
		}

		// Token: 0x060035FB RID: 13819 RVA: 0x00029D99 File Offset: 0x00027F99
		public static Material Get(Color[] spectrum, float val, Shader shader)
		{
			return SolidColorMaterials.NewSolidColorMaterial(ColorsFromSpectrum.Get(spectrum, val), shader);
		}
	}
}
