using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000898 RID: 2200
	public abstract class LordJob_VoluntarilyJoinable : LordJob
	{
		// Token: 0x06003A57 RID: 14935 RVA: 0x000682C5 File Offset: 0x000664C5
		public virtual float VoluntaryJoinPriorityFor(Pawn p)
		{
			return 0f;
		}

		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x06003A58 RID: 14936 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}
	}
}
