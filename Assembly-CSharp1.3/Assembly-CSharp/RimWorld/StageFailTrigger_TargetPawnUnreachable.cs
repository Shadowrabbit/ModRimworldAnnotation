using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000F19 RID: 3865
	public class StageFailTrigger_TargetPawnUnreachable : StageFailTrigger
	{
		// Token: 0x06005C00 RID: 23552 RVA: 0x001FC4F8 File Offset: 0x001FA6F8
		public override bool Failed(LordJob_Ritual ritual, TargetInfo spot, TargetInfo focus)
		{
			Pawn pawn = ritual.PawnWithRole(this.takerId);
			Pawn t = ritual.PawnWithRole(this.takeeId);
			return !pawn.CanReach(t, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn);
		}

		// Token: 0x06005C01 RID: 23553 RVA: 0x001FC531 File Offset: 0x001FA731
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.takerId, "takerId", null, false);
			Scribe_Values.Look<string>(ref this.takeeId, "takeeId", null, false);
		}

		// Token: 0x04003592 RID: 13714
		[NoTranslate]
		public string takerId;

		// Token: 0x04003593 RID: 13715
		[NoTranslate]
		public string takeeId;
	}
}
