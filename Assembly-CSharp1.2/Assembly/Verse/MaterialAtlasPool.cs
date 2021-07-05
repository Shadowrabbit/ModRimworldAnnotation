using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000471 RID: 1137
	public static class MaterialAtlasPool
	{
		// Token: 0x06001CD1 RID: 7377 RVA: 0x0001A051 File Offset: 0x00018251
		public static Material SubMaterialFromAtlas(Material mat, LinkDirections LinkSet)
		{
			if (!MaterialAtlasPool.atlasDict.ContainsKey(mat))
			{
				MaterialAtlasPool.atlasDict.Add(mat, new MaterialAtlasPool.MaterialAtlas(mat));
			}
			return MaterialAtlasPool.atlasDict[mat].SubMat(LinkSet);
		}

		// Token: 0x04001496 RID: 5270
		private static Dictionary<Material, MaterialAtlasPool.MaterialAtlas> atlasDict = new Dictionary<Material, MaterialAtlasPool.MaterialAtlas>();

		// Token: 0x02000472 RID: 1138
		private class MaterialAtlas
		{
			// Token: 0x06001CD3 RID: 7379 RVA: 0x000F1E30 File Offset: 0x000F0030
			public MaterialAtlas(Material newRootMat)
			{
				Vector2 mainTextureScale = new Vector2(0.1875f, 0.1875f);
				for (int i = 0; i < 16; i++)
				{
					float x = (float)(i % 4) * 0.25f + 0.03125f;
					float y = (float)(i / 4) * 0.25f + 0.03125f;
					Vector2 mainTextureOffset = new Vector2(x, y);
					Material material = MaterialAllocator.Create(newRootMat);
					material.name = newRootMat.name + "_ASM" + i;
					material.mainTextureScale = mainTextureScale;
					material.mainTextureOffset = mainTextureOffset;
					this.subMats[i] = material;
				}
			}

			// Token: 0x06001CD4 RID: 7380 RVA: 0x000F1EDC File Offset: 0x000F00DC
			public Material SubMat(LinkDirections linkSet)
			{
				if ((int)linkSet >= this.subMats.Length)
				{
					Log.Warning("Cannot get submat of index " + (int)linkSet + ": out of range.", false);
					return BaseContent.BadMat;
				}
				return this.subMats[(int)linkSet];
			}

			// Token: 0x04001497 RID: 5271
			protected Material[] subMats = new Material[16];

			// Token: 0x04001498 RID: 5272
			private const float TexPadding = 0.03125f;
		}
	}
}
