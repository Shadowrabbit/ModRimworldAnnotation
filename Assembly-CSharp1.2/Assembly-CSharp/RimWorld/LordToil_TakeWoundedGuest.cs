using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E12 RID: 3602
	public class LordToil_TakeWoundedGuest : LordToil
	{
		// Token: 0x17000CA4 RID: 3236
		// (get) Token: 0x060051DC RID: 20956 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CA5 RID: 3237
		// (get) Token: 0x060051DD RID: 20957 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060051DE RID: 20958 RVA: 0x001BCEA4 File Offset: 0x001BB0A4
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.TakeWoundedGuest);
			}
		}
	}
}
