using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000303 RID: 771
	public static class FadedMaterialPool
	{
		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06001642 RID: 5698 RVA: 0x00081BF6 File Offset: 0x0007FDF6
		public static int TotalMaterialCount
		{
			get
			{
				return FadedMaterialPool.cachedMats.Count;
			}
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06001643 RID: 5699 RVA: 0x00081C04 File Offset: 0x0007FE04
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

		// Token: 0x06001644 RID: 5700 RVA: 0x00081C64 File Offset: 0x0007FE64
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

		// Token: 0x06001645 RID: 5701 RVA: 0x00081CDC File Offset: 0x0007FEDC
		private static int IndexFromAlpha(float alpha)
		{
			int num = Mathf.FloorToInt(alpha * 30f);
			if (num == 30)
			{
				num = 29;
			}
			return num;
		}

		// Token: 0x04000F81 RID: 3969
		private static Dictionary<FadedMaterialPool.FadedMatRequest, Material> cachedMats = new Dictionary<FadedMaterialPool.FadedMatRequest, Material>(FadedMaterialPool.FadedMatRequestComparer.Instance);

		// Token: 0x04000F82 RID: 3970
		private const int NumFadeSteps = 30;

		// Token: 0x02001A3B RID: 6715
		private struct FadedMatRequest : IEquatable<FadedMaterialPool.FadedMatRequest>
		{
			// Token: 0x06009C24 RID: 39972 RVA: 0x00368C33 File Offset: 0x00366E33
			public FadedMatRequest(Material mat, int alphaIndex)
			{
				this.mat = mat;
				this.alphaIndex = alphaIndex;
			}

			// Token: 0x06009C25 RID: 39973 RVA: 0x00368C43 File Offset: 0x00366E43
			public override bool Equals(object obj)
			{
				return obj != null && obj is FadedMaterialPool.FadedMatRequest && this.Equals((FadedMaterialPool.FadedMatRequest)obj);
			}

			// Token: 0x06009C26 RID: 39974 RVA: 0x00368C5E File Offset: 0x00366E5E
			public bool Equals(FadedMaterialPool.FadedMatRequest other)
			{
				return this.mat == other.mat && this.alphaIndex == other.alphaIndex;
			}

			// Token: 0x06009C27 RID: 39975 RVA: 0x00368C83 File Offset: 0x00366E83
			public override int GetHashCode()
			{
				return Gen.HashCombineInt(this.mat.GetHashCode(), this.alphaIndex);
			}

			// Token: 0x06009C28 RID: 39976 RVA: 0x00368C9B File Offset: 0x00366E9B
			public static bool operator ==(FadedMaterialPool.FadedMatRequest lhs, FadedMaterialPool.FadedMatRequest rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x06009C29 RID: 39977 RVA: 0x00368CA5 File Offset: 0x00366EA5
			public static bool operator !=(FadedMaterialPool.FadedMatRequest lhs, FadedMaterialPool.FadedMatRequest rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x0400647A RID: 25722
			private Material mat;

			// Token: 0x0400647B RID: 25723
			private int alphaIndex;
		}

		// Token: 0x02001A3C RID: 6716
		private class FadedMatRequestComparer : IEqualityComparer<FadedMaterialPool.FadedMatRequest>
		{
			// Token: 0x06009C2A RID: 39978 RVA: 0x00368CB1 File Offset: 0x00366EB1
			public bool Equals(FadedMaterialPool.FadedMatRequest x, FadedMaterialPool.FadedMatRequest y)
			{
				return x.Equals(y);
			}

			// Token: 0x06009C2B RID: 39979 RVA: 0x00368CBB File Offset: 0x00366EBB
			public int GetHashCode(FadedMaterialPool.FadedMatRequest obj)
			{
				return obj.GetHashCode();
			}

			// Token: 0x0400647C RID: 25724
			public static readonly FadedMaterialPool.FadedMatRequestComparer Instance = new FadedMaterialPool.FadedMatRequestComparer();
		}
	}
}
