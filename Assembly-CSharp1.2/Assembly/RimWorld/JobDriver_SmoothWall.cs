using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B6B RID: 2923
	public class JobDriver_SmoothWall : JobDriver
	{
		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x060044C1 RID: 17601 RVA: 0x00032B7D File Offset: 0x00030D7D
		protected int BaseWorkAmount
		{
			get
			{
				return 6500;
			}
		}

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x060044C2 RID: 17602 RVA: 0x00032B84 File Offset: 0x00030D84
		protected DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.SmoothWall;
			}
		}

		// Token: 0x060044C3 RID: 17603 RVA: 0x00190CB4 File Offset: 0x0018EEB4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.job.targetA.Cell, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060044C4 RID: 17604 RVA: 0x00032B8B File Offset: 0x00030D8B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !this.job.ignoreDesignations && this.Map.designationManager.DesignationAt(this.TargetLocA, this.DesDef) == null);
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
			Toil doWork = new Toil();
			doWork.initAction = delegate()
			{
				this.workLeft = (float)this.BaseWorkAmount;
			};
			doWork.tickAction = delegate()
			{
				float num = doWork.actor.GetStatValue(StatDefOf.SmoothingSpeed, true) * 1.7f;
				this.workLeft -= num;
				if (doWork.actor.skills != null)
				{
					doWork.actor.skills.Learn(SkillDefOf.Construction, 0.1f, false);
				}
				if (this.workLeft <= 0f)
				{
					this.DoEffect();
					Designation designation = this.Map.designationManager.DesignationAt(this.TargetLocA, this.DesDef);
					if (designation != null)
					{
						designation.Delete();
					}
					this.ReadyForNextToil();
					return;
				}
			};
			doWork.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			doWork.WithProgressBar(TargetIndex.A, () => 1f - this.workLeft / (float)this.BaseWorkAmount, false, -0.5f);
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			doWork.activeSkill = (() => SkillDefOf.Construction);
			yield return doWork;
			yield break;
		}

		// Token: 0x060044C5 RID: 17605 RVA: 0x00190D10 File Offset: 0x0018EF10
		protected void DoEffect()
		{
			SmoothableWallUtility.Notify_SmoothedByPawn(SmoothableWallUtility.SmoothWall(base.TargetA.Thing, this.pawn), this.pawn);
		}

		// Token: 0x060044C6 RID: 17606 RVA: 0x00032B9B File Offset: 0x00030D9B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		// Token: 0x04002E9B RID: 11931
		private float workLeft = -1000f;
	}
}
