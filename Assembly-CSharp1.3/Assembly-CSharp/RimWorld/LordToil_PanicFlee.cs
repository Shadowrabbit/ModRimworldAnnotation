using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008BB RID: 2235
	public class LordToil_PanicFlee : LordToil
	{
		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x06003AF4 RID: 15092 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x06003AF5 RID: 15093 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003AF6 RID: 15094 RVA: 0x00149638 File Offset: 0x00147838
		public override void Init()
		{
			base.Init();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (!pawn.InAggroMentalState && (!this.HasFleeingDuty(pawn) || pawn.mindState.duty.def == DutyDefOf.ExitMapRandom))
				{
					pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.PanicFlee, null, false, false, null, false, false, false);
				}
			}
		}

		// Token: 0x06003AF7 RID: 15095 RVA: 0x001496C0 File Offset: 0x001478C0
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (!this.HasFleeingDuty(pawn))
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.ExitMapRandom);
				}
			}
		}

		// Token: 0x06003AF8 RID: 15096 RVA: 0x00149718 File Offset: 0x00147918
		private bool HasFleeingDuty(Pawn pawn)
		{
			return pawn.mindState.duty != null && (pawn.mindState.duty.def == DutyDefOf.ExitMapRandom || pawn.mindState.duty.def == DutyDefOf.Steal || pawn.mindState.duty.def == DutyDefOf.Kidnap);
		}
	}
}
