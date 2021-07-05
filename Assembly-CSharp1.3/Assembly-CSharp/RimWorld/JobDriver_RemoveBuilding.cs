using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006DE RID: 1758
	public abstract class JobDriver_RemoveBuilding : JobDriver
	{
		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x06003105 RID: 12549 RVA: 0x000FE409 File Offset: 0x000FC609
		protected Thing Target
		{
			get
			{
				return this.job.targetA.Thing;
			}
		}

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x06003106 RID: 12550 RVA: 0x0011F02A File Offset: 0x0011D22A
		protected Building Building
		{
			get
			{
				return (Building)this.Target.GetInnerIfMinified();
			}
		}

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x06003107 RID: 12551
		protected abstract DesignationDef Designation { get; }

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x06003108 RID: 12552
		protected abstract float TotalNeededWork { get; }

		// Token: 0x06003109 RID: 12553 RVA: 0x0011F03C File Offset: 0x0011D23C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
			Scribe_Values.Look<float>(ref this.totalNeededWork, "totalNeededWork", 0f, false);
		}

		// Token: 0x0600310A RID: 12554 RVA: 0x0011F070 File Offset: 0x0011D270
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Target, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x0600310B RID: 12555 RVA: 0x0011F092 File Offset: 0x0011D292
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
			doWork.WithProgressBar(TargetIndex.A, () => 1f - this.workLeft / this.totalNeededWork, false, -0.5f, false);
			doWork.activeSkill = (() => SkillDefOf.Construction);
			yield return doWork;
			yield return new Toil
			{
				initAction = delegate()
				{
					if (this.Target.Faction != null)
					{
						this.Target.Faction.Notify_BuildingRemoved(this.Building, this.pawn);
					}
					this.FinishedRemoving();
					base.Map.designationManager.RemoveAllDesignationsOn(this.Target, false);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}

		// Token: 0x0600310C RID: 12556 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void FinishedRemoving()
		{
		}

		// Token: 0x0600310D RID: 12557 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void TickAction()
		{
		}

		// Token: 0x04001D65 RID: 7525
		private float workLeft;

		// Token: 0x04001D66 RID: 7526
		private float totalNeededWork;
	}
}
