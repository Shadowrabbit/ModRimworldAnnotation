using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008C3 RID: 2243
	public class LordToil_TakeWoundedGuest : LordToil
	{
		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x06003B24 RID: 15140 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x06003B25 RID: 15141 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003B26 RID: 15142 RVA: 0x0014A87C File Offset: 0x00148A7C
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.TakeWoundedGuest);
			}
		}
	}
}
