using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F72 RID: 3954
	public class TerrainThreshold
	{
		// Token: 0x060056D5 RID: 22229 RVA: 0x001CBA6C File Offset: 0x001C9C6C
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

		// Token: 0x04003823 RID: 14371
		public TerrainDef terrain;

		// Token: 0x04003824 RID: 14372
		public float min = -1000f;

		// Token: 0x04003825 RID: 14373
		public float max = 1000f;
	}
}
