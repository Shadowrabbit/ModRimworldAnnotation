using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008BA RID: 2234
	public class LordToil_ManClosestTurrets : LordToil
	{
		// Token: 0x06003AF2 RID: 15090 RVA: 0x001495C8 File Offset: 0x001477C8
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.ManClosestTurret, this.lord.ownedPawns[i].Position, -1f);
			}
		}
	}
}
