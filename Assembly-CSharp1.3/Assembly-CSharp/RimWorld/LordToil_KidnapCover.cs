using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020008B8 RID: 2232
	public class LordToil_KidnapCover : LordToil_DoOpportunisticTaskOrCover
	{
		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x06003AEA RID: 15082 RVA: 0x001494D7 File Offset: 0x001476D7
		protected override DutyDef DutyDef
		{
			get
			{
				return DutyDefOf.Kidnap;
			}
		}

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x06003AEB RID: 15083 RVA: 0x001494DE File Offset: 0x001476DE
		public override bool ForceHighStoryDanger
		{
			get
			{
				return this.cover;
			}
		}

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x06003AEC RID: 15084 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003AED RID: 15085 RVA: 0x001494E8 File Offset: 0x001476E8
		protected override bool TryFindGoodOpportunisticTaskTarget(Pawn pawn, out Thing target, List<Thing> alreadyTakenTargets)
		{
			if (pawn.mindState.duty != null && pawn.mindState.duty.def == this.DutyDef && pawn.carryTracker.CarriedThing is Pawn)
			{
				target = pawn.carryTracker.CarriedThing;
				return true;
			}
			Pawn pawn2;
			bool result = KidnapAIUtility.TryFindGoodKidnapVictim(pawn, 8f, out pawn2, alreadyTakenTargets);
			target = pawn2;
			return result;
		}
	}
}
