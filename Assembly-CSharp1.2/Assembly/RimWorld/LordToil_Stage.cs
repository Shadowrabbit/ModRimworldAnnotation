using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E0F RID: 3599
	public class LordToil_Stage : LordToil
	{
		// Token: 0x17000C9E RID: 3230
		// (get) Token: 0x060051D0 RID: 20944 RVA: 0x0003939E File Offset: 0x0003759E
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.stagingPoint;
			}
		}

		// Token: 0x17000C9F RID: 3231
		// (get) Token: 0x060051D1 RID: 20945 RVA: 0x000393AB File Offset: 0x000375AB
		private LordToilData_Stage Data
		{
			get
			{
				return (LordToilData_Stage)this.data;
			}
		}

		// Token: 0x17000CA0 RID: 3232
		// (get) Token: 0x060051D2 RID: 20946 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060051D3 RID: 20947 RVA: 0x000393B8 File Offset: 0x000375B8
		public LordToil_Stage(IntVec3 stagingLoc)
		{
			this.data = new LordToilData_Stage();
			this.Data.stagingPoint = stagingLoc;
		}

		// Token: 0x060051D4 RID: 20948 RVA: 0x001BCD88 File Offset: 0x001BAF88
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
