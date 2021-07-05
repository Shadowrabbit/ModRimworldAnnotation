using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001144 RID: 4420
	public static class HackUtility
	{
		// Token: 0x06006A1F RID: 27167 RVA: 0x0023B85B File Offset: 0x00239A5B
		public static bool IsHackable(this Thing thing)
		{
			return thing.TryGetComp<CompHackable>() != null;
		}

		// Token: 0x06006A20 RID: 27168 RVA: 0x0023B868 File Offset: 0x00239A68
		public static bool IsHacked(this Thing thing)
		{
			CompHackable compHackable = thing.TryGetComp<CompHackable>();
			return compHackable != null && compHackable.IsHacked;
		}

		// Token: 0x06006A21 RID: 27169 RVA: 0x0023B887 File Offset: 0x00239A87
		public static bool IsCapableOfHacking(Pawn pawn)
		{
			return !pawn.WorkTagIsDisabled(WorkTags.Intellectual) && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation);
		}
	}
}
