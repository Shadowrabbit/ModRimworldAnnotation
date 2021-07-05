using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008B2 RID: 2226
	public class LordToil_ExitMapAndDefendSelf : LordToil
	{
		// Token: 0x06003AD1 RID: 15057 RVA: 0x00148CFC File Offset: 0x00146EFC
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.ExitMapBestAndDefendSelf);
			}
		}
	}
}
