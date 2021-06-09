using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DE7 RID: 3559
	public class LordToil_PrepareCaravan_Leave : LordToil
	{
		// Token: 0x17000C76 RID: 3190
		// (get) Token: 0x06005121 RID: 20769 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C77 RID: 3191
		// (get) Token: 0x06005122 RID: 20770 RVA: 0x00038DBA File Offset: 0x00036FBA
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x17000C78 RID: 3192
		// (get) Token: 0x06005123 RID: 20771 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C79 RID: 3193
		// (get) Token: 0x06005124 RID: 20772 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005125 RID: 20773 RVA: 0x00038E09 File Offset: 0x00037009
		public LordToil_PrepareCaravan_Leave(IntVec3 exitSpot)
		{
			this.exitSpot = exitSpot;
		}

		// Token: 0x06005126 RID: 20774 RVA: 0x001BA8F0 File Offset: 0x001B8AF0
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				pawn.mindState.duty = new PawnDuty(DutyDefOf.TravelOrWait, this.exitSpot, -1f);
				pawn.mindState.duty.locomotion = LocomotionUrgency.Jog;
			}
		}

		// Token: 0x06005127 RID: 20775 RVA: 0x001BA960 File Offset: 0x001B8B60
		public override void LordToilTick()
		{
			if (Find.TickManager.TicksGame % 100 == 0)
			{
				GatherAnimalsAndSlavesForCaravanUtility.CheckArrived(this.lord, this.lord.ownedPawns, this.exitSpot, "ReadyToExitMap", (Pawn x) => true, null);
			}
		}

		// Token: 0x0400342A RID: 13354
		private IntVec3 exitSpot;
	}
}
