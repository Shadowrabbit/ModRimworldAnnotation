using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DEA RID: 3562
	public class LordToil_PrepareCaravan_Wait : LordToil
	{
		// Token: 0x17000C7A RID: 3194
		// (get) Token: 0x0600512D RID: 20781 RVA: 0x00038DBA File Offset: 0x00036FBA
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x17000C7B RID: 3195
		// (get) Token: 0x0600512E RID: 20782 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600512F RID: 20783 RVA: 0x00038E24 File Offset: 0x00037024
		public LordToil_PrepareCaravan_Wait(IntVec3 meetingPoint)
		{
			this.meetingPoint = meetingPoint;
		}

		// Token: 0x06005130 RID: 20784 RVA: 0x001BAA10 File Offset: 0x001B8C10
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Wait, this.meetingPoint, -1f);
			}
		}

		// Token: 0x0400342D RID: 13357
		private IntVec3 meetingPoint;
	}
}
