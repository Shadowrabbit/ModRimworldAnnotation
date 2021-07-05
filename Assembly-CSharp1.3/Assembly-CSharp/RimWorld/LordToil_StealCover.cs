using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020008C2 RID: 2242
	public class LordToil_StealCover : LordToil_DoOpportunisticTaskOrCover
	{
		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x06003B1F RID: 15135 RVA: 0x0014A80B File Offset: 0x00148A0B
		protected override DutyDef DutyDef
		{
			get
			{
				return DutyDefOf.Steal;
			}
		}

		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x06003B20 RID: 15136 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x06003B21 RID: 15137 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003B22 RID: 15138 RVA: 0x0014A814 File Offset: 0x00148A14
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
