using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000261 RID: 609
	public static class DamagedMatPool
	{
		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06001145 RID: 4421 RVA: 0x000621A9 File Offset: 0x000603A9
		public static int MatCount
		{
			get
			{
				return DamagedMatPool.damagedMats.Count;
			}
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x000621B8 File Offset: 0x000603B8
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

		// Token: 0x04000D27 RID: 3367
		private static Dictionary<Material, Material> damagedMats = new Dictionary<Material, Material>();

		// Token: 0x04000D28 RID: 3368
		private static readonly Color DamagedMatStartingColor = Color.red;
	}
}
