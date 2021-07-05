using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008BC RID: 2236
	public class LordToil_PrisonerEscape : LordToil_Travel
	{
		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x06003AFA RID: 15098 RVA: 0x0014977C File Offset: 0x0014797C
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.dest;
			}
		}

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x06003AFB RID: 15099 RVA: 0x001171A5 File Offset: 0x001153A5
		private LordToilData_Travel Data
		{
			get
			{
				return (LordToilData_Travel)this.data;
			}
		}

		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x06003AFC RID: 15100 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x06003AFD RID: 15101 RVA: 0x00149789 File Offset: 0x00147989
		protected override float AllArrivedCheckRadius
		{
			get
			{
				return 14f;
			}
		}

		// Token: 0x06003AFE RID: 15102 RVA: 0x00149790 File Offset: 0x00147990
		public LordToil_PrisonerEscape(IntVec3 dest, int sapperThingID) : base(dest)
		{
			this.sapperThingID = sapperThingID;
		}

		// Token: 0x06003AFF RID: 15103 RVA: 0x001497A0 File Offset: 0x001479A0
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

		// Token: 0x06003B00 RID: 15104 RVA: 0x0014986C File Offset: 0x00147A6C
		public override void LordToilTick()
		{
			base.LordToilTick();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].guilt.Notify_Guilty(60000);
			}
		}

		// Token: 0x06003B01 RID: 15105 RVA: 0x001498BC File Offset: 0x00147ABC
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

		// Token: 0x06003B02 RID: 15106 RVA: 0x0014996D File Offset: 0x00147B6D
		private bool IsSapper(Pawn p)
		{
			return p.thingIDNumber == this.sapperThingID;
		}

		// Token: 0x04002024 RID: 8228
		private int sapperThingID;
	}
}
