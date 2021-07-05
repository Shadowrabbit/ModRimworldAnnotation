using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DE6 RID: 3558
	[Obsolete]
	public class LordToil_PrepareCaravan_GatherSlaves : LordToil
	{
		// Token: 0x17000C74 RID: 3188
		// (get) Token: 0x0600511D RID: 20765 RVA: 0x00038DBA File Offset: 0x00036FBA
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x17000C75 RID: 3189
		// (get) Token: 0x0600511E RID: 20766 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600511F RID: 20767 RVA: 0x00038DFA File Offset: 0x00036FFA
		public LordToil_PrepareCaravan_GatherSlaves(IntVec3 meetingPoint)
		{
			this.meetingPoint = meetingPoint;
		}

		// Token: 0x06005120 RID: 20768 RVA: 0x00006A05 File Offset: 0x00004C05
		public override void UpdateAllDuties()
		{
		}

		// Token: 0x04003429 RID: 13353
		private IntVec3 meetingPoint;
	}
}
