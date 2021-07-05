using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000637 RID: 1591
	public class JobGiver_MoveToStandable : ThinkNode_JobGiver
	{
		// Token: 0x06002D67 RID: 11623 RVA: 0x0010FDEC File Offset: 0x0010DFEC
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.Drafted)
			{
				return null;
			}
			if (pawn.pather.Moving)
			{
				return null;
			}
			if (!pawn.Position.Standable(pawn.Map))
			{
				return this.FindBetterPositionJob(pawn);
			}
			List<Thing> thingList = pawn.Position.GetThingList(pawn.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Pawn pawn2 = thingList[i] as Pawn;
				if (pawn2 != null && pawn2 != pawn && pawn2.Faction == pawn.Faction && pawn2.Drafted && !pawn2.pather.MovingNow)
				{
					return this.FindBetterPositionJob(pawn);
				}
			}
			return null;
		}

		// Token: 0x06002D68 RID: 11624 RVA: 0x0010FE90 File Offset: 0x0010E090
		private Job FindBetterPositionJob(Pawn pawn)
		{
			IntVec3 intVec = RCellFinder.BestOrderedGotoDestNear(pawn.Position, pawn, null);
			if (intVec.IsValid && intVec != pawn.Position)
			{
				return JobMaker.MakeJob(JobDefOf.Goto, intVec);
			}
			return null;
		}
	}
}
