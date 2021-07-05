using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008AC RID: 2220
	public class LordToil_DefendBase : LordToil
	{
		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x06003AB8 RID: 15032 RVA: 0x001486FD File Offset: 0x001468FD
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.baseCenter;
			}
		}

		// Token: 0x06003AB9 RID: 15033 RVA: 0x00148705 File Offset: 0x00146905
		public LordToil_DefendBase(IntVec3 baseCenter)
		{
			this.baseCenter = baseCenter;
		}

		// Token: 0x06003ABA RID: 15034 RVA: 0x00148714 File Offset: 0x00146914
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.DefendBase, this.baseCenter, -1f);
			}
		}

		// Token: 0x04002018 RID: 8216
		public IntVec3 baseCenter;
	}
}
