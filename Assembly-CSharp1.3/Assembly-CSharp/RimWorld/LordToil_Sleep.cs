using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008BF RID: 2239
	public class LordToil_Sleep : LordToil
	{
		// Token: 0x06003B16 RID: 15126 RVA: 0x0014A6D0 File Offset: 0x001488D0
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.SleepForever);
			}
		}
	}
}
