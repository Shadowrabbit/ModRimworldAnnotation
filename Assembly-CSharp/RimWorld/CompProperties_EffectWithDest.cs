using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013A3 RID: 5027
	public class CompProperties_EffectWithDest : CompProperties_AbilityEffect
	{
		// Token: 0x04004831 RID: 18481
		public AbilityEffectDestination destination;

		// Token: 0x04004832 RID: 18482
		public bool requiresLineOfSight;

		// Token: 0x04004833 RID: 18483
		public float range;

		// Token: 0x04004834 RID: 18484
		public FloatRange randomRange;

		// Token: 0x04004835 RID: 18485
		public ClamorDef destClamorType;

		// Token: 0x04004836 RID: 18486
		public int destClamorRadius;
	}
}
