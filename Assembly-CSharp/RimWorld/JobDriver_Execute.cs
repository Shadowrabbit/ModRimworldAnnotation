using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C32 RID: 3122
	public class JobDriver_Execute : JobDriver
	{
		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x06004951 RID: 18769 RVA: 0x00031F33 File Offset: 0x00030133
		protected Pawn Victim
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x06004952 RID: 18770 RVA: 0x00034E77 File Offset: 0x00033077
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Victim, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06004953 RID: 18771 RVA: 0x00034E99 File Offset: 0x00033099
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnAggroMentalState(TargetIndex.A);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Victim, PrisonerInteractionModeDefOf.Execution).FailOn(() => !this.Victim.IsPrisonerOfColony || !this.Victim.guest.PrisonerIsSecure);
			Toil execute = new Toil();
			execute.initAction = delegate()
			{
				ExecutionUtility.DoExecutionByCut(execute.actor, this.Victim);
				ThoughtUtility.GiveThoughtsForPawnExecuted(this.Victim, PawnExecutionKind.GenericBrutal);
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
