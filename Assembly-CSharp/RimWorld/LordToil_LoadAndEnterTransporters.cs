using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E03 RID: 3587
	public class LordToil_LoadAndEnterTransporters : LordToil
	{
		// Token: 0x17000C91 RID: 3217
		// (get) Token: 0x06005192 RID: 20882 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005193 RID: 20883 RVA: 0x00039182 File Offset: 0x00037382
		public LordToil_LoadAndEnterTransporters(int transportersGroup)
		{
			this.transportersGroup = transportersGroup;
		}

		// Token: 0x06005194 RID: 20884 RVA: 0x001BBC9C File Offset: 0x001B9E9C
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(DutyDefOf.LoadAndEnterTransporters);
				pawnDuty.transportersGroup = this.transportersGroup;
				this.lord.ownedPawns[i].mindState.duty = pawnDuty;
			}
		}

		// Token: 0x04003449 RID: 13385
		public int transportersGroup = -1;
	}
}
