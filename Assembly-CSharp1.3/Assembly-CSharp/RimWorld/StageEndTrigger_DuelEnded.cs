using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F10 RID: 3856
	public class StageEndTrigger_DuelEnded : StageEndTrigger_AnyPawnDead
	{
		// Token: 0x06005BE5 RID: 23525 RVA: 0x001FBF70 File Offset: 0x001FA170
		protected override bool Trigger(LordJob_Ritual ritual)
		{
			if (base.Trigger(ritual))
			{
				return true;
			}
			foreach (string roleId in this.roleIds)
			{
				using (IEnumerator<Pawn> enumerator2 = ritual.assignments.AssignedPawns(roleId).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.Downed)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}
