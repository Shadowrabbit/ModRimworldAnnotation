using System;

namespace Verse
{
	// Token: 0x020002B5 RID: 693
	public class ScattererValidator_NoNonNaturalEdifices : ScattererValidator
	{
		// Token: 0x060011A3 RID: 4515 RVA: 0x000C334C File Offset: 0x000C154C
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

		// Token: 0x04000E48 RID: 3656
		public int radius = 1;
	}
}
