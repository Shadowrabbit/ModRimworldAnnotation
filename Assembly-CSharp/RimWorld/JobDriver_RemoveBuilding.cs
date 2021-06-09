using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B61 RID: 2913
	public abstract class JobDriver_RemoveBuilding : JobDriver
	{
		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x06004485 RID: 17541 RVA: 0x0002DE6A File Offset: 0x0002C06A
		protected Thing Target
		{
			get
			{
				return this.job.targetA.Thing;
			}
		}

		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x06004486 RID: 17542 RVA: 0x00032991 File Offset: 0x00030B91
		protected Building Building
		{
			get
			{
				return (Building)this.Target.GetInnerIfMinified();
			}
		}

		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x06004487 RID: 17543
		protected abstract DesignationDef Designation { get; }

		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x06004488 RID: 17544
		protected abstract float TotalNeededWork { get; }

		// Token: 0x06004489 RID: 17545 RVA: 0x000329A3 File Offset: 0x00030BA3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
			Scribe_Values.Look<float>(ref this.totalNeededWork, "totalNeededWork", 0f, false);
		}

		// Token: 0x0600448A RID: 17546 RVA: 0x000329D7 File Offset: 0x00030BD7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Target, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x0600448B RID: 17547 RVA: 0x000329F9 File Offset: 0x00030BF9
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnThingMissingDesignation(TargetIndex.A, this.Designation);
			this.FailOnForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, (this.Target is Building_Trap) ? PathEndMode.OnCell : PathEndMode.Touch);
			Toil doWork = new Toil().FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			doWork.initAction = delegate()
			{
				this.totalNeededWork = this.TotalNeededWork;
				this.workLeft = this.totalNeededWork;
			};
			doWork.tickAction = delegate()
			{
				this.workLeft -= this.pawn.GetStatValue(StatDefOf.ConstructionSpeed, true) * 1.7f;
				this.TickAction();
				if (this.workLeft <= 0f)
				{
					doWork.actor.jobs.curDriver.ReadyForNextToil();
				}
			};
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			doWork.WithProgressBar(TargetIndex.A, () => 1f - this.workLeft / this.totalNeededWork, false, -0.5f);
			doWork.activeSkill = (() => SkillDefOf.Construction);
			yield return doWork;
			yield return new Toil
			{
				initAction = delegate()
				{
					this.FinishedRemoving();
					base.Map.designationManager.RemoveAllDesignationsOn(this.Target, false);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}

		// Token: 0x0600448C RID: 17548 RVA: 0x00006A05 File Offset: 0x00004C05
		protected virtual void FinishedRemoving()
		{
		}

		// Token: 0x0600448D RID: 17549 RVA: 0x00006A05 File Offset: 0x00004C05
		protected virtual void TickAction()
		{
		}

		// Token: 0x04002E7F RID: 11903
		private float workLeft;

		// Token: 0x04002E80 RID: 11904
		private float totalNeededWork;
	}
}
