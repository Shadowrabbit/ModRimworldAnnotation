using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008C6 RID: 2246
	public class LordToil_Wait : LordToil
	{
		// Token: 0x06003B30 RID: 15152 RVA: 0x0014AC30 File Offset: 0x00148E30
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty duty = new PawnDuty(DutyDefOf.Idle);
				this.DecoratePawnDuty(duty);
				this.lord.ownedPawns[i].mindState.duty = duty;
			}
		}

		// Token: 0x06003B31 RID: 15153 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void DecoratePawnDuty(PawnDuty duty)
		{
		}
	}
}
