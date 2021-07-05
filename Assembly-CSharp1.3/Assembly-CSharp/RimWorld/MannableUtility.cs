using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200114E RID: 4430
	public static class MannableUtility
	{
		// Token: 0x06006A7A RID: 27258 RVA: 0x0023CFA0 File Offset: 0x0023B1A0
		public static Thing MannedThing(this Pawn pawn)
		{
			if (pawn.Dead)
			{
				return null;
			}
			Thing lastMannedThing = pawn.mindState.lastMannedThing;
			if (lastMannedThing == null || lastMannedThing.TryGetComp<CompMannable>().ManningPawn != pawn)
			{
				return null;
			}
			return lastMannedThing;
		}
	}
}
