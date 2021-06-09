using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DE9 RID: 3561
	public class LordToil_PrepareCaravan_Pause : LordToil
	{
		// Token: 0x0600512B RID: 20779 RVA: 0x001BA9C0 File Offset: 0x001B8BC0
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Pause);
			}
		}
	}
}
