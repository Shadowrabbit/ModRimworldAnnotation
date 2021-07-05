using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AB3 RID: 2739
	public class RiverDef : Def
	{
		// Token: 0x04002652 RID: 9810
		public int spawnFlowThreshold = -1;

		// Token: 0x04002653 RID: 9811
		public float spawnChance = 1f;

		// Token: 0x04002654 RID: 9812
		public int degradeThreshold;

		// Token: 0x04002655 RID: 9813
		public RiverDef degradeChild;

		// Token: 0x04002656 RID: 9814
		public List<RiverDef.Branch> branches;

		// Token: 0x04002657 RID: 9815
		public float widthOnWorld = 0.5f;

		// Token: 0x04002658 RID: 9816
		public float widthOnMap = 10f;

		// Token: 0x04002659 RID: 9817
		public float debugOpacity;

		// Token: 0x02002028 RID: 8232
		public class Branch
		{
			// Token: 0x04007B44 RID: 31556
			public int minFlow;

			// Token: 0x04007B45 RID: 31557
			public RiverDef child;

			// Token: 0x04007B46 RID: 31558
			public float chance = 1f;
		}
	}
}
