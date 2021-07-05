using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008B1 RID: 2225
	public class LordToil_EscortPawn : LordToil
	{
		// Token: 0x06003ACE RID: 15054 RVA: 0x00148C7B File Offset: 0x00146E7B
		public LordToil_EscortPawn(Pawn escortee, float followRadius = 7f)
		{
			this.escortee = escortee;
			this.followRadius = followRadius;
		}

		// Token: 0x06003ACF RID: 15055 RVA: 0x00148C9C File Offset: 0x00146E9C
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty duty = new PawnDuty(DutyDefOf.Escort, this.escortee, this.followRadius);
				this.lord.ownedPawns[i].mindState.duty = duty;
			}
		}

		// Token: 0x0400201F RID: 8223
		public Pawn escortee;

		// Token: 0x04002020 RID: 8224
		public float followRadius = 7f;
	}
}
