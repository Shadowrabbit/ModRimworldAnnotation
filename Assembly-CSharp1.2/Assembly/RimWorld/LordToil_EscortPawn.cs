using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DF7 RID: 3575
	public class LordToil_EscortPawn : LordToil
	{
		// Token: 0x06005165 RID: 20837 RVA: 0x00039003 File Offset: 0x00037203
		public LordToil_EscortPawn(Pawn escortee, float followRadius = 7f)
		{
			this.escortee = escortee;
			this.followRadius = followRadius;
		}

		// Token: 0x06005166 RID: 20838 RVA: 0x001BB46C File Offset: 0x001B966C
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty duty = new PawnDuty(DutyDefOf.Escort, this.escortee, this.followRadius);
				this.lord.ownedPawns[i].mindState.duty = duty;
			}
		}

		// Token: 0x0400343E RID: 13374
		public Pawn escortee;

		// Token: 0x0400343F RID: 13375
		public float followRadius = 7f;
	}
}
