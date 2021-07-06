using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E06 RID: 3590
	public class LordToil_PrisonerEscape : LordToil_Travel
	{
		// Token: 0x17000C94 RID: 3220
		// (get) Token: 0x0600519D RID: 20893 RVA: 0x00039198 File Offset: 0x00037398
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.dest;
			}
		}

		// Token: 0x17000C95 RID: 3221
		// (get) Token: 0x0600519E RID: 20894 RVA: 0x00030E6E File Offset: 0x0002F06E
		private LordToilData_Travel Data
		{
			get
			{
				return (LordToilData_Travel)this.data;
			}
		}

		// Token: 0x17000C96 RID: 3222
		// (get) Token: 0x0600519F RID: 20895 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C97 RID: 3223
		// (get) Token: 0x060051A0 RID: 20896 RVA: 0x000391A5 File Offset: 0x000373A5
		protected override float AllArrivedCheckRadius
		{
			get
			{
				return 14f;
			}
		}

		// Token: 0x060051A1 RID: 20897 RVA: 0x000391AC File Offset: 0x000373AC
		public LordToil_PrisonerEscape(IntVec3 dest, int sapperThingID) : base(dest)
		{
			this.sapperThingID = sapperThingID;
		}

		// Token: 0x060051A2 RID: 20898 RVA: 0x001BBEA0 File Offset: 0x001BA0A0
		public override void UpdateAllDuties()
		{
			LordToilData_Travel data = this.Data;
			Pawn leader = this.GetLeader();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (this.IsSapper(pawn))
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrisonerEscapeSapper, data.dest, -1f);
				}
				else if (leader == null || pawn == leader)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrisonerEscape, data.dest, -1f);
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrisonerEscape, leader, 10f);
				}
			}
		}

		// Token: 0x060051A3 RID: 20899 RVA: 0x001BBF6C File Offset: 0x001BA16C
		public override void LordToilTick()
		{
			base.LordToilTick();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].guilt.Notify_Guilty();
			}
		}

		// Token: 0x060051A4 RID: 20900 RVA: 0x001BBFB8 File Offset: 0x001BA1B8
		private Pawn GetLeader()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				if (!this.lord.ownedPawns[i].Downed && this.IsSapper(this.lord.ownedPawns[i]))
				{
					return this.lord.ownedPawns[i];
				}
			}
			for (int j = 0; j < this.lord.ownedPawns.Count; j++)
			{
				if (!this.lord.ownedPawns[j].Downed)
				{
					return this.lord.ownedPawns[j];
				}
			}
			return null;
		}

		// Token: 0x060051A5 RID: 20901 RVA: 0x000391BC File Offset: 0x000373BC
		private bool IsSapper(Pawn p)
		{
			return p.thingIDNumber == this.sapperThingID;
		}

		// Token: 0x0400344A RID: 13386
		private int sapperThingID;
	}
}
