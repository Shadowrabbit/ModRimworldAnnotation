using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DF2 RID: 3570
	public class LordToil_DefendBase : LordToil
	{
		// Token: 0x17000C83 RID: 3203
		// (get) Token: 0x0600514F RID: 20815 RVA: 0x00038F63 File Offset: 0x00037163
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.baseCenter;
			}
		}

		// Token: 0x06005150 RID: 20816 RVA: 0x00038F6B File Offset: 0x0003716B
		public LordToil_DefendBase(IntVec3 baseCenter)
		{
			this.baseCenter = baseCenter;
		}

		// Token: 0x06005151 RID: 20817 RVA: 0x001BAF88 File Offset: 0x001B9188
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.DefendBase, this.baseCenter, -1f);
			}
		}

		// Token: 0x04003437 RID: 13367
		public IntVec3 baseCenter;
	}
}
