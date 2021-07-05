using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008A2 RID: 2210
	public class LordToil_PrepareCaravan_Wait : LordToil
	{
		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x06003A7F RID: 14975 RVA: 0x00146F17 File Offset: 0x00145117
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x06003A80 RID: 14976 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003A81 RID: 14977 RVA: 0x0014775F File Offset: 0x0014595F
		public LordToil_PrepareCaravan_Wait(IntVec3 meetingPoint)
		{
			this.meetingPoint = meetingPoint;
		}

		// Token: 0x06003A82 RID: 14978 RVA: 0x00147770 File Offset: 0x00145970
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Wait, this.meetingPoint, -1f);
			}
		}

		// Token: 0x04001FFC RID: 8188
		private IntVec3 meetingPoint;
	}
}
