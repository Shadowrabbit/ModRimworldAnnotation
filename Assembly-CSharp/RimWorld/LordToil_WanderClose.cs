using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E14 RID: 3604
	public class LordToil_WanderClose : LordToil
	{
		// Token: 0x060051E3 RID: 20963 RVA: 0x000393DE File Offset: 0x000375DE
		public LordToil_WanderClose(IntVec3 location)
		{
			this.location = location;
		}

		// Token: 0x060051E4 RID: 20964 RVA: 0x001BCF4C File Offset: 0x001BB14C
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty duty = new PawnDuty(DutyDefOf.WanderClose, this.location, -1f);
				this.lord.ownedPawns[i].mindState.duty = duty;
			}
		}

		// Token: 0x0400346F RID: 13423
		private IntVec3 location;
	}
}
