using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200038B RID: 907
	public static class InvisibilityMatPool
	{
		// Token: 0x060016BF RID: 5823 RVA: 0x000D8D84 File Offset: 0x000D6F84
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

		// Token: 0x04001195 RID: 4501
		private static Dictionary<Material, Material> materials = new Dictionary<Material, Material>();

		// Token: 0x04001196 RID: 4502
		private static Color color = new Color(0.75f, 0.93f, 0.98f, 0.5f);

		// Token: 0x04001197 RID: 4503
		private static readonly int NoiseTex = Shader.PropertyToID("_NoiseTex");
	}
}
