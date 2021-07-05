using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200089D RID: 2205
	public abstract class LordToil_PrepareCaravan_RopeAnimals : LordToil
	{
		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x06003A6D RID: 14957 RVA: 0x00146F17 File Offset: 0x00145117
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x06003A6E RID: 14958 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003A6F RID: 14959 RVA: 0x001472F5 File Offset: 0x001454F5
		public LordToil_PrepareCaravan_RopeAnimals(IntVec3 destinationPoint, int? ropeeLimit)
		{
			this.destinationPoint = destinationPoint;
			this.ropeeLimit = ropeeLimit;
		}

		// Token: 0x06003A70 RID: 14960 RVA: 0x0014730C File Offset: 0x0014550C
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn.IsColonist || AnimalPenUtility.NeedsToBeManagedByRope(pawn))
				{
					pawn.mindState.duty = this.MakeRopeDuty();
					pawn.mindState.duty.ropeeLimit = this.ropeeLimit;
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Wait, this.destinationPoint, -1f);
				}
			}
		}

		// Token: 0x06003A71 RID: 14961
		protected abstract PawnDuty MakeRopeDuty();

		// Token: 0x04001FF9 RID: 8185
		protected IntVec3 destinationPoint;

		// Token: 0x04001FFA RID: 8186
		protected int? ropeeLimit;
	}
}
