using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D2F RID: 3375
	public class JobGiver_ReachOutside : ThinkNode_JobGiver
	{
		// Token: 0x06004D3C RID: 19772 RVA: 0x001AD960 File Offset: 0x001ABB60
		protected override Job TryGiveJob(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_Passable);
			if (room.PsychologicallyOutdoors && room.TouchesMapEdge)
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
