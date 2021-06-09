using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E02 RID: 3586
	public class LordToil_KidnapCover : LordToil_DoOpportunisticTaskOrCover
	{
		// Token: 0x17000C8E RID: 3214
		// (get) Token: 0x0600518D RID: 20877 RVA: 0x0003916B File Offset: 0x0003736B
		protected override DutyDef DutyDef
		{
			get
			{
				return DutyDefOf.Kidnap;
			}
		}

		// Token: 0x17000C8F RID: 3215
		// (get) Token: 0x0600518E RID: 20878 RVA: 0x00039172 File Offset: 0x00037372
		public override bool ForceHighStoryDanger
		{
			get
			{
				return this.cover;
			}
		}

		// Token: 0x17000C90 RID: 3216
		// (get) Token: 0x0600518F RID: 20879 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005190 RID: 20880 RVA: 0x001BBC38 File Offset: 0x001B9E38
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
