using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DC0 RID: 7616
	public class RoyalAid
	{
		// Token: 0x04007037 RID: 28727
		public int favorCost;

		// Token: 0x04007038 RID: 28728
		public int points;

		// Token: 0x04007039 RID: 28729
		public int pawnCount;

		// Token: 0x0400703A RID: 28730
		public PawnKindDef pawnKindDef;

		// Token: 0x0400703B RID: 28731
		public float targetingRange;

		// Token: 0x0400703C RID: 28732
		public bool targetingRequireLOS = true;

		// Token: 0x0400703D RID: 28733
		public float aidDurationDays;

		// Token: 0x0400703E RID: 28734
		public float radius;

		// Token: 0x0400703F RID: 28735
		public int intervalTicks;

		// Token: 0x04007040 RID: 28736
		public int explosionCount;

		// Token: 0x04007041 RID: 28737
		public int warmupTicks;

		// Token: 0x04007042 RID: 28738
		public FloatRange explosionRadiusRange;

		// Token: 0x04007043 RID: 28739
		public List<ThingDefCountClass> itemsToDrop;
	}
}
