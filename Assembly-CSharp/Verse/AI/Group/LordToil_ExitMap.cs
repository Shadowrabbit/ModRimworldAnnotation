using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000AD4 RID: 2772
	public class LordToil_ExitMap : LordToil
	{
		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x06004198 RID: 16792 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x06004199 RID: 16793 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x0600419A RID: 16794 RVA: 0x00030D9F File Offset: 0x0002EF9F
		public virtual DutyDef ExitDuty
		{
			get
			{
				return DutyDefOf.ExitMapBest;
			}
		}

		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x0600419B RID: 16795 RVA: 0x00030DA6 File Offset: 0x0002EFA6
		protected LordToilData_ExitMap Data
		{
			get
			{
				return (LordToilData_ExitMap)this.data;
			}
		}

		// Token: 0x0600419C RID: 16796 RVA: 0x00030DB3 File Offset: 0x0002EFB3
		public LordToil_ExitMap(LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false, bool interruptCurrentJob = false)
		{
			this.data = new LordToilData_ExitMap();
			this.Data.locomotion = locomotion;
			this.Data.canDig = canDig;
			this.Data.interruptCurrentJob = interruptCurrentJob;
		}

		// Token: 0x0600419D RID: 16797 RVA: 0x00187FF4 File Offset: 0x001861F4
		public override void UpdateAllDuties()
		{
			LordToilData_ExitMap data = this.Data;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(this.ExitDuty);
				pawnDuty.locomotion = data.locomotion;
				pawnDuty.canDig = data.canDig;
				Pawn pawn = this.lord.ownedPawns[i];
				pawn.mindState.duty = pawnDuty;
				if (this.Data.interruptCurrentJob && pawn.jobs.curJob != null)
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}
	}
}
