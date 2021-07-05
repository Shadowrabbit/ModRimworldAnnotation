using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008B9 RID: 2233
	public class LordToil_LoadAndEnterTransporters : LordToil
	{
		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x06003AEF RID: 15087 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003AF0 RID: 15088 RVA: 0x00149554 File Offset: 0x00147754
		public LordToil_LoadAndEnterTransporters(int transportersGroup)
		{
			this.transportersGroup = transportersGroup;
		}

		// Token: 0x06003AF1 RID: 15089 RVA: 0x0014956C File Offset: 0x0014776C
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(DutyDefOf.LoadAndEnterTransporters);
				pawnDuty.transportersGroup = this.transportersGroup;
				this.lord.ownedPawns[i].mindState.duty = pawnDuty;
			}
		}

		// Token: 0x04002023 RID: 8227
		public int transportersGroup = -1;
	}
}
