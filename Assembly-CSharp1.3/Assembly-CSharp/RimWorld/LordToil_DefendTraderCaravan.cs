using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008AE RID: 2222
	internal class LordToil_DefendTraderCaravan : LordToil_DefendPoint
	{
		// Token: 0x17000A87 RID: 2695
		// (get) Token: 0x06003ABD RID: 15037 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x06003ABE RID: 15038 RVA: 0x00146F17 File Offset: 0x00145117
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x06003ABF RID: 15039 RVA: 0x001487FD File Offset: 0x001469FD
		public LordToil_DefendTraderCaravan() : base(true)
		{
		}

		// Token: 0x06003AC0 RID: 15040 RVA: 0x00148808 File Offset: 0x00146A08
		public LordToil_DefendTraderCaravan(IntVec3 defendPoint) : base(defendPoint, 28f, null)
		{
		}

		// Token: 0x06003AC1 RID: 15041 RVA: 0x0014882C File Offset: 0x00146A2C
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
