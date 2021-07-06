using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CAB RID: 3243
	public static class UnloadCarriersJobGiverUtility
	{
		// Token: 0x06004B61 RID: 19297 RVA: 0x001A5598 File Offset: 0x001A3798
		public static bool HasJobOnThing(Pawn pawn, Thing t, bool forced)
		{
			Pawn pawn2 = t as Pawn;
			return pawn2 != null && pawn2 != pawn && !pawn2.IsFreeColonist && pawn2.inventory.UnloadEverything && (pawn2.Faction == pawn.Faction || pawn2.HostFaction == pawn.Faction) && !t.IsForbidden(pawn) && !t.IsBurning() && pawn.CanReserve(t, 1, -1, null, forced);
		}
	}
}
