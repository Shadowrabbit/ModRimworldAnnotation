using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200047A RID: 1146
	public static class ShadowMeshPool
	{
		// Token: 0x06001CF9 RID: 7417 RVA: 0x0001A27D File Offset: 0x0001847D
		public static Mesh GetShadowMesh(ShadowData sha)
		{
			return ShadowMeshPool.GetShadowMesh(sha.BaseX, sha.BaseZ, sha.BaseY);
		}

		// Token: 0x06001CFA RID: 7418 RVA: 0x0001A296 File Offset: 0x00018496
		public static Mesh GetShadowMesh(float baseEdgeLength, float tallness)
		{
			return ShadowMeshPool.GetShadowMesh(baseEdgeLength, baseEdgeLength, tallness);
		}

		// Token: 0x06001CFB RID: 7419 RVA: 0x000F28A8 File Offset: 0x000F0AA8
		public static Mesh GetShadowMesh(float baseWidth, float baseHeight, float tallness)
		{
			int key = ShadowMeshPool.HashOf(baseWidth, baseHeight, tallness);
			Mesh mesh;
			if (!ShadowMeshPool.shadowMeshDict.TryGetValue(key, out mesh))
			{
				mesh = MeshMakerShadows.NewShadowMesh(baseWidth, baseHeight, tallness);
				ShadowMeshPool.shadowMeshDict.Add(key, mesh);
			}
			return mesh;
		}

		// Token: 0x06001CFC RID: 7420 RVA: 0x000F28E4 File Offset: 0x000F0AE4
		private static int HashOf(float baseWidth, float baseheight, float tallness)
		{
			int num = (int)(baseWidth * 1000f);
			int num2 = (int)(baseheight * 1000f);
			int num3 = (int)(tallness * 1000f);
			return num * 391 ^ 261231 ^ num2 * 612331 ^ num3 * 456123;
		}

		// Token: 0x040014A6 RID: 5286
		private static Dictionary<int, Mesh> shadowMeshDict = new Dictionary<int, Mesh>();
	}
}
