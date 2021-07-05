using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000064 RID: 100
	public static class WildManUtility
	{
		// Token: 0x06000421 RID: 1057 RVA: 0x00015DFD File Offset: 0x00013FFD
		public static bool IsWildMan(this Pawn p)
		{
			return p.kindDef == PawnKindDefOf.WildMan;
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00015E0C File Offset: 0x0001400C
		public static bool AnimalOrWildMan(this Pawn p)
		{
			return p.RaceProps.Animal || p.IsWildMan();
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00015E23 File Offset: 0x00014023
		public static bool NonHumanlikeOrWildMan(this Pawn p)
		{
			return !p.RaceProps.Humanlike || p.IsWildMan();
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00015E3A File Offset: 0x0001403A
		public static bool WildManShouldReachOutsideNow(Pawn p)
		{
			return p.IsWildMan() && !p.mindState.WildManEverReachedOutside && (!p.IsPrisoner || p.guest.Released);
		}
	}
}
