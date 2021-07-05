using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000AD RID: 173
	public static class WildManUtility
	{
		// Token: 0x06000582 RID: 1410 RVA: 0x0000AA7B File Offset: 0x00008C7B
		public static bool IsWildMan(this Pawn p)
		{
			return p.kindDef == PawnKindDefOf.WildMan;
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0000AA8A File Offset: 0x00008C8A
		public static bool AnimalOrWildMan(this Pawn p)
		{
			return p.RaceProps.Animal || p.IsWildMan();
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0000AAA1 File Offset: 0x00008CA1
		public static bool NonHumanlikeOrWildMan(this Pawn p)
		{
			return !p.RaceProps.Humanlike || p.IsWildMan();
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0000AAB8 File Offset: 0x00008CB8
		public static bool WildManShouldReachOutsideNow(Pawn p)
		{
			return p.IsWildMan() && !p.mindState.WildManEverReachedOutside && (!p.IsPrisoner || p.guest.Released);
		}
	}
}
