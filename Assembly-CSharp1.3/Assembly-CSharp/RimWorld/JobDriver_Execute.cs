using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200074C RID: 1868
	public class JobDriver_Execute : JobDriver
	{
		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x060033B4 RID: 13236 RVA: 0x0011D55B File Offset: 0x0011B75B
		protected Pawn Victim
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x060033B5 RID: 13237 RVA: 0x00125AB4 File Offset: 0x00123CB4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Victim, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060033B6 RID: 13238 RVA: 0x00125AD6 File Offset: 0x00123CD6
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnAggroMentalState(TargetIndex.A);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Victim, PrisonerInteractionModeDefOf.Execution).FailOn(() => !this.Victim.IsPrisonerOfColony || !this.Victim.guest.PrisonerIsSecure);
			Toil execute = new Toil();
			execute.initAction = delegate()
			{
				ExecutionUtility.DoExecutionByCut(execute.actor, this.Victim, 8, true);
				ThoughtUtility.GiveThoughtsForPawnExecuted(this.Victim, execute.actor, PawnExecutionKind.GenericBrutal);
				TaleRecorder.RecordTale(TaleDefOf.ExecutedPrisoner, new object[]
				{
					this.pawn,
					this.Victim
				});
			};
			execute.defaultCompleteMode = ToilCompleteMode.Instant;
			execute.activeSkill = (() => SkillDefOf.Melee);
			yield return execute;
			yield break;
		}
	}
}
