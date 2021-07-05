using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000674 RID: 1652
	public class LordToil_Travel : LordToil
	{
		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x06002ECD RID: 11981 RVA: 0x00117198 File Offset: 0x00115398
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.dest;
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x06002ECE RID: 11982 RVA: 0x001171A5 File Offset: 0x001153A5
		private LordToilData_Travel Data
		{
			get
			{
				return (LordToilData_Travel)this.data;
			}
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x06002ECF RID: 11983 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x06002ED0 RID: 11984 RVA: 0x001171B2 File Offset: 0x001153B2
		protected virtual float AllArrivedCheckRadius
		{
			get
			{
				return 10f;
			}
		}

		// Token: 0x06002ED1 RID: 11985 RVA: 0x001171B9 File Offset: 0x001153B9
		public LordToil_Travel(IntVec3 dest)
		{
			this.data = new LordToilData_Travel();
			this.Data.dest = dest;
		}

		// Token: 0x06002ED2 RID: 11986 RVA: 0x001171D8 File Offset: 0x001153D8
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

		// Token: 0x06002ED3 RID: 11987 RVA: 0x0011724C File Offset: 0x0011544C
		public override void LordToilTick()
		{
			if (Find.TickManager.TicksGame % 205 == 0)
			{
				LordToilData_Travel data = this.Data;
				bool flag = true;
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					Pawn pawn = this.lord.ownedPawns[i];
					if (!pawn.Position.InHorDistOf(data.dest, this.AllArrivedCheckRadius) || !pawn.CanReach(data.dest, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
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

		// Token: 0x06002ED4 RID: 11988 RVA: 0x001172F0 File Offset: 0x001154F0
		public bool HasDestination()
		{
			return this.Data.dest.IsValid;
		}

		// Token: 0x06002ED5 RID: 11989 RVA: 0x00117302 File Offset: 0x00115502
		public void SetDestination(IntVec3 dest)
		{
			this.Data.dest = dest;
		}

		// Token: 0x04001CA5 RID: 7333
		public Danger maxDanger;
	}
}
