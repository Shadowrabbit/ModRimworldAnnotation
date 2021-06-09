using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B30 RID: 2864
	public abstract class JobDriver_GatherAnimalBodyResources : JobDriver
	{
		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x06004340 RID: 17216
		protected abstract float WorkTotal { get; }

		// Token: 0x06004341 RID: 17217
		protected abstract CompHasGatherableBodyResource GetComp(Pawn animal);

		// Token: 0x06004342 RID: 17218 RVA: 0x00031E7D File Offset: 0x0003007D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.gatherProgress, "gatherProgress", 0f, false);
		}

		// Token: 0x06004343 RID: 17219 RVA: 0x0002DA01 File Offset: 0x0002BC01
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06004344 RID: 17220 RVA: 0x00031E9B File Offset: 0x0003009B
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
			wait.WithProgressBar(TargetIndex.A, () => this.gatherProgress / this.WorkTotal, false, -0.5f);
			wait.activeSkill = (() => SkillDefOf.Animals);
			yield return wait;
			yield break;
		}

		// Token: 0x04002DF6 RID: 11766
		private float gatherProgress;

		// Token: 0x04002DF7 RID: 11767
		protected const TargetIndex AnimalInd = TargetIndex.A;
	}
}
