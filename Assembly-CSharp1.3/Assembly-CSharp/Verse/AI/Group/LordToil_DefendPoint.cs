using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200066B RID: 1643
	public class LordToil_DefendPoint : LordToil
	{
		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x06002EAD RID: 11949 RVA: 0x00116CD3 File Offset: 0x00114ED3
		protected LordToilData_DefendPoint Data
		{
			get
			{
				return (LordToilData_DefendPoint)this.data;
			}
		}

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x06002EAE RID: 11950 RVA: 0x00116CE0 File Offset: 0x00114EE0
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.defendPoint;
			}
		}

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06002EAF RID: 11951 RVA: 0x00116CED File Offset: 0x00114EED
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return this.allowSatisfyLongNeeds;
			}
		}

		// Token: 0x06002EB0 RID: 11952 RVA: 0x00116CF5 File Offset: 0x00114EF5
		public LordToil_DefendPoint(bool canSatisfyLongNeeds = true)
		{
			this.allowSatisfyLongNeeds = canSatisfyLongNeeds;
			this.data = new LordToilData_DefendPoint();
		}

		// Token: 0x06002EB1 RID: 11953 RVA: 0x00116D16 File Offset: 0x00114F16
		public LordToil_DefendPoint(IntVec3 defendPoint, float defendRadius = 28f, float? wanderRadius = null) : this(true)
		{
			this.Data.defendPoint = defendPoint;
			this.Data.defendRadius = defendRadius;
			this.Data.wanderRadius = wanderRadius;
		}

		// Token: 0x06002EB2 RID: 11954 RVA: 0x00116D44 File Offset: 0x00114F44
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

		// Token: 0x06002EB3 RID: 11955 RVA: 0x00116E16 File Offset: 0x00115016
		public void SetDefendPoint(IntVec3 defendPoint)
		{
			this.Data.defendPoint = defendPoint;
		}

		// Token: 0x04001C9A RID: 7322
		private bool allowSatisfyLongNeeds = true;
	}
}
