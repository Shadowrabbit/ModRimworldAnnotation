using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E13 RID: 3603
	public class LordToil_Wait : LordToil
	{
		// Token: 0x060051E0 RID: 20960 RVA: 0x001BCEF4 File Offset: 0x001BB0F4
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty duty = new PawnDuty(DutyDefOf.Idle);
				this.DecoratePawnDuty(duty);
				this.lord.ownedPawns[i].mindState.duty = duty;
			}
		}

		// Token: 0x060051E1 RID: 20961 RVA: 0x00006A05 File Offset: 0x00004C05
		protected virtual void DecoratePawnDuty(PawnDuty duty)
		{
		}
	}
}
