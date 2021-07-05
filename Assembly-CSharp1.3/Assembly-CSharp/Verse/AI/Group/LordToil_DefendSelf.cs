using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200066D RID: 1645
	public class LordToil_DefendSelf : LordToil
	{
		// Token: 0x06002EB6 RID: 11958 RVA: 0x00116E9C File Offset: 0x0011509C
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
