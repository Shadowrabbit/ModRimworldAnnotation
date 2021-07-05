using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000711 RID: 1809
	public class JobDriver_ExecuteGuiltyColonist : JobDriver
	{
		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x06003235 RID: 12853 RVA: 0x0011D55B File Offset: 0x0011B75B
		protected Pawn Victim
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x06003236 RID: 12854 RVA: 0x001221C3 File Offset: 0x001203C3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Victim, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003237 RID: 12855 RVA: 0x001221E5 File Offset: 0x001203E5
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnAggroMentalState(TargetIndex.A);
			yield return Toils_Interpersonal.GotoGuiltyColonist(this.pawn, this.Victim).FailOn(() => !this.Victim.IsColonist || !this.Victim.guilt.IsGuilty || !this.Victim.guilt.awaitingExecution);
			Toil execute = new Toil();
			execute.initAction = delegate()
			{
				ExecutionUtility.DoExecutionByCut(execute.actor, this.Victim, 8, true);
				ThoughtUtility.GiveThoughtsForPawnExecuted(this.Victim, execute.actor, PawnExecutionKind.GenericBrutal);
			};
			execute.defaultCompleteMode = ToilCompleteMode.Instant;
			execute.activeSkill = (() => SkillDefOf.Melee);
			yield return execute;
			yield break;
		}
	}
}
