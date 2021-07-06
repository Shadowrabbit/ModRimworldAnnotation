using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DB5 RID: 3509
	public class LordToil_BestowingCeremony_MoveInPlace : LordToil
	{
		// Token: 0x06004FFB RID: 20475 RVA: 0x0003822A File Offset: 0x0003642A
		public LordToil_BestowingCeremony_MoveInPlace(IntVec3 spot, Pawn target)
		{
			this.spot = spot;
			this.target = target;
		}

		// Token: 0x06004FFC RID: 20476 RVA: 0x001B6314 File Offset: 0x001B4514
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(DutyDefOf.BestowingCeremony_MoveInPlace, this.spot, -1f);
				pawnDuty.locomotion = LocomotionUrgency.Walk;
				this.lord.ownedPawns[i].mindState.duty = pawnDuty;
			}
		}

		// Token: 0x040033B0 RID: 13232
		public bool? pass;

		// Token: 0x040033B1 RID: 13233
		public IntVec3 spot;

		// Token: 0x040033B2 RID: 13234
		public Pawn target;
	}
}
