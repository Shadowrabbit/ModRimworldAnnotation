using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A4C RID: 2636
	public class TerrainThreshold
	{
		// Token: 0x06003F94 RID: 16276 RVA: 0x001594D0 File Offset: 0x001576D0
		public static TerrainDef TerrainAtValue(List<TerrainThreshold> threshes, float val)
		{
			for (int i = 0; i < threshes.Count; i++)
			{
				if (threshes[i].min <= val && threshes[i].max >= val)
				{
					return threshes[i].terrain;
				}
			}
			return null;
		}

		// Token: 0x04002320 RID: 8992
		public TerrainDef terrain;

		// Token: 0x04002321 RID: 8993
		public float min = -1000f;

		// Token: 0x04002322 RID: 8994
		public float max = 1000f;
	}
}
