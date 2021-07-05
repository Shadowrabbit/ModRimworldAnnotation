using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006C3 RID: 1731
	public abstract class JobDriver_GatherAnimalBodyResources : JobDriver
	{
		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x0600303B RID: 12347
		protected abstract float WorkTotal { get; }

		// Token: 0x0600303C RID: 12348
		protected abstract CompHasGatherableBodyResource GetComp(Pawn animal);

		// Token: 0x0600303D RID: 12349 RVA: 0x0011D52D File Offset: 0x0011B72D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.gatherProgress, "gatherProgress", 0f, false);
		}

		// Token: 0x0600303E RID: 12350 RVA: 0x000FA68B File Offset: 0x000F888B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x0600303F RID: 12351 RVA: 0x0011D54B File Offset: 0x0011B74B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnDowned(TargetIndex.A);
			this.FailOnNotCasualInterruptible(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil wait = new Toil();
			wait.initAction = delegate()
			{
				Pawn actor = wait.actor;
				Pawn pawn = (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
				actor.pather.StopDead();
				PawnUtility.ForceWait(pawn, 15000, null, true);
			};
			wait.tickAction = delegate()
			{
				Pawn actor = wait.actor;
				actor.skills.Learn(SkillDefOf.Animals, 0.13f, false);
				this.gatherProgress += actor.GetStatValue(StatDefOf.AnimalGatherSpeed, true);
				if (this.gatherProgress >= this.WorkTotal)
				{
					this.GetComp((Pawn)((Thing)this.job.GetTarget(TargetIndex.A))).Gathered(this.pawn);
					actor.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
				}
			};
			wait.AddFinishAction(delegate
			{
				Pawn pawn = (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
				if (pawn != null && pawn.CurJobDef == JobDefOf.Wait_MaintainPosture)
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			});
			wait.FailOnDespawnedOrNull(TargetIndex.A);
			wait.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			wait.AddEndCondition(delegate
			{
				if (!this.GetComp((Pawn)((Thing)this.job.GetTarget(TargetIndex.A))).ActiveAndFull)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			wait.defaultCompleteMode = ToilCompleteMode.Never;
			wait.WithProgressBar(TargetIndex.A, () => this.gatherProgress / this.WorkTotal, false, -0.5f, false);
			wait.activeSkill = (() => SkillDefOf.Animals);
			yield return wait;
			yield break;
		}

		// Token: 0x04001D3D RID: 7485
		private float gatherProgress;

		// Token: 0x04001D3E RID: 7486
		protected const TargetIndex AnimalInd = TargetIndex.A;
	}
}
