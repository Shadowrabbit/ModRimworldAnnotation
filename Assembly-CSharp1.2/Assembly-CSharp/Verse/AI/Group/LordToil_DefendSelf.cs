using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000AD2 RID: 2770
	public class LordToil_DefendSelf : LordToil
	{
		// Token: 0x06004193 RID: 16787 RVA: 0x00187F5C File Offset: 0x0018615C
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.Defend, this.lord.ownedPawns[i].Position, -1f);
				this.lord.ownedPawns[i].mindState.duty.radius = 28f;
			}
		}
	}
}
