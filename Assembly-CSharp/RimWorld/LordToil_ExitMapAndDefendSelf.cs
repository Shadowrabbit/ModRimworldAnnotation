using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DF8 RID: 3576
	public class LordToil_ExitMapAndDefendSelf : LordToil
	{
		// Token: 0x06005168 RID: 20840 RVA: 0x001BB4CC File Offset: 0x001B96CC
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.ExitMapBestAndDefendSelf);
			}
		}
	}
}
