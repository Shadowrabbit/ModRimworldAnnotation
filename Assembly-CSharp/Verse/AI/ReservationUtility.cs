using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A72 RID: 2674
	public static class ReservationUtility
	{
		// Token: 0x06003FE4 RID: 16356 RVA: 0x0002FD32 File Offset: 0x0002DF32
		public static bool CanReserve(this Pawn p, LocalTargetInfo target, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			return p.Spawned && p.Map.reservationManager.CanReserve(p, target, maxPawns, stackCount, layer, ignoreOtherReservations);
		}

		// Token: 0x06003FE5 RID: 16357 RVA: 0x0002FD56 File Offset: 0x0002DF56
		public static bool CanReserveAndReach(this Pawn p, LocalTargetInfo target, PathEndMode peMode, Danger maxDanger, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			return p.Spawned && p.CanReach(target, peMode, maxDanger, false, TraverseMode.ByPawn) && p.Map.reservationManager.CanReserve(p, target, maxPawns, stackCount, layer, ignoreOtherReservations);
		}

		// Token: 0x06003FE6 RID: 16358 RVA: 0x0002FD8B File Offset: 0x0002DF8B
		public static bool Reserve(this Pawn p, LocalTargetInfo target, Job job, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool errorOnFailed = true)
		{
			return p.Spawned && p.Map.reservationManager.Reserve(p, job, target, maxPawns, stackCount, layer, errorOnFailed);
		}

		// Token: 0x06003FE7 RID: 16359 RVA: 0x00181B1C File Offset: 0x0017FD1C
		public static void ReserveAsManyAsPossible(this Pawn p, List<LocalTargetInfo> target, Job job, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null)
		{
			if (!p.Spawned)
			{
				return;
			}
			for (int i = 0; i < target.Count; i++)
			{
				p.Map.reservationManager.Reserve(p, job, target[i], maxPawns, stackCount, layer, false);
			}
		}

		// Token: 0x06003FE8 RID: 16360 RVA: 0x0002FDB1 File Offset: 0x0002DFB1
		public static bool HasReserved(this Pawn p, LocalTargetInfo target, Job job = null)
		{
			return p.Spawned && p.Map.reservationManager.ReservedBy(target, p, job);
		}

		// Token: 0x06003FE9 RID: 16361 RVA: 0x0002FDD0 File Offset: 0x0002DFD0
		public static bool HasReserved<TDriver>(this Pawn p, LocalTargetInfo target, LocalTargetInfo? targetAIsNot = null, LocalTargetInfo? targetBIsNot = null, LocalTargetInfo? targetCIsNot = null)
		{
			return p.Spawned && p.Map.reservationManager.ReservedBy<TDriver>(target, p, targetAIsNot, targetBIsNot, targetCIsNot);
		}

		// Token: 0x06003FEA RID: 16362 RVA: 0x0002FDF2 File Offset: 0x0002DFF2
		public static bool CanReserveNew(this Pawn p, LocalTargetInfo target)
		{
			return target.IsValid && !p.HasReserved(target, null) && p.CanReserve(target, 1, -1, null, false);
		}
	}
}
