using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000790 RID: 1936
	public static class UnloadCarriersJobGiverUtility
	{
		// Token: 0x06003512 RID: 13586 RVA: 0x0012C540 File Offset: 0x0012A740
		public static bool HasJobOnThing(Pawn pawn, Thing t, bool forced)
		{
			Pawn pawn2 = t as Pawn;
			return pawn2 != null && pawn2 != pawn && !pawn2.IsFreeColonist && pawn2.inventory.UnloadEverything && (pawn2.Faction == pawn.Faction || pawn2.HostFaction == pawn.Faction) && !t.IsForbidden(pawn) && !t.IsBurning() && pawn.CanReserve(t, 1, -1, null, forced);
		}
	}
}
