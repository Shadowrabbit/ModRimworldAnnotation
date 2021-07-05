using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x0200046A RID: 1130
	public static class FadedMaterialPool
	{
		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06001CAD RID: 7341 RVA: 0x00019ECE File Offset: 0x000180CE
		public static int TotalMaterialCount
		{
			get
			{
				return FadedMaterialPool.cachedMats.Count;
			}
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06001CAE RID: 7342 RVA: 0x000F1944 File Offset: 0x000EFB44
		public static long TotalMaterialBytes
		{
			get
			{
				long num = 0L;
				foreach (KeyValuePair<FadedMaterialPool.FadedMatRequest, Material> keyValuePair in FadedMaterialPool.cachedMats)
				{
					num += Profiler.GetRuntimeMemorySizeLong(keyValuePair.Value);
				}
				return num;
			}
		}

		// Token: 0x06001CAF RID: 7343 RVA: 0x000F19A4 File Offset: 0x000EFBA4
		public static Material FadedVersionOf(Material sourceMat, float alpha)
		{
			int num = FadedMaterialPool.IndexFromAlpha(alpha);
			if (num == 0)
			{
				return BaseContent.ClearMat;
			}
			if (num == 29)
			{
				return sourceMat;
			}
			FadedMaterialPool.FadedMatRequest key = new FadedMaterialPool.FadedMatRequest(sourceMat, num);
			Material material;
			if (!FadedMaterialPool.cachedMats.TryGetValue(key, out material))
			{
				material = MaterialAllocator.Create(sourceMat);
				material.color = new Color(1f, 1f, 1f, (float)FadedMaterialPool.IndexFromAlpha(alpha) / 30f);
				FadedMaterialPool.cachedMats.Add(key, material);
			}
			return material;
		}

		// Token: 0x06001CB0 RID: 7344 RVA: 0x000F1A1C File Offset: 0x000EFC1C
		private static int IndexFromAlpha(float alpha)
		{
			int num = Mathf.FloorToInt(alpha * 30f);
			if (num == 30)
			{
				num = 29;
			}
			return num;
		}

		// Token: 0x04001483 RID: 5251
		private static Dictionary<FadedMaterialPool.FadedMatRequest, Material> cachedMats = new Dictionary<FadedMaterialPool.FadedMatRequest, Material>(FadedMaterialPool.FadedMatRequestComparer.Instance);

		// Token: 0x04001484 RID: 5252
		private const int NumFadeSteps = 30;

		// Token: 0x0200046B RID: 1131
		private struct FadedMatRequest : IEquatable<FadedMaterialPool.FadedMatRequest>
		{
			// Token: 0x06001CB2 RID: 7346 RVA: 0x00019EEB File Offset: 0x000180EB
			public FadedMatRequest(Material mat, int alphaIndex)
			{
				this.mat = mat;
				this.alphaIndex = alphaIndex;
			}

			// Token: 0x06001CB3 RID: 7347 RVA: 0x00019EFB File Offset: 0x000180FB
			public override bool Equals(object obj)
			{
				return obj != null && obj is FadedMaterialPool.FadedMatRequest && this.Equals((FadedMaterialPool.FadedMatRequest)obj);
			}

			// Token: 0x06001CB4 RID: 7348 RVA: 0x00019F16 File Offset: 0x00018116
			public bool Equals(FadedMaterialPool.FadedMatRequest other)
			{
				return this.mat == other.mat && this.alphaIndex == other.alphaIndex;
			}

			// Token: 0x06001CB5 RID: 7349 RVA: 0x00019F3B File Offset: 0x0001813B
			public override int GetHashCode()
			{
				return Gen.HashCombineInt(this.mat.GetHashCode(), this.alphaIndex);
			}

			// Token: 0x06001CB6 RID: 7350 RVA: 0x00019F53 File Offset: 0x00018153
			public static bool operator ==(FadedMaterialPool.FadedMatRequest lhs, FadedMaterialPool.FadedMatRequest rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x06001CB7 RID: 7351 RVA: 0x00019F5D File Offset: 0x0001815D
			public static bool operator !=(FadedMaterialPool.FadedMatRequest lhs, FadedMaterialPool.FadedMatRequest rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x04001485 RID: 5253
			private Material mat;

			// Token: 0x04001486 RID: 5254
			private int alphaIndex;
		}

		// Token: 0x0200046C RID: 1132
		private class FadedMatRequestComparer : IEqualityComparer<FadedMaterialPool.FadedMatRequest>
		{
			// Token: 0x06001CB8 RID: 7352 RVA: 0x00019F69 File Offset: 0x00018169
			public bool Equals(FadedMaterialPool.FadedMatRequest x, FadedMaterialPool.FadedMatRequest y)
			{
				return x.Equals(y);
			}

			// Token: 0x06001CB9 RID: 7353 RVA: 0x00019F73 File Offset: 0x00018173
			public int GetHashCode(FadedMaterialPool.FadedMatRequest obj)
			{
				return obj.GetHashCode();
			}

			// Token: 0x04001487 RID: 5255
			public static readonly FadedMaterialPool.FadedMatRequestComparer Instance = new FadedMaterialPool.FadedMatRequestComparer();
		}
	}
}
