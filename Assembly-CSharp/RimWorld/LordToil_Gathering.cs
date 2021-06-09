using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E16 RID: 3606
	public abstract class LordToil_Gathering : LordToil
	{
		// Token: 0x060051E7 RID: 20967 RVA: 0x00039408 File Offset: 0x00037608
		public LordToil_Gathering(IntVec3 spot, GatheringDef gatheringDef)
		{
			this.spot = spot;
			this.gatheringDef = gatheringDef;
		}

		// Token: 0x060051E8 RID: 20968 RVA: 0x0003941E File Offset: 0x0003761E
		public override ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			return this.gatheringDef.duty.hook;
		}

		// Token: 0x060051E9 RID: 20969 RVA: 0x001BCFAC File Offset: 0x001BB1AC
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(this.gatheringDef.duty, this.spot, -1f);
			}
		}

		// Token: 0x04003471 RID: 13425
		protected IntVec3 spot;

		// Token: 0x04003472 RID: 13426
		protected GatheringDef gatheringDef;
	}
}
