using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200066F RID: 1647
	public class LordToil_ExitMap : LordToil
	{
		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06002EBB RID: 11963 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06002EBC RID: 11964 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x06002EBD RID: 11965 RVA: 0x00116F39 File Offset: 0x00115139
		public virtual DutyDef ExitDuty
		{
			get
			{
				return DutyDefOf.ExitMapBest;
			}
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06002EBE RID: 11966 RVA: 0x00116F40 File Offset: 0x00115140
		protected LordToilData_ExitMap Data
		{
			get
			{
				return (LordToilData_ExitMap)this.data;
			}
		}

		// Token: 0x06002EBF RID: 11967 RVA: 0x00116F4D File Offset: 0x0011514D
		public LordToil_ExitMap(LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false, bool interruptCurrentJob = false)
		{
			this.data = new LordToilData_ExitMap();
			this.Data.locomotion = locomotion;
			this.Data.canDig = canDig;
			this.Data.interruptCurrentJob = interruptCurrentJob;
		}

		// Token: 0x06002EC0 RID: 11968 RVA: 0x00116F84 File Offset: 0x00115184
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
