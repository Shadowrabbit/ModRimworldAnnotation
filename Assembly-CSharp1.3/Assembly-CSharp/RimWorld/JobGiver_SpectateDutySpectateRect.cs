using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007A5 RID: 1957
	public class JobGiver_SpectateDutySpectateRect : ThinkNode_JobGiver
	{
		// Token: 0x06003550 RID: 13648 RVA: 0x0012D960 File Offset: 0x0012BB60
		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			if (duty == null)
			{
				return null;
			}
			IntVec3 intVec;
			if (!this.TryFindSpot(pawn, duty, out intVec))
			{
				return null;
			}
			IntVec3 centerCell = duty.spectateRect.CenterCell;
			Building edifice = intVec.GetEdifice(pawn.Map);
			if (edifice != null && pawn.CanReserveSittableOrSpot(intVec, false))
			{
				return JobMaker.MakeJob(JobDefOf.SpectateCeremony, intVec, centerCell, edifice);
			}
			return JobMaker.MakeJob(JobDefOf.SpectateCeremony, intVec, centerCell);
		}

		// Token: 0x06003551 RID: 13649 RVA: 0x0012D9E8 File Offset: 0x0012BBE8
		protected virtual bool TryFindSpot(Pawn pawn, PawnDuty duty, out IntVec3 spot)
		{
			JobGiver_SpectateDutySpectateRect.<>c__DisplayClass1_0 CS$<>8__locals1 = new JobGiver_SpectateDutySpectateRect.<>c__DisplayClass1_0();
			CS$<>8__locals1.pawn = pawn;
			Precept_Ritual ritual = null;
			LordJob_Ritual lordJob_Ritual;
			if (CS$<>8__locals1.pawn.GetLord() != null && (lordJob_Ritual = (CS$<>8__locals1.pawn.GetLord().LordJob as LordJob_Ritual)) != null)
			{
				ritual = lordJob_Ritual.Ritual;
			}
			if ((duty.spectateRectPreferredSide != SpectateRectSide.None && SpectatorCellFinder.TryFindSpectatorCellFor(CS$<>8__locals1.pawn, duty.spectateRect, CS$<>8__locals1.pawn.Map, out spot, duty.spectateRectPreferredSide, 1, null, ritual, new Func<IntVec3, Pawn, Map, bool>(RitualUility.GoodSpectateCellForRitual))) || SpectatorCellFinder.TryFindSpectatorCellFor(CS$<>8__locals1.pawn, duty.spectateRect, CS$<>8__locals1.pawn.Map, out spot, duty.spectateRectAllowedSides, 1, null, ritual, new Func<IntVec3, Pawn, Map, bool>(RitualUility.GoodSpectateCellForRitual)))
			{
				return true;
			}
			IntVec3 target = duty.spectateRect.CenterCell;
			if (CellFinder.TryFindRandomReachableCellNear(target, CS$<>8__locals1.pawn.MapHeld, 5f, TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), (IntVec3 c) => c.GetRoom(CS$<>8__locals1.pawn.MapHeld) == target.GetRoom(CS$<>8__locals1.pawn.MapHeld) && CS$<>8__locals1.pawn.CanReserveSittableOrSpot(c, false), null, out spot, 999999))
			{
				return true;
			}
			Log.Warning("Failed to find a spectator spot for " + CS$<>8__locals1.pawn);
			return false;
		}
	}
}
