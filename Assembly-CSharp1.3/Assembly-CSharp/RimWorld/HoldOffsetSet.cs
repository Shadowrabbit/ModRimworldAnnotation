using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A28 RID: 2600
	public class HoldOffsetSet
	{
		// Token: 0x06003F20 RID: 16160 RVA: 0x00158468 File Offset: 0x00156668
		public HoldOffset Pick(Rot4 rotation)
		{
			if (rotation == Rot4.North)
			{
				return this.northDefault;
			}
			if (rotation == Rot4.East)
			{
				return this.east;
			}
			if (rotation == Rot4.South)
			{
				return this.south;
			}
			if (rotation == Rot4.West)
			{
				return this.west;
			}
			return null;
		}

		// Token: 0x0400226E RID: 8814
		public HoldOffset northDefault;

		// Token: 0x0400226F RID: 8815
		public HoldOffset east;

		// Token: 0x04002270 RID: 8816
		public HoldOffset south;

		// Token: 0x04002271 RID: 8817
		public HoldOffset west;
	}
}
