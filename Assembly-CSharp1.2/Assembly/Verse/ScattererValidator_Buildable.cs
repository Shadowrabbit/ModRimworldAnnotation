using System;

namespace Verse
{
	// Token: 0x020002B4 RID: 692
	public class ScattererValidator_Buildable : ScattererValidator
	{
		// Token: 0x060011A1 RID: 4513 RVA: 0x000C32C0 File Offset: 0x000C14C0
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

		// Token: 0x04000E46 RID: 3654
		public int radius = 1;

		// Token: 0x04000E47 RID: 3655
		public TerrainAffordanceDef affordance;
	}
}
