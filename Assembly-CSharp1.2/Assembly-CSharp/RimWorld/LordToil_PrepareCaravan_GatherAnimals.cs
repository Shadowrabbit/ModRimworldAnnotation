using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DE2 RID: 3554
	[Obsolete]
	public class LordToil_PrepareCaravan_GatherAnimals : LordToil
	{
		// Token: 0x17000C6E RID: 3182
		// (get) Token: 0x0600510C RID: 20748 RVA: 0x00038DBA File Offset: 0x00036FBA
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x17000C6F RID: 3183
		// (get) Token: 0x0600510D RID: 20749 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600510E RID: 20750 RVA: 0x00038DC6 File Offset: 0x00036FC6
		public LordToil_PrepareCaravan_GatherAnimals(IntVec3 meetingPoint)
		{
			this.meetingPoint = meetingPoint;
		}

		// Token: 0x0600510F RID: 20751 RVA: 0x00006A05 File Offset: 0x00004C05
		public override void UpdateAllDuties()
		{
		}

		// Token: 0x04003425 RID: 13349
		private IntVec3 meetingPoint;
	}
}
