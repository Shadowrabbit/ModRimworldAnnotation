using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200067D RID: 1661
	public class TransitionAction_EndAttackBuildingJobs : TransitionAction
	{
		// Token: 0x06002EFA RID: 12026 RVA: 0x00117CCC File Offset: 0x00115ECC
		public override void DoAction(Transition trans)
		{
			List<Pawn> ownedPawns = trans.target.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				Pawn pawn = ownedPawns[i];
				if (pawn.jobs != null && pawn.jobs.curJob != null && pawn.jobs.curJob.def == JobDefOf.AttackMelee && pawn.jobs.curJob.targetA.Thing is Building)
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}
	}
}
