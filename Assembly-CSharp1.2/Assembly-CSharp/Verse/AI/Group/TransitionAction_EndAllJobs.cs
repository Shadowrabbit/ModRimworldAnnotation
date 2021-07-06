using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000AE3 RID: 2787
	public class TransitionAction_EndAllJobs : TransitionAction
	{
		// Token: 0x060041D9 RID: 16857 RVA: 0x001889F0 File Offset: 0x00186BF0
		public override void DoAction(Transition trans)
		{
			List<Pawn> ownedPawns = trans.target.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				Pawn pawn = ownedPawns[i];
				if (pawn.jobs != null && pawn.jobs.curJob != null)
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}
	}
}
