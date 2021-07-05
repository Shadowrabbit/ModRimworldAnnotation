using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000672 RID: 1650
	public class LordToil_ExitMapTraderFighting : LordToil
	{
		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06002EC5 RID: 11973 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06002EC6 RID: 11974 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002EC7 RID: 11975 RVA: 0x00117074 File Offset: 0x00115274
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				TraderCaravanRole traderCaravanRole = pawn.GetTraderCaravanRole();
				if (traderCaravanRole == TraderCaravanRole.Carrier || traderCaravanRole == TraderCaravanRole.Chattel)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.ExitMapBest);
					pawn.mindState.duty.locomotion = LocomotionUrgency.Jog;
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.ExitMapBestAndDefendSelf);
				}
			}
		}
	}
}
