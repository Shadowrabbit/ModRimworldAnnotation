using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002AE RID: 686
	public class GenStep_TerrainPatches : GenStep
	{
		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06001181 RID: 4481 RVA: 0x00012BF0 File Offset: 0x00010DF0
		public override int SeedPart
		{
			get
			{
				return 1370184742;
			}
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x000C2928 File Offset: 0x000C0B28
		public override void Generate(Map map, GenStepParams parms)
		{
			int num = Mathf.RoundToInt((float)map.Area / 10000f * this.patchesPer10kCellsRange.RandomInRange);
			for (int i = 0; i < num; i++)
			{
				float randomInRange = this.patchSizeRange.RandomInRange;
				IntVec3 a = CellFinder.RandomCell(map);
				foreach (IntVec3 b in GenRadial.RadialPatternInRadius(randomInRange / 2f))
				{
					IntVec3 c = a + b;
					if (c.InBounds(map))
					{
						map.terrainGrid.SetTerrain(c, this.terrainDef);
					}
				}
			}
		}

		// Token: 0x04000E25 RID: 3621
		public TerrainDef terrainDef;

		// Token: 0x04000E26 RID: 3622
		public FloatRange patchesPer10kCellsRange;

		// Token: 0x04000E27 RID: 3623
		public FloatRange patchSizeRange;
	}
}
