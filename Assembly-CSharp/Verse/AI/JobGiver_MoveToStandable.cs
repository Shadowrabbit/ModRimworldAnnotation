using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A95 RID: 2709
	public class JobGiver_MoveToStandable : ThinkNode_JobGiver
	{
		// Token: 0x06004047 RID: 16455 RVA: 0x00182584 File Offset: 0x00180784
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

		// Token: 0x06004048 RID: 16456 RVA: 0x00182628 File Offset: 0x00180828
		private Job FindBetterPositionJob(Pawn pawn)
		{
			IntVec3 intVec = RCellFinder.BestOrderedGotoDestNear(pawn.Position, pawn);
			if (intVec.IsValid && intVec != pawn.Position)
			{
				return JobMaker.MakeJob(JobDefOf.Goto, intVec);
			}
			return null;
		}
	}
}
