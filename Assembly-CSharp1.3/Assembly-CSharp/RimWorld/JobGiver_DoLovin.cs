using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007BC RID: 1980
	public class JobGiver_DoLovin : ThinkNode_JobGiver
	{
		// Token: 0x06003594 RID: 13716 RVA: 0x0012ED04 File Offset: 0x0012CF04
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (Find.TickManager.TicksGame < pawn.mindState.canLovinTick)
			{
				return null;
			}
			if (pawn.CurrentBed() == null || pawn.CurrentBed().Medical || !pawn.health.capacities.CanBeAwake)
			{
				return null;
			}
			Pawn partnerInMyBed = LovePartnerRelationUtility.GetPartnerInMyBed(pawn);
			if (partnerInMyBed == null || !partnerInMyBed.health.capacities.CanBeAwake || Find.TickManager.TicksGame < partnerInMyBed.mindState.canLovinTick)
			{
				return null;
			}
			if (!pawn.CanReserve(partnerInMyBed, 1, -1, null, false) || !partnerInMyBed.CanReserve(pawn, 1, -1, null, false))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Lovin, partnerInMyBed, pawn.CurrentBed());
		}
	}
}
