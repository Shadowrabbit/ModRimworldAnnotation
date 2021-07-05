using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004D2 RID: 1234
	public static class MatsFromSpectrum
	{
		// Token: 0x06002568 RID: 9576 RVA: 0x000E95B6 File Offset: 0x000E77B6
		public static Material Get(Color[] spectrum, float val)
		{
			return MatsFromSpectrum.Get(spectrum, val, ShaderDatabase.MetaOverlay);
		}

		// Token: 0x06002569 RID: 9577 RVA: 0x000E95C4 File Offset: 0x000E77C4
		public static Material Get(Color[] spectrum, float val, Shader shader)
		{
			return SolidColorMaterials.NewSolidColorMaterial(ColorsFromSpectrum.Get(spectrum, val), shader);
		}
	}
}
