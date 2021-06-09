using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000AD9 RID: 2777
	public class LordToil_Travel : LordToil
	{
		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x060041AA RID: 16810 RVA: 0x00030E61 File Offset: 0x0002F061
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.dest;
			}
		}

		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x060041AB RID: 16811 RVA: 0x00030E6E File Offset: 0x0002F06E
		private LordToilData_Travel Data
		{
			get
			{
				return (LordToilData_Travel)this.data;
			}
		}

		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x060041AC RID: 16812 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x060041AD RID: 16813 RVA: 0x00030E7B File Offset: 0x0002F07B
		protected virtual float AllArrivedCheckRadius
		{
			get
			{
				return 10f;
			}
		}

		// Token: 0x060041AE RID: 16814 RVA: 0x00030E82 File Offset: 0x0002F082
		public LordToil_Travel(IntVec3 dest)
		{
			this.data = new LordToilData_Travel();
			this.Data.dest = dest;
		}

		// Token: 0x060041AF RID: 16815 RVA: 0x00188190 File Offset: 0x00186390
		public override void UpdateAllDuties()
		{
			LordToilData_Travel data = this.Data;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(DutyDefOf.TravelOrLeave, data.dest, -1f);
				pawnDuty.maxDanger = this.maxDanger;
				this.lord.ownedPawns[i].mindState.duty = pawnDuty;
			}
		}

		// Token: 0x060041B0 RID: 16816 RVA: 0x00188204 File Offset: 0x00186404
		public override void LordToilTick()
		{
			if (Find.TickManager.TicksGame % 205 == 0)
			{
				LordToilData_Travel data = this.Data;
				bool flag = true;
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					Pawn pawn = this.lord.ownedPawns[i];
					if (!pawn.Position.InHorDistOf(data.dest, this.AllArrivedCheckRadius) || !pawn.CanReach(data.dest, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					this.lord.ReceiveMemo("TravelArrived");
				}
			}
		}

		// Token: 0x060041B1 RID: 16817 RVA: 0x00030EA1 File Offset: 0x0002F0A1
		public bool HasDestination()
		{
			return this.Data.dest.IsValid;
		}

		// Token: 0x060041B2 RID: 16818 RVA: 0x00030EB3 File Offset: 0x0002F0B3
		public void SetDestination(IntVec3 dest)
		{
			this.Data.dest = dest;
		}

		// Token: 0x04002D34 RID: 11572
		public Danger maxDanger;
	}
}
