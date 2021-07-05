using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200089B RID: 2203
	public class LordToil_PrepareCaravan_Leave : LordToil
	{
		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x06003A64 RID: 14948 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x06003A65 RID: 14949 RVA: 0x00146F17 File Offset: 0x00145117
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x06003A66 RID: 14950 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x06003A67 RID: 14951 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003A68 RID: 14952 RVA: 0x001471C8 File Offset: 0x001453C8
		public LordToil_PrepareCaravan_Leave(IntVec3 exitSpot)
		{
			this.exitSpot = exitSpot;
		}

		// Token: 0x06003A69 RID: 14953 RVA: 0x001471D8 File Offset: 0x001453D8
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				pawn.mindState.duty = new PawnDuty(DutyDefOf.TravelOrWait, this.exitSpot, -1f);
				pawn.mindState.duty.locomotion = LocomotionUrgency.Jog;
			}
		}

		// Token: 0x06003A6A RID: 14954 RVA: 0x00147248 File Offset: 0x00145448
		public override void LordToilTick()
		{
			if (Find.TickManager.TicksGame % 100 == 0)
			{
				GatherAnimalsAndSlavesForCaravanUtility.CheckArrived(this.lord, this.lord.ownedPawns, this.exitSpot, "ReadyToExitMap", (Pawn x) => true, null);
			}
		}

		// Token: 0x04001FF8 RID: 8184
		private IntVec3 exitSpot;
	}
}
