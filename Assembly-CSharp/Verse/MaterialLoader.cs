using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000473 RID: 1139
	public static class MaterialLoader
	{
		// Token: 0x06001CD5 RID: 7381 RVA: 0x000F1F20 File Offset: 0x000F0120
		public static List<Material> MatsFromTexturesInFolder(string dirPath)
		{
			return (from Texture2D tex in Resources.LoadAll("Textures/" + dirPath, typeof(Texture2D))
			select MaterialPool.MatFrom(tex)).ToList<Material>();
		}

		// Token: 0x06001CD6 RID: 7382 RVA: 0x000F1F78 File Offset: 0x000F0178
		public static Material MatWithEnding(string dirPath, string ending)
		{
			Material material = (from mat in MaterialLoader.MatsFromTexturesInFolder(dirPath)
			where mat.mainTexture.name.ToLower().EndsWith(ending)
			select mat).FirstOrDefault<Material>();
			if (material == null)
			{
				Log.Warning("MatWithEnding: Dir " + dirPath + " lacks texture ending in " + ending, false);
				return BaseContent.BadMat;
			}
			return material;
		}
	}
}
