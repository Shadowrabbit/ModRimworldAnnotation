using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DF4 RID: 3572
	internal class LordToil_DefendTraderCaravan : LordToil_DefendPoint
	{
		// Token: 0x17000C84 RID: 3204
		// (get) Token: 0x06005154 RID: 20820 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C85 RID: 3205
		// (get) Token: 0x06005155 RID: 20821 RVA: 0x00038DBA File Offset: 0x00036FBA
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x06005156 RID: 20822 RVA: 0x00038F8D File Offset: 0x0003718D
		public LordToil_DefendTraderCaravan() : base(true)
		{
		}

		// Token: 0x06005157 RID: 20823 RVA: 0x001BB064 File Offset: 0x001B9264
		public LordToil_DefendTraderCaravan(IntVec3 defendPoint) : base(defendPoint, 28f, null)
		{
		}

		// Token: 0x06005158 RID: 20824 RVA: 0x001BB088 File Offset: 0x001B9288
		public override void UpdateAllDuties()
		{
			LordToilData_DefendPoint data = base.Data;
			Pawn pawn = TraderCaravanUtility.FindTrader(this.lord);
			if (pawn != null)
			{
				pawn.mindState.duty = new PawnDuty(DutyDefOf.Defend, data.defendPoint, data.defendRadius);
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					Pawn pawn2 = this.lord.ownedPawns[i];
					switch (pawn2.GetTraderCaravanRole())
					{
					case TraderCaravanRole.Carrier:
						pawn2.mindState.duty = new PawnDuty(DutyDefOf.Follow, pawn, 5f);
						pawn2.mindState.duty.locomotion = LocomotionUrgency.Walk;
						break;
					case TraderCaravanRole.Guard:
						pawn2.mindState.duty = new PawnDuty(DutyDefOf.Defend, data.defendPoint, data.defendRadius);
						break;
					case TraderCaravanRole.Chattel:
						pawn2.mindState.duty = new PawnDuty(DutyDefOf.Escort, pawn, 5f);
						pawn2.mindState.duty.locomotion = LocomotionUrgency.Walk;
						break;
					}
				}
				return;
			}
		}
	}
}
