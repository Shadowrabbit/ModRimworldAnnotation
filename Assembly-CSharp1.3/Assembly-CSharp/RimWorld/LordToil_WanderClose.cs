using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008C9 RID: 2249
	public class LordToil_WanderClose : LordToil
	{
		// Token: 0x06003B3E RID: 15166 RVA: 0x0014AEE9 File Offset: 0x001490E9
		public LordToil_WanderClose(IntVec3 location)
		{
			this.location = location;
		}

		// Token: 0x06003B3F RID: 15167 RVA: 0x0014AEF8 File Offset: 0x001490F8
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty duty = new PawnDuty(DutyDefOf.WanderClose, this.location, -1f);
				this.lord.ownedPawns[i].mindState.duty = duty;
			}
		}

		// Token: 0x04002042 RID: 8258
		private IntVec3 location;
	}
}
