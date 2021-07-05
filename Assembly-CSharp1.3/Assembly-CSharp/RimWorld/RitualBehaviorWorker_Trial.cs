using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F29 RID: 3881
	public class RitualBehaviorWorker_Trial : RitualBehaviorWorker
	{
		// Token: 0x06005C53 RID: 23635 RVA: 0x001FDA78 File Offset: 0x001FBC78
		public RitualBehaviorWorker_Trial()
		{
		}

		// Token: 0x06005C54 RID: 23636 RVA: 0x001FDA87 File Offset: 0x001FBC87
		public RitualBehaviorWorker_Trial(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x06005C55 RID: 23637 RVA: 0x001FDA98 File Offset: 0x001FBC98
		public override void Cleanup(LordJob_Ritual ritual)
		{
			Pawn pawn = ritual.PawnWithRole("convict");
			if (pawn.IsPrisonerOfColony)
			{
				pawn.guest.WaitInsteadOfEscapingFor(2500);
			}
		}

		// Token: 0x06005C56 RID: 23638 RVA: 0x001FDACC File Offset: 0x001FBCCC
		public override void PostCleanup(LordJob_Ritual ritual)
		{
			Pawn warden = ritual.PawnWithRole("leader");
			Pawn pawn = ritual.PawnWithRole("convict");
			if (pawn.IsPrisonerOfColony)
			{
				WorkGiver_Warden_TakeToBed.TryTakePrisonerToBed(pawn, warden);
				pawn.guest.WaitInsteadOfEscapingFor(1250);
			}
		}

		// Token: 0x06005C57 RID: 23639 RVA: 0x001FDB10 File Offset: 0x001FBD10
		public override void Tick(LordJob_Ritual ritual)
		{
			base.Tick(ritual);
			if (ritual.StageIndex == 0)
			{
				return;
			}
			if (this.ticksSinceLastInteraction != -1 && this.ticksSinceLastInteraction <= 700)
			{
				this.ticksSinceLastInteraction++;
				return;
			}
			this.ticksSinceLastInteraction = 0;
			Pawn pawn = ritual.PawnWithRole("leader");
			Pawn pawn2 = ritual.PawnWithRole("convict");
			if (Rand.Bool)
			{
				pawn.interactions.TryInteractWith(pawn2, InteractionDefOf.Trial_Accuse);
				return;
			}
			pawn2.interactions.TryInteractWith(pawn, InteractionDefOf.Trial_Defend);
		}

		// Token: 0x06005C58 RID: 23640 RVA: 0x001FDB9D File Offset: 0x001FBD9D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceLastInteraction, "ticksSinceLastInteraction", -1, false);
		}

		// Token: 0x040035C1 RID: 13761
		private int ticksSinceLastInteraction = -1;

		// Token: 0x040035C2 RID: 13762
		public const int SocialInteractionIntervalTicks = 700;
	}
}
