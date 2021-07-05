using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200089C RID: 2204
	public class LordToil_PrepareCaravan_Pause : LordToil
	{
		// Token: 0x06003A6B RID: 14955 RVA: 0x001472A8 File Offset: 0x001454A8
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Pause);
			}
		}
	}
}
