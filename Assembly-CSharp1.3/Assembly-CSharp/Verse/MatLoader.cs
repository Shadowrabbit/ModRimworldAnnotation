using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004D1 RID: 1233
	public static class MatLoader
	{
		// Token: 0x06002566 RID: 9574 RVA: 0x000E951C File Offset: 0x000E771C
		public static Material LoadMat(string matPath, int renderQueue = -1)
		{
			Material material = (Material)Resources.Load("Materials/" + matPath, typeof(Material));
			if (material == null)
			{
				Log.Warning("Could not load material " + matPath);
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

		// Token: 0x0400174B RID: 5963
		private static Dictionary<MatLoader.Request, Material> dict = new Dictionary<MatLoader.Request, Material>();

		// Token: 0x02001CD5 RID: 7381
		private struct Request
		{
			// Token: 0x0600A84B RID: 43083 RVA: 0x003860F7 File Offset: 0x003842F7
			public override int GetHashCode()
			{
				return Gen.HashCombineInt(Gen.HashCombine<string>(0, this.path), this.renderQueue);
			}

			// Token: 0x0600A84C RID: 43084 RVA: 0x00386110 File Offset: 0x00384310
			public override bool Equals(object obj)
			{
				return obj is MatLoader.Request && this.Equals((MatLoader.Request)obj);
			}

			// Token: 0x0600A84D RID: 43085 RVA: 0x00386128 File Offset: 0x00384328
			public bool Equals(MatLoader.Request other)
			{
				return other.path == this.path && other.renderQueue == this.renderQueue;
			}

			// Token: 0x0600A84E RID: 43086 RVA: 0x0038614D File Offset: 0x0038434D
			public static bool operator ==(MatLoader.Request lhs, MatLoader.Request rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x0600A84F RID: 43087 RVA: 0x00386157 File Offset: 0x00384357
			public static bool operator !=(MatLoader.Request lhs, MatLoader.Request rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x0600A850 RID: 43088 RVA: 0x00386163 File Offset: 0x00384363
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

			// Token: 0x04006F46 RID: 28486
			public string path;

			// Token: 0x04006F47 RID: 28487
			public int renderQueue;
		}
	}
}
