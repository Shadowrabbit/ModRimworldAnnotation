using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000682 RID: 1666
	public class TransitionAction_CheckForJobOverride : TransitionAction
	{
		// Token: 0x06002F04 RID: 12036 RVA: 0x00117EF0 File Offset: 0x001160F0
		public override void DoAction(Transition trans)
		{
			List<Pawn> ownedPawns = trans.target.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				if (ownedPawns[i].CurJob != null)
				{
					ownedPawns[i].jobs.CheckForJobOverride();
				}
			}
		}
	}
}
