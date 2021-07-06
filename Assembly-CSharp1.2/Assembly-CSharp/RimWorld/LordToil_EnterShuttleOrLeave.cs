using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DF6 RID: 3574
	public class LordToil_EnterShuttleOrLeave : LordToil
	{
		// Token: 0x17000C88 RID: 3208
		// (get) Token: 0x0600515F RID: 20831 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005160 RID: 20832 RVA: 0x00038FA5 File Offset: 0x000371A5
		public LordToil_EnterShuttleOrLeave(Thing shuttle, LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false, bool interruptCurrentJob = false)
		{
			this.shuttle = shuttle;
			this.locomotion = locomotion;
			this.canDig = canDig;
			this.interruptCurrentJob = interruptCurrentJob;
		}

		// Token: 0x06005161 RID: 20833 RVA: 0x00038FCA File Offset: 0x000371CA
		private DutyDef GetExpectedDutyDef(Pawn pawn)
		{
			if (!this.shuttle.Spawned || !pawn.CanReach(this.shuttle, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				return DutyDefOf.ExitMapBestAndDefendSelf;
			}
			return DutyDefOf.EnterTransporter;
		}

		// Token: 0x06005162 RID: 20834 RVA: 0x001BB380 File Offset: 0x001B9580
		private void EnsureCorrectDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				DutyDef expectedDutyDef = this.GetExpectedDutyDef(pawn);
				if (pawn.mindState != null && (pawn.mindState.duty == null || pawn.mindState.duty.def != expectedDutyDef))
				{
					pawn.mindState.duty = new PawnDuty(expectedDutyDef, this.shuttle, -1f);
					pawn.mindState.duty.locomotion = this.locomotion;
					pawn.mindState.duty.canDig = this.canDig;
					if (this.interruptCurrentJob && pawn.jobs != null && pawn.jobs.curJob != null)
					{
						pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
			}
		}

		// Token: 0x06005163 RID: 20835 RVA: 0x00038FFB File Offset: 0x000371FB
		public override void UpdateAllDuties()
		{
			this.EnsureCorrectDuties();
		}

		// Token: 0x06005164 RID: 20836 RVA: 0x00038FFB File Offset: 0x000371FB
		public override void LordToilTick()
		{
			this.EnsureCorrectDuties();
		}

		// Token: 0x0400343A RID: 13370
		public Thing shuttle;

		// Token: 0x0400343B RID: 13371
		public LocomotionUrgency locomotion;

		// Token: 0x0400343C RID: 13372
		public bool canDig;

		// Token: 0x0400343D RID: 13373
		public bool interruptCurrentJob;
	}
}
