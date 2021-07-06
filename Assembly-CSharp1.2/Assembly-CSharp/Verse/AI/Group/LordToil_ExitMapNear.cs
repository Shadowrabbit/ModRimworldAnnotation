using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000AD8 RID: 2776
	public class LordToil_ExitMapNear : LordToil
	{
		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x060041A6 RID: 16806 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x060041A7 RID: 16807 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060041A8 RID: 16808 RVA: 0x00030E3C File Offset: 0x0002F03C
		public LordToil_ExitMapNear(IntVec3 near, float radius, LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false)
		{
			this.near = near;
			this.radius = radius;
			this.locomotion = locomotion;
			this.canDig = canDig;
		}

		// Token: 0x060041A9 RID: 16809 RVA: 0x00188118 File Offset: 0x00186318
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

		// Token: 0x04002D30 RID: 11568
		private IntVec3 near;

		// Token: 0x04002D31 RID: 11569
		private float radius;

		// Token: 0x04002D32 RID: 11570
		private LocomotionUrgency locomotion;

		// Token: 0x04002D33 RID: 11571
		private bool canDig;
	}
}
