using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200075A RID: 1882
	public class JobDriver_ExecuteSlave : JobDriver
	{
		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x06003421 RID: 13345 RVA: 0x0011D55B File Offset: 0x0011B75B
		protected Pawn Victim
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x06003422 RID: 13346 RVA: 0x00128097 File Offset: 0x00126297
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Victim, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003423 RID: 13347 RVA: 0x001280B9 File Offset: 0x001262B9
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Execute slave"))
			{
				yield break;
			}
			this.FailOnAggroMentalState(TargetIndex.A);
			yield return Toils_Interpersonal.GotoSlave(this.pawn, this.Victim).FailOn(() => !this.Victim.IsSlaveOfColony);
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
