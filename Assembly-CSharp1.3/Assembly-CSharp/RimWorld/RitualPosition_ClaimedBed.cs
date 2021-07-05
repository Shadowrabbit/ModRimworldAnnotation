using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F84 RID: 3972
	public class RitualPosition_ClaimedBed : RitualPosition
	{
		// Token: 0x06005E07 RID: 24071 RVA: 0x002047B8 File Offset: 0x002029B8
		public override PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			Pawn pawn = (this.ofPawn == null) ? p : ritual.assignments.FirstAssignedPawn(this.ofPawn);
			if (pawn.ownership != null && pawn.ownership.OwnedBed != null)
			{
				return new PawnStagePosition(pawn.ownership.OwnedBed.Position, null, Rot4.Invalid, this.highlight);
			}
			return new PawnStagePosition(IntVec3.Invalid, null, Rot4.Invalid, this.highlight);
		}

		// Token: 0x06005E08 RID: 24072 RVA: 0x0020482F File Offset: 0x00202A2F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.ofPawn, "ofPawn", null, false);
		}

		// Token: 0x0400365B RID: 13915
		[NoTranslate]
		public string ofPawn;
	}
}
