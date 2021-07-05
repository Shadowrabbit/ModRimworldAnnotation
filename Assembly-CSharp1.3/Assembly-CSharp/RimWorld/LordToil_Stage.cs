using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008C0 RID: 2240
	public class LordToil_Stage : LordToil
	{
		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x06003B18 RID: 15128 RVA: 0x0014A71D File Offset: 0x0014891D
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.stagingPoint;
			}
		}

		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x06003B19 RID: 15129 RVA: 0x0014A72A File Offset: 0x0014892A
		private LordToilData_Stage Data
		{
			get
			{
				return (LordToilData_Stage)this.data;
			}
		}

		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x06003B1A RID: 15130 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003B1B RID: 15131 RVA: 0x0014A737 File Offset: 0x00148937
		public LordToil_Stage(IntVec3 stagingLoc)
		{
			this.data = new LordToilData_Stage();
			this.Data.stagingPoint = stagingLoc;
		}

		// Token: 0x06003B1C RID: 15132 RVA: 0x0014A758 File Offset: 0x00148958
		public override void UpdateAllDuties()
		{
			LordToilData_Stage data = this.Data;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.Defend, data.stagingPoint, -1f);
				this.lord.ownedPawns[i].mindState.duty.radius = 28f;
			}
		}
	}
}
