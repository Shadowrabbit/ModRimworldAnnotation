using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000AD0 RID: 2768
	public class LordToil_DefendPoint : LordToil
	{
		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x0600418A RID: 16778 RVA: 0x00030CFB File Offset: 0x0002EEFB
		protected LordToilData_DefendPoint Data
		{
			get
			{
				return (LordToilData_DefendPoint)this.data;
			}
		}

		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x0600418B RID: 16779 RVA: 0x00030D08 File Offset: 0x0002EF08
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.defendPoint;
			}
		}

		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x0600418C RID: 16780 RVA: 0x00030D15 File Offset: 0x0002EF15
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return this.allowSatisfyLongNeeds;
			}
		}

		// Token: 0x0600418D RID: 16781 RVA: 0x00030D1D File Offset: 0x0002EF1D
		public LordToil_DefendPoint(bool canSatisfyLongNeeds = true)
		{
			this.allowSatisfyLongNeeds = canSatisfyLongNeeds;
			this.data = new LordToilData_DefendPoint();
		}

		// Token: 0x0600418E RID: 16782 RVA: 0x00030D3E File Offset: 0x0002EF3E
		public LordToil_DefendPoint(IntVec3 defendPoint, float defendRadius = 28f, float? wanderRadius = null) : this(true)
		{
			this.Data.defendPoint = defendPoint;
			this.Data.defendRadius = defendRadius;
			this.Data.wanderRadius = wanderRadius;
		}

		// Token: 0x0600418F RID: 16783 RVA: 0x00187E30 File Offset: 0x00186030
		public override void UpdateAllDuties()
		{
			LordToilData_DefendPoint data = this.Data;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				pawn.mindState.duty = new PawnDuty(DutyDefOf.Defend, data.defendPoint, -1f);
				pawn.mindState.duty.focusSecond = data.defendPoint;
				pawn.mindState.duty.radius = ((pawn.kindDef.defendPointRadius >= 0f) ? pawn.kindDef.defendPointRadius : data.defendRadius);
				pawn.mindState.duty.wanderRadius = data.wanderRadius;
			}
		}

		// Token: 0x06004190 RID: 16784 RVA: 0x00030D6B File Offset: 0x0002EF6B
		public void SetDefendPoint(IntVec3 defendPoint)
		{
			this.Data.defendPoint = defendPoint;
		}

		// Token: 0x04002D29 RID: 11561
		private bool allowSatisfyLongNeeds = true;
	}
}
