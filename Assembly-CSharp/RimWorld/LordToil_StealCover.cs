using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E11 RID: 3601
	public class LordToil_StealCover : LordToil_DoOpportunisticTaskOrCover
	{
		// Token: 0x17000CA1 RID: 3233
		// (get) Token: 0x060051D7 RID: 20951 RVA: 0x000393D7 File Offset: 0x000375D7
		protected override DutyDef DutyDef
		{
			get
			{
				return DutyDefOf.Steal;
			}
		}

		// Token: 0x17000CA2 RID: 3234
		// (get) Token: 0x060051D8 RID: 20952 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CA3 RID: 3235
		// (get) Token: 0x060051D9 RID: 20953 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060051DA RID: 20954 RVA: 0x001BCE3C File Offset: 0x001BB03C
		protected override bool TryFindGoodOpportunisticTaskTarget(Pawn pawn, out Thing target, List<Thing> alreadyTakenTargets)
		{
			if (pawn.mindState.duty != null && pawn.mindState.duty.def == this.DutyDef && pawn.carryTracker.CarriedThing != null)
			{
				target = pawn.carryTracker.CarriedThing;
				return true;
			}
			return StealAIUtility.TryFindBestItemToSteal(pawn.Position, pawn.Map, 7f, out target, pawn, alreadyTakenTargets);
		}
	}
}
