using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001E7 RID: 487
	public class GenStep_TerrainPatches : GenStep
	{
		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000DC3 RID: 3523 RVA: 0x0004DB27 File Offset: 0x0004BD27
		public override int SeedPart
		{
			get
			{
				return 1370184742;
			}
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x0004DB30 File Offset: 0x0004BD30
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

		// Token: 0x04000B3A RID: 2874
		public TerrainDef terrainDef;

		// Token: 0x04000B3B RID: 2875
		public FloatRange patchesPer10kCellsRange;

		// Token: 0x04000B3C RID: 2876
		public FloatRange patchSizeRange;
	}
}
