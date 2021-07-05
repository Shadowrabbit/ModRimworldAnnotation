using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020008D4 RID: 2260
	public class LordToil_Speech : LordToil_Ritual
	{
		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x06003B65 RID: 15205 RVA: 0x0014BCCE File Offset: 0x00149ECE
		public new LordToilData_Speech Data
		{
			get
			{
				return (LordToilData_Speech)this.data;
			}
		}

		// Token: 0x06003B66 RID: 15206 RVA: 0x0014BCDB File Offset: 0x00149EDB
		public LordToil_Speech(IntVec3 spot, Precept_Ritual ritual, LordJob_Ritual lordJob, Pawn organizer) : base(spot, lordJob, null, organizer)
		{
			this.organizer = organizer;
			this.data = new LordToilData_Speech();
		}

		// Token: 0x06003B67 RID: 15207 RVA: 0x0014BCFC File Offset: 0x00149EFC
		public override void Init()
		{
			base.Init();
			this.Data.spectateRect = CellRect.CenteredOn(this.spot, 0);
			Rot4 rotation = this.spot.GetFirstThing(this.organizer.MapHeld).Rotation;
			SpectateRectSide asSpectateSide = rotation.Opposite.AsSpectateSide;
			this.Data.spectateRectAllowedSides = (SpectateRectSide.All & ~asSpectateSide);
			this.Data.spectateRectPreferredSide = rotation.AsSpectateSide;
		}

		// Token: 0x06003B68 RID: 15208 RVA: 0x0014BD74 File Offset: 0x00149F74
		public override ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			if (p == this.organizer)
			{
				return DutyDefOf.GiveSpeech.hook;
			}
			return DutyDefOf.Spectate.hook;
		}

		// Token: 0x06003B69 RID: 15209 RVA: 0x0014BD94 File Offset: 0x00149F94
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn == this.organizer)
				{
					Building_Throne firstThing = this.spot.GetFirstThing(base.Map);
					pawn.mindState.duty = new PawnDuty(DutyDefOf.GiveSpeech, this.spot, firstThing, -1f);
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
				else
				{
					PawnDuty pawnDuty = new PawnDuty(DutyDefOf.Spectate);
					pawnDuty.spectateRect = this.Data.spectateRect;
					pawnDuty.spectateRectAllowedSides = this.Data.spectateRectAllowedSides;
					pawnDuty.spectateRectPreferredSide = this.Data.spectateRectPreferredSide;
					pawn.mindState.duty = pawnDuty;
				}
			}
		}
	}
}
