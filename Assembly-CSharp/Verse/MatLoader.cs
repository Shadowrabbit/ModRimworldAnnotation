using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200087B RID: 2171
	public static class MatLoader
	{
		// Token: 0x060035F2 RID: 13810 RVA: 0x0015B148 File Offset: 0x00159348
		public static Material LoadMat(string matPath, int renderQueue = -1)
		{
			Material material = (Material)Resources.Load("Materials/" + matPath, typeof(Material));
			if (material == null)
			{
				Log.Warning("Could not load material " + matPath, false);
			}
			MatLoader.Request key = new MatLoader.Request
			{
				path = matPath,
				renderQueue = renderQueue
			};
			Material material2;
			if (!MatLoader.dict.TryGetValue(key, out material2))
			{
				material2 = MaterialAllocator.Create(material);
				if (renderQueue != -1)
				{
					material2.renderQueue = renderQueue;
				}
				MatLoader.dict.Add(key, material2);
			}
			return material2;
		}

		// Token: 0x0400258B RID: 9611
		private static Dictionary<MatLoader.Request, Material> dict = new Dictionary<MatLoader.Request, Material>();

		// Token: 0x0200087C RID: 2172
		private struct Request
		{
			// Token: 0x060035F4 RID: 13812 RVA: 0x00029CE3 File Offset: 0x00027EE3
			public override int GetHashCode()
			{
				return Gen.HashCombineInt(Gen.HashCombine<string>(0, this.path), this.renderQueue);
			}

			// Token: 0x060035F5 RID: 13813 RVA: 0x00029CFC File Offset: 0x00027EFC
			public override bool Equals(object obj)
			{
				return obj is MatLoader.Request && this.Equals((MatLoader.Request)obj);
			}

			// Token: 0x060035F6 RID: 13814 RVA: 0x00029D14 File Offset: 0x00027F14
			public bool Equals(MatLoader.Request other)
			{
				return other.path == this.path && other.renderQueue == this.renderQueue;
			}

			// Token: 0x060035F7 RID: 13815 RVA: 0x00029D39 File Offset: 0x00027F39
			public static bool operator ==(MatLoader.Request lhs, MatLoader.Request rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x060035F8 RID: 13816 RVA: 0x00029D43 File Offset: 0x00027F43
			public static bool operator !=(MatLoader.Request lhs, MatLoader.Request rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x060035F9 RID: 13817 RVA: 0x00029D4F File Offset: 0x00027F4F
			public override string ToString()
			{
				return string.Concat(new object[]
				{
					"MatLoader.Request(",
					this.path,
					", ",
					this.renderQueue,
					")"
				});
			}

			// Token: 0x0400258C RID: 9612
			public string path;

			// Token: 0x0400258D RID: 9613
			public int renderQueue;
		}
	}
}
