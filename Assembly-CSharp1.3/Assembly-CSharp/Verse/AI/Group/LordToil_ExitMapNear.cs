using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000673 RID: 1651
	public class LordToil_ExitMapNear : LordToil
	{
		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x06002EC9 RID: 11977 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x06002ECA RID: 11978 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002ECB RID: 11979 RVA: 0x001170FA File Offset: 0x001152FA
		public LordToil_ExitMapNear(IntVec3 near, float radius, LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false)
		{
			this.near = near;
			this.radius = radius;
			this.locomotion = locomotion;
			this.canDig = canDig;
		}

		// Token: 0x06002ECC RID: 11980 RVA: 0x00117120 File Offset: 0x00115320
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(DutyDefOf.ExitMapNearDutyTarget, this.near, this.radius);
				pawnDuty.locomotion = this.locomotion;
				pawnDuty.canDig = this.canDig;
				this.lord.ownedPawns[i].mindState.duty = pawnDuty;
			}
		}

		// Token: 0x04001CA1 RID: 7329
		private IntVec3 near;

		// Token: 0x04001CA2 RID: 7330
		private float radius;

		// Token: 0x04001CA3 RID: 7331
		private LocomotionUrgency locomotion;

		// Token: 0x04001CA4 RID: 7332
		private bool canDig;
	}
}
