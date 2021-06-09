using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E1D RID: 3613
	public class LordToil_Speech : LordToil_Gathering
	{
		// Token: 0x17000CA9 RID: 3241
		// (get) Token: 0x06005202 RID: 20994 RVA: 0x00039568 File Offset: 0x00037768
		public LordToilData_Speech Data
		{
			get
			{
				return (LordToilData_Speech)this.data;
			}
		}

		// Token: 0x06005203 RID: 20995 RVA: 0x00039575 File Offset: 0x00037775
		public LordToil_Speech(IntVec3 spot, GatheringDef gatheringDef, Pawn organizer) : base(spot, gatheringDef)
		{
			this.organizer = organizer;
			this.data = new LordToilData_Speech();
		}

		// Token: 0x06005204 RID: 20996 RVA: 0x001BD3D0 File Offset: 0x001BB5D0
		public override void Init()
		{
			base.Init();
			this.Data.spectateRect = CellRect.CenteredOn(this.spot, 0);
			Rot4 rotation = this.spot.GetFirstThing(this.organizer.MapHeld).Rotation;
			SpectateRectSide asSpectateSide = rotation.Opposite.AsSpectateSide;
			this.Data.spectateRectAllowedSides = (SpectateRectSide.All & ~asSpectateSide);
			this.Data.spectateRectPreferredSide = rotation.AsSpectateSide;
		}

		// Token: 0x06005205 RID: 20997 RVA: 0x00039591 File Offset: 0x00037791
		public override ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			if (p == this.organizer)
			{
				return DutyDefOf.GiveSpeech.hook;
			}
			return DutyDefOf.Spectate.hook;
		}

		// Token: 0x06005206 RID: 20998 RVA: 0x001BD448 File Offset: 0x001BB648
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

		// Token: 0x04003480 RID: 13440
		public Pawn organizer;
	}
}
