using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008B0 RID: 2224
	public class LordToil_EnterShuttleOrLeave : LordToil
	{
		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x06003AC8 RID: 15048 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003AC9 RID: 15049 RVA: 0x00148B30 File Offset: 0x00146D30
		public LordToil_EnterShuttleOrLeave(Thing shuttle, LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false, bool interruptCurrentJob = false)
		{
			this.shuttle = shuttle;
			this.locomotion = locomotion;
			this.canDig = canDig;
			this.interruptCurrentJob = interruptCurrentJob;
		}

		// Token: 0x06003ACA RID: 15050 RVA: 0x00148B55 File Offset: 0x00146D55
		private DutyDef GetExpectedDutyDef(Pawn pawn)
		{
			if (!this.shuttle.Spawned || !pawn.CanReach(this.shuttle, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return DutyDefOf.ExitMapBestAndDefendSelf;
			}
			return DutyDefOf.EnterTransporter;
		}

		// Token: 0x06003ACB RID: 15051 RVA: 0x00148B88 File Offset: 0x00146D88
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

		// Token: 0x06003ACC RID: 15052 RVA: 0x00148C73 File Offset: 0x00146E73
		public override void UpdateAllDuties()
		{
			this.EnsureCorrectDuties();
		}

		// Token: 0x06003ACD RID: 15053 RVA: 0x00148C73 File Offset: 0x00146E73
		public override void LordToilTick()
		{
			this.EnsureCorrectDuties();
		}

		// Token: 0x0400201B RID: 8219
		public Thing shuttle;

		// Token: 0x0400201C RID: 8220
		public LocomotionUrgency locomotion;

		// Token: 0x0400201D RID: 8221
		public bool canDig;

		// Token: 0x0400201E RID: 8222
		public bool interruptCurrentJob;
	}
}
