using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000870 RID: 2160
	public class LordToil_BestowingCeremony_MoveInPlace : LordToil
	{
		// Token: 0x060038FB RID: 14587 RVA: 0x0013F2C5 File Offset: 0x0013D4C5
		public LordToil_BestowingCeremony_MoveInPlace(IntVec3 spot, Pawn target)
		{
			this.spot = spot;
			this.target = target;
		}

		// Token: 0x060038FC RID: 14588 RVA: 0x0013F2DC File Offset: 0x0013D4DC
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(DutyDefOf.BestowingCeremony_MoveInPlace, this.spot, -1f);
				pawnDuty.locomotion = LocomotionUrgency.Walk;
				this.lord.ownedPawns[i].mindState.duty = pawnDuty;
			}
		}

		// Token: 0x04001F4F RID: 8015
		public bool? pass;

		// Token: 0x04001F50 RID: 8016
		public IntVec3 spot;

		// Token: 0x04001F51 RID: 8017
		public Pawn target;
	}
}
