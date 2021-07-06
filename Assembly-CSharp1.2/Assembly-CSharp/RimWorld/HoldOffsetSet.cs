using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F38 RID: 3896
	public class HoldOffsetSet
	{
		// Token: 0x060055A8 RID: 21928 RVA: 0x001C8BB4 File Offset: 0x001C6DB4
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

		// Token: 0x0400370B RID: 14091
		public HoldOffset northDefault;

		// Token: 0x0400370C RID: 14092
		public HoldOffset east;

		// Token: 0x0400370D RID: 14093
		public HoldOffset south;

		// Token: 0x0400370E RID: 14094
		public HoldOffset west;
	}
}
