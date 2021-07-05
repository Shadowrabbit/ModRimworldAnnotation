using System;

namespace Verse
{
	// Token: 0x020001EB RID: 491
	public class ScattererValidator_Buildable : ScattererValidator
	{
		// Token: 0x06000DDD RID: 3549 RVA: 0x0004E528 File Offset: 0x0004C728
		public override bool Allows(IntVec3 c, Map map)
		{
			CellRect cellRect = CellRect.CenteredOn(c, this.radius);
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c2 = new IntVec3(j, 0, i);
					if (!c2.InBounds(map))
					{
						return false;
					}
					if (c2.InNoBuildEdgeArea(map))
					{
						return false;
					}
					if (this.affordance != null && !c2.GetTerrain(map).affordances.Contains(this.affordance))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04000B58 RID: 2904
		public int radius = 1;

		// Token: 0x04000B59 RID: 2905
		public TerrainAffordanceDef affordance;
	}
}
