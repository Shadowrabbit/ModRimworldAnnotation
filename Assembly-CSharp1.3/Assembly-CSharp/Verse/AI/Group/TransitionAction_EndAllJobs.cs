using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x0200067C RID: 1660
	public class TransitionAction_EndAllJobs : TransitionAction
	{
		// Token: 0x06002EF8 RID: 12024 RVA: 0x00117C68 File Offset: 0x00115E68
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
