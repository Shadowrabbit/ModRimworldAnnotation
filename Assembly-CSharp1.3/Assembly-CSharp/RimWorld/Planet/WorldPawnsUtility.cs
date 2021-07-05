using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001805 RID: 6149
	public static class WorldPawnsUtility
	{
		// Token: 0x06008FCC RID: 36812 RVA: 0x00337C09 File Offset: 0x00335E09
		public static bool IsWorldPawn(this Pawn p)
		{
			return Find.WorldPawns.Contains(p);
		}
	}
}
