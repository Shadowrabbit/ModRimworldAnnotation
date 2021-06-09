using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000AEA RID: 2794
	public class TransitionAction_CheckForJobOverride : TransitionAction
	{
		// Token: 0x060041E8 RID: 16872 RVA: 0x00188C4C File Offset: 0x00186E4C
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
