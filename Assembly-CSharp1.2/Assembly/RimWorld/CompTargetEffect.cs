using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018C2 RID: 6338
	public abstract class CompTargetEffect : ThingComp
	{
		// Token: 0x06008CA9 RID: 36009
		public abstract void DoEffectOn(Pawn user, Thing target);
	}
}
