using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000248 RID: 584
	public static class DebugSolidColorMats
	{
		// Token: 0x06000EDD RID: 3805 RVA: 0x000B4634 File Offset: 0x000B2834
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

		// Token: 0x04000C3D RID: 3133
		private static Dictionary<Color, Material> colorMatDict = new Dictionary<Color, Material>();
	}
}
