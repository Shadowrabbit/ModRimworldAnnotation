using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000305 RID: 773
	public static class MaterialAtlasPool
	{
		// Token: 0x06001650 RID: 5712 RVA: 0x00082116 File Offset: 0x00080316
		public static Material SubMaterialFromAtlas(Material mat, LinkDirections LinkSet)
		{
			if (!MaterialAtlasPool.atlasDict.ContainsKey(mat))
			{
				MaterialAtlasPool.atlasDict.Add(mat, new MaterialAtlasPool.MaterialAtlas(mat));
			}
			return MaterialAtlasPool.atlasDict[mat].SubMat(LinkSet);
		}

		// Token: 0x04000F86 RID: 3974
		private static Dictionary<Material, MaterialAtlasPool.MaterialAtlas> atlasDict = new Dictionary<Material, MaterialAtlasPool.MaterialAtlas>();

		// Token: 0x02001A40 RID: 6720
		private class MaterialAtlas
		{
			// Token: 0x06009C3A RID: 39994 RVA: 0x00368D80 File Offset: 0x00366F80
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

			// Token: 0x06009C3B RID: 39995 RVA: 0x00368E2C File Offset: 0x0036702C
			public Material SubMat(LinkDirections linkSet)
			{
				if ((int)linkSet >= this.subMats.Length)
				{
					Log.Warning("Cannot get submat of index " + (int)linkSet + ": out of range.");
					return BaseContent.BadMat;
				}
				return this.subMats[(int)linkSet];
			}

			// Token: 0x04006488 RID: 25736
			protected Material[] subMats = new Material[16];

			// Token: 0x04006489 RID: 25737
			private const float TexPadding = 0.03125f;
		}
	}
}
