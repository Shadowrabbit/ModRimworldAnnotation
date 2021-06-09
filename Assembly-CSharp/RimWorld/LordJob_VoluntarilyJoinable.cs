using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DE0 RID: 3552
	public abstract class LordJob_VoluntarilyJoinable : LordJob
	{
		// Token: 0x06005109 RID: 20745 RVA: 0x00016647 File Offset: 0x00014847
		public virtual float VoluntaryJoinPriorityFor(Pawn p)
		{
			return 0f;
		}

		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x0600510A RID: 20746 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}
	}
}
