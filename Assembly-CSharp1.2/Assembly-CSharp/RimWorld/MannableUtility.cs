using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017DE RID: 6110
	public static class MannableUtility
	{
		// Token: 0x0600873C RID: 34620 RVA: 0x0027B5A8 File Offset: 0x002797A8
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
