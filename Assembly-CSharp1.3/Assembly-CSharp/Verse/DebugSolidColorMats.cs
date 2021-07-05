using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200018B RID: 395
	public static class DebugSolidColorMats
	{
		// Token: 0x06000B2D RID: 2861 RVA: 0x0003CB50 File Offset: 0x0003AD50
		public static Material MaterialOf(Color col)
		{
			Material material;
			if (DebugSolidColorMats.colorMatDict.TryGetValue(col, out material))
			{
				return material;
			}
			material = SolidColorMaterials.SimpleSolidColorMaterial(col, false);
			DebugSolidColorMats.colorMatDict.Add(col, material);
			return material;
		}

		// Token: 0x0400094C RID: 2380
		private static Dictionary<Color, Material> colorMatDict = new Dictionary<Color, Material>();
	}
}
