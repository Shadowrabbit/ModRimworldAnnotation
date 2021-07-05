using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D7E RID: 3454
	public class CompProperties_EffectWithDest : CompProperties_AbilityEffect
	{
		// Token: 0x04002FD6 RID: 12246
		public AbilityEffectDestination destination;

		// Token: 0x04002FD7 RID: 12247
		public bool requiresLineOfSight;

		// Token: 0x04002FD8 RID: 12248
		public float range;

		// Token: 0x04002FD9 RID: 12249
		public FloatRange randomRange;

		// Token: 0x04002FDA RID: 12250
		public ClamorDef destClamorType;

		// Token: 0x04002FDB RID: 12251
		public int destClamorRadius;
	}
}
