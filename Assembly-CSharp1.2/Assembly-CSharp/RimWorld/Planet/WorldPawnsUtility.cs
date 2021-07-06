using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020021BE RID: 8638
	public static class WorldPawnsUtility
	{
		// Token: 0x0600B8E8 RID: 47336 RVA: 0x00077D1A File Offset: 0x00075F1A
		public static bool IsWorldPawn(this Pawn p)
		{
			return Find.WorldPawns.Contains(p);
		}
	}
}
