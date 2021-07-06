using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E04 RID: 3588
	public class LordToil_ManClosestTurrets : LordToil
	{
		// Token: 0x06005195 RID: 20885 RVA: 0x001BBCF8 File Offset: 0x001B9EF8
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.ManClosestTurret, this.lord.ownedPawns[i].Position, -1f);
			}
		}
	}
}
