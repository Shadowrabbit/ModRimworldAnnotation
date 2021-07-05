using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001533 RID: 5427
	public class RoyalAid
	{
		// Token: 0x0400505B RID: 20571
		public int favorCost;

		// Token: 0x0400505C RID: 20572
		public int points;

		// Token: 0x0400505D RID: 20573
		public int pawnCount;

		// Token: 0x0400505E RID: 20574
		public PawnKindDef pawnKindDef;

		// Token: 0x0400505F RID: 20575
		public float targetingRange;

		// Token: 0x04005060 RID: 20576
		public bool targetingRequireLOS = true;

		// Token: 0x04005061 RID: 20577
		public float aidDurationDays;

		// Token: 0x04005062 RID: 20578
		public float radius;

		// Token: 0x04005063 RID: 20579
		public int intervalTicks;

		// Token: 0x04005064 RID: 20580
		public int explosionCount;

		// Token: 0x04005065 RID: 20581
		public int warmupTicks;

		// Token: 0x04005066 RID: 20582
		public FloatRange explosionRadiusRange;

		// Token: 0x04005067 RID: 20583
		public List<ThingDefCountClass> itemsToDrop;
	}
}
