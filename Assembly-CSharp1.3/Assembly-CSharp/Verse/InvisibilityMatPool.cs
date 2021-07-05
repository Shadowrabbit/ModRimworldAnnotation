using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000268 RID: 616
	public static class InvisibilityMatPool
	{
		// Token: 0x0600117A RID: 4474 RVA: 0x00064BD8 File Offset: 0x00062DD8
		public static Material GetInvisibleMat(Material baseMat)
		{
			Material material;
			if (!InvisibilityMatPool.materials.TryGetValue(baseMat, out material))
			{
				material = MaterialAllocator.Create(baseMat);
				material.shader = ShaderDatabase.Invisible;
				material.SetTexture(InvisibilityMatPool.NoiseTex, TexGame.InvisDistortion);
				material.color = InvisibilityMatPool.color;
				InvisibilityMatPool.materials.Add(baseMat, material);
			}
			return material;
		}

		// Token: 0x04000D6F RID: 3439
		private static Dictionary<Material, Material> materials = new Dictionary<Material, Material>();

		// Token: 0x04000D70 RID: 3440
		private static Color color = new Color(0.75f, 0.93f, 0.98f, 0.5f);

		// Token: 0x04000D71 RID: 3441
		private static readonly int NoiseTex = Shader.PropertyToID("_NoiseTex");
	}
}
