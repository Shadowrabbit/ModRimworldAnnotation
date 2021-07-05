using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x020001F9 RID: 505
	public static class ReachabilityUtility
	{
		// Token: 0x06000E3F RID: 3647 RVA: 0x000506F8 File Offset: 0x0004E8F8
		public static bool CanReach(this Pawn pawn, LocalTargetInfo dest, PathEndMode peMode, Danger maxDanger, bool canBashDoors = false, bool canBashFences = false, TraverseMode mode = TraverseMode.ByPawn)
		{
			return pawn.Spawned && pawn.Map.reachability.CanReach(pawn.Position, dest, peMode, TraverseParms.For(pawn, maxDanger, mode, canBashDoors, false, canBashFences));
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x00050738 File Offset: 0x0004E938
		public static bool CanReachNonLocal(this Pawn pawn, TargetInfo dest, PathEndMode peMode, Danger maxDanger, bool canBashDoors = false, TraverseMode mode = TraverseMode.ByPawn)
		{
			return pawn.Spawned && pawn.Map.reachability.CanReachNonLocal(pawn.Position, dest, peMode, TraverseParms.For(pawn, maxDanger, mode, canBashDoors, false, false));
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x00050774 File Offset: 0x0004E974
		public static bool CanReachMapEdge(this Pawn p)
		{
			return p.Spawned && p.Map.reachability.CanReachMapEdge(p.Position, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false, false, false));
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x000507A4 File Offset: 0x0004E9A4
		public static void ClearCache()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].reachability.ClearCache();
			}
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x000507DC File Offset: 0x0004E9DC
		public static void ClearCacheFor(Pawn p)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].reachability.ClearCacheFor(p);
			}
		}
	}
}
