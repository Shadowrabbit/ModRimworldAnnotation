using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200030B RID: 779
	public static class ShadowMeshPool
	{
		// Token: 0x06001674 RID: 5748 RVA: 0x00082D5E File Offset: 0x00080F5E
		public static Mesh GetShadowMesh(ShadowData sha)
		{
			return ShadowMeshPool.GetShadowMesh(sha.BaseX, sha.BaseZ, sha.BaseY);
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x00082D77 File Offset: 0x00080F77
		public static Mesh GetShadowMesh(float baseEdgeLength, float tallness)
		{
			return ShadowMeshPool.GetShadowMesh(baseEdgeLength, baseEdgeLength, tallness);
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x00082D84 File Offset: 0x00080F84
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

		// Token: 0x06001677 RID: 5751 RVA: 0x00082DC0 File Offset: 0x00080FC0
		private static int HashOf(float baseWidth, float baseheight, float tallness)
		{
			int num = (int)(baseWidth * 1000f);
			int num2 = (int)(baseheight * 1000f);
			int num3 = (int)(tallness * 1000f);
			return num * 391 ^ 261231 ^ num2 * 612331 ^ num3 * 456123;
		}

		// Token: 0x04000F93 RID: 3987
		private static Dictionary<int, Mesh> shadowMeshDict = new Dictionary<int, Mesh>();
	}
}
