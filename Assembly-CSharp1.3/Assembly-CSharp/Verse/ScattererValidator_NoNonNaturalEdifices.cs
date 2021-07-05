using System;

namespace Verse
{
	// Token: 0x020001EC RID: 492
	public class ScattererValidator_NoNonNaturalEdifices : ScattererValidator
	{
		// Token: 0x06000DDF RID: 3551 RVA: 0x0004E5C4 File Offset: 0x0004C7C4
		public override bool Allows(IntVec3 c, Map map)
		{
			CellRect cellRect = CellRect.CenteredOn(c, this.radius);
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					if (new IntVec3(j, 0, i).GetEdifice(map) != null)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04000B5A RID: 2906
		public int radius = 1;
	}
}
