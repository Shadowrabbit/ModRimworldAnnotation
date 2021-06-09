using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x020002CA RID: 714
	public static class ReachabilityUtility
	{
		// Token: 0x06001212 RID: 4626 RVA: 0x00013108 File Offset: 0x00011308
		public static bool CanReach(this Pawn pawn, LocalTargetInfo dest, PathEndMode peMode, Danger maxDanger, bool canBash = false, TraverseMode mode = TraverseMode.ByPawn)
		{
			return pawn.Spawned && pawn.Map.reachability.CanReach(pawn.Position, dest, peMode, TraverseParms.For(pawn, maxDanger, mode, canBash));
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x00013137 File Offset: 0x00011337
		public static bool CanReachNonLocal(this Pawn pawn, TargetInfo dest, PathEndMode peMode, Danger maxDanger, bool canBash = false, TraverseMode mode = TraverseMode.ByPawn)
		{
			return pawn.Spawned && pawn.Map.reachability.CanReachNonLocal(pawn.Position, dest, peMode, TraverseParms.For(pawn, maxDanger, mode, canBash));
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x00013166 File Offset: 0x00011366
		public static bool CanReachMapEdge(this Pawn p)
		{
			return p.Spawned && p.Map.reachability.CanReachMapEdge(p.Position, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false));
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x000C4C34 File Offset: 0x000C2E34
		public static void ClearCache()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].reachability.ClearCache();
			}
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x000C4C6C File Offset: 0x000C2E6C
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
