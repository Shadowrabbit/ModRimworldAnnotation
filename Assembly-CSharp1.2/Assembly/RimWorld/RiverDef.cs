using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FD1 RID: 4049
	public class RiverDef : Def
	{
		// Token: 0x04003A90 RID: 14992
		public int spawnFlowThreshold = -1;

		// Token: 0x04003A91 RID: 14993
		public float spawnChance = 1f;

		// Token: 0x04003A92 RID: 14994
		public int degradeThreshold;

		// Token: 0x04003A93 RID: 14995
		public RiverDef degradeChild;

		// Token: 0x04003A94 RID: 14996
		public List<RiverDef.Branch> branches;

		// Token: 0x04003A95 RID: 14997
		public float widthOnWorld = 0.5f;

		// Token: 0x04003A96 RID: 14998
		public float widthOnMap = 10f;

		// Token: 0x04003A97 RID: 14999
		public float debugOpacity;

		// Token: 0x02000FD2 RID: 4050
		public class Branch
		{
			// Token: 0x04003A98 RID: 15000
			public int minFlow;

			// Token: 0x04003A99 RID: 15001
			public RiverDef child;

			// Token: 0x04003A9A RID: 15002
			public float chance = 1f;
		}
	}
}
