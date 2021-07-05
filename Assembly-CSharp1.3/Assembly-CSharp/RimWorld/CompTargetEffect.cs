using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011E0 RID: 4576
	public abstract class CompTargetEffect : ThingComp
	{
		// Token: 0x06006E6B RID: 28267
		public abstract void DoEffectOn(Pawn user, Thing target);
	}
}
