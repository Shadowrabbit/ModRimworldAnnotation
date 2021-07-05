using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000612 RID: 1554
	public static class ReservationUtility
	{
		// Token: 0x06002CFF RID: 11519 RVA: 0x0010ECE4 File Offset: 0x0010CEE4
		public static bool CanReserveSittableOrSpot(this Pawn pawn, IntVec3 exactSittingPos, bool ignoreOtherReservations = false)
		{
			Building edifice = exactSittingPos.GetEdifice(pawn.Map);
			if (exactSittingPos.Impassable(pawn.Map))
			{
				return false;
			}
			if (edifice == null || edifice.def.building.multiSittable)
			{
				return pawn.CanReserve(exactSittingPos, 1, -1, null, ignoreOtherReservations);
			}
			return (edifice == null || !edifice.def.building.isSittable || !edifice.def.hasInteractionCell || !(exactSittingPos != edifice.InteractionCell)) && pawn.CanReserve(edifice, 1, -1, null, ignoreOtherReservations);
		}

		// Token: 0x06002D00 RID: 11520 RVA: 0x0010ED78 File Offset: 0x0010CF78
		public static bool ReserveSittableOrSpot(this Pawn pawn, IntVec3 exactSittingPos, Job job, bool errorOnFailed = true)
		{
			Building edifice = exactSittingPos.GetEdifice(pawn.Map);
			if (exactSittingPos.Impassable(pawn.Map))
			{
				Log.Error("Tried reserving impassable sittable or spot.");
				return false;
			}
			if (edifice == null || edifice.def.building.multiSittable)
			{
				return pawn.Reserve(exactSittingPos, job, 1, -1, null, errorOnFailed);
			}
			return (edifice == null || !edifice.def.building.isSittable || !edifice.def.hasInteractionCell || !(exactSittingPos != edifice.InteractionCell)) && pawn.Reserve(edifice, job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002D01 RID: 11521 RVA: 0x0010EE16 File Offset: 0x0010D016
		public static bool CanReserve(this Pawn p, LocalTargetInfo target, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			return p.Spawned && p.Map.reservationManager.CanReserve(p, target, maxPawns, stackCount, layer, ignoreOtherReservations);
		}

		// Token: 0x06002D02 RID: 11522 RVA: 0x0010EE3A File Offset: 0x0010D03A
		public static bool CanReserveAndReach(this Pawn p, LocalTargetInfo target, PathEndMode peMode, Danger maxDanger, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			return p.Spawned && p.CanReach(target, peMode, maxDanger, false, false, TraverseMode.ByPawn) && p.Map.reservationManager.CanReserve(p, target, maxPawns, stackCount, layer, ignoreOtherReservations);
		}

		// Token: 0x06002D03 RID: 11523 RVA: 0x0010EE70 File Offset: 0x0010D070
		public static bool Reserve(this Pawn p, LocalTargetInfo target, Job job, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool errorOnFailed = true)
		{
			return p.Spawned && p.Map.reservationManager.Reserve(p, job, target, maxPawns, stackCount, layer, errorOnFailed);
		}

		// Token: 0x06002D04 RID: 11524 RVA: 0x0010EE98 File Offset: 0x0010D098
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

		// Token: 0x06002D05 RID: 11525 RVA: 0x0010EEDF File Offset: 0x0010D0DF
		public static bool HasReserved(this Pawn p, LocalTargetInfo target, Job job = null)
		{
			return p.Spawned && p.Map.reservationManager.ReservedBy(target, p, job);
		}

		// Token: 0x06002D06 RID: 11526 RVA: 0x0010EEFE File Offset: 0x0010D0FE
		public static bool HasReserved<TDriver>(this Pawn p, LocalTargetInfo target, LocalTargetInfo? targetAIsNot = null, LocalTargetInfo? targetBIsNot = null, LocalTargetInfo? targetCIsNot = null)
		{
			return p.Spawned && p.Map.reservationManager.ReservedBy<TDriver>(target, p, targetAIsNot, targetBIsNot, targetCIsNot);
		}

		// Token: 0x06002D07 RID: 11527 RVA: 0x0010EF20 File Offset: 0x0010D120
		public static bool CanReserveNew(this Pawn p, LocalTargetInfo target)
		{
			return target.IsValid && !p.HasReserved(target, null) && p.CanReserve(target, 1, -1, null, false);
		}
	}
}
