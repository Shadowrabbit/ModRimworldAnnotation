using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000306 RID: 774
	public static class MaterialLoader
	{
		// Token: 0x06001652 RID: 5714 RVA: 0x00082154 File Offset: 0x00080354
		public static List<Material> MatsFromTexturesInFolder(string dirPath)
		{
			return (from Texture2D tex in Resources.LoadAll("Textures/" + dirPath, typeof(Texture2D))
			select MaterialPool.MatFrom(tex)).ToList<Material>();
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x000821AC File Offset: 0x000803AC
		public static Material MatWithEnding(string dirPath, string ending)
		{
			Material material = (from mat in MaterialLoader.MatsFromTexturesInFolder(dirPath)
			where mat.mainTexture.name.ToLower().EndsWith(ending)
			select mat).FirstOrDefault<Material>();
			if (material == null)
			{
				Log.Warning("MatWithEnding: Dir " + dirPath + " lacks texture ending in " + ending);
				return BaseContent.BadMat;
			}
			return material;
		}
	}
}
