using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007FB RID: 2043
	public class JobGiver_ReachOutside : ThinkNode_JobGiver
	{
		// Token: 0x06003695 RID: 13973 RVA: 0x001356A0 File Offset: 0x001338A0
		protected override Job TryGiveJob(Pawn pawn)
		{
			District district = pawn.GetDistrict(RegionType.Set_Passable);
			if (district.Room.PsychologicallyOutdoors && district.TouchesMapEdge)
			{
				return null;
			}
			if (!pawn.CanReachMapEdge())
			{
				return null;
			}
			IntVec3 intVec;
			if (!RCellFinder.TryFindRandomSpotJustOutsideColony(pawn, out intVec))
			{
				return null;
			}
			if (intVec == pawn.Position)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Goto, intVec);
		}
	}
}
