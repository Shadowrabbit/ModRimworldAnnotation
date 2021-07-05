using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000385 RID: 901
	public static class DamagedMatPool
	{
		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06001693 RID: 5779 RVA: 0x00015FF6 File Offset: 0x000141F6
		public static int MatCount
		{
			get
			{
				return DamagedMatPool.damagedMats.Count;
			}
		}

		// Token: 0x06001694 RID: 5780 RVA: 0x000D71FC File Offset: 0x000D53FC
		public static Material GetDamageFlashMat(Material baseMat, float damPct)
		{
			if (damPct < 0.01f)
			{
				return baseMat;
			}
			Material material;
			if (!DamagedMatPool.damagedMats.TryGetValue(baseMat, out material))
			{
				material = MaterialAllocator.Create(baseMat);
				DamagedMatPool.damagedMats.Add(baseMat, material);
			}
			Color color = Color.Lerp(baseMat.color, DamagedMatPool.DamagedMatStartingColor, damPct);
			material.color = color;
			return material;
		}

		// Token: 0x0400115B RID: 4443
		private static Dictionary<Material, Material> damagedMats = new Dictionary<Material, Material>();

		// Token: 0x0400115C RID: 4444
		private static readonly Color DamagedMatStartingColor = Color.red;
	}
}
